using System.Security.Claims;
using Domain.Enums;
using Infrastructure.Authentication;

namespace WebAPI.Authentication;

public static class ClaimExtractor
{
    public static ExtractedClaims GetUserInfo(ClaimsPrincipal user)
    {
        var claimId = user.FindFirst(CustomClaims.Id)!.Value;
        long.TryParse(claimId, out var userId);

        var claimUserType = user.FindFirst(CustomClaims.UserType)!.Value;
        Enum.TryParse<UserType>(claimUserType, true, out var userType);

        return new ExtractedClaims()
        {
            Id = userId,
            UserType = userType,
        };
    }
}