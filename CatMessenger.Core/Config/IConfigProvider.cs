namespace CatMessenger.Core.Config;

public interface IConfigProvider
{
    public bool IsDebug();
    public string GetId();
    public string GetName();

    public string GetRabbitMqHost();
    public int GetRabbitMqPort();
    public string GetRabbitMqVirtualHost();
    public string GetRabbitMqUsername();
    public string GetRabbitMqPassword();
}