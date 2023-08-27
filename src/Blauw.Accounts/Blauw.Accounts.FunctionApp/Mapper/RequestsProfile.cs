using AutoMapper;
using Blauw.Accounts.Abstractions.Models;
using Blauw.Accounts.FunctionApp.Dto;

namespace Blauw.Accounts.FunctionApp.Mapper;

public class RequestsProfile : Profile
{
    public RequestsProfile()
    {
        CreateMap<Account, SingleAccountDto>()
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id));
    }
}
