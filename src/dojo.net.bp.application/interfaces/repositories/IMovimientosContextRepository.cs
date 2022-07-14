using dojo.net.bp.domain.entities.ContextEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.repositories
{
    public interface IMovimientosContextRepository
    {
        public Task<List<Movimiento>> GetMovimientosBySelector(Expression<Func<Movimiento, bool>> query);
        public Task<Movimiento> InsertNuevoMovimiento(Movimiento movimiento);
        public Task<Movimiento> ReversarMovimiento(Movimiento movimientoAReversar);
    }
}
