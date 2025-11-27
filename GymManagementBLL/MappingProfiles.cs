using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            MapSession();

            MapPlan();

            MapMember();

            #region Trainer Mapping
            CreateMap<Trainer, TrainerViewModel>()
                 .ForMember(TVM => TVM.Specialties, Options => Options.MapFrom(T => T.Specialties.ToString()))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                 $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"
             ));

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(T => T.Address, op => op.MapFrom(CT => new Address()
                {
                    City = CT.City,
                    Street = CT.Street,
                    BuildingNumber = CT.BuildingNumber,
                }));





            CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.Specializations, opt => opt.MapFrom(src => src.Specialties))
                    .ReverseMap()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                    {
                        City = src.City,
                        Street = src.Street,
                        BuildingNumber = src.BuildingNumber
                    }))
                    .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specializations));



            #endregion



        }

        private void MapSession()
        {

            #region Session Mapping
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>()
            .ForMember(SV => SV.TrainerName, Options => Options.MapFrom(S => S.SessionTrainer.Name))
            .ForMember(SV => SV.CategoryName, Options => Options.MapFrom(S => S.SessionCategory.CategoryName))
            .ForMember(SV => SV.AvailableSlots, Options => Options.Ignore());
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dist => dist.Name, Options => Options.MapFrom(s => s.CategoryName));
            #endregion
        }

        private void MapPlan()
        {
            #region Plan Mapping

            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(P => P.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            #endregion

        }

        private void MapMember()
        {
            #region Member Mapping

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    BuildingNumber = src.BuildingNumber
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(MVM => MVM.Gender, Options => Options.MapFrom(MVM => MVM.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));


            CreateMap<Member, MemberToUpdateViewModel>()
                  .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                  .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                  .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<MemberToUpdateViewModel, Member>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
          .ForMember(dest => dest.Photo, opt => opt.Ignore())
          .AfterMap((src, dest) =>
          {
              dest.Address.BuildingNumber = src.BuildingNumber;
              dest.Address.City = src.City;
              dest.Address.Street = src.Street;
              dest.UpdatedAt = DateTime.Now;
          });

            #endregion
        }



    }
}
