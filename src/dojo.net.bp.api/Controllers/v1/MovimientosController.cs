using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.models.dtos;
using dojo.net.bp.domain.entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojo.net.bp.api.Controllers.v1
{
    public class MovimientosController : BaseApiController
    {
        private readonly IMovimientosService _movimientosService;

        public MovimientosController(IMovimientosService movimientosService)
        {
            _movimientosService = movimientosService;
        }



        [HttpGet]
        [Route("api-transacciones/v1/movimientos")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<List<MovimientoDTO>>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<List<MovimientoDTO>>>> ConsultarMovimientos([FromBody] ConsultaMovimientoRequestDTO consultaMovimientos)
        {
            var movsDTO = await _movimientosService.GetMovimientosByNumCta_RangoFechas(consultaMovimientos);

            return Ok(new MsDtoResponse<List<MovimientoDTO>>(movsDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPost]
        [Route("api-transacciones/v1/movimientos/retiro")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<MovimientoDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<MovimientoDTO>>> RetirarFondosCuenta([FromBody] InsertMovimientoRequestDTO datosRetiro)
        {
            var movDTO = await _movimientosService.InsertNuevoMovimientoDebito(datosRetiro);

            return Ok(new MsDtoResponse<MovimientoDTO>(movDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPost]
        [Route("api-transacciones/v1/movimientos/credito")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<MovimientoDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<MovimientoDTO>>> AcreditarFondosCuenta([FromBody] InsertMovimientoRequestDTO datosCredito)
        {
            var movDTO = await _movimientosService.InsertNuevoMovimientoCredito(datosCredito);

            return Ok(new MsDtoResponse<MovimientoDTO>(movDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        [HttpDelete]
        [Route("api-transacciones/v1/movimientos")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<string>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<string>>> ReversarMovimiento([FromBody] ReversoMovimientoRequestDTO datosMovimiento)
        {
            var movDTO = await _movimientosService.ReversarMovimiento(datosMovimiento);

            return Ok(new MsDtoResponse<string>($"Movimiento {datosMovimiento.MovimientoId} fue reversado correctamente", HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

    }
}
