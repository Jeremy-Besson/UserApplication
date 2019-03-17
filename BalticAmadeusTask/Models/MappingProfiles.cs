using AutoMapper;

namespace BalticAmadeusTask.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisteredUser, RegisteredUserD>().ReverseMap();
        }
    }
}
