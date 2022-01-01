using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PMDConsole
{
    internal class Program
    {


        private static readonly HttpClient _httpClient = new HttpClient();

        static void Main(string[] args)
        {

            string chapter = "301";
            string extension = ".jpg";
            int numberOfFiles = 18;
            string fileName = string.Empty;



           


            for (int i = 16; i <= numberOfFiles; i++)
            {



               


                var result = OrganizeUrl("http://umangas.club/leitor/mangas/Boku%20no%20Hero%20Academia%20(pt-br)/301/0000.jpg",
                    fileName, extension, i, chapter);

                var isURLValid = IsUrlValidAsync(result.uri);

                if (isURLValid.Result)
                {

                    DownloadImageAsync(result.uri, @"C:\ESD-VSEN\0000_1.png");
                }
                else if (isURLValid.Result == false && extension == ".jpg")
                {

                    //everything working fine, but what do that when U change to png
                    //and then you need to change back to jpg and vice-versa
                    //without ifs and ifs


                    //since I believe that it came here because of the extension
                    //try with a new extension
                    extension = ".png";
                    //var result2 = OrganizeUrl(fileName, extension, i, chapter);
                    //DownloadImage(result2.uri, result2.fileName);
                }
                else
                {
                    Console.WriteLine($"Something wrong {result.uri}");

                }

            }


            Console.WriteLine("Download complete");
            Console.ReadLine();

        }

       

        static (string uri, string fileName) OrganizeUrl(string uri,string outputFileName, string extension, int chapterNumber, string chapter = "")
        {

            if (chapterNumber <= 9)
            {
                outputFileName = $"000{chapterNumber}{extension}";
                uri = $"http://umangas.club/leitor/mangas/Boku%20no%20Hero%20Academia%20(pt-br)/{chapter}/{outputFileName}";

                return (uri, outputFileName);
            }
            else
            {
                outputFileName = $"00{chapterNumber}{extension}";
                uri = $"http://umangas.club/leitor/mangas/Boku%20no%20Hero%20Academia%20(pt-br)/{chapter}/{outputFileName}";
                return (uri, outputFileName);
            }

        }


        public static async void DownloadImageAsync(string uri, string outputPath)
        {
            Uri uriResult;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                throw new InvalidOperationException("URI is invalid.");


            byte[] fileBytes = await _httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(outputPath, fileBytes);
        }


        public static async Task<bool> IsUrlValidAsync(string url = "http://umangas.club/leitor/mangas/Boku%20no%20Hero%20Academia%20(pt-br)/301/0000.jpg")
        {
            try
            {

                var result = await _httpClient.GetAsync(url);
                return result.StatusCode != HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Could not test url {0}.", url), ex);
            }
            return false;
        }




    }
}
