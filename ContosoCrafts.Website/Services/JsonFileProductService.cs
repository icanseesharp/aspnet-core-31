using System;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Text.Json;
using ContosoCrafts.Website.Models;

namespace ContosoCrafts.Website.Services
{
    public class JsonFileProductService
    {
        public IWebHostEnvironment WebHostEnvironment;
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<Product> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetProducts();
            if (products.First(p => p.Id == productId).Ratings == null)
            {
                products.First(p => p.Id == productId).Ratings = new int[] { rating };
            }
            else
            {
                var ratings = products.First(p => p.Id == productId).Ratings.ToList();
                ratings.Add(rating);
                products.First(p => p.Id == productId).Ratings = ratings.ToArray();
            }

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<Product>>(new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    Indented = true,
                    SkipValidation = true
                }),
                products);
            }
        }
    }
}