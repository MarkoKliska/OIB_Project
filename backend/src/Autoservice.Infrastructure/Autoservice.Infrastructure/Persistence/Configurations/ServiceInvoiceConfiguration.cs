using Autoservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autoservice.Infrastructure.Persistence.Configurations;

public class ServiceInvoiceConfiguration : IEntityTypeConfiguration<ServiceInvoice>
{
    public void Configure(EntityTypeBuilder<ServiceInvoice> builder)
    {
        builder.HasKey(si => si.Id);
        builder.Property(si => si.MechanicName).IsRequired().HasMaxLength(200);
        builder.Property(si => si.TotalAmount).HasPrecision(10, 2);
        builder.Property(si => si.IssuedAt).IsRequired();

        builder.HasOne(si => si.Vehicle)
            .WithOne(v => v.ServiceInvoice)
            .HasForeignKey<ServiceInvoice>(si => si.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(si => si.Mechanic)
            .WithMany(u => u.IssuedInvoices)
            .HasForeignKey(si => si.MechanicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
