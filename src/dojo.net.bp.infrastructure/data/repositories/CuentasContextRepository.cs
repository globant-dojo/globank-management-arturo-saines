using AutoMapper;
using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.models.exeptions;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.infrastructure.data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.infrastructure.data.repositories
{
    public class CuentasContextRepository : ICuentasContextRepository
    {
        private readonly IDbContextFactory<BP_DBContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public CuentasContextRepository(IDbContextFactory<BP_DBContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        

        public async Task<List<Cuenta>> GetCuentasBySelector(Expression<Func<Cuenta, bool>> query)
        {
            List<Cuenta> op = new List<Cuenta>();
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    op = await _dbContext.Cuentas.Where(query).ToListAsync();
                }

            }

            return op;
        }

        public async Task<Cuenta> InsertNuevaCuenta(Cuenta cuenta)
        {
            Cuenta op = new Cuenta();


            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    try
                    {
                        //We have to insert the Cuenta data
                        if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();
                        _dbContext.Cuentas.Add(cuenta);
                        cuenta.ClienteId = cuenta.Cliente.ClienteId;
                        _dbContext.Entry(cuenta.Cliente).State = EntityState.Unchanged;
                        int rowsAff = await _dbContext.SaveChangesAsync();

                        if (rowsAff > 0)
                        {
                            var res = await GetCuentasBySelector(c => c.NumeroCuenta == cuenta.NumeroCuenta);
                            if (res != null && res.Count() > 0) op = res.FirstOrDefault();
                            else throw new APIException("No se encontro la cuenta despues de haber sido creada");
                        }
                    }
                    catch (APIException ax)
                    {
                        throw new APIException(ax.Message, "", "0004");
                    }
                    catch (Exception ex)
                    {
                        throw new APIException("Error al momento de insertar los datos de la nueva cuenta", ex.Message, "9999");
                    }

                }

            }

            return op;
        }

        public async Task<Cuenta> UpdateCuenta(Cuenta cuenta)
        {
            Cuenta op = new Cuenta();


            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    try
                    {
                        //We have to modify the Cuenta data
                        if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();
                        _dbContext.Cuentas.Attach(cuenta);
                        cuenta.ClienteId = cuenta.Cliente.ClienteId;
                        _dbContext.Entry(cuenta.Cliente).State = EntityState.Unchanged;
                        _dbContext.Entry(cuenta.Cliente).Property(p => p.ClienteId).IsModified = false;
                        _dbContext.Entry(cuenta).Property(p => p.CuentaId).IsModified = false;
                        _dbContext.Entry(cuenta).Property(p => p.SaldoInicial).IsModified = true;
                        _dbContext.Entry(cuenta).Property(p => p.SaldoDisponible).IsModified = true;
                        _dbContext.Entry(cuenta).Property(p => p.Estado).IsModified = true;
                        int rowsAff = await _dbContext.SaveChangesAsync();

                        if (rowsAff > 0)
                        {
                            var res = await GetCuentasBySelector(c => c.NumeroCuenta == cuenta.NumeroCuenta);
                            if (res != null && res.Count() > 0) op = res.FirstOrDefault();
                            else throw new APIException("No se encontro la cuenta despues de haber sido modificada");
                        }
                    }
                    catch (APIException ax)
                    {
                        throw new APIException(ax.Message, "", "0004");
                    }
                    catch (Exception ex)
                    {
                        throw new APIException("Error al momento de modficar los datos de la nueva cuenta", ex.Message, "9999");
                    }

                }

            }

            return op;
        }

        public async Task<bool> DeleteCuenta(Cuenta cuenta)
        {
            bool op = true;


            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    try
                    {
                        //We have to modify the Cuenta data (Set ESTADO = 0)
                        if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();
                        _dbContext.Cuentas.Attach(cuenta);
                        cuenta.ClienteId = cuenta.Cliente.ClienteId;
                        cuenta.Estado = false;
                        _dbContext.Entry(cuenta.Cliente).State = EntityState.Unchanged;
                        _dbContext.Entry(cuenta.Cliente).Property(p => p.ClienteId).IsModified = false;
                        _dbContext.Entry(cuenta).Property(p => p.CuentaId).IsModified = false;
                        _dbContext.Entry(cuenta).Property(p => p.SaldoInicial).IsModified = false;
                        _dbContext.Entry(cuenta).Property(p => p.SaldoDisponible).IsModified = false;
                        _dbContext.Entry(cuenta).Property(p => p.Estado).IsModified = true;
                        int rowsAff = await _dbContext.SaveChangesAsync();

                        op = (rowsAff > 0);
                    }
                    catch (APIException ax)
                    {
                        op = false;
                        throw new APIException(ax.Message, "", "0004");
                    }
                    catch (Exception ex)
                    {
                        op = false;
                        throw new APIException("Error al momento de desactivar la cuenta", ex.Message, "9999");
                    }

                }

            }

            return op;
        }









        public async Task<Cuenta> UpdateCuentaConTransaccion(Cuenta cuenta, ABP_DBContext dbContext, DbTransaction trxGlobal)
        {
            Cuenta op = new Cuenta();

            try
            {
                //We have to modify the Cuenta data
                if (dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) dbContext.Database.OpenConnection();
                dbContext.Database.UseTransaction(trxGlobal);

                dbContext.Set<Cuenta>().Update(cuenta);
                //cuenta.ClienteId = cuenta.Cliente.ClienteId;
                dbContext.Entry(cuenta.Cliente).State = EntityState.Unchanged;
                dbContext.Entry(cuenta.Cliente).Property(p => p.ClienteId).IsModified = false;
                dbContext.Entry(cuenta).Property(p => p.CuentaId).IsModified = false;
                dbContext.Entry(cuenta).Property(p => p.SaldoInicial).IsModified = true;
                dbContext.Entry(cuenta).Property(p => p.SaldoDisponible).IsModified = true;
                dbContext.Entry(cuenta).Property(p => p.Estado).IsModified = true;
                int rowsAff = await dbContext.SaveChangesAsync();

                if (rowsAff > 0)
                {
                    //var res = await GetCuentasBySelector(c => c.NumeroCuenta == cuenta.NumeroCuenta);
                    //if (res != null && res.Count() > 0) op = res.FirstOrDefault();
                    //else throw new APIException("No se encontro la cuenta despues de haber sido modificada");

                    op = cuenta;
                }
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message, "", "0004");
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de modficar los datos de la nueva cuenta", ex.Message, "9999");
            }

            return op;
        }
    }
}
