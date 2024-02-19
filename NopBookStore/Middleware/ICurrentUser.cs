using System.Security.Claims;

namespace NopBookStore.Middleware;

public interface ICurrentUser
{
    string? Name { get; }

    int? GetUserId();

    string? GetUserEmail();


    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
    void SetCurrentUser(ClaimsPrincipal user);


}