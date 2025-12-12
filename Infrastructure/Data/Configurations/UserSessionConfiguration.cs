using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.UserSession)
            .HasForeignKey<UserSession>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.OwnsOne(x => x.Token, options =>
        {
            options.Property(p => p.Hash)
                .HasColumnName("TokenHash")
                .IsRequired();

            options.Property(p => p.Value)
                .HasColumnName("TokenValue")
                .IsRequired();
        });
    }
}