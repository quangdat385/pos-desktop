namespace PosApi.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using PosApi.Middlewares;


    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseRouting();
            app.UseCors("DevCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimitingPolicies();
            app.MapControllers();

            return app;
        }
    }
}
