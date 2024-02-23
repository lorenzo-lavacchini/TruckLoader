using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckLoader
{
    public class Result
    {
        public string message { get; set; }
        public SpaceLocation[,] loadingMatrix { get; set; }

        public Result(string message, SpaceLocation[,] locationMatrix) 
        {
            this.message = message;
            this.loadingMatrix = locationMatrix;
        }
    }
}
