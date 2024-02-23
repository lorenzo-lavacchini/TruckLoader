using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using TruckLoader;

Truck furgone = new Truck(2.14, 3.90, 2.05, "furgone");
Truck motrice = new Truck(2.46, 7.20, 2.48, "motrice");
Truck bilico = new Truck(2.46, 13.60, 2.50, "bilico");

List<Truck> trucks = new List<Truck>() {furgone, motrice, bilico};

Pallet p1 = new Pallet("A", 1.0, 1, false);
Pallet p2 = new Pallet("B", 1.24, 1, true);
Pallet p3 = new Pallet("C", 2.0, 2, true);
Pallet p4 = new Pallet("D", 2.1, 3, true);
Pallet p5 = new Pallet("E", 0.5, 3, false);
Pallet p6 = new Pallet("F", 0.5, 3, true);
Pallet p7 = new Pallet("G", 0.3, 4, true);
Pallet p8 = new Pallet("H", 0.6, 4, true);
Pallet p9 = new Pallet("I", 1.6, 5, true);
Pallet p10 = new Pallet("J", 0.3, 6, false);
Pallet p11 = new Pallet("K", 1.3, 6, false);
Pallet p12 = new Pallet("L", 1.3, 6, false);

List<Pallet> carico = new List<Pallet>();

carico.Add(p1);
carico.Add(p2);
carico.Add(p3);
carico.Add(p4);
carico.Add(p5);
carico.Add(p6); 
carico.Add(p7);
carico.Add(p8);
carico.Add(p9);
carico.Add(p10);
carico.Add(p11);
carico.Add(p12);

/*
 * false -> pallet tutti inseriti con lato lungo perpendicolare alla lunghezza del mezzo
 * true -> pallet tutti inseriti con lato lungo parallelo alla lunghezza del mezzo
*/

bool orientazionePallet = true;

Analyzer an = new Analyzer(trucks, carico, orientazionePallet);

List<Result> results = an.Analyze();


foreach (Result result in results)
{
    Console.WriteLine("------------------------------------------------------------------");
    Console.WriteLine(result.message);
    int rows = result.loadingMatrix.GetLength(0);
    int columns = result.loadingMatrix.GetLength(1);
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < columns; j++)
        {
            Console.Write($"[{i},{j}] = {Convert.ToString(result.loadingMatrix[i, j].GetAllPalletsInsideToString())}        ");
        }
        Console.WriteLine();
    }
}



// Stampa gli elementi della matrice con numeri di riga e colonna
