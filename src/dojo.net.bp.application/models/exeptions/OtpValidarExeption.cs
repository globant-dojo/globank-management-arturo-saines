using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.models.exeptions
{
    public class OtpValidarExeption : BaseCustomException
    {
        public OtpValidarExeption(string message = "OtpValidarExeption Exeption", string description = "", string statuscode = "9999") : base(message, description, statuscode)
        {

        }
    }
}
