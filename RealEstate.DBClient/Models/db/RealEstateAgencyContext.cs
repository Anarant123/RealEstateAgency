using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.DBClient.Models.db;

public partial class RealEstateAgencyContext : DbContext
{
    public RealEstateAgencyContext()
    {
    }

    public RealEstateAgencyContext(DbContextOptions<RealEstateAgencyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Deal> Deals { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<House> Houses { get; set; }

    public virtual DbSet<LandPlot> LandPlots { get; set; }

    public virtual DbSet<Need> Needs { get; set; }

    public virtual DbSet<NeedForApartment> NeedForApartments { get; set; }

    public virtual DbSet<NeedForHome> NeedForHomes { get; set; }

    public virtual DbSet<NeedForLandPlot> NeedForLandPlots { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<PropertyAddress> PropertyAddresses { get; set; }

    public virtual DbSet<RealEstate> RealEstates { get; set; }

    public virtual DbSet<Realtor> Realtors { get; set; }

    public virtual DbSet<TypeOfRealEstate> TypeOfRealEstates { get; set; }

    public virtual DbSet<Сoordinate> Сoordinates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=RealEstateAgency;Trusted_Connection=True;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Apartmen__3214EC07D50E2577");

            entity.ToTable("Apartment");

            entity.HasIndex(e => e.RealEstateId, "UQ__Apartmen__C03786346B254B2A").IsUnique();

            entity.Property(e => e.CountRooms).HasMaxLength(255);
            entity.Property(e => e.Floor).HasMaxLength(255);

            entity.HasOne(d => d.RealEstate).WithOne(p => p.Apartment)
                .HasForeignKey<Apartment>(d => d.RealEstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Apartment__RealE__398D8EEE");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Client__3214EC0713C582FA");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.MiddleName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(16);
        });

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deal__3214EC072337E882");

            entity.ToTable("Deal");

            entity.HasOne(d => d.Need).WithMany(p => p.Deals)
                .HasForeignKey(d => d.NeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Deal__NeedId__59063A47");

            entity.HasOne(d => d.Offer).WithMany(p => p.Deals)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Deal__OfferId__59FA5E80");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC076E1830C7");

            entity.ToTable("District");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__House__3214EC07FEF9A4D6");

            entity.ToTable("House");

            entity.HasIndex(e => e.RealEstateId, "UQ__House__C0378634F68000CA").IsUnique();

            entity.Property(e => e.CountFloors).HasMaxLength(255);
            entity.Property(e => e.CountRooms).HasMaxLength(255);

            entity.HasOne(d => d.RealEstate).WithOne(p => p.House)
                .HasForeignKey<House>(d => d.RealEstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__House__RealEstat__3D5E1FD2");
        });

        modelBuilder.Entity<LandPlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LandPlot__3214EC071C070675");

            entity.ToTable("LandPlot");

            entity.HasIndex(e => e.RealEstateId, "UQ__LandPlot__C0378634CCFC009B").IsUnique();

            entity.HasOne(d => d.RealEstate).WithOne(p => p.LandPlot)
                .HasForeignKey<LandPlot>(d => d.RealEstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LandPlot__RealEs__412EB0B6");
        });

        modelBuilder.Entity<Need>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Need__3214EC076423C665");

            entity.ToTable("Need");

            entity.HasOne(d => d.Client).WithMany(p => p.Needs)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Need__ClientId__49C3F6B7");

            entity.HasOne(d => d.PropertyAddress).WithMany(p => p.Needs)
                .HasForeignKey(d => d.PropertyAddressId)
                .HasConstraintName("FK__Need__PropertyAd__4CA06362");

            entity.HasOne(d => d.Realtor).WithMany(p => p.Needs)
                .HasForeignKey(d => d.RealtorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Need__RealtorId__4AB81AF0");

            entity.HasOne(d => d.Type).WithMany(p => p.Needs)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Need__TypeId__4BAC3F29");
        });

        modelBuilder.Entity<NeedForApartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NeedForA__3214EC07AF770413");

            entity.ToTable("NeedForApartment");

            entity.Property(e => e.MaxCountRooms).HasMaxLength(255);
            entity.Property(e => e.MaxFloor).HasMaxLength(255);
            entity.Property(e => e.MinCountRooms).HasMaxLength(255);
            entity.Property(e => e.MinFloor).HasMaxLength(255);

            entity.HasOne(d => d.Need).WithMany(p => p.NeedForApartments)
                .HasForeignKey(d => d.NeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NeedForAp__NeedI__5070F446");
        });

        modelBuilder.Entity<NeedForHome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NeedForH__3214EC0777D6B9C8");

            entity.ToTable("NeedForHome");

            entity.Property(e => e.MaxCountFloors).HasMaxLength(255);
            entity.Property(e => e.MaxCountRooms).HasMaxLength(255);
            entity.Property(e => e.MinCountFloors).HasMaxLength(255);
            entity.Property(e => e.MinCountRooms).HasMaxLength(255);

            entity.HasOne(d => d.Need).WithMany(p => p.NeedForHomes)
                .HasForeignKey(d => d.NeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NeedForHo__NeedI__534D60F1");
        });

        modelBuilder.Entity<NeedForLandPlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NeedForL__3214EC07C86FFA24");

            entity.ToTable("NeedForLandPlot");

            entity.HasOne(d => d.Need).WithMany(p => p.NeedForLandPlots)
                .HasForeignKey(d => d.NeedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NeedForLa__NeedI__5629CD9C");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Offer__3214EC0730DD8613");

            entity.ToTable("Offer");

            entity.HasOne(d => d.Client).WithMany(p => p.Offers)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Offer__ClientId__440B1D61");

            entity.HasOne(d => d.RealEstate).WithMany(p => p.Offers)
                .HasForeignKey(d => d.RealEstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Offer__RealEstat__45F365D3");

            entity.HasOne(d => d.Realtor).WithMany(p => p.Offers)
                .HasForeignKey(d => d.RealtorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Offer__RealtorId__44FF419A");
        });

        modelBuilder.Entity<PropertyAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Property__3214EC07A508F4BB");

            entity.ToTable("PropertyAddress");

            entity.Property(e => e.ApartmentNumber).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.HouseNumber).HasMaxLength(255);
            entity.Property(e => e.Street).HasMaxLength(255);
        });

        modelBuilder.Entity<RealEstate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RealEsta__3214EC070565F9CF");

            entity.ToTable("RealEstate");

            entity.HasOne(d => d.Coordinates).WithMany(p => p.RealEstates)
                .HasForeignKey(d => d.CoordinatesId)
                .HasConstraintName("FK__RealEstat__Coord__35BCFE0A");

            entity.HasOne(d => d.PropertyAddress).WithMany(p => p.RealEstates)
                .HasForeignKey(d => d.PropertyAddressId)
                .HasConstraintName("FK__RealEstat__Prope__33D4B598");

            entity.HasOne(d => d.Type).WithMany(p => p.RealEstates)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RealEstat__TypeI__34C8D9D1");
        });

        modelBuilder.Entity<Realtor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Realtor__3214EC07B7C23B14");

            entity.ToTable("Realtor");

            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.MiddleName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ShareCommission).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<TypeOfRealEstate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeOfRe__3214EC0751AA91DD");

            entity.ToTable("TypeOfRealEstate");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Сoordinate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Сoordina__3214EC073B24D052");

            entity.HasOne(d => d.District).WithMany(p => p.Сoordinates)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Сoordinat__Distr__30F848ED");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
