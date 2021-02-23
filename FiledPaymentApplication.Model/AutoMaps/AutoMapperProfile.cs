using AutoMapper;
using FiledPaymentApplication.DTO;

namespace FiledPaymentApplication.Model.AutoMaps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PaymentToCreateDTO, Payment>();
        }
    }
}
