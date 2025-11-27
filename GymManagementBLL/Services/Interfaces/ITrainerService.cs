using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel model);

        TrainerViewModel? GetTrainerDetails(int trainerId);

        UpdateTrainerViewModel? GetTrainerToUpdate(int trainerId);

        bool UpdateTrainerDetails(UpdateTrainerViewModel model , int trainerId);

        bool DeleteTrainer(int trainerId);


    }

}
