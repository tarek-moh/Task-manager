using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Access the current session
        var session = context.HttpContext.Session;

        // Check if session is available and has "AuthorizationObject"
        if (session == null || string.IsNullOrEmpty(session.GetString("authenticated")))
        {
            context.Result = new RedirectToActionResult("Login", "Accounts", null); // Redirect to Unauthorized page
            return;
        }

        // Validate if "AuthorizationObject" is equal to "valid"
        var authorizationObject = session.GetString("authenticated");
        if (authorizationObject != "valid")
        {
            context.Result = new RedirectToActionResult("Login", "Accounts", null); // Redirect to Unauthorized page
            return;
        }
    }
}
