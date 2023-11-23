using AutoMapper;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<GetTripDto, Trip>();
            CreateMap<Trip, GetTripDto>();
            // .ForMember(dest => dest.Name, act => act.MapFrom(src => src.TripName));
            CreateMap<GetUserTripDto, UserTrip>();
            CreateMap<UserTrip, GetUserTripDto>();
            CreateMap<CreateTripDto, Trip>();

            CreateMap<GetAccomodationTypeDto, AccomodationType>();
            CreateMap<AccomodationType, GetAccomodationTypeDto>();
            CreateMap<CreateAccomodationTypeDto, AccomodationType>();
            CreateMap<UpdateAccomodationTypeDto, AccomodationType>();
            CreateMap<AccomodationType, UpdateAccomodationTypeDto>();

            CreateMap<GetActivityTypeDto, ActivityType>();
            CreateMap<ActivityType, GetActivityTypeDto>();
            CreateMap<CreateActivityTypeDto, ActivityType>();
            CreateMap<PutActivityTypeDto, ActivityType>();
            CreateMap<GetActivityTypeDto, ActivityType>();
        }
    }
}
