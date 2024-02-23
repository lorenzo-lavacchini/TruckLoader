using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckLoader
{
    using System;
    using System.Collections.Generic;

    public class AutomaticLoader
    {
        public List<Pallet> pallets { get; set; }
        public Truck truck { get; set; }
        public SpaceLocation[,] loadingMatrix { get; set; }

        public int rows { get; set; }

        public int columns { get; set; }
        
        public AutomaticLoader(Truck truck, List<Pallet> pallets, bool orientazionePallet)
        {
            this.pallets = pallets;
            this.truck = truck;
            if (orientazionePallet)
            {
                this.rows = (int)Math.Floor(truck.length / pallets[0].longerSide);
                this.columns = (int)Math.Floor(truck.width / pallets[0].shorterSide);
            }
            else 
            {
                this.rows = (int)Math.Floor(truck.length / pallets[0].shorterSide);
                this.columns = (int)Math.Floor(truck.width / pallets[0].longerSide);
            }
            
            this.loadingMatrix = new SpaceLocation[rows, columns];
            InitializeLoadingMatrix(rows, columns);
            VerifyPallets();
        }
        // Metodo per controllare che l'altezza dei pallet non superi quella del mezzo scelto per il trasporto. Nell'eventualità
        // in cui questo avvenga, il programma scarta i pallet non idonei, fornisce un avviso, e prosegue con la soluzione senza di essi.
        private void VerifyPallets()
        {

            for(int i = 0; i < pallets.Count; i++)
            {
                if (pallets[i].height > truck.height)
                {
                    Console.WriteLine("Attenzione! Caricamento " + truck.name + ": Pallet [" + pallets[i].ID + "] scartati per altezza superiore al mezzo di trasporto.");
                    pallets.Remove(pallets[i]);
                }
            }

            
        }

        // Metodo per inizializzare la matrice di SpaceLocations
        private void InitializeLoadingMatrix(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    loadingMatrix[i, j] = new SpaceLocation(truck.height);
                }
            }
        }

        // Metodo per caricare i pallet in maniera ottimizzata nel cassone
        public SpaceLocation[,] Load()
        {
            /*
             * Intanto ordino i pallets in base a:
             * - loro ordine di consegna in modo decrescente 
             * - al fatto che siano sovrapponibili
             * - all'altezza
             */

            var orderedPallets = pallets
                .OrderByDescending(p => p.unloadingOrder)
                .ThenByDescending(p => p.stackedPalletAbovePermitted)
                .ThenBy(p => p.height)
                .ToList();

            /*
				Adesso ho una lista di pallets in cui il primo deve essere scaricato per ultimo,
				e quindi lo posso mettere "in fondo" al camion.
			*/

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //Console.WriteLine("Sto analizzando la casella (" + i + "," + j + ")");
                    bool insertSucceded = true;
                    while (insertSucceded && orderedPallets.Count > 0)
                    {

                        if((loadingMatrix[i, j].pallets.Count == 0) || (loadingMatrix[i, j].pallets.Last().stackedPalletAbovePermitted))
                        {
                            //allora cerco una serie di pallets che hanno ordine di scarico massimo, da inserire nella 
                            //posizione [i, j].
                            foreach (Pallet p in orderedPallets)
                            {
                                //Console.WriteLine("Sto analizzando il pallet " + p.ID + " da inserire nella casella (" + i + "," + j + ")");                                
                                int unloadingOrderOfLastPallet;

                                if (loadingMatrix[i, j].pallets.Any() == false)
                                {
                                    unloadingOrderOfLastPallet = -1;
                                }
                                else
                                {
                                    unloadingOrderOfLastPallet = loadingMatrix[i, j].pallets.Last().unloadingOrder;
                                }

                                if (p.unloadingOrder == unloadingOrderOfLastPallet ||
                                     p.unloadingOrder == unloadingOrderOfLastPallet - 1 || unloadingOrderOfLastPallet == -1)
                                {
                                    insertSucceded = loadingMatrix[i, j].AddPallet(orderedPallets[0]);
                                }
                                else
                                {
                                    insertSucceded = false;
                                }
                                
                                if (insertSucceded)
                                    p.removed = true;
                                break;

                            }

                            for(int k = 0; k < orderedPallets.Count; k++)
                            {
                                if (orderedPallets[k].removed)
                                    orderedPallets.Remove(orderedPallets[k]);
                            }
                               
                        }
                        else
                        {
                            insertSucceded = false;
                        }
                    }
                }
            }

            if (orderedPallets.Count != 0)
            {
                string excludedPallets = "";
                foreach (Pallet p in orderedPallets)
                {
                    if(excludedPallets != "")
                    {
                        excludedPallets = excludedPallets + "," + p.ID;
                    }
                    else
                    {
                        excludedPallets = excludedPallets + p.ID;
                    }
                }
                Console.WriteLine("Attenzione! Caricamento: " + truck.name + ": non tutti i pallet sono stati inseriti per mancanza di spazio." +
                    " Rimangono esclusi \n i pallet: [" + excludedPallets + "]");
            }
            else
                Console.WriteLine("Caricamento: " + truck.name + ": tutti i pallet sono stati inseriti correttamente.");

            return loadingMatrix;
        }

    }
}
