using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{

    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly string _owner;
        private readonly string _repo;
        private readonly string _url;
        private readonly string _token;

        public GitHubService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _url = _config["GitHub:Url"]!;
            _owner = _config["GitHub:Owner"]!;
            _repo = _config["GitHub:Repo"]!;
            _token = _config["GitHub:Token"]!;
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitIssueAPI", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        }

        public async Task<string> GetIssueAsync(int issueNumber)
        {
            var url = $"{_url}/{_owner}/{_repo}/issues/{issueNumber}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetIssuesAsync()
        {
            var url = $"{_url}/{_owner}/{_repo}/issues";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateIssueAsync([FromBody] Issue issue)
        {
            _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            var url = $"{_url}/{_owner}/{_repo}/issues";
            var content = new StringContent(JsonSerializer.Serialize(issue), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateIssueAsync([FromBody] Issue issue, int issueNumber)
        {
            var url = $"{_url}/{_owner}/{_repo}/issues/{issueNumber}";
            var currentDataResponse = await _httpClient.GetAsync(url);
            currentDataResponse.EnsureSuccessStatusCode();
            var currentJson = await currentDataResponse.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(currentJson);
            var currentTitle = doc.RootElement.GetProperty("title").GetString();
            var currentBody = doc.RootElement.GetProperty("body").GetString();
            var newTitle = string.IsNullOrWhiteSpace(issue.Title) ? currentTitle : issue.Title;
            var newBody = string.IsNullOrWhiteSpace(issue.Body) ? currentBody : issue.Body;
            var patchData = new
            {
                title = newTitle,
                body = newBody
            };
            var content = new StringContent(JsonSerializer.Serialize(patchData), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CloseIssueAsync(int issueNumber)
        {
            var url = $"{_url}/{_owner}/{_repo}/issues/{issueNumber}";
            var patchData = new { state = "closed" };
            var content = new StringContent(JsonSerializer.Serialize(patchData), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}