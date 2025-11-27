using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper) {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSessionViewModel)
        {
            //check Trainer Exist
            if(!IsTrainerExist(createSessionViewModel.TrainerId)) return false;
            //check Category Exist
            if(!IsCategoryExist(createSessionViewModel.CategoryId)) return false;
            //check StartDate < EndDate 
            if(!IsDateTimeValid(createSessionViewModel.StartDate, createSessionViewModel.EndDate)) return false;
            //check Capacity is between 1 and 25
            if(createSessionViewModel.Capacity > 25 || createSessionViewModel.Capacity <= 0) return false;

            try
            {
                var Session = _mapper.Map<CreateSessionViewModel , Session>(createSessionViewModel);
                _unitOfWork.SessionRepository.Add(Session);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed: {ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionAndTrainerAndCategory();
            if (!Sessions.Any()) return [];

            var viewSession = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in viewSession) {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);
            }
            return viewSession;
            
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (session is null) return null;

           var MappedSession = _mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(MappedSession.Id);
            return MappedSession;
        }


        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableForUpdating(session!)) return null;

            return _mapper.Map<Session,UpdateSessionViewModel>(session!);

        }

        public bool UpdateSession(UpdateSessionViewModel updateSessionViewModel, int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if(!IsSessionAvailableForUpdating( session!)) return false;
                if(!IsTrainerExist(updateSessionViewModel.TrainerId)) return false;
                if(!IsDateTimeValid(updateSessionViewModel.StartDate, updateSessionViewModel.EndDate)) return false;

                 _mapper.Map(updateSessionViewModel, session);

               session!.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Session>().Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Updated Session Failed: {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);

            try
            {
                if(!IsSessionAvailableForRemoving(session!)) return false;
                _unitOfWork.SessionRepository.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed: {ex}");
                return false;
            }

        }


        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainer);
             
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
           var category = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(category);
        }


        #region Helper Methods

        public bool IsSessionAvailableForUpdating(Session session)
        {
            if(session is null) return false;
            // Check Session Completed 
            if(session.EndDate <  DateTime.Now) return false;
            // Check Session Started
            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now) return false;
            // Check Has Active Booking
            bool HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;
        } 
        public bool IsSessionAvailableForRemoving(Session session)
        {
            if(session is null) return false;
            // Check Session Started
            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now) return false;
            // Check Session Upcoming
            if (session.StartDate > DateTime.Now) return false;
            // Check Has Active Booking
            bool HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;
        }

        public bool IsTrainerExist(int trainerId) {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }

        public bool IsCategoryExist(int categoryId) { 
        return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }

        public bool IsDateTimeValid(DateTime startDate, DateTime endDate) { 
        return startDate < endDate && startDate >= DateTime.Now;
        }


        #endregion
    }
}
