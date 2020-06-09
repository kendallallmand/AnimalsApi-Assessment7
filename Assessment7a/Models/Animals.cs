
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment7a.Models
{

    public class Animals
    {
        public bool success { get; set; }
        public int count { get; set; }

        public Result[] results { get; set; }
        public Legend legend { get; set; }

    }
        public class Legend
        {
            public string weight { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string name { get; set; }
            public float weight { get; set; }
            public Species species { get; set; }
        }

        public class Specie
        {
            public string name { get; set; }
            public string _ref { get; set; }
        }
 
}
