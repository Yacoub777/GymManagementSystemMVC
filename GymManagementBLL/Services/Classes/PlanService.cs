using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
         var Plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (!Plans.Any()) return [];

            return _mapper.Map<IEnumerable<Plan>,IEnumerable<PlanViewModel>>(Plans);
            
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null) return null;
            return _mapper.Map<Plan,PlanViewModel>(Plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null  || Plan.IsActive == false || HasActiveMemberships(PlanId)) return null;
            return _mapper.Map<Plan,UpdatePlanViewModel>(Plan);
            
        }
        public bool UpdatePlan(UpdatePlanViewModel planToUpdate, int PlanId)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(PlanId);
            if (plan is null || HasActiveMemberships(PlanId)) return false;

            try
            {
                _mapper.Map(planToUpdate, plan);
                planRepo.Update(plan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool ToggleStatus(int planId)
        {
          var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || HasActiveMemberships(planId)) return false;

            if (plan.IsActive)
                plan.IsActive = false;
            else
                plan.IsActive = true;
            plan.UpdatedAt = DateTime.Now;
            try
            {
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }



        #region Helper

        private bool HasActiveMemberships(int PlanId)
        {
            var activeMemberShips = _unitOfWork.GetRepository<Membership>().GetAll(MS=> MS.PlanId == PlanId &&
            MS.Status == "Active");
            return activeMemberShips.Any();

        }

        #endregion
    }
}
