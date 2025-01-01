namespace N9.WebApi.Extensions;

public static class ErrorHandlingExtensions
{
    public static WebApplication UseExceptionHandling(this WebApplication app)
    {
        app.UseStatusCodePages(async statusCodeContext
            => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                .ExecuteAsync(statusCodeContext.HttpContext));

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseExceptionHandler(a => { a.Run(async ctx => await Results.Problem().ExecuteAsync(ctx)); });

        return app;
    }
}