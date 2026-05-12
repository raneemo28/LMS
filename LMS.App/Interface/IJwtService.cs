using LMS.Domain.Entities;

namespace LMS.App.Interface;

public interface IJwtService
{
    string GenerateJwtToken(ApplicationUser user, string role);
}
