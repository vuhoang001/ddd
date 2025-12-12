using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(x => x.Password, p =>
        {
            p.Property(x => x.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired();

            p.Property(x => x.Salt)
                .HasColumnName("PasswordSalt")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, p =>
        {
            p.Property(x => x.Value)
                .HasColumnName("Email")
                .IsRequired();
        });
    }
}