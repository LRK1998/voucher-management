using Microsoft.EntityFrameworkCore;
using VoucherManagement.Api.Enums;

namespace VoucherManagement.Api.Dtos
{
    public class UpdateVoucherDto
    {
        public decimal OriginalValue { get; set; }
        public decimal CurrentValue { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public VoucherStatus Status { get; set; } = VoucherStatus.Created;
    }
}
