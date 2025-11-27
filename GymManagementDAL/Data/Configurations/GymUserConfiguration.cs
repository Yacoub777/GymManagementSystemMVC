using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(T => T.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(T=>T.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.Property(T => T.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);


            builder
                .HasIndex(T => T.Email)
                .IsUnique();

            builder
                .HasIndex(T => T.Phone)
                .IsUnique();

            builder.OwnsOne(T => T.Address, AB =>
            {
                AB.Property(T=>T.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AB.Property(T => T.City)
                .HasColumnName("City")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AB.Property(T => T.BuildingNumber)
                .HasColumnName("BuildingNumber");

                builder.ToTable
                    (T => T.HasCheckConstraint($"CK_{typeof(T).Name}_ValidEmail", "Email LIKE '_%@_%._%'"));
                builder.ToTable
                    (T => T.HasCheckConstraint($"CK_{typeof(T).Name}_ValidPhone", "Phone Like '01_________' and Phone Not Like '%[^0-9]%'"));

            });

        }
    }

}
