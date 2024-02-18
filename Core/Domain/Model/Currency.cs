namespace Core.Domain.Model;

public class Currency : Entity
{
    public Currency()
    {
        City = new HashSet<City>();
    }

    public string Name { get; set; }
    public string IsoCode { get; set; }
    public ICollection<City> City { get; set; }
}