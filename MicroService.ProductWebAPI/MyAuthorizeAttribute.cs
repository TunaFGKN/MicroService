using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroService.ProductWebAPI;

public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _role;

    public MyAuthorizeAttribute(string role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        /// Here you can implement your custom authorization logic.
    }
}