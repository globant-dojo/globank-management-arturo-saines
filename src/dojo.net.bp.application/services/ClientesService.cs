using AutoMapper;
using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.models.exeptions;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.services
{
    public class ClientesService : IClientesService
    {
        private readonly IClientesContextRepository _clientesContextRepository;
        private readonly IMapper _mapper;

        public ClientesService(IClientesContextRepository clientesContextRepository, IMapper mapper)
        {
            _clientesContextRepository = clientesContextRepository;
            _mapper = mapper;
        }

        public async Task<ClienteDTO> GetClienteById(long idCliente)
        {
            ClienteDTO clte = new ClienteDTO();
            try
            {
                clte = await GetClienteBySelector(c => c.ClienteId == idCliente, "No se encontro ningun cliente con ese Id");
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message, "", "0001");
            }
            catch (Exception ex)
            {
                throw new APIException("Hubo un error al momento de obtener el Cliente por Id", ex.StackTrace,"9999");
            }

            return clte;
        }

        public async Task<ClienteDTO> GetClienteByCedula(string cedula)
        {
            ClienteDTO clte = new ClienteDTO();
            try
            {
                clte = await GetClienteBySelector(c => c.Identificacion == cedula, "No se encontro ningun cliente con esa cedula");
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message, "", "0001");
            }
            catch (Exception ex)
            {
                throw new APIException("Hubo un error al momento de obtener el Cliente por Id", ex.StackTrace, "9999");
            }

            return clte;
        }

        private async Task<ClienteDTO> GetClienteBySelector(Expression<Func<Cliente, bool>> selector, string defErrorMsg = "")
        {
            ClienteDTO clte = new ClienteDTO();
            var lstCltes = await _clientesContextRepository.GetClientesBySelectors(selector);
            if (lstCltes != null && lstCltes.Count() > 0)
            {
                clte = _mapper.Map<ClienteDTO>(lstCltes.FirstOrDefault());
                clte.Contrasena = ""; //Se limpia la contraseña
            }
            else
                throw new APIException(defErrorMsg, "", "9999");

            return clte;
        }

        public async Task<ClienteDTO> InsertNuevoCliente(ClienteDTO nuevoCliente)
        {
            ClienteDTO clteDto = new ClienteDTO();
            try
            {
                //Validar que no exista otro cliente con dicha identificacion
                var resBusq = await _clientesContextRepository.GetClientesBySelectors(p => p.Identificacion == nuevoCliente.Identificacion);
                if(resBusq != null && resBusq.Count()>0) throw new APIException("Cliente ya existe con dicha Identificacion");


                //Si no existe, insertar los datos
                Cliente clte = _mapper.Map<Cliente>(nuevoCliente);
                clte = await _clientesContextRepository.InsertCliente(clte);


                //Mapear los datos de la entidad al DTO
                if (clte != null) clteDto = _mapper.Map<ClienteDTO>(clte);
                else throw new APIException("No se pudo insertar el nuevo cliente");
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message, "", "0002");
            }
            catch (Exception ex)
            {
                throw new APIException("Hubo un error al momento de insertar los datos del nuevo cliente", ex.StackTrace, "9999");
            }

            return clteDto;
        }

        public async Task<ClienteDTO> UpdateCliente(ClienteDTO cliente)
        {
            ClienteDTO clteDto = new ClienteDTO();

            try
            {
                //1. Nos aseguramos de que el cliente exista. Caso contrario, se va por exception
                var res = await _clientesContextRepository.GetClientesBySelectors(p =>  p.ClienteId == cliente.ClienteId);
                if (res == null || res.Count() == 0) throw new APIException("Cliente no existe para actualizar");

                //2. Actuzalizamos los datos del cliente.
                var resClte = res.FirstOrDefault();
                resClte = _mapper.Map<Cliente>(cliente);
                resClte.PersonaId = res.FirstOrDefault().PersonaId;
                bool op = await _clientesContextRepository.UpdateCliente(resClte);


                //3. Devolvemos los datos del nuevo cliente.
                if (op)
                {
                    var seleccion = await _clientesContextRepository.GetClientesBySelectors(p => p.ClienteId == cliente.ClienteId);
                    if (seleccion != null && seleccion.Count() > 0) clteDto = _mapper.Map<ClienteDTO>(seleccion.FirstOrDefault());
                }
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message, "", "0003");
            }
            catch (Exception ex)
            {
                throw new APIException("Hubo un error al momento de actualizar los datos del cliente", ex.StackTrace, "9999");
            }

            return clteDto;
        }
    }
}
