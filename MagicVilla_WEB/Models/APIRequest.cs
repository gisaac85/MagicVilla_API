using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_WEB.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
