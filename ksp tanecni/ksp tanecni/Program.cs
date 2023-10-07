using System;
using System.Collections.Generic;
using System.Linq;

namespace ksp_tanecni
{
    class Sipka
    {
        public int Start;
        public int Konec;

        public List<Sipka> Krach = new List<Sipka>();

        public Sipka(int st, int kon)
        {
            Start = st;
            Konec = kon;
        }

    }
    class Tanecni
    {
        List<Sipka> Ucastnici = new List<Sipka>();
        public IOrderedEnumerable<Sipka> Serazeni;
        List<Sipka> Starteri = new List<Sipka>();

        public void Nacitani()
        {
            int pocetUcastniku = int.Parse(Console.ReadLine());

            for (int i = 0; i < pocetUcastniku; i++)
            {
                    Ucastnici.Add(new Sipka(i+1, int.Parse(Console.ReadLine())));
            }
        }
        public void Bourani()
        {
            for (int i = 0; i < Ucastnici.Count-1; i++)
            {
                for (int j = i+1; j < Ucastnici.Count; j++)
                {
                    
                        decimal krok = Ucastnici[i].Start - Ucastnici[j].Start;
                        decimal pokroku = Ucastnici[i].Konec - Ucastnici[j].Konec;
                        decimal krokpokroku = krok / pokroku;
                        if (krokpokroku < 0)
                        {
                            
                                Ucastnici[i].Krach.Add(Ucastnici[j]);
                                Ucastnici[j].Krach.Add(Ucastnici[i]);
                            
                            
                        }
                    
                    
                }
            }

            Serazeni = Ucastnici.OrderBy(Sipka => Sipka.Krach.Count);
            Ucastnici = Serazeni.ToList<Sipka>();
        }

        public List<Sipka> Startovani()
        {
            List<Sipka> krusnuti = new List<Sipka>();

            foreach(Sipka s in Ucastnici)
            {
                if (!krusnuti.Contains(s))
                {
                    Starteri.Add(s);

                    foreach (Sipka k in s.Krach)
                    {
                        krusnuti.Add(k);
                    }
                }

                
            }


            return Starteri;
        }
        public void Output()
        {
            Nacitani();
            Bourani();
            Startovani();

            Console.WriteLine(Starteri.Count);

            foreach(Sipka s in Starteri)
            {
                Console.WriteLine(s.Start);

            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tanecni tanecni = new Tanecni();

            tanecni.Output();

            Console.ReadKey();
        }
    }
}
