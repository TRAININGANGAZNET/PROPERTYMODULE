using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyModule.Model
{
    public class PropertyResponse
    {
        public string Responses { get; set; }
        public string id { get; set; }
    }

    public class Getpropresponse
    {
        public string Property { get; set; }
        public string Locality { get; set; }
        public float Budget { get; set; }
        public string Responses { get; set; }
    }
}
