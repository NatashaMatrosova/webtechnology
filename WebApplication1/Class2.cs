using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class hh
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Naviki> key_skills { get; set; }
        public List<Specializaciya> specializations { get; set; }
        public Zarplata salary { get; set; }
    }

     public class Naviki
    {
        public string name { get; set; }
    }

        public class Specializaciya
    {
        public string name { get; set; }
    }

        public class Zarplata
    {
        public int? to { get; set; }
        public int? from  { get; set; }
    }

        public class ResultatPoiska
    {
        public List<hh> items { get; set; }
    }
}
