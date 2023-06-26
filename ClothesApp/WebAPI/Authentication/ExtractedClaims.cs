using Domain.Enums;

namespace WebAPI.Authentication;

public struct ExtractedClaims
{
    public long Id { get; set; }

    public UserType UserType { get; set; }
}