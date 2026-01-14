using Microsoft.EntityFrameworkCore;
using Payments.Api.Domain;
using Payments.Api.Infrastructure.Events;

namespace Payments.Api.Infrastructure.Persistence;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options)
        : base(options) { }

    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<StoredEvent> StoredEvents => Set<StoredEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredEvent>(entity =>
        {
            entity.ToTable("stored_events");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Data).IsRequired();
            entity.Property(e => e.OccurredOn).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}

//using Microsoft.EntityFrameworkCore;
//using Payments.Api.Domain;

//namespace Payments.Api.Infrastructure.Persistence;

//public class PaymentsDbContext : DbContext
//{
//    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<Payment> Payments => Set<Payment>();
//}
