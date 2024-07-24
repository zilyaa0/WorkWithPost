using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorkWithPost.Models;
using WorkWithPost.ViewModel;

namespace WorkWithPost.Services
{
    #region Interface
    interface IApiService
    {
        Task<LettersQueryResult?> GetLetters(string searchString, int page, int limit);
        Task<LettersQueryResult?> GetAllLetters(int page, int limit);
        Task<List<FileName>> GetFilesNames(string uniqueId);
        Task LoadFile(string uniqueId, string fileName);
    }
    #endregion

    #region Public Metods
    public class ApiService : IApiService
    {
        public async Task<LettersQueryResult?> GetLetters(string searchString, int page, int limit)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7261/api/Letter/GetLettersByPage?page={page}&limit={limit}&searchString={searchString}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    LettersQueryResult letters = JsonConvert.DeserializeObject<LettersQueryResult>(json);
                    return letters;
                }
                else { return null; }
            }
        }

        public async Task<LettersQueryResult?> GetAllLetters(int page, int limit)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7261/api/Letter/GetAllLetters?page={page}&limit={limit}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    LettersQueryResult letters = JsonConvert.DeserializeObject<LettersQueryResult>(json);
                    return letters;
                }
                else { return null; }
            }
        }

        public async Task<List<FileName>> GetFilesNames(string uniqueId)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7261/api/Letter/GetFilesNames?uniqueId={uniqueId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<FileName> files = JsonConvert.DeserializeObject<List<FileName>>(json);
                    return files;
                }
                else { return null; }
            }
        }

        public async Task LoadFile(string uniqueId, string fileName)
        {
            using (var client = new HttpClient())
            {
                var pathDir = Path.Combine(Environment.CurrentDirectory, "Emails", uniqueId);
                if (!Directory.Exists(pathDir))
                    Directory.CreateDirectory(pathDir);
                var path = Path.Combine(pathDir, fileName);
                if (!File.Exists(path))
                {
                    await using var stream = await client.GetStreamAsync($"https://localhost:7261/api/Letter/LoadFile?uniqueId={uniqueId}&fileName={fileName}");
                    await using var file = File.Create(path);
                    await stream.CopyToAsync(file);
                }
                var process = new Process();
                process.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                process.Start();
            }
        }
        #endregion
    }
}
