using Microsoft.EntityFrameworkCore;
using VoucherManagement.Api.Enums;

namespace VoucherManagement.Api.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal OriginalValue { get; set; }
        [Precision(18, 2)]
        public decimal CurrentValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public VoucherStatus Status { get; set; } = VoucherStatus.Created;
    }
}
