using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = "";
            string pass = "";
            string authorityUrl = "";
            string resource = "";
            string requestUri = "";

            var credentials = new UserPasswordCredential(user, pass);
            var context = new AuthenticationContext(authorityUrl);
            var authResult = context.AcquireTokenAsync(resource, "6ee49733-a2ed-47a2-8dd3-be070d0a38d6", credentials).Result;

            using (HttpClient httpClient1 = new HttpClient())
            {
                httpClient1.BaseAddress = new Uri(resource);
                httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                HttpResponseMessage response = httpClient1.GetAsync(requestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine("Success");
                }
                string message = response.Content.ReadAsStringAsync().Result;
                System.Console.WriteLine("URL responese : " + message);
            }

            Console.ReadLine();
        }
    }
}