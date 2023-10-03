using Slugify;

namespace SlugUtils;

public class SlugGenerator
{


    public string GenerateSlug(string input)
    {
        var config = new SlugHelperConfiguration();

        var slugger = new SlugHelper(config);
        return slugger.GenerateSlug(input);
    }
}