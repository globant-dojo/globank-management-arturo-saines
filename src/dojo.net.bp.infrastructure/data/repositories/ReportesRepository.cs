using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.models.exeptions;
using dojo.net.bp.domain.entities.DTOs;
using dojo.net.bp.infrastructure.data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.infrastructure.data.repositories
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly IDbContextFactory<BP_DBContext> _dbContextFactory;
        public ReportesRepository(IDbContextFactory<BP_DBContext> dbContextFactory)
        { 
            _dbContextFactory = dbContextFactory;
        }

        public async Task<object> ConsultarEstadoCuenta(ConsultaEstadoCuentaRequestDTO datosConsulta)
        {
            object data;
            try
            {
                DateTime fechaIni = datosConsulta.FechaInicioBusqueda.ToDateTime(new TimeOnly(0, 0, 0));
                DateTime fechaFin = datosConsulta.FechaFinBusqueda.ToDateTime(new TimeOnly(23, 59, 59));

                using (var _dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    var opdata = _dbContext.Movimientos.AsQueryable()
                        .Where(d => d.FechaMovimiento >= fechaIni && d.FechaMovimiento <= fechaFin)
                        .Join(_dbContext.Cuentas
                                .Where(cta => cta.NumeroCuenta == datosConsulta.NumeroCuenta &&
                                        cta.TipoCuenta.ToUpper() == datosConsulta.TipoCuenta.ToUpper()),
                              mov => mov.CuentaId,
                              cta => cta.CuentaId,
                              (movimiento, cuenta) => new
                              {
                                  Fecha = movimiento.FechaMovimiento,
                                  ClienteId = cuenta.ClienteId,
                                  NumeroCuenta = cuenta.NumeroCuenta,
                                  TipoCuenta = cuenta.TipoCuenta,
                                  SaldoInicial = cuenta.SaldoInicial,
                                  EstadoCuenta = cuenta.Estado,
                                  Movimiento = movimiento.Valor,
                                  SaldoDisponible = movimiento.Saldo,
                                  TipoMovimiento = movimiento.TipoMovimiento
                              })
                        .Join(_dbContext.Clientes,
                                data => data.ClienteId,
                                clte => clte.ClienteId,
                                (dat, clt) => new
                                {
                                    Fecha = dat.Fecha,
                                    Cliente = clt.NombresCompletos,
                                    NumeroCuenta = dat.NumeroCuenta,
                                    TipoCuenta = dat.TipoCuenta,
                                    SaldoInicial = dat.SaldoInicial,
                                    EstadoCuenta = dat.EstadoCuenta,
                                    TipoMovimiento = dat.TipoMovimiento,
                                    Movimiento = dat.Movimiento,
                                    SaldoDisponible = dat.SaldoDisponible
                                }
                                );

                    var stringQ = opdata.ToQueryString();
                    var data2 = await opdata.ToListAsync();

                    if (data2 == null || data2.Count() == 0) throw new APIException("No existen datos para el Estado de Cuenta", "", "0011");
                
                    data = data2;
                }
            }
            catch (APIException ax)
            {
                throw ax;
            }
            catch (Exception ex)
            {
                throw new APIException("Ocurrio un error al consultar los datos del Estado de Cuenta");
            }


            return data;
        }
    }
}
