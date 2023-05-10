using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiClient
{
    class Program
    {
        public class Joke
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("setup")]
            public string Setup { get; set; }

            [JsonPropertyName("punchline")]
            public string PunchLine { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }
        }
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            var responseAsStream = await client.GetStreamAsync("https://official-joke-api.appspot.com/random_joke");

            var randomJoke = await JsonSerializer.DeserializeAsync<Joke>(responseAsStream);

            Console.WriteLine(randomJoke.Setup);
            Console.Read();
            Console.WriteLine(randomJoke.PunchLine);
        }
    }
}
