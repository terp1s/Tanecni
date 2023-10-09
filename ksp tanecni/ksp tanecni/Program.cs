using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ksp_tanecni
{
    class Sipka : IComparable
    {
        public int Start { get; set; }
        public int Konec { get; set; }

        public List<Sipka> Krach = new List<Sipka>();

        public Sipka(int st, int kon)
        {
            Start = st;
            Konec = kon;
        }

        public int CompareTo(object obj)
        {
            var sipka = obj as Sipka;
            if (sipka == null) 
            {
                throw new ArgumentException();
            }
            return Start.CompareTo(sipka.Start);
        }

       
    }
    class Kombinace
    {
        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }

    class Tanecni
    {
        List<Sipka> Ucastnici = new List<Sipka>();
        public IOrderedEnumerable<Sipka> Serazeni;
        List<Sipka> Starteri = new List<Sipka>();

        public void Nacitani(string path)
        {
            string[] input = File.ReadAllLines(path);

            int pocetUcastniku = int.Parse(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                    Ucastnici.Add(new Sipka(i, int.Parse(input[i])));
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

        public List<Sipka> Startovani(List<Sipka> ucastnici)
        {
            List<Sipka> krusnuti = new List<Sipka>();

            foreach(Sipka s in ucastnici)
            {
                if (!krusnuti.Contains(s))
                {
                    Starteri.Add(s);

                    foreach (Sipka k in s.Krach)
                    {
                        if (!krusnuti.Contains(k))
                        {
                            krusnuti.Add(k);
                        }
                    }
                }
                
            }


            return Starteri;
        }

        public void VsechnyMoznosti()
        {
            IEnumerable<IEnumerable<Sipka>> komba = Kombinace.GetKCombs(Ucastnici, Ucastnici.Count).Select(x => x.ToList()).ToList();

            List<List<Sipka>> holciny = new List<List<Sipka>>();

            foreach(IEnumerable<Sipka> s in komba)
            {
                holciny.Add(s.ToList());
            }

            foreach(List<Sipka> moznost in holciny)
            {
                List<Sipka> krusnuti = new List<Sipka>();

                foreach (Sipka s in moznost)
                {

                    if (!krusnuti.Contains(s))
                    {
                        foreach (Sipka k in s.Krach)
                        {
                            if (!krusnuti.Contains(k))
                            {
                                krusnuti.Add(k);
                            }
                        }
                    }
                    else
                    {
                        moznost.Clear();
                    }

                }

                if(moznost.Count != 0)
                {
                    Console.WriteLine(moznost.Count);
                }
            }

            

        }

        public void Komplet(string path)
        {
            Nacitani(path);
            Bourani();
            Startovani(Ucastnici);
            //VsechnyMoznosti();

            string[] output = new string[Starteri.Count+1];

            output[0] = Starteri.Count.ToString();

            for (int i = 0; i < Starteri.Count; i++)
            {
                output[i+1] = Starteri[i].ToString();
            }

            using (FileStream fs = File.Create(@"C:\Users\pisko\Downloads\output.in"))
            {
                File.WriteAllLines(@"C:\Users\pisko\Downloads\output.in", output);
            }


        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tanecni tanecni = new Tanecni();

            string path = @"C:\Users\pisko\Downloads\01 (3).in";

            tanecni.Komplet(path);

            


            Console.ReadKey();
        }
    }
}
