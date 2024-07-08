using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorkWithPost.Models;

namespace WorkWithPost.Services
{
    public class ApiService
    {
        public async Task<LettersQueryResult?> GetLetters(string searchString, int page, int limit)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7261/api/Letter?page={page}&limit={limit}&searchString={searchString}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    LettersQueryResult letters = JsonConvert.DeserializeObject<LettersQueryResult>(json);
                    return letters;
                }
                else { return null; }
            }
        }
    }
}
