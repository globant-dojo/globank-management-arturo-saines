using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.services
{
    public interface IClientesService
    {
        public Task<ClienteDTO> GetClienteById(long idCliente);

        public Task<ClienteDTO> GetClienteByCedula(string cedula);

        public Task<ClienteDTO> InsertNuevoCliente(ClienteDTO nuevoCliente);

        public Task<ClienteDTO> UpdateCliente(ClienteDTO cliente);
    }
}
