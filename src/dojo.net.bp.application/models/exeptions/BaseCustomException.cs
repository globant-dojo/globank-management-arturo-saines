using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.models.exeptions
{
    public class BaseCustomException : Exception
    {
        public string Code { get; }
        public override string StackTrace { get; }

        public BaseCustomException(string message, string stackTrace, string code) : base(message)
        {
            Code = code;
            StackTrace = stackTrace;
        }
    }
}
