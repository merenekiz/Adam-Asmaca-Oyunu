using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
                                // Kategori ve kelimeler tanımladım
        var kategoriler = new Dictionary<string, string[]>()
        {
            { "Hayvanlar", new string[] { "kedi", "köpek", "kuş", "balık", "fil", "aslan", "kaplan", "tavşan" } },
            { "Meyveler", new string[] { "elma", "armut", "muz", "çilek", "portakal", "şeftali", "kiraz", "üzüm" } },
            { "Ülkeler", new string[] { "türkiye", "almanya", "fransa", "italya", "japonya", "kanada", "brazil", "avustralya" } },
            { "Teknoloji", new string[] { "bilgisayar", "telefon", "tablet", "yazılım", "robot", "internet", "yapay zeka", "programlama" } },
            { "Renkler", new string[] { "kırmızı", "mavi", "yeşil", "sarı", "turuncu", "mor", "pembe", "siyah" } },
            { "Filmler", new string[] { "inception", "gladyatör", "matrix", "avatar", "interstellar", "parasite", "joker", "titanic" } },
            { "Oyunlar", new string[] { "fortnite", "minecraft", "valorant", "overwatch", "zelda", "tetris", "amongus", "halo", "pacman", "doom" } },
            { "Yiyecekler", new string[] { "pizza", "hamburger", "sushi", "kebap", "salata", "çorba", "dondurma", "makarna" } }
        };

                            // Kategorileri ekrana yazdırdım
        Console.WriteLine("Kategoriler:");
        int index = 1;
        List<string> kategoriListesi = new List<string>(kategoriler.Keys);

        foreach (var kategori in kategoriListesi)
        {
            Console.WriteLine($"{index++}. {kategori}");
        }

                         // Kullanıcının kategori seçmesini sağladım
        int secim = GetCategoryChoice(kategoriListesi.Count);
        string kategoriSecim = kategoriListesi[secim - 1];
        string[] kelimeler = kategoriler[kategoriSecim];

                           // Rastgele bir kelime seçtirtim
        Random rastgele = new Random();
        string secilenKelime = kelimeler[rastgele.Next(kelimeler.Length)];
        HashSet<char> tahminler = new HashSet<char>();
        HashSet<string> yanlisTahminler = new HashSet<string>();
        int hak = 10;                  // 10 tane tahmin hakkı verdim 

           // Oyun döngüsünü başlattım
        while (hak > 0)
        {
            Console.Clear();
            DisplayGameState(kategoriSecim, hak, tahminler, yanlisTahminler, secilenKelime);
            string tahmin = Console.ReadLine();

                                     // Harf tahmini kontrol eden kod
            if (tahmin.Length == 1 && char.TryParse(tahmin, out char harf))
            {
                HandleLetterGuess(ref hak, secilenKelime, harf, tahminler);
            }
                                     // Kelime tahmini kontrol eden kod
            else
            {
                HandleWordGuess(ref hak, secilenKelime, tahmin, yanlisTahminler);
            }

                                    // Kelime tamamlandı mı kontrol eden kod
            if (KelimeTamamlandiMi(secilenKelime, tahminler))
            {
                Console.Clear();
                Console.WriteLine("Tebrikler! Doğru Kelime: " + secilenKelime);
                Console.WriteLine("Oyunu kapatmak için bir tuşa basın...");
                Console.ReadKey();        // Kullanıcının hanhangi bir tuşa basmasını bekler
                Environment.Exit(0);      // Programı kapatır
            }
        }

        // Kullanıcı 10 hakkını bitirdiğini bildiren kod
        if (hak == 0)
        {
            Console.Clear();
            Console.WriteLine("Kaybettiniz! Doğru Kelime: " + secilenKelime);
        }
    }

    // Kullanıcının kategori seçmesini sağlayan kod
    static int GetCategoryChoice(int maxChoice)
    {
        while (true)
        {
            Console.Write($"Bir kategori seçin (1-{maxChoice}): ");
            if (int.TryParse(Console.ReadLine(), out int secim) && secim >= 1 && secim <= maxChoice)
            {
                return secim;
            }
            Console.WriteLine("Geçersiz kategori seçimi! Lütfen tekrar deneyin.");
        }
    }

    // Oyun durumunu ekrana yazdıran kod
    static void DisplayGameState(string kategoriSecim, int hak, HashSet<char> tahminler, HashSet<string> yanlisTahminler, string secilenKelime)
    {
        Console.WriteLine($"Kategori: {kategoriSecim}");
        Console.WriteLine("Kalan hak: " + hak);
        Console.WriteLine("Tahmin edilen harfler: " + string.Join(", ", tahminler));
        Console.WriteLine("Yanlış tahminler: " + string.Join(", ", yanlisTahminler));
        Console.WriteLine("Kelime: " + GosterKelime(secilenKelime, tahminler));
        Console.Write("Bir harf tahmin edin veya kelimeyi tahmin edin: ");
    }

    // Harf tahminini oyuna işleyen kod
    static void HandleLetterGuess(ref int hak, string kelime, char harf, HashSet<char> tahminler)
    {
        if (tahminler.Contains(harf))
        {
            Console.WriteLine("Bu harfi zaten tahmin ettiniz!");
            return;
        }

        tahminler.Add(harf);

        if (!kelime.Contains(harf))
        {
            hak--;
            Console.WriteLine("Yanlış tahmin!");
        }
    }

    // Kelime tahminini oyuna işleyen kod
    static void HandleWordGuess(ref int hak, string kelime, string tahmin, HashSet<string> yanlisTahminler)
    {
        if (tahmin.Equals(kelime, StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            Console.WriteLine("Tebrikler! Doğru Kelime: " + kelime);
            Console.WriteLine("Oyunu kapatmak için bir tuşa basın...");
            Console.ReadKey();              // Kullanıcının bir tuşa basmasını bekler
            Environment.Exit(0);            // Programı kapatır
        }
        else
        {
            yanlisTahminler.Add(tahmin);
            Console.WriteLine("Yanlış kelime tahmini!");
            hak--;
        }
    }

    // Kelimeyi oyunda gösteren kod
    static string GosterKelime(string kelime, HashSet<char> tahminler)
    {
        List<string> gosterim = new List<string>();
        foreach (char harf in kelime)
        {
            gosterim.Add(tahminler.Contains(harf) ? harf.ToString() : "_");
        }
        return string.Join(" ", gosterim); // Harflerin arasına boşluk ekleyen kod
    }

    // Kelimenin tamamlanıp tamamlanmadığını kontrol eden kod
    static bool KelimeTamamlandiMi(string kelime, HashSet<char> tahminler)
    {
        foreach (char harf in kelime)
        {
            if (!tahminler.Contains(harf))
                return false;
        }
        return true;
    }
}
