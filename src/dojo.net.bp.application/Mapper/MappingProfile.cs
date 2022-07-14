using AutoMapper;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<ClienteDTO, Cliente>().ReverseMap();
            CreateMap<CuentaDTO, Cuenta>().ReverseMap();
            CreateMap<MovimientoDTO, Movimiento>().ReverseMap();
            CreateMap<InsertCuentaRequestDTO, Cuenta>().ReverseMap();
            CreateMap<UpdateCuentaRequestDTO, Cuenta>().ReverseMap();
            CreateMap<DeleteCuentaRequestDTO, Cuenta>().ReverseMap();
            CreateMap<Cliente, Cliente>().ReverseMap();
            CreateMap<Cuenta, Cuenta>().ReverseMap();
            CreateMap<Movimiento, Movimiento>().ReverseMap();
            CreateMap<ConsultaMovimientoRequestDTO, Movimiento>().ReverseMap();
            CreateMap<InsertMovimientoRequestDTO, Movimiento>().ReverseMap();

        }
    }
}
