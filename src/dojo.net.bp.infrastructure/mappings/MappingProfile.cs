using AutoMapper;
using dojo.net.bp.infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dojo.net.bp.infrastructure.mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {


            //CreateMap<XmlElement, OtpGenerarResponse>()
            //   .ForMember(dest =>
            //   dest.OtpEncriptado,
            //   opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "OTPgenerationResult")))

            //  .ForMember(dest =>
            //  dest.CodigoRetorno,
            //  opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "cretorno")))

            //  .ForMember(dest =>
            //  dest.Mensaje,
            //  opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "err")))
            //   .ReverseMap();

            //CreateMap<XmlElement, OtpValidarResponse>()
            //.ForMember(dest =>
            //dest.CodigoRetorno,
            //opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "cretorno")))
            //.ForMember(dest =>
            //dest.Mensaje,
            //opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "err")))
            // .ReverseMap();

        }

    }
}
