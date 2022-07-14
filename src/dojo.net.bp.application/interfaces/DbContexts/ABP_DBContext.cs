using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.interfaces.DbContexts
{
    public abstract class ABP_DBContext : DbContext
    {
        public ABP_DBContext(DbContextOptions<ABP_DBContext> options) : base(options)
        { }

        public ABP_DBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
