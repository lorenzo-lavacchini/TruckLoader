using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TruckLoader;
namespace TruckLoader
{
    public class Analyzer
    {
        bool orientazionePallet { get; set; }
        public List<Truck> trucks { get; set; }

        public List<Pallet> pallets { get; set; }

        public List<SpaceLocation[,]> spaceLocations { get; set; }

        public List<double> volumeUnusedForEachTruck { get; set; }

        public List<Result> results { get; set; }

        public Analyzer(List<Truck> trucks, List<Pallet> pallets, bool orientazionePallet)
        {
            this.trucks = trucks;
            this.pallets = pallets;
            this.orientazionePallet = orientazionePallet;
            results = new List<Result>();
        }

        
        public List<Result> Analyze()
        {
            List<AutomaticLoader> automaticLoaders = new List<AutomaticLoader>();
            foreach(Truck truck in trucks)
            {
                List<Pallet> palletsToPass = new List<Pallet>();
                foreach (Pallet p in pallets)
                {
                    palletsToPass.Add(new Pallet(p));
                }

                automaticLoaders.Add(new AutomaticLoader(truck, palletsToPass, orientazionePallet));
            }

            foreach(AutomaticLoader aL in automaticLoaders)
            {
                SpaceLocation[,] spaceLocationMatrix = aL.Load();

                double totVolumeUnused = 0;
                foreach (SpaceLocation sL in spaceLocationMatrix)
                {
                    totVolumeUnused += (sL.remainingHeight * GlobalConstants.palletLongerSide * GlobalConstants.palletShorterSide);
                }

                //Non è tutto il volume: manca quello dovuto allo spazio dove non c'è proprio nessun pallet.
                double remainingLength;
                double remainingWidth;
                if (orientazionePallet)
                {
                    remainingLength = aL.truck.length - ((int)Math.Floor((aL.truck.length / pallets[0].longerSide)) * pallets[0].longerSide);
                    remainingWidth = aL.truck.width - ((int)Math.Floor((aL.truck.width / pallets[0].shorterSide)) * pallets[0].shorterSide);
                }
                else
                {
                    remainingLength = aL.truck.length - ((int)Math.Floor((aL.truck.length / pallets[0].shorterSide)) * pallets[0].shorterSide);
                    remainingWidth = aL.truck.width - ((int)Math.Floor((aL.truck.width / pallets[0].longerSide)) * pallets[0].longerSide);
                }
                totVolumeUnused += remainingLength * aL.truck.height * aL.truck.width;
                totVolumeUnused += remainingWidth * aL.truck.height * aL.truck.length;

                string result = "Usando il mezzo: '" + aL.truck.name + "', ho uno spreco di volume totale pari a: " + totVolumeUnused + " metri cubi";

                results.Add(new Result(result, aL.loadingMatrix));
            }

            return results;
        } 
    }
}
