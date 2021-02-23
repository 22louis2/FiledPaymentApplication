using FiledPaymentApplication.Model;
using FiledPaymentApplication.Model.Enum;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Transaction && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Transaction)entityEntry.Entity).Status = PaymentStatus.pending;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Transaction)entityEntry.Entity).Status = PaymentStatus.processed;
                }
                else
                {
                    ((Transaction)entityEntry.Entity).Status = PaymentStatus.failed;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
