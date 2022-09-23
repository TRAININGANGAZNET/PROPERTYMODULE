using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyModule.Model
{
    public class PropertyRequest
    {
        public string Property { get; set; }
        public string Locality { get; set; }
        public float Budget { get; set; }
    }
    public class PropertyTypeRequest
    {
        public string Property { get; set; }
    }
    public class LocalityType
    {
        public string Locality { get; set; }
    }
}
