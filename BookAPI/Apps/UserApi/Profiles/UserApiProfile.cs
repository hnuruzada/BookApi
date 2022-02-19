using AutoMapper;
using BookAPI.Apps.UserApi.DTOs.AccountDtos;
using BookAPI.Data.Entity;

namespace BookAPI.Apps.UserApi.Profiles
{
    public class UserApiProfile:Profile
    {
        public UserApiProfile()
        {
            CreateMap<AppUser,AccountGetDto>();
        }
    }

}
