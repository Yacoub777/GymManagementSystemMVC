using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        public IEnumerable<SessionViewModel> GetAllSessions();

        public SessionViewModel? GetSessionById(int sessionId);

        public bool CreateSession(CreateSessionViewModel createSessionViewModel);

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        public bool UpdateSession(UpdateSessionViewModel updateSessionViewModel , int sessionId);

        public bool RemoveSession(int sessionId);

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown();
        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown();

    }
}
