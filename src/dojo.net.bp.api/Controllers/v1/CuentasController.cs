using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.models.dtos;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojo.net.bp.api.Controllers.v1
{
    public class CuentasController : BaseApiController
    {
        private readonly ICuentasService _cuentasService;

        public CuentasController(ICuentasService cuentasService)
        {
            _cuentasService = cuentasService;
        }

        [HttpGet]
        [Route("api-transacciones/v1/cuentas/{numCuenta}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CuentaDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CuentaDTO>>> GetClienteById([FromRoute] string numCuenta)
        {
            var clienteDTO = await _cuentasService.GetCuentaByNumCta(numCuenta);

            return Ok(new MsDtoResponse<CuentaDTO>(clienteDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        [HttpPost]
        [Route("api-transacciones/v1/cuentas/")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CuentaDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CuentaDTO>>> InsertNuevaCuenta([FromBody] InsertCuentaRequestDTO nuevaCuenta)
        {
            var clienteDTO = await _cuentasService.InsertNuevaCuenta(nuevaCuenta);

            return Ok(new MsDtoResponse<CuentaDTO>(clienteDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPatch]
        [Route("api-transacciones/v1/cuentas/")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CuentaDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CuentaDTO>>> UpdateCuenta([FromBody] UpdateCuentaRequestDTO cuenta)
        {
            var clienteDTO = await _cuentasService.UpdateCuenta(cuenta);

            return Ok(new MsDtoResponse<CuentaDTO>(clienteDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpDelete]
        [Route("api-transacciones/v1/cuentas/")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<string>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<string>>> DeleteCuenta([FromBody] DeleteCuentaRequestDTO cuenta)
        {
            var op = await _cuentasService.DeleteCuenta(cuenta);

            return Ok(new MsDtoResponse<string>($"Cuenta {cuenta.NumeroCuenta} - {cuenta.TipoCuenta.ToUpper()} eliminada correctamente", HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
    }
}
