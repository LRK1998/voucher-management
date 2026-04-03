using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using VoucherManagement.Api.Configuration;
using VoucherManagement.Api.Data;
using VoucherManagement.Api.Dtos;
using VoucherManagement.Api.Helpers;
using VoucherManagement.Api.Models;
using VoucherManagement.Api.Services;
using static VoucherManagement.Api.Helpers.VoucherHelper;

namespace VoucherManagement.Api.Controllers
{
    [ApiController]
    [Route("api/vouchers")]
    public class VoucherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly VoucherOptions _voucherOptions;
        private readonly IDbContextFactory<AppDbContext> _appDbContext;
        private readonly VoucherService _voucherService;

        public VoucherController(IConfiguration configuration, IOptions<VoucherOptions> voucherOptions, VoucherService voucherService, IDbContextFactory<AppDbContext> appDbContext)
        {
            _configuration = configuration;
            _voucherOptions = voucherOptions.Value;
            _voucherService = voucherService;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Authorize(Policy = "ManageVouchers")]
        public async Task<ActionResult> GetVouchers()
        {
            var data = await _voucherService.GetVouchers();

            return Ok(data);
        }

        [HttpPost]
        [Authorize(Policy = "ManageVouchers")]
        public async Task<ActionResult> CreateVouchers()
        {
            var data = await _voucherService.CreateVoucher();

            return Ok(data);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "ManageVouchers")]
        public async Task<IActionResult> UpdateVoucher(int id,[FromBody] UpdateVoucherDto dto)
        {
            await using var context = await _appDbContext.CreateDbContextAsync();

            var dbEntry = await context.Vouchers
                .FirstOrDefaultAsync(v => v.Id == id);

            if (dbEntry == null)
                return NotFound();

            // Issue
            if(dbEntry.Status == Enums.VoucherStatus.Printed && dto.OriginalValue > 0)
            {
                dbEntry.OriginalValue = dto.OriginalValue;
                dbEntry.CurrentValue = dto.OriginalValue;
                dbEntry.Status = Enums.VoucherStatus.Issued;
            }

            // Redeem
            else if (dbEntry.Status == Enums.VoucherStatus.Issued || dbEntry.Status == Enums.VoucherStatus.PartiallyRedeemed)
            {
                if (dto.CurrentValue == 0)
                {
                    dbEntry.CurrentValue = dto.CurrentValue;
                    dbEntry.Status = Enums.VoucherStatus.FullyRedeemed;
                }

                if(dto.CurrentValue > 0)
                {
                    dbEntry.CurrentValue = dto.CurrentValue;
                    dbEntry.Status = Enums.VoucherStatus.PartiallyRedeemed;
                }
            }

            dbEntry.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        [Authorize(Policy = "ManageVouchers")]
        public async Task<ActionResult> GetVoucherPdf(int id)
        {
            await using var context = await _appDbContext.CreateDbContextAsync();

            var dbEntry = await context.Vouchers
                .FirstOrDefaultAsync(v => v.Id == id);

            if (dbEntry != null)
            {
                var templateFilePath = Path.Combine(_voucherOptions.TemplatePath, "voucher_template.pdf");
                var pdfBytes = VoucherHelper.GeneratePdfWithIText(dbEntry, 
                    templateFilePath, 
                    _voucherOptions.PdfPage,
                    _voucherOptions.QrCodeSize,
                    _voucherOptions.FontSize,
                    _voucherOptions.SpacingRight,
                    _voucherOptions.SpacingBottom,
                    _voucherOptions.TextSpacingBottom);

                dbEntry.Status = Enums.VoucherStatus.Printed;
                await context.SaveChangesAsync();

                return File(pdfBytes, "application/pdf", $"voucher_{dbEntry.Code}.pdf");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
