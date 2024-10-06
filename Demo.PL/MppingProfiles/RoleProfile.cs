using AutoMapper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.MppingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile() {
            CreateMap<IdentityRole, RoleViewModel>().ForMember(d => d.RoleName, O => O.MapFrom(S=>S.Name)).ReverseMap();
          //  CreateMap<RoleViewModel, IdentityRole>().ForMember(d => d.Name, O => O.MapFrom(S => S.RoleName));
        }
    }
}
