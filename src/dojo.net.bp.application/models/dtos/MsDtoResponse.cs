using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.application.models.dtos
{
    public class MsDtoResponse<T>
    {

        public string traceid { get; set; }
        public T data { get; set; }
        public MsDtoResponse(T data, string traceId)
        {
            this.traceid = traceId;
            this.data = data;
        }
    }
}
