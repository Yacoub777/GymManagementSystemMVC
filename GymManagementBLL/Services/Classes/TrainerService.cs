using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var allTrainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (allTrainers is null ||  !allTrainers.Any()) return []; 

           return _mapper.Map<IEnumerable<Trainer>,IEnumerable<TrainerViewModel>>(allTrainers);
            

        }

        public bool CreateTrainer(CreateTrainerViewModel model)
        {
            if (HasExistEmail(model.Email) || HasExistPhone(model.Phone))
                return false;

            try
            {
                //Trainer trainer = new Trainer()
                //{
                //    Phone = model.Phone,
                //    Name = model.Name,
                //    Email = model.Email,
                //    Address = new Address()
                //    {
                //        City = model.City,
                //        BuildingNumber = model.BuildingNumber,
                //        Street = model.Street,
                //    },
                //    DateOfBirth = model.DateOfBirth,
                //    Gender = model.Gender,
                //    Specialties = model.Specializations,

                //};
                Trainer trainer = _mapper.Map<Trainer>(model);

                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            //return new TrainerViewModel()
            //{
            //    Name = trainer.Name,
            //    Phone = trainer.Phone,
            //    Email = trainer.Email,
            //    Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
            //    Specialization = trainer.Specialties.ToString(),
                
            //};
            return _mapper.Map<Trainer, TrainerViewModel>(trainer);
        }

        public UpdateTrainerViewModel? GetTrainerToUpdate(int trainerId)
        {
            
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            //return new UpdateTrainerViewModel() 
            //{
            //    Name= trainer.Name,
            //    EmailAddress = trainer.Email,
            //    PhoneNumber = trainer.Phone,
            //    BuildingNumber = trainer.Address.BuildingNumber,
            //    City = trainer.Address.City,
            //    Street = trainer.Address.Street,
            //    Specializations = trainer.Specialties,
            //};
            return _mapper.Map<UpdateTrainerViewModel>(trainer);
            

        }

        public bool UpdateTrainerDetails(UpdateTrainerViewModel model, int trainerId)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainerUpdated = trainerRepo.GetById(trainerId);
            if (trainerUpdated is null) return false;

            var EmailExist = trainerRepo.GetAll(T => T.Email == model.EmailAddress && T.Id != trainerId).Any();
            var PhoneExist = trainerRepo.GetAll(T => T.Phone == model.PhoneNumber && T.Id != trainerId).Any();

            if (EmailExist || PhoneExist)
                return false;

            try
            {
                //trainerUpdated.Email = model.EmailAddress;
                //trainerUpdated.Phone = model.PhoneNumber;
                //trainerUpdated.Address.BuildingNumber = model.BuildingNumber;
                //trainerUpdated.Address.City = model.City;
                //trainerUpdated.Address.Street = model.Street;
                //trainerUpdated.Specialties = model.Specializations;
                _mapper.Map(model , trainerUpdated);
                trainerUpdated.UpdatedAt = DateTime.Now;

                trainerRepo.Update(trainerUpdated);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Updated Trainer Failed: {ex} ");
                return false;
            }
        }
        public bool DeleteTrainer(int trainerId)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();


            var trainer = trainerRepo.GetById(trainerId);
            if (trainer == null) return false;

            var futureSessions = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now).Any();
            if (futureSessions) return false;
            try
            {

                trainerRepo.Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception ex)
            {

                Console.WriteLine($"Delete Trainer Failed: {ex}");
                return false;
            }
        }

        #region Helper

        private bool HasExistEmail(string email) {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T=>T.Email == email).Any();
        
        }
        private bool HasExistPhone(string phone) {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T=>T.Email == phone).Any();
        
        }

        #endregion
    }
}
