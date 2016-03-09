using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0, 100000, (i) =>
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost/watchdog/");//api/v1/logentries/app1234/debug");
                        HttpResponseMessage response = client.PostAsJsonAsync("api/v1/logentries/app1234/debug", new { message = "test message", occurredOn = DateTime.Now }).Result;
                        Console.WriteLine("Response: {0}", response);
                    }
                });
        }
    }
}
