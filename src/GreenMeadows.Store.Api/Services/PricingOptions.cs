namespace GreenMeadows.Store.Api.Services;

/// <summary>
/// Pricing config bound from appsettings ("Pricing" section)
/// </summary>
public class PricingOptions
{
    public decimal TaxRate { get; set; } = 0.20m;
    public string Currency { get; set; } = "NZD";
}
