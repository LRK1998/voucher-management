using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using VoucherManagement.Api.Data;
using VoucherManagement.Api.Enums;
using VoucherManagement.Api.Helpers;
using VoucherManagement.Api.Models;

namespace VoucherManagement.Api.Services
{
    public class VoucherService
    {
        private readonly IDbContextFactory<AppDbContext> _appDbContext;

        public VoucherService(IDbContextFactory<AppDbContext> appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Voucher>> GetVouchers()
        {
            using var context = await _appDbContext.CreateDbContextAsync();
            return await context.Vouchers.OrderByDescending(x => x.CreatedAt).ToListAsync();    
        }

        public async Task<Voucher> CreateVoucher()
        {
            using var context = await _appDbContext.CreateDbContextAsync();

            var voucher = new Voucher
            {
                Code = VoucherHelper.GenerateSecureId(),
                OriginalValue = 0,
                CurrentValue = 0,
                Status = VoucherStatus.Created
            };

            context.Vouchers.Add(voucher);
            await context.SaveChangesAsync();

            return voucher;
        }

        public async Task UpdateVoucher(Voucher updatedVoucher)
        {
            using var context = await _appDbContext.CreateDbContextAsync();

            var dbEntry = await context.Vouchers
                    .FirstOrDefaultAsync(v => v.Id == updatedVoucher.Id);

            if (dbEntry != null)
            {
                dbEntry.OriginalValue = updatedVoucher.OriginalValue;
                dbEntry.CurrentValue = updatedVoucher.CurrentValue;
                dbEntry.UpdatedAt = DateTime.UtcNow;
                dbEntry.Status = updatedVoucher.Status;
            }

            await context.SaveChangesAsync();
        }

        public async Task DelteVoucher(Voucher updatedVoucher)
        {
            using var context = await _appDbContext.CreateDbContextAsync();

            var dbEntry = await context.Vouchers
                    .FirstOrDefaultAsync(v => v.Id == updatedVoucher.Id);

            if (dbEntry != null)
            {
                context.Vouchers.Remove(dbEntry);
            }

            await context.SaveChangesAsync();
        }

        public async Task<Voucher?> GetVoucherById(int voucherId)
        {
            using var context = await _appDbContext.CreateDbContextAsync();

            return await context.Vouchers
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == voucherId);
        }
    }
}
