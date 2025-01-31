using CatMessenger.Core.Config;
using Microsoft.Extensions.Configuration;

namespace CatMessenger.Matrix.Config;

public class ConfigManager(IConfiguration config) : IConfigProvider
{
    public static string GetDevEnvironmentVariable()
    {
        return Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
    }

    private static bool HasDevEnvironmentVariable()
    {
        return GetDevEnvironmentVariable().Equals("Development", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsDebug()
    {
        return HasDevEnvironmentVariable() || config.GetValue<bool>("Debug");
    }

    public string GetId()
    {
        return config.GetValue<string>("Id") ?? "";
    }

    public string GetName()
    {
        return config.GetValue<string>("Name") ?? "";
    }

    public string GetConnectorHost()
    {
        return config.GetValue<string>("Connector:Host") ?? "";
    }

    public int GetConnectorPort()
    {
        return config.GetValue<int>("Connector:Port");
    }

    public string GetConnectorVirtualHost()
    {
        return config.GetValue<string>("Connector:VirtualHost") ?? "";
    }

    public string GetConnectorUsername()
    {
        return config.GetValue<string>("Connector:Username") ?? "";
    }

    public string GetConnectorPassword()
    {
        return config.GetValue<string>("Connector:Password") ?? "";
    }

    public int GetConnectorMaxRetry()
    {
        return config.GetValue<int>("Connector:MaxRetry");
    }

    public string GetMatrixUri()
    {
        return config.GetValue<string>("Matrix:Uri") ?? "";
    }

    public string GetMatrixUsername()
    {
        return config.GetValue<string>("Matrix:Username") ?? "";
    }

    public string GetMatrixPassword()
    {
        return config.GetValue<string>("Matrix:Password") ?? "";
    }

    public string GetMatrixRoomId()
    {
        return config.GetValue<string>("Matrix:RoomId") ?? "";
    }
}