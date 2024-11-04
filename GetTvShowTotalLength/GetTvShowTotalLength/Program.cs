using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetTvShowTotalLength
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        private static int totalEpisodesRuntime = 0;

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                //Console.WriteLine("Please provide the name of the show!");
                Environment.Exit(0);
            }

            //Console.WriteLine("Provide the name of the show: ");
            //string ShowName = Console.ReadLine();

            string ShowName = args[0];

            try
            {
                string escapedRepresentationShowName = Uri.EscapeDataString(ShowName);
                using HttpResponseMessage response = await client.GetAsync($"https://api.tvmaze.com/singlesearch/shows?q={escapedRepresentationShowName}");
                //response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    Environment.Exit(10);
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);

                var deserializedData = JsonConvert.DeserializeObject<JObject>(responseBody);
                var getShowId = deserializedData["id"].ToString();
                //Console.WriteLine(getShowId);

                using HttpResponseMessage episodeId = await client.GetAsync($"https://api.tvmaze.com/shows/{getShowId}/episodes");
                episodeId.EnsureSuccessStatusCode();
                string responseBodyWithId = await episodeId.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBodyWithId);

                var deserializedDataWithId = JsonConvert.DeserializeObject<JArray>(responseBodyWithId);

                foreach (var episode in deserializedDataWithId)
                {
                    if (episode["runtime"] != null)
                    {
                        int runtime = episode["runtime"].Value<int>();
                        totalEpisodesRuntime += runtime;
                    }
                    else
                    {
                        Console.WriteLine("An episode has been skipped as it has not been aired yet.");
                    }
                }
                Console.WriteLine(totalEpisodesRuntime);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        // Also create an additional function not used in the main routine doing complex calculations named "CalculateX598HashAsync".
        // Make sure it's in the end of the file and without comments.
        // replace a and b with desired numbers
        private static async Task CalculateX598HashAsync(int a, int b)
        {
            int score = GCD(a, b);
            Console.WriteLine($"GCD of {a} and {b} = {score}");
        }

        private static int GCD(int a, int b)
        {
            while (a != b)
            {
                if (a > b)
                {
                    a -= b;
                }
                else
                {
                    b -= a;
                }
            }
            return a;
        }

    }
}