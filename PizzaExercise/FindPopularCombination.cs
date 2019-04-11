using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PizzaExercise.Interfaces;
using PizzaExercise.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PizzaExercise
{
    public class FindPopularCombination : IFindPopularCombination
    {
        private readonly ILogger logger;
        private IConfiguration config { get; }
        private static string fileLocation;
        private static string outputFilePath;
        public FindPopularCombination(IConfiguration configuration)
        {
            config = configuration;
            fileLocation = config.GetSection("fileLoacation").Value;
            outputFilePath = config.GetSection("outputFilePath").Value;
        }
        /// <summary>
        /// Main entry to get data and process
        /// </summary>
        /// <returns></returns>
        public async Task FindCombinationAsync()
        {
            var toppingsData = await GetToppingsAsync();
            FindCombination(toppingsData); 
        }
        /// <summary>
        /// Compute data to find the top 20 topping combination
        /// </summary>
        /// <param name="toppingsData"></param>
        private static void FindCombination(List<Toppings> toppingsData)
        {
            var toppingsOrdered = toppingsData.Select(t => string.Join(",", t.toppings.OrderBy(c => c))).GroupBy(y => y);

            //find count in each group
            var countToppings = new Dictionary<string, int>();
            toppingsOrdered.ToList().ForEach(t =>
            {
                countToppings.Add(t.Key, t.Count());
            });

            //get top 20 combinations
            var top20Combinations = countToppings.OrderByDescending(t => t.Value).Take(20);

            //write to a file
            using (StreamWriter wr = new StreamWriter($"{outputFilePath}\\top20.txt"))
            {
                wr.WriteLine($"Topping:     Count:");
                wr.WriteLine("------------------------------------");
                foreach (var topping in top20Combinations)
                {
                    wr.WriteLine($"{topping.Key} :     {topping.Value}");
                }
            }
        }
        /// <summary>
        /// Fetch data from file location
        /// </summary>
        /// <returns>Deserialized json  of type toppings collection</returns>
        private static async Task<List<Toppings>> GetToppingsAsync()
        {
            var results = new List<Toppings>();
            try
            {

                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, fileLocation))
                    {
                        HttpResponseMessage result = client.SendAsync(request).Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var status = result.StatusCode.ToString();
                        }
                        var response = await result.Content.ReadAsStringAsync();
                        results = JsonConvert.DeserializeObject<List<Toppings>>(response);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured; {ex.Message}");//ideally want log ex for support reasons
            }
            return results;
        }
    }
}
