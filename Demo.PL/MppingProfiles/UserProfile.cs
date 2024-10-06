using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;

namespace Demo.PL.MppingProfiles
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserViewModel>().ReverseMap();
        }
    }
}
