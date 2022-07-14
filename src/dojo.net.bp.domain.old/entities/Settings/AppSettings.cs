using System;
using System.Collections.Generic;
using System.Text;

namespace dojo.net.bp.domain.entities.Settings
{
    public class AppSettings
    {
        public PruebaSett PruebaSett { get; set; }
    }

    public class PruebaSett
    { 
        public string Prueba { get; set; }
    }
}
