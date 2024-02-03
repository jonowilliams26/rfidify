
namespace RFIDify.Api.Filters;

public class RequestValidationFilter<TRequest>(IValidator<TRequest> validator, ILogger<RequestValidationFilter<TRequest>> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var requestType = typeof(TRequest).FullName;

        logger.LogInformation("HTTP {Method} {Path} validating {Request}", method, path, requestType);
        var request = context.Arguments.OfType<TRequest>().Single();
        var cancellationToken = context.HttpContext.RequestAborted;
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("HTTP {Method} {Path} request validation failed for {Request}", method, path, requestType);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        logger.LogInformation("HTTP {Method} {Path} request validation succeeded for {Request}", method, path, requestType);
        return await next(context);
    }
}

public static class RequestValidationFilterExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<RequestValidationFilter<TRequest>>()
            .ProducesValidationProblem();

        return builder;
    }
}