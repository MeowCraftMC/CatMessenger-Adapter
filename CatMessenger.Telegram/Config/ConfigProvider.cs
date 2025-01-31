using CatMessenger.Core.Config;
using Microsoft.Extensions.Configuration;

namespace CatMessenger.Telegram.Config;

public class ConfigProvider(IConfiguration config) : IConfigProvider
{
    public static string GetDevEnvironmentVariable()
    {
        return Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
    }

    private static bool HasDevEnvironmentVariable()
    {
        return GetDevEnvironmentVariable().Equals("Development", StringComparison.OrdinalIgnoreCase);
    }

    public string GetTelegramToken()
    {
        return config.GetValue<string>("Telegram:Token") ?? "";
    }

    public bool IsTelegramProxyEnabled()
    {
        return config.GetValue<bool>("Telegram:Proxy:Enabled");
    }

    public string GetTelegramProxyUrl()
    {
        return config.GetValue<string>("Telegram:Proxy:Url") ?? "";
    }

    public long GetTelegramChatId()
    {
        return long.Parse(config.GetValue<string>("Telegram:ChatId")!);
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
        return config.GetValue<string>("Name")!;
    }

    public string GetRabbitMqHost()
    {
        return config.GetValue<string>("Connector:Host") ?? "";
    }

    public int GetRabbitMqPort()
    {
        return config.GetValue<int>("Connector:Port");
    }

    public string GetRabbitMqVirtualHost()
    {
        return config.GetValue<string>("Connector:VirtualHost") ?? "";
    }

    public string GetRabbitMqUsername()
    {
        return config.GetValue<string>("Connector:Username") ?? "";
    }

    public string GetRabbitMqPassword()
    {
        return config.GetValue<string>("Connector:Password") ?? "";
    }
}