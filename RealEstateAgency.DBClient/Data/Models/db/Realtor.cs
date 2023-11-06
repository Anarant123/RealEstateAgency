namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class Realtor : Person
{
    public int Id { get; set; }

    public int? ShareCommission { get; set; }

    public virtual ICollection<Need> Needs { get; set; } = new List<Need>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
