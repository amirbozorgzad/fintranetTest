namespace Core.Domain.Model;

public class Fee : Entity
{
    public long CityId { get; set; }
    public decimal Charge { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}