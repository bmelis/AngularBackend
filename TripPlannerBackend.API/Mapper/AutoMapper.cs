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

            CreateMap<GetActivityDto, Activity>();
            CreateMap<Activity, GetActivityDto>();
            CreateMap<CreateActivityDto, Activity>();

            CreateMap<GetActivityTypeDto, ActivityType>();
            CreateMap<ActivityType, GetActivityTypeDto>();
            CreateMap<CreateActivityTypeDto, ActivityType>();
            CreateMap<UpdateActivityTypeDto, ActivityType>();

            CreateMap<GetDestinationDto, Destination>();
            CreateMap<Destination, GetDestinationDto>();
            CreateMap<CreateDestinationDto, Destination>();

            CreateMap<GetAccommodationDto, Accommodation>();
            CreateMap<Accommodation, GetAccommodationDto>();
            CreateMap<CreateAccommodationDto, Accommodation>();

            CreateMap<GetAccommodationTypeDto, AccommodationType>();
            CreateMap<AccommodationType, GetAccommodationTypeDto>();
            CreateMap<CreateAccommodationTypeDto, AccommodationType>();
            CreateMap<UpdateAccommodationTypeDto, AccommodationType>();
        }
    }
}
