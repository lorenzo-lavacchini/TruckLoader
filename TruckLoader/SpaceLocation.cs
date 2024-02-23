using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckLoader
{
    using System.Collections.Generic;

    public class SpaceLocation
    {
        public double remainingHeight { get; set; }
        public List<Pallet> pallets { get; set; }

        public SpaceLocation(double remainingHeight)
        {
            this.remainingHeight = remainingHeight;
            this.pallets = new List<Pallet>();
        }

        public bool AddPallet(Pallet pallet)
        {
            if ((remainingHeight - pallet.height) < 0)
            {
                return false;
            }
            else
            {
                pallets.Add(pallet);
                remainingHeight -= pallet.height;
                return true;
            }
        }

        public string GetAllPalletsInsideToString()
        {
            string result = "";
            foreach(Pallet p in pallets)
            {
                if (result == "")
                    result = p.ID;
                else
                    result = $"{result},{p.ID}";
            }
            return result;
        }
    }
}
