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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.
                ToTable(S=>S.HasCheckConstraint("CK_Session_CheckCapacity", "[Capacity] BETWEEN 1 AND 25"));
             
             builder
                .ToTable(S=>S.HasCheckConstraint("CK_Session_EndDateAfterStartDate", "EndDate > StartDate"));

            builder.HasOne(S => S.SessionCategory)
                    .WithMany(C => C.Sessions)
                    .HasForeignKey(S => S.CategoryId);

            builder.HasOne(S=>S.SessionTrainer)
                .WithMany(T=>T.TrainerSessions)
                .HasForeignKey(S => S.TrainerId);
        }
    }
}
