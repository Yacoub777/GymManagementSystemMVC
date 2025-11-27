using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {

        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createMember);

        MemberViewModel? GetMemberDetails(int memberId);

        HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);

        bool UpdateMemberDetails(MemberToUpdateViewModel memberToUpdate , int memberId);

        bool RemoveMember(int memberId);

    }
}
