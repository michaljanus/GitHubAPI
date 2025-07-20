using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Interfaces
{
    public interface IGitHubService
    {
        Task<string> GetIssueAsync(int issueNumber);
        Task<string> GetIssuesAsync();
        Task<string> CreateIssueAsync(Issue issue);
        Task<string> UpdateIssueAsync(Issue issue, int issueNumber);
        Task<string> CloseIssueAsync(int issueNumber);
    }
}
