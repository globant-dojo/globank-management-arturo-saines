using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.services
{
    public interface IMovimientosService
    {
        public Task<List<MovimientoDTO>> GetMovimientosByNumCta_RangoFechas(ConsultaMovimientoRequestDTO movimiento);
        public Task<MovimientoDTO> InsertNuevoMovimientoCredito(InsertMovimientoRequestDTO movimiento);
        public Task<MovimientoDTO> InsertNuevoMovimientoDebito(InsertMovimientoRequestDTO movimiento);
        public Task<bool> ReversarMovimiento(ReversoMovimientoRequestDTO movimiento);
    }
}
