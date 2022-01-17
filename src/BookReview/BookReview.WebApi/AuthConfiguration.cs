using BookReview.Service;

namespace BookReview.WebApi;

public class AuthConfiguration : IAuthConfiguration
{
    private readonly IConfiguration _configuration;

    public AuthConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetConnectionString()
    {
        return _configuration.GetConnectionString("Auth");
    }

    public int? GetMinimumPasswordLength()
    {
        return _configuration.GetValue<int?>("MinimumPasswordLength");
    }
}
