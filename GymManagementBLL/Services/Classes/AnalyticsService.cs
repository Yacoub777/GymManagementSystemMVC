using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = unitOfWork.GetRepository<Session>().GetAll();
            return new AnalyticsViewModel() { 
            ActiveMembers = unitOfWork.GetRepository<Membership>().GetAll(M=>M.Status == "Active").Count(),
            TotalMembers = unitOfWork.GetRepository<Member>().GetAll().Count(),
            TotalTrainers = unitOfWork.GetRepository<Trainer>().GetAll().Count(),
            UpcomingSessions = Sessions.Select(s=>s.StartDate >  DateTime.UtcNow).Count(),
            OngoingSessions = Sessions.Select(s=>s.StartDate <= DateTime.Now &&  s.EndDate >= DateTime.Now).Count(),
            CompletedSessions = Sessions.Select(s=>s.EndDate < DateTime.Now).Count()
            
            };
        }
    }
}
