using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using DAL.Models;
namespace BLL.Profiles
{
    class RefreshTokenProfile: Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDTO>();

            CreateMap<RefreshTokenDTO, RefreshToken>();
        }
    }
}
