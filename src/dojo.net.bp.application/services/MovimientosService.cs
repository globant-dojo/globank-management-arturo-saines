using AutoMapper;
using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.models.exeptions;
using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.DTOs;
using dojo.net.bp.domain.entities.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.services
{
    public class MovimientosService : IMovimientosService
    {
        private readonly ICuentasContextRepository _cuentasContextRepository;
        private readonly IMovimientosContextRepository _movimientosContextRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _config;

        public MovimientosService(IMovimientosContextRepository movimientosContextRepository, ICuentasContextRepository cuentasContextRepository, IMapper mapper, IOptions<AppSettings> config)
        {
            _movimientosContextRepository = movimientosContextRepository;
            _cuentasContextRepository = cuentasContextRepository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<List<MovimientoDTO>> GetMovimientosByNumCta_RangoFechas(ConsultaMovimientoRequestDTO movimiento)
        {
            List<MovimientoDTO> movDto = new List<MovimientoDTO>();
            try
            {
                DateTime fechaIni = movimiento.FechaInicioBusqueda.Value.ToDateTime(new TimeOnly(0, 0, 0));
                DateTime fechaFin = movimiento.FechaFinBusqueda.Value.ToDateTime(new TimeOnly(23, 59, 59));

                //1. Obtener los datos de la cuenta.
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(c => c.NumeroCuenta == movimiento.NumeroCuenta && c.TipoCuenta == movimiento.TipoCuenta);
                if (resCta == null || resCta.Count() == 0) throw new APIException("No existe la cuenta del movimiento");


                //2. obtener los movimientos de dicha cuenta
                var cuentaEnt = resCta.FirstOrDefault();


                var movimientos = new List<Movimiento>();
                if (!string.IsNullOrWhiteSpace(movimiento.TipoMovimiento))
                {
                    movimientos = await _movimientosContextRepository.GetMovimientosBySelector(p => p.CuentaId == cuentaEnt.CuentaId &&
                                            (p.FechaMovimiento >= fechaIni && p.FechaMovimiento <= fechaFin) &&
                                            (p.TipoMovimiento == movimiento.TipoMovimiento));
                }
                else
                {
                    movimientos = await _movimientosContextRepository.GetMovimientosBySelector(p => p.CuentaId == cuentaEnt.CuentaId &&
                                            (p.FechaMovimiento >= fechaIni && p.FechaMovimiento <= fechaFin));
                }



                if (movimientos == null || movimientos.Count() == 0) throw new APIException("No existen movimientos con los parametros ingresados", "", "0009");
                else movDto = _mapper.Map<List<Movimiento>, List<MovimientoDTO>>(movimientos);
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message);
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de realizar la consulta de datos de los movimientos", ex.Message, "9999");
            }

            return movDto;
        }

        public Task<MovimientoDTO> InsertNuevoMovimientoCredito(InsertMovimientoRequestDTO movimiento)
        {
            movimiento.TipoMovimiento = _config.Value.TiposMovimientos.CreditoMov;
            movimiento.Valor = (movimiento.Valor < 0) ? -(movimiento.Valor) : movimiento.Valor;

            return RegistrarNuevoMovimiento(movimiento);
        }

        public Task<MovimientoDTO> InsertNuevoMovimientoDebito(InsertMovimientoRequestDTO movimiento)
        {
            movimiento.TipoMovimiento = _config.Value.TiposMovimientos.DebitoMov;
            movimiento.Valor = (movimiento.Valor >= 0) ? -(movimiento.Valor) : movimiento.Valor;

            return RegistrarNuevoMovimiento(movimiento);
        }

        public async Task<bool> ReversarMovimiento(ReversoMovimientoRequestDTO movimiento)
        {
            bool op = true;
            try
            {
                //1. Obtener los datos de la cuenta.
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(c => c.NumeroCuenta == movimiento.NumeroCuenta && c.TipoCuenta == movimiento.TipoCuenta);
                if (resCta == null || resCta.Count() == 0) throw new APIException("No existe la cuenta del movimiento a reversar");

                //2. Obtener el movimiento a reversar
                var resMov = await _movimientosContextRepository.GetMovimientosBySelector(m => m.MovimientoId == movimiento.MovimientoId && m.CuentaId == resCta.FirstOrDefault().CuentaId);
                if (resMov == null || resMov.Count() == 0) throw new APIException("No existe el movimiento a reversar");

                //3. Hacer el reverso del movimiento
                var movRev = resMov.FirstOrDefault();
                movRev.CuentaId = resCta.FirstOrDefault().CuentaId;
                movRev.Cuenta = resCta.FirstOrDefault();
                var movEnt = await _movimientosContextRepository.ReversarMovimiento(movRev);

                //4. Setear la variable
                op = (movEnt != null && movEnt.MovimientoId > 0);

            }
            catch (Exception)
            {

                throw;
            }

            return op;
        }





        private async Task<MovimientoDTO> RegistrarNuevoMovimiento(InsertMovimientoRequestDTO movimiento)
        {
            MovimientoDTO movDto = new MovimientoDTO();
            try
            {
                //1. Obtener los datos de la cuenta.
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(c => c.NumeroCuenta == movimiento.NumeroCuenta && c.TipoCuenta == movimiento.TipoCuenta && c.Estado == true);
                if (resCta == null || resCta.Count() == 0) throw new APIException("No existe la cuenta para registrar el movimiento");
                Cuenta ctaRet = resCta.FirstOrDefault();

                //2. Verificar si el saldo disponible - valor a debitar (cuando TipoMovimiento = DEBITO) es mayor o igual a cero
                if (movimiento.Valor < 0 && ctaRet.SaldoDisponible + movimiento.Valor < 0)
                    throw new APIException("Saldo no disponible", "", "0011");

                //3. Parsear la entidad externa a la entidad interna para poder insertar los datos
                var nuevoMov = _mapper.Map<Movimiento>(movimiento);
                nuevoMov.Saldo = ctaRet.SaldoDisponible + movimiento.Valor;

                //4. Realizar la insercion del nuevo movimiento de la cuenta
                nuevoMov.CuentaId = ctaRet.CuentaId;
                nuevoMov.Cuenta = ctaRet;
                var mov = await _movimientosContextRepository.InsertNuevoMovimiento(nuevoMov);

                //5. Parsear la entidad de la cuenta al tipo MovimientoDTO para la salida
                if(mov != null) movDto = _mapper.Map<MovimientoDTO>(mov);
            }
            catch (APIException ax)
            {
                throw new APIException(ax.Message);
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de realizar la consulta de datos de los movimientos", ex.Message, "9999");
            }

            return movDto;

        }
    }
}
