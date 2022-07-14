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
    internal class PersonasContextRepository : IPersonasContextRepository
    {
        private readonly IDbContextFactory<BP_DBContext> _dbContextFactory;

        public PersonasContextRepository(IDbContextFactory<BP_DBContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        

        public async Task<List<Persona>> GetPersonasBySelectors(Expression<Func<Persona, bool>> query)
        {
            List<Persona> op = new List<Persona>();
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    op = await _dbContext.Set<Persona>().Where(query).ToListAsync();
                    if (op != null && op.Count() > 0)
                        op.ForEach(x => ((Cliente)x).Contrasena = "");
                }

            }

            return op;
        }


        public async Task<Persona> InsertPersona(Persona persona)
        {
            Persona op = new Persona();
            try
            {
                using (var _dbContext = _dbContextFactory.CreateDbContext())
                {
                    if (_dbContext != null)
                    {
                        if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                        _dbContext.Add<Persona>(persona);
                        int rowsAff = await _dbContext.SaveChangesAsync();

                        if (rowsAff > 0)
                        {
                            var op2 = await GetPersonasBySelectors(p => p.Identificacion == persona.Identificacion);

                            if (op2 != null && op2.Count() > 0)
                            {
                                op = op2.FirstOrDefault();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
               throw new APIException("Error en el proceso de guardar los datos del nuevo cliente", ex.Message, "9999");
            }
            

            return op;
        }


        public async Task<Persona> InsertPersona(Persona persona, IDbContextTransaction dbTrx)
        {
            Persona op = null;
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    await _dbContext.Database.UseTransactionAsync(dbTrx.GetDbTransaction());

                    _dbContext.Add<Persona>(persona);
                    int rowsAff = await _dbContext.SaveChangesAsync();

                    if (rowsAff > 0)
                    {
                        var op2 = await GetPersonasBySelectors(p => p.Identificacion == persona.Identificacion);

                        if (op2 != null && op2.Count() > 0)
                        {
                            op = op2.FirstOrDefault();
                        }
                    }

                }

            }

            return op;
        }


        public async Task<bool> UpdatePersona(Persona persona)
        {
            bool op = true;
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    _dbContext.Update<Persona>(persona);
                    await _dbContext.SaveChangesAsync();
                }

            }

            return op;
        }


        public async Task<bool> UpdatePersona(Persona persona, IDbContextTransaction dbTrx)
        {
            bool op = true;
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    await _dbContext.Database.UseTransactionAsync(dbTrx.GetDbTransaction());

                    _dbContext.Update<Persona>(persona);
                    await _dbContext.SaveChangesAsync();
                }

            }

            return op;
        }


        public async Task<bool> DeletePersona(long idPersona)
        {
            bool op = true;
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    _dbContext.Remove<Persona>(new Cliente() { PersonaId = idPersona });
                    await _dbContext.SaveChangesAsync();
                }

            }

            return op;
        }


        public async Task<bool> DeletePersona(long idPersona, IDbContextTransaction dbTrx)
        {
            bool op = true;
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    await _dbContext.Database.UseTransactionAsync(dbTrx.GetDbTransaction());

                    _dbContext.Remove<Persona>(new Cliente() { PersonaId = idPersona });
                    await _dbContext.SaveChangesAsync();
                }

            }

            return op;
        }
    }
}
