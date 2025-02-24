namespace WebDiaryAPI;

public partial class WeatherForecast : Forecast
{
    public string? Description { get; set; }

    public Forecast[]? Forecast { get; set; }
}

public class Forecast
{
    public string? Day { get; set; }

    public string? Temperature { get; set; }

    public string? Wind { get; set; }
}