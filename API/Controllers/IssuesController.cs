using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public IssuesController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("{issueNumber}")]
        public async Task<IActionResult> GetIssue(int issueNumber)
        {
            var result = await _gitHubService.GetIssueAsync(issueNumber);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetIssues()
        {
            var result = await _gitHubService.GetIssuesAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] Issue issue)
        {
            var result = await _gitHubService.CreateIssueAsync(issue);

            return Ok(result);
        }

        [HttpPatch("{issueNumber}/update")]
        public async Task<IActionResult> UpdateIssue([FromBody] Issue issue, int issueNumber)
        {
            var result = await _gitHubService.UpdateIssueAsync(issue, issueNumber);

            return Ok(result);
        }

        [HttpPatch("{issueNumber}/close")]
        public async Task<IActionResult> CloseIssue(int issueNumber)
        {
            var result = await _gitHubService.CloseIssueAsync(issueNumber);

            return Ok(result);
        }
    }
}
