using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SinemaOtomasyon
{
    public interface IFilmler
    {
        string FilmAdı { get; set; }
        int SalonNo { get; set; }
    }

    public struct Koltuk
    {
        public string KoltukS { get; set; }
        public bool DoluBos { get; set; }
    }

    public class Film : IFilmler
    {
        public string FilmAdı { get; set; }
        public int SalonNo { get; set; }
        public int FilmSure { get; set; }
        public Koltuk[] koltuks;

        public string FilmDosyaYolu = @"D:\github-project\SinemaOtomasyon\SinemaOtomasyon\Filmler.txt";

        public static List<Film> films = new List<Film>();

        /// <summary>
        /// Filmleri films listesine ekleme. Default olarak tanımlı dışarıdan almaya mümkün tasarlandı.
        /// </summary>
        public void FilmCreate()
        {
            Film yfilm = new Film();
            yfilm.FilmAdı = "Aquamen";
            yfilm.FilmSure = 120;
            yfilm.KoltukOlustur();
            yfilm.SalonNo = 1;
            FilmEkle(yfilm);
            yfilm = new Film();
            yfilm.FilmAdı = "Glass";
            yfilm.FilmSure = 110;
            yfilm.KoltukOlustur();
            yfilm.SalonNo = 2;
            FilmEkle(yfilm);
            yfilm = new Film();
            yfilm.FilmAdı = "Robin Hood";
            yfilm.FilmSure = 119;
            yfilm.SalonNo = 3;
            yfilm.KoltukOlustur();
            FilmEkle(yfilm);
        }

        /// <summary>
        /// Film Clasında alınan parametreyi Films listesine ekler
        /// </summary>
        /// <param name="filmekle">Film Class</param>
        public void FilmEkle(Film filmekle)
        {
            films.Add(filmekle);
        }

        /// <summary>
        /// films Listesinin txt dosyasına yazar.
        /// </summary>
        public void FilmLog()
        {
            using (StreamWriter strw = new StreamWriter(FilmDosyaYolu))
            {
                foreach (Film item in films)
                {
                    strw.Write(item.FilmAdı + ",");
                    strw.Write(item.FilmSure + ",");
                    strw.Write(item.SalonNo + ",");
                    for (int i = 0; i < item.koltuks.Length; i++)
                    {
                        if (i == item.koltuks.Length-1)
                            strw.Write(item.koltuks[i].KoltukS + "*" + item.koltuks[i].DoluBos);
                        else
                            strw.Write(item.koltuks[i].KoltukS + "*" + item.koltuks[i].DoluBos+"-");
                    }
                    strw.WriteLine();
                }
            }
        }

        /// <summary>
        /// Koltukları boş şekilde oluşturma
        /// </summary>
        public void KoltukOlustur()
        {
            koltuks = new Koltuk[40];
            int sayac = 1;
            for (int i = 0; i < koltuks.Length; i++)
            {
                if (sayac == 9)
                    sayac = 1;

                if (i >= 0 && i <= 7)
                    koltuks[i].KoltukS = "A" + sayac.ToString();
                else if (i>7&& i<=15)
                    koltuks[i].KoltukS = "B" + sayac.ToString();
                else if (i>15 && i<=23)
                    koltuks[i].KoltukS = "C" + sayac.ToString();
                else if (i > 23 && i <= 31)
                    koltuks[i].KoltukS = "D" + sayac.ToString();
                else if (i > 31 && i <= 39)
                    koltuks[i].KoltukS = "E" + sayac.ToString();
                koltuks[i].DoluBos = false;
                sayac++;
            }
        }

        /// <summary>
        /// Koltuk dizisinin ekrana yazdırma metodu
        /// </summary>
        public void KoltukListele()
        {
            for (int i = 1; i <= koltuks.Length; i++)
            {
                if (koltuks[i-1].DoluBos == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(koltuks[i - 1].KoltukS + "-");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(koltuks[i - 1].KoltukS+ "-");
                    Console.ResetColor();
                }

                if (i % 8 == 0&& i != 0)
                {
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Filmleri txt dosyasından okuyup films listesine yükler
        /// </summary>
        public void FilmleriCek()
        {
            films.Clear();
            int satirsayisi = File.ReadAllLines(FilmDosyaYolu).Length;

            FileStream fs = new FileStream(FilmDosyaYolu, FileMode.Open);

            using (StreamReader stread= new StreamReader(fs))
            {
                for (int i = 0; i < satirsayisi; i++)
                {
                    Film yfilm = new Film();
                    string satir = stread.ReadLine();
                    string[] satirdeger = satir.Split(',');
                    for (int j = 0; j < satirdeger.Length; j++)
                    {
                        yfilm.FilmAdı = satirdeger[0];
                        yfilm.FilmSure = int.Parse(satirdeger[1]);
                        yfilm.SalonNo = int.Parse(satirdeger[2]);
                        string[] koltuks = satirdeger[3].Split('-');
                        Koltuk[] ykoltuk = new Koltuk[koltuks.Length];
                        for (int k = 0; k < koltuks.Length; k++)
                        {
                            ykoltuk[k].KoltukS = koltuks[k].Split('*')[0];
                            ykoltuk[k].DoluBos = Convert.ToBoolean(koltuks[k].Split('*')[1]);
                        }
                        yfilm.koltuks = ykoltuk;
                    }
                    films.Add(yfilm);
                }
            }
        }
    }
}
