using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.repositories
{
    public interface IReportesRepository
    {
        public Task<object> ConsultarEstadoCuenta(ConsultaEstadoCuentaRequestDTO datosConsulta);
    }
}
