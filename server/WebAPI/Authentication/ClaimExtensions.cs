using System.Security.Claims;
using Domain.Enums;
using Infrastructure.Authentication;

namespace WebAPI.Authentication;

public static class ClaimExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var claimId = user.FindFirst(CustomClaims.Id)!.Value;
        long.TryParse(claimId, out var userId);

        return userId;
    }

    public static UserType GetUserType(this ClaimsPrincipal user)
    {
        var claimUserType = user.FindFirst(CustomClaims.UserType)!.Value;
        Enum.TryParse<UserType>(claimUserType, true, out var userType);

        return userType;
    }
}