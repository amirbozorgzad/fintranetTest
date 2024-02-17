namespace Core.Contract;

public class TaxInputDto
{
    public string CityName { get; set; }
    public string vehicleName { get; set; }
    public List<DateTime> dates { get; set; }
}