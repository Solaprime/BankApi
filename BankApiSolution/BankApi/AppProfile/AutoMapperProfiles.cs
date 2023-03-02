using AutoMapper;
using BankApi.Entities;
using BankApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.AppProfile
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<RegisterAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap< Account, GetAccountModel>();
            CreateMap<TransactionRequestModel, Transaction>();
        }
        
    }
}
