using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using static System.FormattableString;

public class AppSettings
{
    private IConfiguration _config;

    public AppSettings(IConfiguration config)
    {
        _config = config;
    }

    public string BackupFolder => GetStringFromConfiguration("BackupFolder");

    public string GithubToken => GetStringFromConfiguration("GithubToken");

    public string GitCommand => GetStringFromConfiguration("GitCommand");

    private string GetConnectionStringFromConfiguration(string name)
    {
        try
        {
            return _config.GetConnectionString(name);
        }
        catch (Exception e)
        {
            Console.WriteLine(Invariant($"Couldn't find {name} in configuration. {e}"));
            throw;
        }
    }

    private int GetIntFromConfiguration(string name)
    {
        try
        {
            object value = _config.GetValue<object?>(name, null) ?? _config.GetSection("Settings").GetValue<object>(name);
            if (value is int)
            {
                return (int)value;
            }
            int ans;
            if (!int.TryParse((string)value, out ans))
            {
                Console.WriteLine(Invariant($"Couldn't parse {name} as int."));
                throw new ArgumentException($"Couldn't parse {name} as int.");
            }

            return ans;
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Couldn't find {name} in configuration.", e);
        }
    }

    private string GetStringFromConfiguration(string name)
    {
        try
        {
            return _config.GetValue<string?>(name, null) ?? _config.GetSection("Settings").GetValue<string>(name);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Couldn't find {name} in configuration. {e}", e);
        }
    }

    private string[] GetDelimitedListFromConfiguration(string name)
    {
        try
        {
            var list = GetStringFromConfiguration(name);
            return list.Split(';');
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Couldn't find {name} in configuration., or couldn't split it {e}", e);
        }
    }
}