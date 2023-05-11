using System;
using System.Collections.Generic;
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
            var keepGoing = true;

            while (keepGoing)
            {
                Console.Clear();
                Console.Write("(R)andom joke, (T)en random jokes, or (Q)uit:");
                var choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "Q":
                        keepGoing = false;
                        break;
                    case "R":
                        var client = new HttpClient();

                        var responseAsStream = await client.GetStreamAsync("https://official-joke-api.appspot.com/random_joke");

                        var randomJoke = await JsonSerializer.DeserializeAsync<Joke>(responseAsStream);

                        Console.WriteLine(randomJoke.Setup);
                        Console.Read();
                        Console.WriteLine(randomJoke.PunchLine);
                        Console.Read();
                        break;
                    case "T":
                        var client2 = new HttpClient();

                        var responseAsStream2 = await client2.GetStreamAsync("https://official-joke-api.appspot.com/random_ten");

                        var tenRandomJokes = await JsonSerializer.DeserializeAsync<List<Joke>>(responseAsStream2);

                        foreach (var joke in tenRandomJokes)
                        {
                            Console.WriteLine(joke.Setup);
                            Console.WriteLine(joke.PunchLine);
                            Console.WriteLine();
                        }
                        Console.Read();
                        break;
                }
            }
        }
    }
}
