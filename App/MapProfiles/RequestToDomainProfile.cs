using Banking.Contracts.Response.Deposit;
using AutoMapper;
using GODP.Entities.Models; 

namespace Banking.MapProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<AddUpdateAccountTypeObj, deposit_accountype>();
        }
    }
}
