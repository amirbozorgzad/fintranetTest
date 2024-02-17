namespace Core.Domain.Model;

public class Vehicle : Entity
{
    public Vehicle()
    {
        Toll = new HashSet<Toll>();
    }

    public string Name { get; set; }
    public ICollection<Toll> Toll { get; set; }
}