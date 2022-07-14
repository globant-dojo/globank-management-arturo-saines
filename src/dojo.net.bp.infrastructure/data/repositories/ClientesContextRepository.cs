using AutoMapper;
using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.models.exeptions;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.infrastructure.data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.infrastructure.data.repositories
{
    public class ClientesContextRepository : IClientesContextRepository
    {
        private readonly IDbContextFactory<BP_DBContext> _dbContextFactory;
        private readonly IPersonasContextRepository _personasContextRepository;
        private readonly IMapper _mapper;

        public ClientesContextRepository(IDbContextFactory<BP_DBContext> dbContextFactory,
            IPersonasContextRepository personasContextRepository, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _personasContextRepository = personasContextRepository;
            _mapper = mapper;
        }




        public async Task<List<Cliente>> GetClientesBySelectors(Expression<Func<Cliente, bool>> query)
        {
            List<Cliente> op = new List<Cliente>();
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    op = await _dbContext.Set<Cliente>().Where(query).ToListAsync();
                    if (op != null && op.Count > 0)
                        op.ForEach(x => x.Contrasena = "");
                }

            }

            return op;
        }

        public async Task<Cliente> InsertCliente(Cliente cliente)
        {
            Cliente op = new Cliente();
            

            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    Persona persona = (Persona)cliente;
                    try
                    {
                        //We have to insert the Persona data
                        persona = await _personasContextRepository.InsertPersona(persona);
                    }
                    catch (Exception ex)
                    {
                        throw new APIException("Error al momento de insertar los datos del nuevo cliente", ex.Message, "9999");
                    }


                    if (persona != null && persona.PersonaId > 0)
                    {
                        op = (Cliente)persona;
                    }
                    else
                        throw new APIException("No se pudo insertar el nuevo cliente", "", "9999");
                    
                }

            }

            return op;
        }

        public async Task<bool> UpdateCliente(Cliente cliente)
        {
            bool op = true;


            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    //Persona persona = (Persona)cliente;
                    try
                    {
                        //Abrir la conexion si esta cerrada
                        if(_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) await _dbContext.Database.OpenConnectionAsync();

                        //Actualizar en la BD
                        _dbContext.Clientes.Update(cliente);
                        //_dbContext.Attach(clteAct);
                        _dbContext.Entry(cliente).Property(c => c.PersonaId).IsModified = false;
                        _dbContext.Entry(cliente).Property(c => c.ClienteId).IsModified = false;
                        _dbContext.Entry(cliente).Property(c => c.Identificacion).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.NombresCompletos).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.Genero).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.FechaNacimiento).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.DireccionDomicilio).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.Telefono).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.Contrasena).IsModified = true;
                        _dbContext.Entry(cliente).Property(c => c.Estado).IsModified = true;

                        var rowsAff = await _dbContext.SaveChangesAsync();

                        op = (rowsAff > 0);
                    }
                    catch (Exception ex)
                    {
                        throw new APIException("Error al momento de actualizar los datos del nuevo cliente", ex.Message, "9999");
                    }


                    //if (persona != null && persona.PersonaId > 0)
                    //{
                    //    op = (Cliente)persona;
                    //}
                    //else
                    //    throw new APIException("No se pudo insertar el nuevo cliente", "", "9999");

                }

            }

            return op;
        }


        public Task<bool> DeleteCliente(long idCliente)
        {
            throw new NotImplementedException();
        }


    }
}
