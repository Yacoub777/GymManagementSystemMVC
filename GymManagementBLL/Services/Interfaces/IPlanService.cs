using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public  interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlanById(int PlanId);

        UpdatePlanViewModel? GetPlanToUpdate (int PlanId);

        bool UpdatePlan(UpdatePlanViewModel planToUpdate , int PlanId);

        bool ToggleStatus(int PlanId);
    }
}
