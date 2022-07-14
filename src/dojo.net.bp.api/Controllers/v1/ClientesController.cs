using AutoMapper;
using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.models.dtos;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojo.net.bp.api.Controllers.v1
{
    public class ClientesController : BaseApiController
    {
        private readonly IClientesService _clientesServices;

        public ClientesController(IClientesService clientesServices)
        {
            _clientesServices = clientesServices;
        }


        [HttpGet]
        [Route("api-transacciones/v1/clientes/{cedula}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ClienteDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ClienteDTO>>> GetClienteById([FromRoute]string cedula)
        {
            var clienteDTO = await _clientesServices.GetClienteByCedula(cedula);

            return Ok(new MsDtoResponse<ClienteDTO>(clienteDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPost]
        [Route("api-transacciones/v1/clientes")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ClienteDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ClienteDTO>>> InsertNuevoCliente([FromBody] ClienteDTO nuevoCliente)
        {
            var clienteDTO = await _clientesServices.InsertNuevoCliente(nuevoCliente);

            return Ok(new MsDtoResponse<ClienteDTO>(clienteDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPatch]
        [Route("api-transacciones/v1/clientes")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ClienteDTO>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ClienteDTO>>> UpdateCliente([FromBody] ClienteDTO nuevoCliente)
        {
            var clienteDTO = await _clientesServices.UpdateCliente(nuevoCliente);

            return Ok(new MsDtoResponse<ClienteDTO>(clienteDTO, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
    }
}
