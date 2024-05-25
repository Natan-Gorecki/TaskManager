using FastEndpoints;
using Namotion.Reflection;

namespace TaskManager.Service.Utils;

public class RequestNullabilityValidator : IGlobalPreProcessor
{
    public async Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        var validationResult = context.Request.ValidateNullability();
        if (!validationResult.Any())
        {
            return;
        }

        foreach (var item in validationResult)
        {
            context.ValidationFailures.Add(new(item, $"Non-null value is required."));
        }
        await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures);
    }
}
