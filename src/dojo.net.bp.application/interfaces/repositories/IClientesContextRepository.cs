using dojo.net.bp.domain.entities.ContextEntities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.repositories
{
    public interface IClientesContextRepository
    {
        public Task<List<Cliente>> GetClientesBySelectors(Expression<Func<Cliente, bool>> query);

        public Task<Cliente> InsertCliente(Cliente cliente);

        public Task<bool> UpdateCliente(Cliente cliente);

        public Task<bool> DeleteCliente(long idCliente);

    }
}
