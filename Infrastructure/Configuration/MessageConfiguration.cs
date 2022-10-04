using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            #region Table
            builder.HasKey(x => x.Id);
            #endregion

            #region Relations
            builder.HasOne(x => x.FromUser)
                    .WithMany(x => x.FromMessages)
                    .HasForeignKey(x => x.FromId);

            builder.HasOne(x => x.ToUser)
                    .WithMany(x => x.ToMessages)
                    .HasForeignKey(x => x.ToId);
            #endregion

            #region Properties
            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnOrder(1);

            builder.Property(s => s.Text)
                .HasColumnType("text")
                .HasMaxLength(4000)
                .HasColumnOrder(2);

            builder.Property(s => s.MessageStatus)
                .IsRequired()
                .HasColumnType("numeric")
                .HasPrecision(8)
                .HasColumnOrder(3);

            builder.Property(s => s.SendDate)
                .IsRequired()
                .HasColumnType("numeric")
                .HasColumnOrder(4);

            builder.Property(s => s.SendTime)
                .IsRequired()
                .HasColumnType("numeric")
                .HasColumnOrder(5);

            builder.Property(s => s.ReceivedDate)
                .HasColumnType("numeric")
                .HasColumnOrder(6);

            builder.Property(s => s.ReceivedTime)
                .HasColumnType("numeric")
                .HasColumnOrder(7);

            builder.Property(s => s.ReadDate)
                .HasColumnType("numeric")
                .HasColumnOrder(8);

            builder.Property(s => s.ReadTime)
                .HasColumnType("numeric")
                .HasColumnOrder(9);

            builder.Property(s => s.FromId)
                .IsRequired()
                .HasColumnOrder(10);

            builder.Property(s => s.ToId)
                .IsRequired()
                .HasColumnOrder(11);

            builder.Property(s => s.IsDeleted)
                .IsRequired()
                .HasColumnType("boolean")
                .HasColumnOrder(12);

            
            #endregion
        }
    }
}
