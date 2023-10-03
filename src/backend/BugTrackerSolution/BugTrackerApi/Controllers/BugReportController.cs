using BugTrackerApi.Models;
using BugTrackerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerApi.Controllers;

public class BugReportController : ControllerBase
{
    private readonly ISystemTime _systemTime;

    public BugReportController(ISystemTime systemTime)
    {
        _systemTime = systemTime;
    }

    [Authorize]
    [HttpPost("/catalog/excel/bugs")]
    public async Task<ActionResult> AddABugReport([FromBody] BugReportCreateRequest request)
    {
        var name = User.Identity.Name;
        var response = new BugReportCreateResponse
        {
            Id = "excel-goes-boom",
            Issue = request,
            Software = "Excel",
            Status = IssueStatus.InTriage,
            User = name,
            Created = _systemTime.GetCurrent()
        };
        return StatusCode(201, response);
    }
}
