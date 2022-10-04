using Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Core.Extension
{
    public static class ApplicationBuilder
    {
        public static IApplicationBuilder ConfigSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WhatsUp Api v1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.DefaultModelsExpandDepth(0);
                c.DisplayRequestDuration();
            });

            app.UseCors(builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());

            return app;
        }

        public static IApplicationBuilder AddCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<HttpLogging>()
               .UseMiddleware<Response>()
               .ConfigureException();
             //.UseMiddleware<Authentication>()

            return app;
        }

        public static IApplicationBuilder ConfigureException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = Extensions.CustomException,
                AllowStatusCode404Response = true
            });

            return app;
        }

    }
}
