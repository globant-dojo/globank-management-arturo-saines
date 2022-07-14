using AutoMapper;
using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.models.exeptions;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.Settings;
using dojo.net.bp.infrastructure.data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.infrastructure.data.repositories
{
    public class MovimientosContextRepository : IMovimientosContextRepository
    {
        private readonly ICuentasContextRepository _cuentasContextRepository;
        private readonly IDbContextFactory<BP_DBContext> _dbContextFactory;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _config;

        public MovimientosContextRepository(ICuentasContextRepository cuentasContextRepository, 
            IDbContextFactory<BP_DBContext> dbContextFactory,
            IMapper mapper, IOptions<AppSettings> config)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _config = config;
            _cuentasContextRepository = cuentasContextRepository;
        }

        public async Task<List<Movimiento>> GetMovimientosBySelector(Expression<Func<Movimiento, bool>> query)
        {
            List<Movimiento> op = new List<Movimiento>();
            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) _dbContext.Database.OpenConnection();

                    op = await _dbContext.Movimientos.Where(query).ToListAsync();
                }

            }

            return op;
        }

        public async Task<Movimiento> InsertNuevoMovimiento(Movimiento movimiento)
        {
            Movimiento op = new Movimiento();


            using (var _dbContext = _dbContextFactory.CreateDbContext())
            {
                if (_dbContext != null)
                {
                    if (_dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) await _dbContext.Database.OpenConnectionAsync();
                    using (var trxGral = await _dbContext.Database.GetDbConnection().BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        try
                        {
                            //1. Insertar los datos del nuevo movimiento
                            var newMov = await RegistrarNuevoMovimiento(movimiento, _dbContext, trxGral);

                            //2. Modificar el Saldo Disponible de la Cuenta 
                            var ctaAct = newMov.Cuenta;
                            ctaAct.SaldoDisponible += movimiento.Valor;
                            var cta = await _cuentasContextRepository.UpdateCuentaConTransaccion(ctaAct, _dbContext, trxGral);

                            trxGral.Commit();

                            op = newMov;
                        }
                        catch (APIException ax)
                        {
                            await trxGral.RollbackAsync();
                            throw new APIException(ax.Message);
                        }
                        catch (Exception ex)
                        {
                            await trxGral.RollbackAsync();
                            throw new APIException("Error al momento de insertar los datos del nuevo movimiento", ex.Message, "9999");
                        }
                    }

                }
            }


            return op;
        }


        


        public async Task<Movimiento> ReversarMovimiento(Movimiento movimientoAReversar)
        {
            Movimiento op = new Movimiento();

            try
            {
                //1. Modificar el valor por el mismo valor pero * -1
                var movReverso = _mapper.Map<Movimiento>(movimientoAReversar);
                movReverso.MovimientoId = 0; //se hace esto para que se agregue como nuevo movimiento
                movReverso.CuentaId = movimientoAReversar.Cuenta.CuentaId;
                movReverso.Cuenta = movimientoAReversar.Cuenta;
                movReverso.TipoMovimiento = _config.Value.TiposMovimientos.ReversoMov;
                movReverso.Saldo = movimientoAReversar.Cuenta.SaldoDisponible + (movReverso.Valor * (-1));
                movReverso.Valor = movReverso.Valor * (-1);
               


                using (var dbContextGrl = _dbContextFactory.CreateDbContext())
                {
                    if (dbContextGrl.Database.GetDbConnection().State == System.Data.ConnectionState.Closed) await dbContextGrl.Database.OpenConnectionAsync();
                    using (var trxGral = await dbContextGrl.Database.GetDbConnection().BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        //2. Insertar el movimiento de reverso
                        op = await RegistrarNuevoMovimiento(movReverso, dbContextGrl, trxGral);


                        //3. Modificar el Saldo Disponible de la Cuenta 
                        var ctaAct = op.Cuenta;
                        ctaAct.SaldoDisponible = movReverso.Saldo;
                        var cta = await _cuentasContextRepository.UpdateCuentaConTransaccion(ctaAct, dbContextGrl, trxGral);


                        //4. Aprobar la transaccion
                        trxGral.Commit();
                    }
                }
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message);
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de reversar los datos del movimiento", ex.Message, "9999");
            }

            return op;
        }










        private async Task<Movimiento> RegistrarNuevoMovimiento(Movimiento movimiento, ABP_DBContext dbContext, DbTransaction trx)
        {
            Movimiento op = new Movimiento();

            try
            {
                DateTime fechaRegistroMov = DateTime.Now;

                if (dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Open) await dbContext.Database.OpenConnectionAsync();
                await dbContext.Database.UseTransactionAsync(trx);

                // get the Cuenta data
                var cuentaEnt = dbContext.Set<Cuenta>().Where(c => c.CuentaId == movimiento.CuentaId).FirstOrDefault();
                movimiento.Cuenta = cuentaEnt;

                //We have to insert the Movimiento data
                dbContext.Set<Movimiento>().Add(movimiento);
                dbContext.Entry(cuentaEnt).State = EntityState.Unchanged;

                movimiento.FechaMovimiento = fechaRegistroMov;
                movimiento.CuentaId = cuentaEnt.CuentaId;
                dbContext.Entry<Movimiento>(movimiento).Property(p => p.CuentaId).IsModified = false;


                int rowsAff = await dbContext.SaveChangesAsync();

                if (rowsAff > 0)
                {
                    //var res = await GetMovimientosBySelector(c => c.CuentaId == movimiento.Cuenta.CuentaId && c.FechaMovimiento == fechaRegistroMov);
                    //if (res != null && res.Count() > 0) op = res.OrderBy(c => c.FechaMovimiento).FirstOrDefault();
                    //else throw new APIException("No se encontro el Movimiento luego de haber sido creado", "", "0008");

                    op = movimiento;
                }

            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message);
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de insertar los datos del nuevo movimiento", ex.Message, "9999");
            }

            return op;
        }
    }
}
