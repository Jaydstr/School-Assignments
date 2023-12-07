//Group members
//Jaida Mendez – 0698734
//Amy Paterson - 0641278S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_emergency_room
{
    class Program                                   //main program
    {
        static void Main(string[] args)
        {
            try
            {
                int N = 3;                          // number of doctors
                int T = 1000;                       // time to treat each patient (seconds)
                int M = 60;                         // mean of time between patient arrivals (M seconds)

                
                Simulation simulation = new Simulation(N);
                simulation.RunSimulation(T, M);     //run simulation using the given T and M

                Console.WriteLine("The hopital is now closed, all patients have been treated.");
                Console.ReadKey();
            }
            catch                                   //will catch any errors and printout the error message
            {
                Console.WriteLine("An error has occured, please close the window and try again.");
            }
        }
    }
}
