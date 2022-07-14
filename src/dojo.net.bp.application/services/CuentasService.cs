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
    public class CuentasService : ICuentasService
    {
        private readonly ICuentasContextRepository _cuentasContextRepository;
        private readonly IClientesContextRepository _clientesContextRepository;
        private readonly IMapper _mapper;
        public CuentasService(ICuentasContextRepository cuentasContextRepository, IClientesContextRepository clientesContextRepository,
            IMapper mapper)
        {
            _cuentasContextRepository = cuentasContextRepository;
            _clientesContextRepository = clientesContextRepository;
            _mapper = mapper;
        }




        public async Task<CuentaDTO> GetCuentaByNumCta(string numeroCuenta)
        {
            CuentaDTO ctaDto = new CuentaDTO();
            try
            {
                var cta = new Cuenta();
                //1. Obtener los datos de la Cta.
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(c => c.NumeroCuenta == numeroCuenta);
                if (resCta != null && resCta.Count() > 0) cta = resCta.FirstOrDefault();
                else throw new APIException("No se encontraron los datos de la Cuenta con dicho numero de cuenta", "", "0005");

                //2. Obtener los datos del cliente de esta forma xq no se puede usar Eager Loading
                //ya que Cliente es derivado de Persona y hay problemas con aquello
                var resCli = await _clientesContextRepository.GetClientesBySelectors(cli => cli.ClienteId == cta.ClienteId);
                if (resCta != null && resCta.Count() > 0) cta.Cliente = resCli.FirstOrDefault();

                //3. Mapear el objeto Cuenta al objeto CuentaDTO
                ctaDto = _mapper.Map<CuentaDTO>(cta);
            }
            catch (APIException ax)
            { 
                throw ax;
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de consultar la cuenta", ex.StackTrace);
            }

            return ctaDto;
        }

        public async Task<CuentaDTO> InsertNuevaCuenta(InsertCuentaRequestDTO nuevaCuenta)
        {
            CuentaDTO ctaDto = new CuentaDTO();
            try
            {
                //1. Asegurarnos de que el cliente existe con la cedula.
                var resCli = await _clientesContextRepository.GetClientesBySelectors(c => c.Identificacion == nuevaCuenta.Cedula);
                if (resCli == null || resCli.Count() == 0) throw new APIException("No se puede crear la nueva cuenta porque el Cliente no existe. Intente crear el Cliente en primer lugar.", "", "0006");

                //2. Asegurarnos de que la cuenta no exista por su Numero de Cuenta y Tipo
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(cta => cta.NumeroCuenta == nuevaCuenta.NumeroCuenta && cta.TipoCuenta.ToUpper() == nuevaCuenta.TipoCuenta.ToUpper());
                if(resCta != null && resCta.Count() > 0) throw new APIException("No se puede crear la nueva cuenta porque la Cuenta ya existe con ese Numero de Cuenta y Tipo de Cuenta.", "", "0007");


                //3. Asociar el Id del Cliente a la nueva cuenta.
                Cuenta ctaParsed = _mapper.Map<Cuenta>(nuevaCuenta);
                ctaParsed.Cliente = resCli.FirstOrDefault();
                ctaParsed.ClienteId = resCli.FirstOrDefault().ClienteId;


                //4. Insertar la nueva cuenta y obtener los datos de la nueva cuenta creada
                var ctaCreada = await _cuentasContextRepository.InsertNuevaCuenta(ctaParsed);
                ctaCreada.Cliente = resCli.FirstOrDefault();


                //5. Parsear los datos de la cuenta a un tipo CuentaDTO para la salida
                ctaDto = _mapper.Map<CuentaDTO>(ctaCreada);
            }
            catch (APIException ax)
            {
                throw ax;
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de crear la nueva cuenta", ex.StackTrace);
            }

            return ctaDto;
        }

        public async Task<CuentaDTO> UpdateCuenta(UpdateCuentaRequestDTO cuentaCreada)
        {
            CuentaDTO ctaDto = new CuentaDTO();
            try
            {
                //1. Asegurarnos de que el cliente existe con la cedula.
                var resCli = await _clientesContextRepository.GetClientesBySelectors(c => c.Identificacion == cuentaCreada.Cedula);
                if (resCli == null || resCli.Count() == 0) throw new APIException("No se puede modificar la cuenta porque el Cliente no existe.", "", "0008");

                //2. Asegurarnos de que la cuenta exista por su Numero de Cuenta y Tipo
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(cta => cta.ClienteId == resCli.FirstOrDefault().ClienteId && cta.NumeroCuenta == cuentaCreada.NumeroCuenta && cta.TipoCuenta.ToUpper() == cuentaCreada.TipoCuenta.ToUpper());
                if (resCta == null || resCta.Count() == 0) throw new APIException("No se puede modificar la cuenta porque la Cuenta no existe con ese Numero de Cuenta y Tipo de Cuenta.", "", "0009");


                //3. Asociar el Id del Cliente a la cuenta.
                Cuenta ctaParsed = _mapper.Map<Cuenta>(cuentaCreada);
                ctaParsed.Cliente = resCli.FirstOrDefault();
                ctaParsed.ClienteId = resCli.FirstOrDefault().ClienteId;
                ctaParsed.CuentaId = resCta.FirstOrDefault().CuentaId;


                //4. Modificar la cuenta y obtener los datos de la cuenta existente
                var ctaModif = await _cuentasContextRepository.UpdateCuenta(ctaParsed);
                ctaModif.Cliente = resCli.FirstOrDefault();


                //5. Parsear los datos de la cuenta a un tipo CuentaDTO para la salida
                ctaDto = _mapper.Map<CuentaDTO>(ctaModif);
            }
            catch (APIException ax)
            {
                throw ax;
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de crear la nueva cuenta", ex.StackTrace);
            }

            return ctaDto;
        }

        public async Task<bool> DeleteCuenta(DeleteCuentaRequestDTO cuentaCreada)
        {
            bool op = true;
            try
            {
                //1. Asegurarnos de que la cuenta exista por su Numero de Cuenta y Tipo
                var resCta = await _cuentasContextRepository.GetCuentasBySelector(cta => cta.NumeroCuenta == cuentaCreada.NumeroCuenta && cta.TipoCuenta.ToUpper() == cuentaCreada.TipoCuenta.ToUpper() && cta.Estado == true);
                if (resCta == null || resCta.Count() == 0) throw new APIException("No se puede eliminar la cuenta porque la Cuenta no existe con ese Numero de Cuenta y Tipo de Cuenta o ya se encuentra eliminada.", "", "0009");

                //2. Obtener los datos del cliente asociado a esta cuenta
                var resCli = await _clientesContextRepository.GetClientesBySelectors(c => c.ClienteId == resCta.FirstOrDefault().ClienteId);


                //3. Asociar el Id del Cliente a la cuenta.
                Cuenta ctaParsed = _mapper.Map<Cuenta>(cuentaCreada);
                ctaParsed.Cliente = resCli.FirstOrDefault();
                ctaParsed.ClienteId = resCli.FirstOrDefault().ClienteId;
                ctaParsed.CuentaId = resCta.FirstOrDefault().CuentaId;
                ctaParsed.Estado = false; //Campo para desactivar la cuenta


                //4. Modificar la cuenta y obtener los datos de la cuenta existente
                op = await _cuentasContextRepository.DeleteCuenta(ctaParsed);

            }
            catch (APIException ax)
            {
                throw ax;
            }
            catch (Exception ex)
            {
                throw new APIException("Error al momento de eliminar la cuenta", ex.StackTrace);
            }

            return op;
        }
    }
}
