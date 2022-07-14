using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.models.dtos;
using dojo.net.bp.domain.entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojo.net.bp.api.Controllers.v1
{
    public class ReportesController : BaseApiController
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }




        [HttpGet]
        [Route("api-transacciones/v1/ConsultaEstadoCuenta")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<object>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<object>>> ConsultaEstadoCuenta([FromBody] ConsultaEstadoCuentaRequestDTO datosConsulta)
        {
            var datosRes = await _reportesService.ConsultarEstadoCuenta(datosConsulta);

            return Ok(new MsDtoResponse<object>(datosRes, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
    }
}
