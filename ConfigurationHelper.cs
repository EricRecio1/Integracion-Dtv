using Microsoft.Extensions.Configuration;

public class ConfigurationHelper
{
	public static string GetValue(string seccion, string value)
	{
		IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
		return config[seccion + ":" + value];
	}
}
