using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.services
{
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository _reportesRepository;

        public ReportesService(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<object> ConsultarEstadoCuenta(ConsultaEstadoCuentaRequestDTO datosConsulta)
        {
            return await _reportesRepository.ConsultarEstadoCuenta(datosConsulta);
        }
    }
}
