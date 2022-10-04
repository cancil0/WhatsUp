using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            #region Table
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.MobilePhone);
            #endregion

            #region Relations
            #endregion

            #region Properties
            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnOrder(1);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .HasColumnOrder(2);

            builder.Property(s => s.Surname)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .HasColumnOrder(3);

            builder.Property(s => s.UserName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .HasColumnOrder(4);

            builder.Property(s => s.MobilePhone)
                .IsRequired()
                .HasColumnType("varchar")
                .HasColumnOrder(5);

            builder.Property(s => s.BirthDate)
                .IsRequired()
                .HasColumnType("numeric")
                .HasPrecision(8)
                .HasColumnOrder(6);

            builder.Property(s => s.IsDeleted)
                .IsRequired()
                .HasColumnType("boolean")
                .HasColumnOrder(7);

            builder.Property(s => s.CreatedDate)
                .IsRequired()
                .HasColumnType("numeric")
                .HasColumnOrder(8);

            builder.Property(s => s.CreatedTime)
                .IsRequired()
                .HasColumnType("numeric")
                .HasColumnOrder(9);

            builder.Property(s => s.CreatedBy)
                .HasColumnType("varchar")
                .HasColumnOrder(10);

            builder.Property(s => s.UpdatedDate)
                .HasColumnType("numeric")
                .HasColumnOrder(11);

            builder.Property(s => s.UpdatedTime)
                .HasColumnType("numeric")
                .HasColumnOrder(12);

            builder.Property(s => s.UpdatedBy)
                .HasColumnType("varchar")
                .HasColumnOrder(13);
            #endregion
        }
    }
}
