using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        string user = "";
        string pass = "";
        string authority = "";
        string clientId = "";
        string resource = "";
        string requestUri = "";
        string clientSecret = "";

        // Getting access token with user credentials
        public void test1()
        {
            var credentials = new UserPasswordCredential(user, pass);
            var context = new AuthenticationContext(authority);
            var authResult = context.AcquireTokenAsync(resource, clientId, credentials).Result;

            using (HttpClient httpClient1 = new HttpClient())
            {
                httpClient1.BaseAddress = new Uri(resource);
                httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                HttpResponseMessage response = httpClient1.GetAsync(requestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine("Success");
                }
                string message = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("URL responese : " + message);
            }
        }

        /// Access token request with a shared secret
        public async void test2()
        {
            HttpClient client = new HttpClient();

            // get the token
            
            var clientCredential = new ClientCredential(clientId, clientSecret);
            AuthenticationContext context = new AuthenticationContext(authority, false);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(resource, clientCredential); 

            // set the auth header with the aquired Bearer token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

            // make the call to the resource requiring auth!
            var resp = await client.GetAsync(resource + "//" + requestUri);

            // do something with the response
            string message = resp.Content.ReadAsStringAsync().Result;
            Console.WriteLine("URL responese : " + message);
        }

        static void Main(string[] args)
        {
            Program obj = new Program();
            //obj.test1();
            obj.test2();

            Console.ReadLine();
        }

        public Program()
        {
        }
    }
}