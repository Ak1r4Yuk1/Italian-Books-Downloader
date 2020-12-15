using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace BooksConsole
{
    class Program
    {

        public static string GetDirectoryListingRegexForUrl(string url)
        {
            if (url.Equals(url))
            {
                return "<a href=\".*\">(?<name>.*)</a>";
            }
            throw new NotSupportedException();
        }
        public static string GetDirectoryFiles(string url)
        {
            if (url.Equals(url))
            {
                return "<a href=\".*\">(?<name>.*)</a>";
            }
            throw new NotSupportedException();
        }
        public static void Main(String[] args)
        {
            try
            {
                Console.WriteLine("Inserisci il nome dell'autore");
                string name = Console.ReadLine();
                string author = name.ToLower().Replace(" ", "-");
                string url = $"https://dwnlg.tel/book-n/{author}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string html = reader.ReadToEnd();
                        Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));
                        MatchCollection matches = regex.Matches(html);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine(match.Groups["name"].ToString().ToUpper().Replace("-"," "));
                                }
                            }
                        }
                        ListFiles(author);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Non ci sono libri per questo autore");
                Console.ReadKey();
            }

        }
        public static void ListFiles(string author)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Scegli un titolo");
            Console.ForegroundColor = ConsoleColor.Blue;
            string titolo = Console.ReadLine().ToLower().Replace(" ", "-");
            string url = $"https://dwnlg.tel/book-n/{author}/{titolo}/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    Regex regex = new Regex(GetDirectoryFiles(url));
                    MatchCollection matches = regex.Matches(html);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            if (match.Success)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(match.Groups["name"].ToString().ToUpper().Replace("-", " "));
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Scegli un formato");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    var format = Console.ReadLine().ToString().ToLower().Replace(" ", "-");
                    string urlf = $"https://dwnlg.tel/book-n/{author}/{titolo}/{format}";
                    FileFormat(urlf, titolo);
                    


                }
            }

        }
        public static void FileFormat(string urlf, string titolo)
        {
            using (var client = new WebClient())
            {
                var dir = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                client.DownloadFile(urlf, $"{dir}/{titolo}.pdf");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Download Completato\n");
            Console.WriteLine("Premi un tasto per uscire");
            Console.ReadKey();
        }
    }
}

