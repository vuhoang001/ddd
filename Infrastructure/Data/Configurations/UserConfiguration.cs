using Domain.Entities.User;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email).IsRequired();


        builder.OwnsOne(x => x.Password, p =>
        {
            p.Property(x => x.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired();

            p.Property(x => x.Salt)
                .HasColumnName("PasswordSalt")
                .IsRequired();
        });
    }
}