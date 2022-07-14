using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.domain.entities.ContextEntities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.repositories
{
    public interface ICuentasContextRepository
    {
        public Task<List<Cuenta>> GetCuentasBySelector(Expression<Func<Cuenta, bool>> query);
        public Task<Cuenta> InsertNuevaCuenta(Cuenta cuenta);
        public Task<Cuenta> UpdateCuenta(Cuenta cuenta);
        public Task<Cuenta> UpdateCuentaConTransaccion(Cuenta cuenta, ABP_DBContext dBContext, DbTransaction trxGlobal);
        public Task<bool> DeleteCuenta(Cuenta cuenta);
    }
}
