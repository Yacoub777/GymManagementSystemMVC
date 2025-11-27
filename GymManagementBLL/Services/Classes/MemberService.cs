using AutoMapper;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Data.Repositories.Classes;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._attachmentService = attachmentService;
        }

        public bool CreateMember(CreateMemberViewModel CreateMember)
        {
           

                if (IsPoneExist(CreateMember.Phone) || IsEmailExist(CreateMember.Email))
                    return false;
            try
            {
                var PhotoName = _attachmentService.Upload("members", CreateMember.PhotoFile);
                if (PhotoName is null) return false;


               
                var member = _mapper.Map<CreateMemberViewModel , Member>(CreateMember);
                 _unitOfWork.GetRepository<Member>().Add(member);
                member.Photo = PhotoName;
                var isCreated =  _unitOfWork.SaveChanges()>0;
                if (!isCreated)
                {
                    _attachmentService.Delete(PhotoName, "members");
                    return isCreated;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            } 

        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {

            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any())
            {
                return [];
            }

            #region Manual Mapping
            //var MemberViewModels = new List<MemberViewModels>();

            //foreach (var Member in Members)
            //{
            //    var memberViewModel = new MemberViewModels()
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Email = Member.Email,
            //        Phone = Member.Phone,
            //        Photo = Member.Photo,
            //        Gender = Member.Gender.ToString()
            //    };
            //    MemberViewModels.Add(memberViewModel);
            //}
            //return MemberViewModels; 
            #endregion

            #region Way02
            //    var MemberViewModels = Members.Select(Member => new MemberViewModels
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Email = Member.Email,
            //        Phone = Member.Phone,
            //        Photo = Member.Photo,
            //        Gender = Member.Gender.ToString()
            //    });


            #endregion
            //return MemberViewModels;
            return _mapper.Map<IEnumerable<Member> , IEnumerable<MemberViewModel>>(Members);
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (memberHealthRecord is null) return null;

            //return new HealthRecordViewModel() { 
            //Height = memberHealthRecord.Height,
            //Weight = memberHealthRecord.Weight,
            //Note = memberHealthRecord.Note,
            
            //};
            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
           var Member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (Member is null) return null;


            var ViewModel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Photo = Member.Photo,
                Gender = Member.Gender.ToString(),
                Address = $"{Member.Address.BuildingNumber} - {Member.Address.Street} - {Member.Address.City}",
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
            };

            //Active Membership

            var ActiveMembership = _unitOfWork.GetRepository<Membership>().GetAll(M=>M.MemberId == memberId&& M.Status == "Active").FirstOrDefault();
            if (ActiveMembership is not null)
            {

            ViewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
            ViewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();

            var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);

            ViewModel.PlanName = Plan?.Name;

            }

            return ViewModel;


        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            //return new MemberToUpdateViewModel()
            //{
            //    Name= member.Name,
            //    Email = member.Email,
            //    Phone = member.Phone,
            //    Photo = member.Photo,
            //    City = member.Address.City,
            //    BuildingNumber = member.Address.BuildingNumber,
            //    Street = member.Address.Street,
                
            //};

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(MemberToUpdateViewModel memberToUpdate, int memberId)
        {
            try
            {
                var EmailExist = _unitOfWork.GetRepository<Member>().GetAll(M => M.Id != memberId && M.Email == memberToUpdate.Email);
                var PhoneExist = _unitOfWork.GetRepository<Member>().GetAll(M => M.Id != memberId && M.Phone == memberToUpdate.Phone);
                if (EmailExist.Any() || PhoneExist.Any())
                    return false;

                var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
                if (member is null) return false;
                //member.Email = memberToUpdate.Email;
                //member.Phone = memberToUpdate.Phone;
                //member.Address.City = memberToUpdate.City;
                //member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                //member.Address.Street = memberToUpdate.Street;
                _mapper.Map(memberToUpdate,member);
                member.UpdatedAt = DateTime.Now;

                _unitOfWork.GetRepository<Member>().Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Member Failed: {ex}");
                return false;
            }

        }

        public bool RemoveMember(int memberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();

            var member = MemberRepo.GetById(memberId);
            if (member is null) return false;

            var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(MS=> MS.MemberId == memberId).Select(X=>X.SessionId);

            var HasFutureSession = _unitOfWork.GetRepository<Session>().GetAll(
                X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any()
                ;


            if (HasFutureSession) return false;

            var MembershipRepo = _unitOfWork.GetRepository<Membership>();
            var memberships = MembershipRepo.GetAll(MS=>MS.MemberId == memberId);
            try
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships)
                    {
                        MembershipRepo.Delete(membership);
                    }
                }
                 MemberRepo.Delete(member);
                var isDeleted =  _unitOfWork.SaveChanges() > 0;
                if (isDeleted)
                {
                    _attachmentService.Delete(member.Photo, "members");
                }
                return isDeleted;
            }
            catch (Exception)
            {

                return false;
            }
                


        }
        
        
        #region Helper Methods

        private bool IsEmailExist(string email) {

            var emailExist = _unitOfWork.GetRepository<Member>().GetAll(M => M.Email ==email).Any();
            return emailExist;
        }
        private bool IsPoneExist(string phone) {

            var phoneExist = _unitOfWork.GetRepository<Member>().GetAll(M => M.Phone ==phone).Any();
            return phoneExist;
        }


        #endregion
    }
}
