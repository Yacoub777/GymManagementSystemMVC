using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member: GymUser
    {
      //JoinDate == CreatedAt Of BaseEntity 
      public string Photo { get; set; } = null!;

        #region Relationship

        #region Member - HealthRecord 

        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion

        #region Member - MemberShips

        public ICollection<Membership> MemberShips { get; set; } = null!;

        #endregion

        #region Member - MemberSessions

        public ICollection<MemberSession> MemberSessions { get; set; } = null!;

        #endregion

         

        #endregion


    }
}
