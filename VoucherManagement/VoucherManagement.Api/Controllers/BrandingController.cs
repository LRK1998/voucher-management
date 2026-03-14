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
    [Route("api/branding/")]
    public class BrandingController : ControllerBase
    {
        private readonly string _brandingPath;

        public BrandingController(IConfiguration configuration)
        {
            _brandingPath = configuration["Branding:Path"] ?? "Branding";
        }

        [HttpGet("{filename}")]
        public IActionResult GetFile(string filename)
        {
            var filePath = Path.Combine(_brandingPath, filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = GetContentType(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, contentType);
        }

        private string GetContentType(string path)
        {
            return Path.GetExtension(path).ToLower() switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".ico" => "image/x-icon",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream"
            };
        }
    }
}
