using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Utility.Exceptions;
using ILogger = Serilog.ILogger;

namespace PetHealthCareSystemRazorPages.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleUnauthorizedAsync(context);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    var roles = GetRequiredRoles(context);
                    await HandleForbiddenAsync(context, roles);
                }
            }
            catch (AppException ex)
            {
                await HandleAppExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static string GetRequiredRoles(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var authorizeData = endpoint?.Metadata?.GetMetadata<IAuthorizeData>();

            return authorizeData?.Roles;
        }

        private static Task HandleUnauthorizedAsync(HttpContext context)
        {
            context.Response.Redirect("/Account/Login");
            return Task.CompletedTask;
        }

        private static Task HandleForbiddenAsync(HttpContext context, string requiredRoles = null)
        {
            context.Response.Redirect("/Account/AccessDenied");
            return Task.CompletedTask;
        }

        private static async Task HandleAppExceptionAsync(HttpContext context, AppException ex)
        {
            // Get the route data to determine where to redirect
            var routeValues = context.Request.RouteValues;
            var redirectUrl = routeValues["page"]?.ToString();
            // Get the model state from the page model
            var modelState = context.Items["ModelState"] as ModelStateDictionary;

            if (redirectUrl != null)
            {
                modelState?.AddModelError(string.Empty, ex.Message);
                context.Response.Redirect(redirectUrl);
            }
            await Task.CompletedTask;
        }


        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.Redirect("/Error");
            await Task.CompletedTask;
        }
    }
}
