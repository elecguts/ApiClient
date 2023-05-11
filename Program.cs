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
                Console.Write("(R)andom joke, (T)en random jokes, (S)earch joke library, (F)ind joke by ID or (Q)uit:");
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
                    case "S":
                        Console.WriteLine("What joke topic would you like to search for?");
                        var jokeSearch = Console.ReadLine().ToLower();

                        var client3 = new HttpClient();

                        var responseAsStream3 = await client3.GetStreamAsync($"https://official-joke-api.appspot.com/jokes/{jokeSearch}/random");

                        var topicalJoke = await JsonSerializer.DeserializeAsync<List<Joke>>(responseAsStream3);

                        if (topicalJoke.Count == 1)
                        {
                            foreach (var joke in topicalJoke)
                            {
                                Console.WriteLine(joke.Setup);
                                Console.Read();
                                Console.WriteLine(joke.PunchLine);
                                Console.Read();
                            }
                        }
                        else
                        {
                            Console.WriteLine("I didn't find any jokes on that topic");
                            Console.Read();
                        }
                        break;
                    case "F":
                        Console.WriteLine("What joke number Id would you like to search for?");
                        var searchJokeById = int.Parse(Console.ReadLine());

                        try
                        {
                            var client4 = new HttpClient();

                            var responseAsStream4 = await client4.GetStreamAsync($"https://official-joke-api.appspot.com/jokes/{searchJokeById}");

                            var jokeById = await JsonSerializer.DeserializeAsync<Joke>(responseAsStream4);

                            Console.WriteLine(jokeById.Setup);
                            Console.Read();
                            Console.WriteLine(jokeById.PunchLine);
                            Console.Read();
                        }
                        catch (HttpRequestException)
                        {
                            Console.WriteLine("I didn't find a joke with that Id");
                            Console.Read();
                        }
                        break;
                }
            }
        }
    }
}
