using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vjezbanje
{
    class Program
    {
        static string pretvorba (long bajt)
        {
            string Pr= Math.Round(((double)bajt/(1024*1024)), 2).ToString();
            return Pr + "MB"; 
        } // pretvarabje bajtova u mb, zaokruzivanje na decimalno i pretvarenje toga u string
        static void Main(string[] args)
        {
            Process[] procesi; //lista tipa proces
            string prvi = "";
            try
            {
                prvi = args[0]; //nulti element iz liste argumenata
            }
            catch
            {

            }
            
             procesi = Process.GetProcesses().OrderByDescending (proc => proc.WorkingSet64).ToArray(); //uzimanje svih procesa
            

            Console.WriteLine("{0,-3} {1,-8} {2,-25} {3,-13} {4,18}", "RBR", "PID", "NAZIV", "MEM[MB]", "ZAUSTAVLJEN");
            Console.WriteLine("=== ======== ========================= ============= ===================");

            int brojac = 0;
            int ubijeni = 0; // broj ubojenih procesa 
            long trazena_memorija = 0; // sprema se mem izlistanih procesa
            long oslo_mem = 0; // oslobođena memorija kill procesa 
            long svii_mem = 0; //sprema se ukupna memorija svih procesa

            foreach (Process P in procesi){

                svii_mem += P.WorkingSet64;

                if (prvi.ToUpper()==P.ProcessName.ToUpper())
                {
                    bool x = false; 
                    try
                    {
                        P.Kill();
                        ubijeni++;
                        oslo_mem +=P.WorkingSet64;

                        x = true; 
                    }
                    catch
                    {

                    }
                    Console.WriteLine("{0,-3} {1,-8} {2,-25} {3,-13} {4,18}", brojac, P.Id, P.ProcessName, pretvorba(P.WorkingSet64), x?"DA":"NE");
                    brojac++;
                    trazena_memorija += P.WorkingSet64; 


                }
                if (prvi == "")
                {
                    Console.WriteLine("{0,-3} {1,-8} {2,-25} {3,-13}", brojac, P.Id, P.ProcessName, pretvorba(P.WorkingSet64));
                    brojac++;
                    trazena_memorija += P.WorkingSet64;
                }
            } // ispis procesa

            Console.WriteLine("ukupno trazeni/zaustavljenih / svih procesa {0},{1} ,{2}",brojac,ubijeni,procesi.Length);
            Console.WriteLine("ukupno trazena mem./oslobođena mem. / mem. svih procesa {0},{1} ,{2}",pretvorba(trazena_memorija),pretvorba(oslo_mem),pretvorba(svii_mem));
        }
    }
}
