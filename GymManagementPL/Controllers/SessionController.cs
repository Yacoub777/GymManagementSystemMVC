using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService) 
        {
            this._sessionService = sessionService;
        }
        #region Get All Sessions
        public ActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        #endregion


        #region Get Session By Id
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        #endregion


        #region Create Session

        public ActionResult Create()
        {
            LoadTrainerDropdown();
            LoadCategoryDropdown();

            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                LoadTrainerDropdown();
                LoadCategoryDropdown();
                return View(model);
            }
            var Result = _sessionService.CreateSession(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Create";
                LoadTrainerDropdown();
                LoadCategoryDropdown();
                return View(model);
            }
        }


        #endregion

        #region Edit Session
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Can't Be Update";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainerDropdown();
            return View(session);


        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                LoadTrainerDropdown();
                return View(model);

            }
            var Result = _sessionService.UpdateSession(model, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = session.Id;
            return View(session);

        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromRoute]int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SessionMessage"] = "Session Deleted";

            }
            else
            {
                TempData["ErrorMessage"] = "Session Can't Be Deleted";
            }
            return RedirectToAction(nameof(Index));
        }
        #region HelperMethods

        private void LoadTrainerDropdown()
        {
            var Trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");

        }
        private void LoadCategoryDropdown()
        {
            var Categories = _sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

        }


        #endregion
    }
}
