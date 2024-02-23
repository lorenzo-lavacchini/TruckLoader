using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckLoader
{
    public class Pallet
    {
        public string ID { get; set; }
        public double height { get; set; }
        public double maxHeight { get; set; }
        public double longerSide { get; set; }
        public double shorterSide { get; set; }
        public int unloadingOrder { get; set; } //ordine di scarico (numero minore significa che scarico che avviene prima)
        public bool stackedPalletAbovePermitted { get; set; }

        public bool removed { get; set; }
        
        public Pallet(string Id, double height, int unloadingOrder, bool stackedPalletAbovePermitted)
        {
            this.ID = Id;
            this.height = height;
            this.maxHeight = 2.20;
            this.longerSide = 1.20;
            this.shorterSide = 0.80;
            this.unloadingOrder = unloadingOrder;
            this.stackedPalletAbovePermitted = stackedPalletAbovePermitted;
            this.removed = false;
        }

        public Pallet(Pallet originalPallet)
        {
            this.ID = originalPallet.ID;
            this.height = originalPallet.height;
            this.maxHeight = originalPallet.maxHeight;
            this.longerSide = originalPallet.longerSide;
            this.shorterSide = originalPallet.shorterSide;
            this.unloadingOrder = originalPallet.unloadingOrder;
            this.stackedPalletAbovePermitted = originalPallet.stackedPalletAbovePermitted;
            this.removed = originalPallet.removed;
        }

    }
}
