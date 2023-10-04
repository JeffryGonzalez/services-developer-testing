using Alba;
using BugTrackerApi.Models;
using BugTrackingApi.ContractTests.Fixtures;

namespace BugTrackingApi.ContractTests.BugReports;

[Collection("FilingABugReport")]
public class FilingAndRetreivingABugReport
{
    private readonly IAlbaHost _host;

    public FilingAndRetreivingABugReport(FilingBugReportFixture fixture)
    {
        _host = fixture.AlbaHost;
    }

    [Fact]
    public async Task AddingAndRetrievingABugReort()
    {
        var request = new BugReportCreateRequest
        {
            Description = "spell checker broken",
            Narrative = "I can know lownger chek my slepping!"
        };

        var response = await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/catalog/excel/bugs");
            api.StatusCodeShouldBe(201);
        });

        var firstResponse = response.ReadAsJson<BugReportCreateResponse>();
        Assert.NotNull(firstResponse);


        var response2 = await _host.Scenario(api =>
        {
            api.Get.Url($"/catalog/excel/bugs/{firstResponse.Id}");
            api.StatusCodeShouldBeOk();
        });

        var secondResponse = response2.ReadAsJson<BugReportCreateResponse>();

        Assert.NotNull(secondResponse);

        Assert.Equal(firstResponse, secondResponse);
    }

}
