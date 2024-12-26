using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IntegracionDtvContext : DbContext
{
	public virtual DbSet<DtvAsn> DtvAsns { get; set; }
	public virtual DbSet<DtvAsnProduct> DtvAsnProducts { get; set; }
	public virtual DbSet<DtvAsnRecProd> DtvAsnRecProds { get; set; }
	public virtual DbSet<DtvAsnRecSeri> DtvAsnRecSeris { get; set; }
	public virtual DbSet<DtvAsnRecep> DtvAsnReceps { get; set; }
	public virtual DbSet<DtvAsnSeries> DtvAsnSeries { get; set; }
	public virtual DbSet<DtvDespaProd> DtvDespaProds { get; set; }
	public virtual DbSet<DtvDespaSerie> DtvDespaSeries { get; set; }
	public virtual DbSet<DtvDespaTran> DtvDespaTrans { get; set; }
	public virtual DbSet<DtvDevolFalla> DtvDevolFallas { get; set; }
	public virtual DbSet<DtvDevolPedid> DtvDevolPedids { get; set; }
	public virtual DbSet<DtvDevolProd> DtvDevolProds { get; set; }
	public virtual DbSet<DtvDevolSerie> DtvDevolSeries { get; set; }
	public virtual DbSet<DtvKitOrder> DtvKitOrders { get; set; }
	public virtual DbSet<DtvKitProduct> DtvKitProducts { get; set; }
	public virtual DbSet<DtvKitSusti> DtvKitSustis { get; set; }
	public virtual DbSet<DtvLog> DtvLogs { get; set; }
	public virtual DbSet<DtvRecepProdu> DtvRecepProdus { get; set; }
	public virtual DbSet<DtvRecepSerie> DtvRecepSeries { get; set; }
	public virtual DbSet<DtvRecepSucur> DtvRecepSucurs { get; set; }
	public virtual DbSet<DtvTransOrder> DtvTransOrders { get; set; }
	public virtual DbSet<DtvTransProd> DtvTransProds { get; set; }
	public virtual DbSet<DtvTraslaObserv> DtvTraslaObservs { get; set; }
	public virtual DbSet<DtvTraslaProd> DtvTraslaProds { get; set; }
	public virtual DbSet<DtvTraslaSeri> DtvTraslaSeris { get; set; }
	public virtual DbSet<DtvTraslado> DtvTraslados { get; set; }
	public virtual DbSet<DtvMoniError> DtvMoniError { get; set; }

	public IntegracionDtvContext()
	{
	}

	public IntegracionDtvContext(DbContextOptions<IntegracionDtvContext> options)
		: base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer("Server=serverdb-directv.database.windows.net; Database=db-directv; Trusted_Connection=False; User Id=dtv; password=waThU$mhEuk_rK7s;");
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvAsn> entity)
		{
			entity.HasKey((DtvAsn e) => e.IdMensaje).HasName("PK__DTV_ASN__E4D2A47F9D427723");
			entity.ToTable("DTV_ASN");
			entity.Property((DtvAsn e) => e.IdMensaje).ValueGeneratedNever();
			entity.Property((DtvAsn e) => e.Cantlineas).HasMaxLength(10);
			entity.Property((DtvAsn e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvAsn e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvAsn e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvAsn e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvAsn e) => e.FechaMensaje).HasMaxLength(35);
			entity.Property((DtvAsn e) => e.FechaOc).HasMaxLength(35).HasColumnName("FechaOC");
			entity.Property((DtvAsn e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvAsn e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvAsn e) => e.IdIntegracion).HasMaxLength(20);
			entity.Property((DtvAsn e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvAsn e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvAsn e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvAsn e) => e.NroOc).HasMaxLength(20).HasColumnName("NroOC");
			entity.Property((DtvAsn e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvAsnProduct> entity)
		{
			entity.HasKey((DtvAsnProduct e) => e.IdAsnProduct).HasName("PK__DTV_ASN___140D88450CA3A137");
			entity.ToTable("DTV_ASN_Product");
			entity.Property((DtvAsnProduct e) => e.IdAsnProduct).HasColumnName("Id_ASN_Product");
			entity.Property((DtvAsnProduct e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvAsnProduct e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvAsnProduct e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvAsnProduct e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvAsnProduct e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvAsnProduct e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvAsnProduct e) => e.IdPallet).HasMaxLength(50);
			entity.Property((DtvAsnProduct e) => e.IdProductPaired).HasMaxLength(50);
			entity.Property((DtvAsnProduct e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvAsnProduct e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvAsnProduct d) => d.IdMensajeNavigation).WithMany((DtvAsn p) => p.DtvAsnProducts).HasForeignKey((DtvAsnProduct d) => d.IdMensaje)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_ASN_P__IdMen__52593CB8");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvAsnRecProd> entity)
		{
			entity.HasKey((DtvAsnRecProd e) => e.IdAsnRecProd).HasName("PK__DTV_ASN___2866DDECBE16D58D");
			entity.ToTable("DTV_ASN_RecProd");
			entity.Property((DtvAsnRecProd e) => e.IdAsnRecProd).HasColumnName("Id_ASN_RecProd");
			entity.Property((DtvAsnRecProd e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvAsnRecProd e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvAsnRecProd e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvAsnRecProd e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvAsnRecProd e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvAsnRecProd e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvAsnRecProd e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvAsnRecProd e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvAsnRecProd d) => d.IdMensajeNavigation).WithMany((DtvAsnRecep p) => p.DtvAsnRecProds).HasForeignKey((DtvAsnRecProd d) => d.IdMensaje)
				.HasConstraintName("FK__DTV_ASN_R__IdMen__534D60F1");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvAsnRecSeri> entity)
		{
			entity.HasKey((DtvAsnRecSeri e) => e.IdAsnRecSeri).HasName("PK__DTV_ASN___1DCBB5948C2438D6");
			entity.ToTable("DTV_ASN_RecSeri");
			entity.Property((DtvAsnRecSeri e) => e.IdAsnRecSeri).HasColumnName("Id_ASN_RecSeri");
			entity.Property((DtvAsnRecSeri e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvAsnRecSeri e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvAsnRecSeri e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvAsnRecSeri e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvAsnRecSeri e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvAsnRecSeri e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvAsnRecSeri e) => e.IdAsnRecProd).HasColumnName("Id_ASN_RecProd");
			entity.Property((DtvAsnRecSeri e) => e.IdProductPaired).HasMaxLength(50);
			entity.Property((DtvAsnRecSeri e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvAsnRecSeri e) => e.NroSerie).HasMaxLength(50);
			entity.Property((DtvAsnRecSeri e) => e.NroSeriePaired).HasMaxLength(50);
			entity.Property((DtvAsnRecSeri e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvAsnRecSeri d) => d.IdAsnRecProdNavigation).WithMany((DtvAsnRecProd p) => p.DtvAsnRecSeris).HasForeignKey((DtvAsnRecSeri d) => d.IdAsnRecProd)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_ASN_R__Id_AS__5441852A");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvAsnRecep> entity)
		{
			entity.HasKey((DtvAsnRecep e) => e.IdMensaje).HasName("PK__DTV_ASN___E4D2A47F470E7407");
			entity.ToTable("DTV_ASN_Recep");
			entity.Property((DtvAsnRecep e) => e.IdMensaje).ValueGeneratedNever();
			entity.Property((DtvAsnRecep e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvAsnRecep e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvAsnRecep e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvAsnRecep e) => e.FecTransaccion).HasMaxLength(10);
			entity.Property((DtvAsnRecep e) => e.FechaMensaje).HasMaxLength(10);
			entity.Property((DtvAsnRecep e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvAsnRecep e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvAsnRecep e) => e.IdDocumento).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.IdIntegracion).HasMaxLength(20);
			entity.Property((DtvAsnRecep e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvAsnRecep e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.Localizador).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.NroOc).HasMaxLength(20).HasColumnName("NroOC");
			entity.Property((DtvAsnRecep e) => e.Organizacion).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.Subinventario).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.TipoDocumento).HasMaxLength(50);
			entity.Property((DtvAsnRecep e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvAsnSeries> entity)
		{
			entity.HasKey((DtvAsnSeries e) => e.IdAsnSerie).HasName("PK__DTV_ASN___6D7B1398C73DED7D");
			entity.ToTable("DTV_ASN_Series");
			entity.Property((DtvAsnSeries e) => e.IdAsnSerie).HasColumnName("Id_ASN_Serie");
			entity.Property((DtvAsnSeries e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvAsnSeries e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvAsnSeries e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvAsnSeries e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvAsnSeries e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvAsnSeries e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvAsnSeries e) => e.IdAsnProduct).HasColumnName("Id_ASN_Product");
			entity.Property((DtvAsnSeries e) => e.IdPallet).HasMaxLength(50);
			entity.Property((DtvAsnSeries e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvAsnSeries e) => e.NroMacAddress).HasMaxLength(50);
			entity.Property((DtvAsnSeries e) => e.NroSerie).HasMaxLength(50);
			entity.Property((DtvAsnSeries e) => e.NroSeriePaired).HasMaxLength(50);
			entity.Property((DtvAsnSeries e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvAsnSeries d) => d.IdAsnProductNavigation).WithMany((DtvAsnProduct p) => p.DtvAsnSeries).HasForeignKey((DtvAsnSeries d) => d.IdAsnProduct)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_ASN_S__Id_AS__5535A963");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDespaProd> entity)
		{
			entity.HasKey((DtvDespaProd e) => e.IdDespaProd).HasName("PK__DTV_Desp__517C119A19EFB575");
			entity.ToTable("DTV_Despa_Prod");
			entity.Property((DtvDespaProd e) => e.IdDespaProd).HasColumnName("Id_Despa_Prod");
			entity.Property((DtvDespaProd e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvDespaProd e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDespaProd e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDespaProd e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDespaProd e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDespaProd e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDespaProd e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvDespaProd e) => e.LocalizadorOri).HasMaxLength(50);
			entity.Property((DtvDespaProd e) => e.OrganizacionOri).HasMaxLength(50);
			entity.Property((DtvDespaProd e) => e.SubinventariOri).HasMaxLength(50);
			entity.Property((DtvDespaProd e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			//entity.Property((DtvDespaProd e) => e.Type).HasColumnType("Type");
			entity.HasOne((DtvDespaProd d) => d.IdMensajeNavigation).WithMany((DtvDespaTran p) => p.DtvDespaProds).HasForeignKey((DtvDespaProd d) => d.IdMensaje)
				.HasConstraintName("FK__DTV_Despa__IdMen__5629CD9C");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDespaSerie> entity)
		{
			entity.HasKey((DtvDespaSerie e) => e.IdDespaSerie).HasName("PK__DTV_Desp__6BBC3669E5821AF0");
			entity.ToTable("DTV_Despa_Serie");
			entity.Property((DtvDespaSerie e) => e.IdDespaSerie).HasColumnName("Id_Despa_Serie");
			entity.Property((DtvDespaSerie e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvDespaSerie e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDespaSerie e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDespaSerie e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDespaSerie e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDespaSerie e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDespaSerie e) => e.IdDespaProd).HasColumnName("Id_Despa_Prod");
			entity.Property((DtvDespaSerie e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvDespaSerie e) => e.NroSerie).HasMaxLength(50);
			entity.Property((DtvDespaSerie e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvDespaSerie d) => d.IdDespaProdNavigation).WithMany((DtvDespaProd p) => p.DtvDespaSeries).HasForeignKey((DtvDespaSerie d) => d.IdDespaProd)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Despa__Id_De__571DF1D5");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDespaTran> entity)
		{
			entity.HasKey((DtvDespaTran e) => e.IdMensaje).HasName("PK__DTV_Desp__E4D2A47F6530CC31");
			entity.ToTable("DTV_Despa_Trans");
			entity.Property((DtvDespaTran e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDespaTran e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDespaTran e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDespaTran e) => e.FecDocumento).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.FechaMensaje).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDespaTran e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDespaTran e) => e.IdDocumento).HasMaxLength(15);
			entity.Property((DtvDespaTran e) => e.IdIntegracion).HasMaxLength(20);
			entity.Property((DtvDespaTran e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvDespaTran e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.LocalizadorDes).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.OrganizacionDes).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.SubinventariDes).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.TipoDocumento).HasMaxLength(50);
			entity.Property((DtvDespaTran e) => e.Archivo).HasMaxLength(100);
			entity.Property((DtvDespaTran e) => e.JsonRequest);
			entity.Property((DtvDespaTran e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDevolFalla> entity)
		{
			entity.HasKey((DtvDevolFalla e) => e.IdDevolFalla).HasName("PK__DTV_Devo__362ED3230428EB90");
			entity.ToTable("DTV_Devol_Falla");
			entity.Property((DtvDevolFalla e) => e.IdDevolFalla).HasColumnName("Id_Devol_Falla");
			entity.Property((DtvDevolFalla e) => e.Clave).IsRequired().HasMaxLength(50);
			entity.Property((DtvDevolFalla e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDevolFalla e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDevolFalla e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDevolFalla e) => e.Falla).HasMaxLength(250);
			entity.Property((DtvDevolFalla e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDevolFalla e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDevolFalla e) => e.IdDevolSerie).HasColumnName("Id_Devol_Serie");
			entity.Property((DtvDevolFalla e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvDevolFalla d) => d.IdDevolSerieNavigation).WithMany((DtvDevolSerie p) => p.DtvDevolFallas).HasForeignKey((DtvDevolFalla d) => d.IdDevolSerie)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Devol__Id_De__5812160E");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDevolPedid> entity)
		{
			entity.HasKey((DtvDevolPedid e) => e.IdMensaje).HasName("PK__DTV_Devo__E4D2A47F5EDDFA6E");
			entity.ToTable("DTV_Devol_Pedid");
			entity.Property((DtvDevolPedid e) => e.IdMensaje).ValueGeneratedNever();
			entity.Property((DtvDevolPedid e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.ContactoDescrip).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.ContactoEmail).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.ContactoFax).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.ContactoTelefon).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.Cpdestino).HasMaxLength(50).HasColumnName("CPDestino");
			entity.Property((DtvDevolPedid e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDevolPedid e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDevolPedid e) => e.DescModFactura).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.DireccionDest).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDevolPedid e) => e.FecDocumento).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.FecEstimEntrega).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.FechaMensaje).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDevolPedid e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDevolPedid e) => e.GeoLatitud).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.GeoLongitud).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.IdDocumento).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.IdIntegracion).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.IdModeloFactura).HasMaxLength(20);
			entity.Property((DtvDevolPedid e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvDevolPedid e) => e.IdPaisDest).HasMaxLength(10);
			entity.Property((DtvDevolPedid e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.LocalidadDest).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.LocalizadorDes).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.LocalizadorOri).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.Observaciones).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.OrganizacionDes).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.OrganizacionOri).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.ProvinciaDest).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.SubinventariDes).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.SubinventariOri).HasMaxLength(50);
			entity.Property((DtvDevolPedid e) => e.TipoDocumento).HasMaxLength(50);
            entity.Property((DtvDevolPedid e) => e.Archivo).HasMaxLength(100);
            entity.Property((DtvDevolPedid e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDevolProd> entity)
		{
			entity.HasKey((DtvDevolProd e) => e.IdDevolProd).HasName("PK__DTV_Devo__1CC304F2B2A7B7EE");
			entity.ToTable("DTV_Devol_Prod");
			entity.Property((DtvDevolProd e) => e.IdDevolProd).HasColumnName("Id_Devol_Prod");
			entity.Property((DtvDevolProd e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvDevolProd e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDevolProd e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDevolProd e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDevolProd e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDevolProd e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDevolProd e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvDevolProd e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvDevolProd d) => d.IdMensajeNavigation).WithMany((DtvDevolPedid p) => p.DtvDevolProds).HasForeignKey((DtvDevolProd d) => d.IdMensaje)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Devol__IdMen__59063A47");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvDevolSerie> entity)
		{
			entity.HasKey((DtvDevolSerie e) => e.IdDevolSerie).HasName("PK__DTV_Devo__A8F3957303094561");
			entity.ToTable("DTV_Devol_Serie");
			entity.Property((DtvDevolSerie e) => e.IdDevolSerie).HasColumnName("Id_Devol_Serie");
			entity.Property((DtvDevolSerie e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvDevolSerie e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvDevolSerie e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvDevolSerie e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvDevolSerie e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvDevolSerie e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvDevolSerie e) => e.IdDevolProd).HasColumnName("Id_Devol_Prod");
			entity.Property((DtvDevolSerie e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvDevolSerie e) => e.NroSerie).HasMaxLength(50);
			entity.Property((DtvDevolSerie e) => e.Status).HasMaxLength(50);
			entity.Property((DtvDevolSerie e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvDevolSerie d) => d.IdDevolProdNavigation).WithMany((DtvDevolProd p) => p.DtvDevolSeries).HasForeignKey((DtvDevolSerie d) => d.IdDevolProd)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Devol__Id_De__59FA5E80");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvKitOrder> entity)
		{
			entity.HasKey((DtvKitOrder e) => e.IdMensaje).HasName("PK__DTV_Kit___E4D2A47F94792997");
			entity.ToTable("DTV_Kit_Order");
			entity.Property((DtvKitOrder e) => e.IdMensaje).ValueGeneratedNever();
			entity.Property((DtvKitOrder e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvKitOrder e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvKitOrder e) => e.DireccionDest).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvKitOrder e) => e.FecDocumento).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.FechaMensaje).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvKitOrder e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvKitOrder e) => e.IdDocumento).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.IdIntegracion).HasMaxLength(20);
			entity.Property((DtvKitOrder e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvKitOrder e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.LocalizadorDes).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.Observaciones).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.OrganizacionDes).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.SubinventariDes).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.TipoDocumento).HasMaxLength(50);
			entity.Property((DtvKitOrder e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
            entity.Property((DtvKitOrder e) => e.Archivo).HasMaxLength(100);
        });
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvKitProduct> entity)
		{
			entity.HasKey((DtvKitProduct e) => e.IdKitProduct).HasName("PK__DTV_Kit___7EA932F27A67D1EF");
			entity.ToTable("DTV_Kit_Product");
			entity.Property((DtvKitProduct e) => e.IdKitProduct).HasColumnName("Id_Kit_Product");
			entity.Property((DtvKitProduct e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvKitProduct e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvKitProduct e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvKitProduct e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvKitProduct e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvKitProduct e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvKitProduct e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvKitProduct e) => e.LocalizadorOri).HasMaxLength(50);
			entity.Property((DtvKitProduct e) => e.OrganizacionOri).HasMaxLength(50);
			entity.Property((DtvKitProduct e) => e.SubinventariOri).HasMaxLength(50);
			entity.Property((DtvKitProduct e) => e.TipoProducto).HasMaxLength(20);
			entity.Property((DtvKitProduct e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvKitProduct d) => d.IdMensajeNavigation).WithMany((DtvKitOrder p) => p.DtvKitProducts).HasForeignKey((DtvKitProduct d) => d.IdMensaje)
				.HasConstraintName("FK__DTV_Kit_P__IdMen__5AEE82B9");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvKitSusti> entity)
		{
			entity.HasKey((DtvKitSusti e) => e.IdKitSusti).HasName("PK__DTV_Kit___22B4D9C4FCE1FC63");
			entity.ToTable("DTV_Kit_Susti");
			entity.Property((DtvKitSusti e) => e.IdKitSusti).HasColumnName("Id_Kit_Susti");
			entity.Property((DtvKitSusti e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvKitSusti e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvKitSusti e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvKitSusti e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvKitSusti e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvKitSusti e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvKitSusti e) => e.IdKitProduct).HasColumnName("Id_Kit_Product");
			entity.Property((DtvKitSusti e) => e.IdSustituto).HasMaxLength(50);
			entity.Property((DtvKitSusti e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvKitSusti d) => d.IdKitProductNavigation).WithMany((DtvKitProduct p) => p.DtvKitSustis).HasForeignKey((DtvKitSusti d) => d.IdKitProduct)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Kit_S__Id_Ki__5BE2A6F2");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvLog> entity)
		{
			entity.ToTable("DTV_Logs");
			entity.Property((DtvLog e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvLog e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvLog e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvLog e) => e.Destino).IsRequired().HasMaxLength(10);
			entity.Property((DtvLog e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvLog e) => e.Evento).IsRequired().HasMaxLength(50);
			entity.Property((DtvLog e) => e.FechaHora).HasColumnType("datetime");
			entity.Property((DtvLog e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvLog e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvLog e) => e.IdIntegracion).HasMaxLength(20);
			entity.Property((DtvLog e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvLog e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvLog e) => e.NombreArchivo).IsRequired().HasMaxLength(80);
			entity.Property((DtvLog e) => e.Origen).IsRequired().HasMaxLength(10);
			entity.Property((DtvLog e) => e.TipoArchivo).IsRequired().HasMaxLength(10);
			entity.Property((DtvLog e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvRecepProdu> entity)
		{
			entity.HasKey((DtvRecepProdu e) => e.IdRecepProdu).HasName("PK__DTV_Rece__7FAE3969082033D5");
			entity.ToTable("DTV_Recep_Produ");
			entity.Property((DtvRecepProdu e) => e.IdRecepProdu).HasColumnName("Id_Recep_Produ");
			entity.Property((DtvRecepProdu e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvRecepProdu e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvRecepProdu e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvRecepProdu e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvRecepProdu e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvRecepProdu e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvRecepProdu e) => e.IdProducto).HasMaxLength(50);
            entity.Property((DtvRecepProdu e) => e.CantProducto).HasColumnType("int");
            entity.Property((DtvRecepProdu e) => e.SubinventariOri).HasMaxLength(50);
            entity.Property((DtvRecepProdu e) => e.LocalizadorOri).HasMaxLength(50);
			entity.Property((DtvRecepProdu e) => e.OrganizacionOri).HasMaxLength(50);
			entity.Property((DtvRecepProdu e) => e.SubinventariOri).HasMaxLength(50);
			entity.Property((DtvRecepProdu e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvRecepProdu d) => d.IdMensajeNavigation).WithMany((DtvRecepSucur p) => p.DtvRecepProdus).HasForeignKey((DtvRecepProdu d) => d.IdMensaje)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Recep__IdMen__5CD6CB2B");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvRecepSerie> entity)
		{
			entity.HasKey((DtvRecepSerie e) => e.IdRecepSerie).HasName("PK__DTV_Rece__9801C33E8BD60DFF");
			entity.ToTable("DTV_Recep_Serie");
			entity.Property((DtvRecepSerie e) => e.IdRecepSerie).HasColumnName("Id_Recep_Serie");
			entity.Property((DtvRecepSerie e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvRecepSerie e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvRecepSerie e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvRecepSerie e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvRecepSerie e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvRecepSerie e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvRecepSerie e) => e.IdMensaje).HasMaxLength(18);
			entity.Property((DtvRecepSerie e) => e.IdProductPaired).HasMaxLength(50);
			entity.Property((DtvRecepSerie e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvRecepSerie e) => e.IdRecepProdu).HasColumnName("Id_Recep_Produ");
			entity.Property((DtvRecepSerie e) => e.NroSerie).HasMaxLength(50);
			entity.Property((DtvRecepSerie e) => e.NroSeriePaired).HasMaxLength(50);
			entity.Property((DtvRecepSerie e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvRecepSerie d) => d.IdRecepProduNavigation).WithMany((DtvRecepProdu p) => p.DtvRecepSeries).HasForeignKey((DtvRecepSerie d) => d.IdRecepProdu)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Recep__Id_Re__5DCAEF64");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvRecepSucur> entity)
		{
			entity.HasKey((DtvRecepSucur e) => e.IdMensaje).HasName("PK__DTV_Rece__E4D2A47FDE579F6B");
			entity.ToTable("DTV_Recep_Sucur");
			entity.Property((DtvRecepSucur e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvRecepSucur e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvRecepSucur e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvRecepSucur e) => e.FecDocumento).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.FechaMensaje).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvRecepSucur e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvRecepSucur e) => e.IdDocumento).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.IdIntegracion).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvRecepSucur e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.LocalizadorDes).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.OrganizacionDes).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.SubinventariDes).HasMaxLength(50);
			entity.Property((DtvRecepSucur e) => e.TipoDocumento).HasMaxLength(50);
            entity.Property((DtvRecepSucur e) => e.Archivo).HasMaxLength(200);
			entity.Property((DtvRecepSucur e) => e.JsonRequest);
			entity.Property((DtvRecepSucur e) => e.Processed).HasColumnType("bit").HasColumnName("Processed");
            entity.Property((DtvRecepSucur e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvTransOrder> entity)
		{
			entity.HasKey((DtvTransOrder e) => e.IdMensaje).HasName("PK__DTV_Tran__E4D2A47FCFC396DB");
			entity.ToTable("DTV_Trans_Order");
			entity.Property((DtvTransOrder e) => e.IdMensaje).ValueGeneratedNever();
			entity.Property((DtvTransOrder e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.ContactoDescrip).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.ContactoEmail).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.ContactoFax).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.ContactoTelefon).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.Cpdestino).HasMaxLength(50).HasColumnName("CPDestino");
			entity.Property((DtvTransOrder e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvTransOrder e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvTransOrder e) => e.DescModFactura).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.DireccionDest).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvTransOrder e) => e.FecDocumento).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.FecEstimEntrega).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.FechaMensaje).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvTransOrder e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvTransOrder e) => e.GeoLatitud).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.GeoLongitud).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.IdDocumento).HasMaxLength(15);
			entity.Property((DtvTransOrder e) => e.IdIntegracion).HasMaxLength(20);
			entity.Property((DtvTransOrder e) => e.IdModeloFactura).HasMaxLength(10);
			entity.Property((DtvTransOrder e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvTransOrder e) => e.IdPaisDest).HasMaxLength(10);
			entity.Property((DtvTransOrder e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.LocalidadDest).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.LocalizadorDes).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.Observaciones).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.OrganizacionDes).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.ProvinciaDest).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.SubinventariDes).HasMaxLength(50);
			entity.Property((DtvTransOrder e) => e.TipoDocumento).HasMaxLength(50);
            entity.Property((DtvTransOrder e) => e.Archivo).HasMaxLength(100);
            entity.Property((DtvTransOrder e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvTransProd> entity)
		{
			entity.HasKey((DtvTransProd e) => e.IdTransProd).HasName("PK__DTV_Tran__E76BA7254E791364");
			entity.ToTable("DTV_Trans_Prod");
			entity.Property((DtvTransProd e) => e.IdTransProd).HasColumnName("Id_Trans_Prod");
			entity.Property((DtvTransProd e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvTransProd e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvTransProd e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvTransProd e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvTransProd e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvTransProd e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvTransProd e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvTransProd e) => e.LocalizadorOri).HasMaxLength(50);
			entity.Property((DtvTransProd e) => e.OrganizacionOri).HasMaxLength(50);
			entity.Property((DtvTransProd e) => e.SubinventariOri).HasMaxLength(50);
			entity.Property((DtvTransProd e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvTransProd d) => d.IdMensajeNavigation).WithMany((DtvTransOrder p) => p.DtvTransProds).HasForeignKey((DtvTransProd d) => d.IdMensaje)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Trans__IdMen__5EBF139D");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvTraslaObserv> entity)
		{
			entity.HasKey((DtvTraslaObserv e) => e.IdTraslaObserv).HasName("PK__DTV_Tras__0006F42D9DF36736");
			entity.ToTable("DTV_Trasla_Observ");
			entity.Property((DtvTraslaObserv e) => e.IdTraslaObserv).HasColumnName("Id_Trasla_Observ");
			entity.Property((DtvTraslaObserv e) => e.Clave).IsRequired().HasMaxLength(50);
			entity.Property((DtvTraslaObserv e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvTraslaObserv e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvTraslaObserv e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvTraslaObserv e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvTraslaObserv e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvTraslaObserv e) => e.Observacion).HasMaxLength(250);
			entity.Property((DtvTraslaObserv e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvTraslaObserv d) => d.IdMensajeNavigation).WithMany((DtvTraslado p) => p.DtvTraslaObservs).HasForeignKey((DtvTraslaObserv d) => d.IdMensaje)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Trasl__IdMen__5FB337D6");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvTraslaProd> entity)
		{
			entity.HasKey((DtvTraslaProd e) => e.IdTraslaProd).HasName("PK__DTV_Tras__7D45CE0EB3FB964B");
			entity.ToTable("DTV_Trasla_Prod");
			entity.Property((DtvTraslaProd e) => e.IdTraslaProd).HasColumnName("Id_Trasla_Prod");
			entity.Property((DtvTraslaProd e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvTraslaProd e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvTraslaProd e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvTraslaProd e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvTraslaProd e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvTraslaProd e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvTraslaProd e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvTraslaProd e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvTraslaProd d) => d.IdMensajeNavigation).WithMany((DtvTraslado p) => p.DtvTraslaProds).HasForeignKey((DtvTraslaProd d) => d.IdMensaje)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Trasl__IdMen__60A75C0F");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvTraslaSeri> entity)
		{
			entity.HasKey((DtvTraslaSeri e) => e.IdTraslaSeri).HasName("PK__DTV_Tras__655E9A5686A81E0A");
			entity.ToTable("DTV_Trasla_Seri");
			entity.Property((DtvTraslaSeri e) => e.IdTraslaSeri).HasColumnName("Id_Trasla_Seri");
			entity.Property((DtvTraslaSeri e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvTraslaSeri e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvTraslaSeri e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvTraslaSeri e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvTraslaSeri e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvTraslaSeri e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvTraslaSeri e) => e.IdProductPaired).HasMaxLength(50);
			entity.Property((DtvTraslaSeri e) => e.IdProducto).HasMaxLength(50);
			entity.Property((DtvTraslaSeri e) => e.IdTraslaProd).HasColumnName("Id_Trasla_Prod");
			entity.Property((DtvTraslaSeri e) => e.NroSerie).HasMaxLength(50);
			entity.Property((DtvTraslaSeri e) => e.NroSeriePaired).HasMaxLength(50);
			entity.Property((DtvTraslaSeri e) => e.Status).HasMaxLength(50);
			entity.Property((DtvTraslaSeri e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
			entity.HasOne((DtvTraslaSeri d) => d.IdTraslaProdNavigation).WithMany((DtvTraslaProd p) => p.DtvTraslaSeries).HasForeignKey((DtvTraslaSeri d) => d.IdTraslaProd)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DTV_Trasl__Id_Tr__619B8048");
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvTraslado> entity)
		{
			entity.HasKey((DtvTraslado e) => e.IdMensaje).HasName("PK__DTV_Tras__E4D2A47FEA15BC54");
			entity.ToTable("DTV_Traslados");
			entity.Property((DtvTraslado e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.DescCorta).HasMaxLength(12).HasColumnName("Desc_Corta");
			entity.Property((DtvTraslado e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvTraslado e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvTraslado e) => e.FecDocumento).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.FechaMensaje).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvTraslado e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvTraslado e) => e.IdDocumento).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.IdIntegracion).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.IdPais).HasMaxLength(2);
			entity.Property((DtvTraslado e) => e.IntegOperacion).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.IntegProceso).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.LocalizadorDes).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.LocalizadorOri).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.OrganizacionDes).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.OrganizacionOri).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.SubinventariDes).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.SubinventariOri).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.TipoDocumento).HasMaxLength(50);
			entity.Property((DtvTraslado e) => e.Usuario).HasMaxLength(50).IsUnicode(unicode: false);
		});
		modelBuilder.Entity(delegate (EntityTypeBuilder<DtvMoniError> entity)
		{
			entity.HasKey((DtvMoniError e) => e.Clave).HasName("PK__DTV_Moni__E8181E1026BF7767");
			entity.ToTable("DTV_Moni_Error");
			entity.Property((DtvMoniError e) => e.Clave).HasMaxLength(50);
			entity.Property((DtvMoniError e) => e.FechaSys).HasColumnType("datetime").HasColumnName("Fecha_sys");
			entity.Property((DtvMoniError e) => e.FechaVcia).HasColumnType("datetime").HasColumnName("Fecha_Vcia");
			entity.Property((DtvMoniError e) => e.Usuario).HasMaxLength(12).HasColumnName("Usuario")
				.IsUnicode(unicode: false);
			entity.Property((DtvMoniError e) => e.DescCorta).HasMaxLength(50).HasColumnName("Desc_Corta");
			entity.Property((DtvMoniError e) => e.DescLarga).HasMaxLength(50).HasColumnName("Desc_Larga");
			entity.Property((DtvMoniError e) => e.Estado).HasMaxLength(1);
			entity.Property((DtvMoniError e) => e.IdDocumento).HasMaxLength(18);
			entity.Property((DtvMoniError e) => e.FechaError).HasMaxLength(10).HasColumnName("FechaError");
			entity.Property((DtvMoniError e) => e.NombreArchivo).HasMaxLength(100).HasColumnName("NombreArchivo");
			entity.Property((DtvMoniError e) => e.IdIntegracion).HasMaxLength(18);
			entity.Property((DtvMoniError e) => e.IdPais).HasMaxLength(2).HasColumnName("IdPais");
			entity.Property((DtvMoniError e) => e.IdEstado).HasMaxLength(18);
			entity.Property((DtvMoniError e) => e.IdResponsable).HasMaxLength(18);
			entity.Property((DtvMoniError e) => e.FechaCierre).HasMaxLength(10).HasColumnName("FechaCierre");
			entity.Property((DtvMoniError e) => e.Error).HasMaxLength(4000).HasColumnName("Error");
			entity.Property((DtvMoniError e) => e.Observacion).HasMaxLength(4000).HasColumnName("Observacion");
		});
	}
}
