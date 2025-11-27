using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService) {
            this._trainerService = trainerService;
        }
        #region  Get All Members
        public IActionResult Index()
        {
            var Trainers = _trainerService.GetAllTrainers();
            return View(Trainers);
        }

        #endregion

        #region Get Member Details
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(Trainer);

        }

        #endregion

        #region Create Trainer

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel model ) {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                    return RedirectToAction(nameof(Create) , model);
            }
            var Result = _trainerService.CreateTrainer(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Can't Creating";
            }
            return RedirectToAction(nameof(Index));
        
        }

        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id) {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerToUpdate(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));

            }

            return View(Trainer);
        }
        [HttpPost]
        public ActionResult Edit(UpdateTrainerViewModel model , [FromRoute]int id)
        {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(model);
                
            }
            var Result = _trainerService.UpdateTrainerDetails(model,id);
            if (Result) {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Update";
            }
            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Delete Trainer

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));

            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = Trainer.Name;
            return View();
        }

        public ActionResult DeleteConfirmed(int id)
        {
           var Result = _trainerService.DeleteTrainer(id);
            if (Result) 
                {
                    TempData["SuccessMessage"] = "Trainer Deleted Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Trainer Can Not Deleted";
                }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
