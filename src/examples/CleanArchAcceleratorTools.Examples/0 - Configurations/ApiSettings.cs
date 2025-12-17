
namespace CleanArchAcceleratorTools.Examples.Configs;

public class ApiSettings
{

    public bool ShowException { get; set; }
    public string DefaultErrorMessage { get; set; }

    public ConnectionConfiguration ConnectionConfiguration { get; set; }

    public ApiSettings()
    {
        ShowException = false;
        DefaultErrorMessage = string.Empty;
        ConnectionConfiguration = new ConnectionConfiguration();
    }
}

public class ConnectionConfiguration
{
    public int MaxTimeoutInSeconds { get; set; }

    public ConnectionStrings ConnectionStrings { get; set; }

    public ConnectionConfiguration()
    {
        ConnectionStrings = new ConnectionStrings();
    }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }

    public ConnectionStrings()
    {
        DefaultConnection = string.Empty;
    }
}
