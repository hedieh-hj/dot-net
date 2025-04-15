using ota.permission.repository.Repositories;
using System.Security.Claims;

namespace ota.ms.api.Extensions
{

    public static class ApiMapExtenstion
    {
        public static void AddAuthorizedRequirements(
            this RouteHandlerBuilder endpoints,
            string api_name,
            string api_desc,
            string permission_name = "permission_name")
        {
            endpoints.DisableAntiforgery()
                .WithName(api_name)
                .WithDescription(api_desc)
                .WithOpenApi()
                .RequireAuthorization()
                .AddEndpointFilter(new PermissionCheckFilter(permission_name));
        }
    }

    // this is your attribute to call before endpoints run
    public class PermissionCheckFilter : IEndpointFilter
    {        
        private readonly string _permission_name;

        public PermissionCheckFilter(string permission_name)
        {
            _permission_name = permission_name;     
        }
    
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (string.IsNullOrEmpty(_permission_name))
            {
                return await next(context);
            }

            var permissionRelRepository = context.HttpContext.RequestServices.GetRequiredService<PermissionRelRepository>();                             
            
            var _user_id = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;            

            if (!await permissionRelRepository.CheckUserPermissionsAsync(_user_id, _permission_name))
            {
                return Results.Forbid();                
            }
            
            return await next(context);
        }
    }
}
