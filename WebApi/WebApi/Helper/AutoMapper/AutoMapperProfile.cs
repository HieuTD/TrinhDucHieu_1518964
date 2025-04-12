using AutoMapper;
using WebApi.DTOs.Users;
using WebApi.Models;

namespace WebApi.Helper.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UserCreateRequest, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email)).ReverseMap();
        }
    }
}
