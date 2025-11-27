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
    internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasOne(MS => MS.Plan)
                .WithMany(P => P.PlanMembers)
                .HasForeignKey(MS => MS.PlanId);

            builder.HasOne(MS=>MS.Member)
                .WithMany(M=>M.MemberShips)
                .HasForeignKey(M => M.MemberId);

            builder.HasKey(MS => new
            {
                MS.MemberId,
                MS.PlanId,
                
            });
            builder.Ignore(MS=>MS.Id);

            builder.Property(MS => MS.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");

        }
    }
}
