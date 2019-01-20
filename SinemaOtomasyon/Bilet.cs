using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;

namespace SinemaOtomasyon
{
    class Bilet : Film
    {
        public string[] AdSoyad { get; set; }
        public int KisiSayisi { get; set; }
        public string[] Koltuksecim { get; set; }
        public static List<Bilet> bilets = new List<Bilet>();
        string BiletDosyaYolu = @"D:\github-project\SinemaOtomasyon\SinemaOtomasyon\Biletler.txt";

        /// <summary>
        /// Filmler Listelenip seçilen filme göre
        /// </summary>
        public void BiletAlMenu()
        {
            FilmleriCek();
            Console.WriteLine("Film Seçiniz");
            ConsoleKeyInfo cki;
            bool menusecim = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"1-{films[0].FilmAdı}\n2-{films[1].FilmAdı}\n3-{films[2].FilmAdı}");
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.Clear();
                        BiletAl(0);
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.Clear();
                        BiletAl(1);
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Console.Clear();
                        BiletAl(2);
                        break;
                    default:
                        menusecim = true;
                        break;
                }
            } while (true);
        }

        /// <summary>
        /// Film numarasına göre bilet değerlerin girildiği metod.
        /// </summary>
        /// <param name="filmno"></param>
        public void BiletAl(int filmno)
        {
            Bilet ybilet = new Bilet();
            ybilet.BiletCek();
            Console.WriteLine($"Film Adı: {films[filmno].FilmAdı}\nSalon No: {films[filmno].SalonNo}\nSüre: {films[filmno].FilmSure} dk");
            Koltuk[] KoltukKontrol = films[filmno].koltuks;
            Console.WriteLine("Kaç Kişilik Bilet İstersiniz");
            int kisisayisi;
            while (!int.TryParse(Console.ReadLine(), out kisisayisi))
            {
                Console.WriteLine("Lütfen Sayısal değer giriniz");
            }
            ybilet.KisiSayisi = kisisayisi;
            ybilet.AdSoyad = new string[ybilet.KisiSayisi];
            ybilet.Koltuksecim = new string[ybilet.KisiSayisi];
            bool koltukdolumu=false;

            for (int i = 0; i < ybilet.KisiSayisi; i++)
            {
                Console.Clear();
                films[filmno].KoltukListele();
                Console.WriteLine($"{i + 1}. Kişinin adını soyadını Giriniz");
                ybilet.AdSoyad[i] = Console.ReadLine();
                do
                {
                    Console.WriteLine("Yukarıda Listelenmiş Kısımda Yeşil Renkli Koltuklardan bir tanesini seçiniz");
                    string SecilenKoltuk = Console.ReadLine().ToUpper();

                    for (int k = 0; k < KoltukKontrol.Length; k++)
                    {
                        if (KoltukKontrol[k].KoltukS == SecilenKoltuk && KoltukKontrol[k].DoluBos == false)
                        {
                            films[filmno].koltuks[k].DoluBos = true;
                            ybilet.Koltuksecim[i] = SecilenKoltuk;
                            koltukdolumu = false;
                            break;
                        }
                        else if (KoltukKontrol[k].KoltukS == SecilenKoltuk && KoltukKontrol[k].DoluBos == true)
                        {
                            koltukdolumu = true;
                            break;
                        }
                    }
                    if (koltukdolumu)
                    {
                        Console.WriteLine("Lütfen Boş Koltuklardan birini giriniz");
                    }
                } while (koltukdolumu);
            }
            ybilet.FilmAdı = films[filmno].FilmAdı;
            ybilet.FilmSure = films[filmno].FilmSure;
            ybilet.SalonNo = films[filmno].SalonNo;
            bilets.Add(ybilet);
            FilmLog();
            BiletLog();
        }

        /// <summary>
        /// Bilets listesini
        /// </summary>
        public void BiletLog()
        {
            using (StreamWriter strw = new StreamWriter(BiletDosyaYolu))
            {
                foreach (Bilet item in bilets)
                {
                    strw.Write(item.FilmAdı + ",");
                    strw.Write(item.FilmSure + ",");
                    strw.Write(item.SalonNo + ",");

                    for (int i = 0; i < item.AdSoyad.Length; i++)
                    {
                        if (i+1 == item.AdSoyad.Length)
                            strw.Write(item.AdSoyad[i] + ",");
                        else
                            strw.Write(item.AdSoyad[i] + "-");
                    }
                    for (int i = 0; i < item.Koltuksecim.Length; i++)
                    {
                        if (i+1 == item.Koltuksecim.Length)
                            strw.Write(item.Koltuksecim[i]);
                        else
                            strw.Write(item.Koltuksecim[i] + "-");
                    }
                    strw.WriteLine();
                }
            }
        }

        /// <summary>
        /// Biletler.txt dosyasından okuduğu değerleri bilets listesinde uygun alanlara aktarır.
        /// </summary>
        public void BiletCek()
        {
            bilets.Clear();
            int satirsayisi = File.ReadAllLines(BiletDosyaYolu).Length;

            FileStream fs = new FileStream(BiletDosyaYolu, FileMode.Open);

            using (StreamReader stread = new StreamReader(fs))
            {
                for (int i = 0; i < satirsayisi; i++)
                {
                    Bilet ybilet = new Bilet();
                    string satir = stread.ReadLine();
                    string[] satirdeger = satir.Split(',');
                    for (int j = 0; j < satirdeger.Length; j++)
                    {
                        ybilet.FilmAdı = satirdeger[0];
                        ybilet.FilmSure = int.Parse(satirdeger[1]);
                        ybilet.SalonNo = int.Parse(satirdeger[2]);
                        string[] okunanAdlar = satirdeger[3].Split('-');
                        ybilet.AdSoyad = okunanAdlar;
                        string[] okunanKoltuklar = satirdeger[4].Split('-');
                        ybilet.Koltuksecim = okunanKoltuklar;
                    }
                    bilets.Add(ybilet);
                }
            }
        }
    }
}