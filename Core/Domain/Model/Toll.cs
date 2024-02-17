namespace Core.Domain.Model;

public class Toll : Entity
{
    public long VehicleId { get; set; }
    public long CityId { get; set; }
    public bool IsFree { get; set; }
    public Vehicle Vehicle { get; set; }
    public City City { get; set; }
}