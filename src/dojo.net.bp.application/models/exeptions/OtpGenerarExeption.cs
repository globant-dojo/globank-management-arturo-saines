using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.models.exeptions
{
    public class OtpGenerarExeption : BaseCustomException
    {
        public OtpGenerarExeption(string message = "OtpGenerar Exeption", string description = "", string statuscode = "9999") : base(message, description, statuscode)
        {

        }
    }
}
