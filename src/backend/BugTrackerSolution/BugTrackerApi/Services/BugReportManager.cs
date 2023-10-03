using BugTrackerApi.Models;
using OneOf;
using SlugUtils;

namespace BugTrackerApi.Services;

public class BugReportManager
{
    private readonly SoftwareCatalogManager _softwareCatalog;
    private readonly ISystemTime _systemTime;
    private readonly SlugUtils.SlugGenerator _slugGenerator;

    public BugReportManager(SoftwareCatalogManager softwareCatalog, ISystemTime systemTime, SlugGenerator slugGenerator)
    {
        _softwareCatalog = softwareCatalog;
        _systemTime = systemTime;
        _slugGenerator = slugGenerator;
    }




    // CreateBugReportAsync(user, software, request);
    public async Task<OneOf<BugReportCreateResponse, SoftwareNotInCatalog>> CreateBugReportAsync(string user, string software, BugReportCreateRequest request)
    {
        var softwareLookup = await _softwareCatalog.IsSofwareInOurCatalogAsync(software);

        if (softwareLookup.TryPickT0(out SoftwareEntity entity, out SoftwareNotInCatalog notFound))
        {
            if (entity is not null)
            {
                return new BugReportCreateResponse
                {
                    Created = _systemTime.GetCurrent(),
                    Id = await _slugGenerator.GenerateSlugAsync(request.Description, (_) => Task.FromResult(true)),
                    Issue = request,
                    Software = entity.Name,
                    Status = IssueStatus.InTriage,
                    User = user
                };
            }

        }
        return new SoftwareNotInCatalog();


    }
}


public record SoftwareNotInCatalog();