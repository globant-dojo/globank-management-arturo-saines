using System;
using System.Collections.Generic;
using System.Text;

namespace dojo.net.bp.domain.entities.Settings
{
    public class AppSettings
    {
        public TiposMovimientos TiposMovimientos { get; set; }
    }

    public class TiposMovimientos
    { 
        public string CreditoMov { get; set; }
        public string DebitoMov { get; set; }
        public string ReversoMov { get; set; }
    }
}
