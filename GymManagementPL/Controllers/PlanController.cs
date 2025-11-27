using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            this._planService = planService;
        }
        #region Get All Plans
        public ActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        #endregion

        #region Get Plan Details

        public ActionResult Details(int id) {
            if (id <= 0) {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        
        }

        #endregion

        #region Edit Plan


        // Get : Edit

        // Can update Only Active , Has Not MemberShip and Not Null
        public ActionResult Edit(int id) {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction("Index");
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can't Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);

        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(model);
            }
            var Result = _planService.UpdatePlan(model,id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed Updated";
            }
            return RedirectToAction(nameof(Index));

        }


        #endregion


        #region Toggle Plan

        public ActionResult Activate(int id) { 
            
            var Result = _planService.ToggleStatus(id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed";

            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Change Plan Status";

            }
            return RedirectToAction(nameof(Index));
        
        }
        #endregion


    }
}
