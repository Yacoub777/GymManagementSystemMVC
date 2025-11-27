using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
        [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            this._memberService = memberService;
        }
        #region Get All Members
        public ActionResult Index()
        {

            var members = _memberService.GetAllMembers();
            return View(members);
        } 
        #endregion

        #region Get Member Data
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);

        }

        #endregion

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);

        }

        #region Create Member

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), createdModel);
            }
            var Result = _memberService.CreateMember(createdModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            { 
                TempData["ErrorMessage"] = "Member Failed To Create , Check Email and Phone Number";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Edit Member
        public ActionResult MemberEdit(int id)
        {

            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);

        }

        [HttpPost]
        public ActionResult MemberEdit(MemberToUpdateViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            bool Result = _memberService.UpdateMemberDetails(model, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Update ";
            }
            return RedirectToAction(nameof(Index));


        }
        #endregion


        #region Delete Member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Can Not Be Negative Or Zero";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Can Not Deleted";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
