namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class Client : Person
{
    public int Id { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Need> Needs { get; set; } = new List<Need>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
