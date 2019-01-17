#region --Using--
using AutoMapper;
using Core.Entities;
using System.Linq;
using WebAPI.DTO;
#endregion

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDTO>()
                .ForMember(m => m.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(_ => _.IsMainPhoto).Url);
                })
                .ForMember(m => m.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<User, UserForDetailsDTO>()
                .ForMember(m => m.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(_ => _.IsMainPhoto).Url);
                })
                .ForMember(m => m.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });

            CreateMap<Photo, PhotosForUserDetailsDTO>();
            CreateMap<UserForUpdateDTO, User>();

        }
    }
}
