using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class AllowGuestUsersHandler : AuthorizationHandler<AllowGuestUsersRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   AllowGuestUsersRequirement requirement)
    {

        var guestTokenClaim = context.User.FindFirst(c => c.Type == "ll-GuestToken");
        var userIdClaim = context.User.FindFirst(c => c.Type == "ll-UserId");
        if (guestTokenClaim != null && guestTokenClaim.Value != string.Empty)
             context.Succeed(requirement);

        if (userIdClaim != null && userIdClaim.Value != string.Empty)
            context.Succeed(requirement);
        return Task.CompletedTask;

    }
}
public class AllowGuestUsersRequirement : IAuthorizationRequirement
{

    public AllowGuestUsersRequirement()
    {
    }
}