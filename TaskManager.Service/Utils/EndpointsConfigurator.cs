using Ardalis.GuardClauses;
using FastEndpoints;
using System.Text.RegularExpressions;

namespace TaskManager.Service.Utils;

public static class EndpointsConfigurator
{
    public static void ConfigureRouteVersioning(EndpointOptions options)
    {
        options.Configurator = x =>
        {
            // TODO Enable HTTPS for endpoints
            x.AllowAnonymous();

            var version = GetVersion(x);
            Guard.Against.Null(version);
            x.EndpointVersion(version.Value);
        };
    }

    private static int? GetVersion(EndpointDefinition endpointDefinition)
    {
        var fullName = endpointDefinition.EndpointType.FullName;
        Guard.Against.NullOrEmpty(fullName);

        var versionPattern = @"\.v(\d+)\.";
        var match = Regex.Match(fullName, versionPattern);

        if (!match.Success || match.Groups.Count == 0)
        {
            return null;
        }

        var versionString = match.Groups[1].Value;
        return int.Parse(versionString);
    }
}
