using AutoMapper;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using static ConferencesManagementAPI.Data.DTO.DelegatesDTO;

namespace ConferencesManagementAPI.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDelegatesDTO, Delegate>();
            CreateMap<AuthRequestDTO, Delegate>();
            CreateMap<AddDelegatesRequestDTO, Delegates>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.UseDestinationValue());
            CreateMap<UpdateDelegatesRequestDTO, Delegates>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.UseDestinationValue());

            CreateMap<Delegates, GetDelegatesResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.UseDestinationValue());


            CreateMap<ChangePasswordDTO, UpdateDelegatesRequestDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword));


            CreateMap<AddConferenceRequestDTO, Conference>();
            CreateMap<UpdateConferenceRequestDTO, Conference>();
            CreateMap<Conference, ConferenceResponseDTO>()
                .ForMember(dest => dest.HostById, opt => opt.MapFrom(src => src.HostBy))
                .ForMember(dest => dest.HostByName, opt => opt.MapFrom(src => src.HostByNavigation != null ? src.HostByNavigation.FullName : ""));

            CreateMap<Registration, RegistrationResponseDTO>();
            CreateMap<AddRegistrationRequestDTO, Registration>()
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateRegistrationRequestDTO, Registration>();

            CreateMap<AddDelegateConferenceRoleDTO, DelegateConferenceRole>();
            CreateMap<UpdateDelegateConferenceRoleDTO, DelegateConferenceRole>();


            CreateMap<ConferenceRole, ConferenceRoleResponseDTO>();
            CreateMap<AddConferenceRoleRequestDTO, ConferenceRole>();
            CreateMap<UpdateConferenceRoleRequestDTO, ConferenceRole>();

            CreateMap<DelegateConferenceRole, DelegateConferenceRoleResponseDTO>()
                .ForMember(dest => dest.DelegateName, opt => opt.MapFrom(src => src.Delegate.FullName))
                .ForMember(dest => dest.ConferenceName, opt => opt.MapFrom(src => src.Conference.Name))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<Registration, RegistrationResponseDTO>()
                .ForMember(dest => dest.DelegateName, opt => opt.MapFrom(src => src.Delegate != null ? src.Delegate.FullName : ""))
                .ForMember(dest => dest.ConferenceName, opt => opt.MapFrom(src => src.Conference != null ? src.Conference.Name : ""));

            CreateMap<CreateConferenceHostingRegistrationDTO, ConferenceHostingRegistration>();

            CreateMap<ConferenceHostingRegistration, ConferenceHostingRegistrationDTO>()
                .ForMember(dest => dest.RegisterName, opt => opt.MapFrom(src => src.Register == null ? "" : src.Register.FullName));


            // Mapping từ Entity sang DTO
            CreateMap<Delegates, DelegatesDTO>();
        }
    }
}
