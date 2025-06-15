using System;
using System.Net;
using System.Text.Json;

namespace PID
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var client = new HttpClient();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("You want to download a picture of which pokemon?");
                string pokemonName = Console.ReadLine().ToLower();
                var pokemon = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{pokemonName}");

                if (pokemon.IsSuccessStatusCode)
                {

                    JsonDocument pokemonObject = JsonSerializer.Deserialize<JsonDocument>(await pokemon.Content.ReadAsStringAsync());

                    string pokemonImage = pokemonObject.RootElement.GetProperty("sprites").GetProperty("front_default").GetString();

                    var pokemonBytes = await client.GetByteArrayAsync(pokemonImage);

                    try
                    {

                        await File.WriteAllBytesAsync($"./{pokemonName}{Path.GetExtension(pokemonImage)}", pokemonBytes);

                        System.Console.WriteLine("Your download is complete.");

                        await Task.Delay(1000);
                    }
                    catch (HttpRequestException ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                    }
                    
                }
                else
                {
                    Console.WriteLine("Invalid Pokemon Name Retry");
                    await Task.Delay(3000);
                }
            }
            
            
        } 
    }
}