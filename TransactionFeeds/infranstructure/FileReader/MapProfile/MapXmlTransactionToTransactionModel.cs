using Abstracts.ModelBase;
using AutoMapper;
using infranstructure.FileReader.Dto;
using System;
using System.Globalization;

namespace infranstructure.FileReader.MapProfile
{
    public class MapXmlTransactionToTransactionModel : Profile
    {
        public MapXmlTransactionToTransactionModel() : this("DefaultProfile")
        {

        }
        public MapXmlTransactionToTransactionModel(string profilename) : base(profilename)
        {
            CreateMap<XmlTransaction, TransactionModel>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => decimal.Parse(src.Details.Amount)))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Details.CurrencyCode))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == "Approved" ? TransactionStatus.A :
                                                                        src.Status == "Rejected" ? TransactionStatus.R :
                                                                        TransactionStatus.D))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src =>
                DateTime.ParseExact(src.TransactionDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
                ))
                .ForMember(dest => dest.TransactionIdentificator, opt => opt.MapFrom(src => src.TransactionID))
                ;
        }


    }
}
