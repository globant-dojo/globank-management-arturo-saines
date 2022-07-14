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
    public interface IPersonasContextRepository
    {
        public Task<List<Persona>> GetPersonasBySelectors(Expression<Func<Persona, bool>> query);

        public Task<Persona> InsertPersona(Persona persona);

        public Task<Persona> InsertPersona(Persona persona, IDbContextTransaction dbTrx);

        public Task<bool> UpdatePersona(Persona persona);

        public Task<bool> UpdatePersona(Persona persona, IDbContextTransaction dbTrx);

        public Task<bool> DeletePersona(Int64 idPersona);

        public Task<bool> DeletePersona(long idPersona, IDbContextTransaction dbTrx);
    }
}
