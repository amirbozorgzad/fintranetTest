namespace Core.Domain.Model;

public class City : Entity
{
    public City()
    {
        Toll = new HashSet<Toll>();
    }

    public string Name { get; set; }
    public int DaysBeforeHoliday { get; set; }
    public decimal MaximumCongestionTaxPerDay { get; set; }
    public int ChargeInterspacedInMinute { get; set; }
    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public Calendar Calendar { get; set; }
    public ICollection<Toll> Toll { get; set; }
}