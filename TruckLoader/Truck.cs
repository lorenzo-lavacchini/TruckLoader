using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckLoader
{
    public class Truck
    {
        public double width { get; set; }
        public double length { get; set; }
        public double height { get; set; }

        public string name { get; set; }

        public Truck(double width, double length, double height, string name)
        {
            this.width = width;
            this.length = length;
            this.height = height;
            this.name = name;
        }
    }

}
