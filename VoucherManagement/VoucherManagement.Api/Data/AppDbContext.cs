using Microsoft.EntityFrameworkCore;
using VoucherManagement.Api.Models;

namespace VoucherManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Voucher> Vouchers { get; set; }
        //public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
