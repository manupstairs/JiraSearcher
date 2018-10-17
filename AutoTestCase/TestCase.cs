using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestCase
{
    public class TestCase
    {
        public string Id { get; set; }

        //public double Possibility { get; set; }

        public string Description { get; set; }

        //public double Weight { get; set; }

        public Dictionary<int,Step> Steps { get; set; }

        public string Title { get; set; }

        public string Priority { get; set; }

    }

    public class Step
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
