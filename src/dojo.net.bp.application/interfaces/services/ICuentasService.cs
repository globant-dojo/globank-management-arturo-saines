using dojo.net.bp.domain.entities.ContextEntities;
using dojo.net.bp.domain.entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.services
{
    public interface ICuentasService
    {
        public Task<CuentaDTO> GetCuentaByNumCta(string numeroCuenta);
        public Task<CuentaDTO> InsertNuevaCuenta(InsertCuentaRequestDTO cuenta);
        public Task<CuentaDTO> UpdateCuenta(UpdateCuentaRequestDTO cuenta);
        public Task<bool> DeleteCuenta(DeleteCuentaRequestDTO cuenta);
    }
}
