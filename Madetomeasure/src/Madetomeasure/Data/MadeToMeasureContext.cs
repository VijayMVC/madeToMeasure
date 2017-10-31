using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Madetomeasure.Data
{
    public partial class MadeToMeasureContext : DbContext
    {
        public virtual DbSet<BlazerMeasurements> BlazerMeasurements { get; set; }
        public virtual DbSet<BusinessEntity> BusinessEntity { get; set; }
        public virtual DbSet<BusinessEntityType> BusinessEntityType { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<DepartmentType> DepartmentType { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<JobType> JobType { get; set; }
        public virtual DbSet<KurtaMeasurements> KurtaMeasurements { get; set; }
        public virtual DbSet<OrderfromVendor> OrderfromVendor { get; set; }
        public virtual DbSet<PantMeasurements> PantMeasurements { get; set; }
        public virtual DbSet<ProductionActivity> ProductionActivity { get; set; }
        public virtual DbSet<Receipt> Receipt { get; set; }
        public virtual DbSet<ShalwarMeasurements> ShalwarMeasurements { get; set; }
        public virtual DbSet<ShirtMeasurements> ShirtMeasurements { get; set; }
        public virtual DbSet<Shop> Shop { get; set; }
        public virtual DbSet<ShopOrder> ShopOrder { get; set; }
        public virtual DbSet<StitchingJob> StitchingJob { get; set; }
        public virtual DbSet<StitchingJobDetails> StitchingJobDetails { get; set; }
        public virtual DbSet<StitchingUnitDepartmentHead> StitchingUnitDepartmentHead { get; set; }
        public virtual DbSet<StitchingUnitEmployee> StitchingUnitEmployee { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<SuitMeasurements> SuitMeasurements { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }

        public MadeToMeasureContext(DbContextOptions<MadeToMeasureContext> options)
       : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlazerMeasurements>(entity =>
            {
                entity.HasKey(e => e.StitchingJobId)
                    .HasName("PK__BlazerMe__9E3F41C6E8346ECD");

                entity.Property(e => e.StitchingJobId).ValueGeneratedNever();

                entity.Property(e => e.OtherDetails)
                    .IsRequired()
                    .HasColumnName("Other_Details");

                entity.HasOne(d => d.StitchingJob)
                    .WithOne(p => p.BlazerMeasurements)
                    .HasForeignKey<BlazerMeasurements>(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__BlazerMea__Stitc__59063A47");
            });

            modelBuilder.Entity<BusinessEntity>(entity =>
            {
                entity.HasKey(e => e.EntityCode)
                    .HasName("PK__Business__D062AD0BDBD1EEC0");

                entity.Property(e => e.EntityAddress).HasColumnName("Entity_Address");

                entity.HasOne(d => d.EntityTypeNavigation)
                    .WithMany(p => p.BusinessEntity)
                    .HasForeignKey(d => d.EntityType)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__BusinessE__Entit__145C0A3F");
            });

            modelBuilder.Entity<BusinessEntityType>(entity =>
            {
                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasColumnName("Entity Name");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).IsRequired();
            });

            modelBuilder.Entity<DepartmentType>(entity =>
            {
                entity.Property(e => e.DepartmentName).IsRequired();
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.SalesPerson)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.SalesPersonId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Invoice__SalesPe__412EB0B6");

                entity.HasOne(d => d.ShopCodeNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.ShopCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Invoice__ShopCod__403A8C7D");
            });

            modelBuilder.Entity<InvoiceDetails>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceId, e.StitchingJobId })
                    .HasName("PK__InvoiceD__AE755EA949DD13F9");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__InvoiceDe__Invoi__440B1D61");

                entity.HasOne(d => d.StitchingJob)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__InvoiceDe__Stitc__44FF419A");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItemCode)
                    .HasName("PK__tmp_ms_x__3ECC0FEB81D0863B");

                entity.Property(e => e.BrandName).IsRequired();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UnitofMeasure)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.VendorName).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Item__CategoryId__17F790F9");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Item__SubCategor__18EBB532");

                entity.HasOne(d => d.WarehouseCodeNavigation)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.WarehouseCode)
                    .HasConstraintName("FK__Item__WarehouseC__17036CC0");
            });

            modelBuilder.Entity<JobType>(entity =>
            {
                entity.Property(e => e.JobName).IsRequired();
            });

            modelBuilder.Entity<KurtaMeasurements>(entity =>
            {
                entity.HasKey(e => e.StitchingJobId)
                    .HasName("PK__KurtaMea__9E3F41C67007BDFC");

                entity.Property(e => e.StitchingJobId).ValueGeneratedNever();

                entity.Property(e => e.OtherDetails)
                    .IsRequired()
                    .HasColumnName("Other_Details");

                entity.HasOne(d => d.StitchingJob)
                    .WithOne(p => p.KurtaMeasurements)
                    .HasForeignKey<KurtaMeasurements>(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__KurtaMeas__Stitc__534D60F1");
            });

            modelBuilder.Entity<OrderfromVendor>(entity =>
            {
                entity.Property(e => e.BrandName).IsRequired();

                entity.Property(e => e.UnitofMeasure)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.VendorName).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.OrderfromVendor)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Orderfrom__Categ__4BAC3F29");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.OrderfromVendor)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Orderfrom__SubCa__4CA06362");

                entity.HasOne(d => d.WarehouseManagerNavigation)
                    .WithMany(p => p.OrderfromVendor)
                    .HasForeignKey(d => d.WarehouseManager)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Orderfrom__Wareh__4AB81AF0");
            });

            modelBuilder.Entity<PantMeasurements>(entity =>
            {
                entity.HasKey(e => e.StitchingJobId)
                    .HasName("PK__PantMeas__9E3F41C65B739BEB");

                entity.Property(e => e.StitchingJobId).ValueGeneratedNever();

                entity.Property(e => e.OtherDetails)
                    .IsRequired()
                    .HasColumnName("Other_Details");

                entity.HasOne(d => d.StitchingJob)
                    .WithOne(p => p.PantMeasurements)
                    .HasForeignKey<PantMeasurements>(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__PantMeasu__Stitc__5EBF139D");
            });

            modelBuilder.Entity<ProductionActivity>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.StitchingJob)
                    .WithMany(p => p.ProductionActivity)
                    .HasForeignKey(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Productio__Stitc__4F7CD00D");

                entity.HasOne(d => d.StitchingUnitEmployee)
                    .WithMany(p => p.ProductionActivity)
                    .HasForeignKey(d => d.StitchingUnitEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Productio__Stitc__5070F446");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.Property(e => e.ReceiptId)
                    .HasColumnName("ReceiptID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Receipt)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Receipt__Invoice__47DBAE45");
            });

            modelBuilder.Entity<ShalwarMeasurements>(entity =>
            {
                entity.HasKey(e => e.StitchingJobId)
                    .HasName("PK__ShalwarM__9E3F41C622CCE744");

                entity.Property(e => e.StitchingJobId).ValueGeneratedNever();

                entity.Property(e => e.OtherDetails)
                    .IsRequired()
                    .HasColumnName("Other_Details");

                entity.HasOne(d => d.StitchingJob)
                    .WithOne(p => p.ShalwarMeasurements)
                    .HasForeignKey<ShalwarMeasurements>(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__ShalwarMe__Stitc__5629CD9C");
            });

            modelBuilder.Entity<ShirtMeasurements>(entity =>
            {
                entity.HasKey(e => e.StitchingJobId)
                    .HasName("PK__ShirtMea__9E3F41C6F61478E0");

                entity.Property(e => e.StitchingJobId).ValueGeneratedNever();

                entity.Property(e => e.OtherDetails)
                    .IsRequired()
                    .HasColumnName("Other_Details");

                entity.HasOne(d => d.StitchingJob)
                    .WithOne(p => p.ShirtMeasurements)
                    .HasForeignKey<ShirtMeasurements>(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__ShirtMeas__Stitc__619B8048");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => new { e.ShopCode, e.AssociatedWarehouseCode })
                    .HasName("PK__Shop__174134DB216AA017");

                entity.HasOne(d => d.AssociatedWarehouseCodeNavigation)
                    .WithMany(p => p.ShopAssociatedWarehouseCodeNavigation)
                    .HasForeignKey(d => d.AssociatedWarehouseCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Shop__Associated__182C9B23");

                entity.HasOne(d => d.ShopCodeNavigation)
                    .WithMany(p => p.ShopShopCodeNavigation)
                    .HasForeignKey(d => d.ShopCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Shop__ShopCode__173876EA");
            });

            modelBuilder.Entity<ShopOrder>(entity =>
            {
                entity.HasOne(d => d.ItemCodeNavigation)
                    .WithMany(p => p.ShopOrder)
                    .HasForeignKey(d => d.ItemCode)
                    .HasConstraintName("FK__ShopOrder__ItemC__160F4887");

                entity.HasOne(d => d.ShopCodeNavigation)
                    .WithMany(p => p.ShopOrder)
                    .HasForeignKey(d => d.ShopCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__ShopOrder__ShopC__02FC7413");
            });

            modelBuilder.Entity<StitchingJob>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK__Stitchin__056690C24874F51C");

                entity.Property(e => e.JobId).ValueGeneratedNever();

                entity.Property(e => e.ExpectedDate).HasColumnType("date");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.StitchingJob)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Stitching__Custo__35BCFE0A");

                entity.HasOne(d => d.JobType)
                    .WithMany(p => p.StitchingJob)
                    .HasForeignKey(d => d.JobTypeId)
                    .HasConstraintName("FK__Stitching__JobTy__34C8D9D1");

                entity.HasOne(d => d.ShopCodeNavigation)
                    .WithMany(p => p.StitchingJob)
                    .HasForeignKey(d => d.ShopCode)
                    .HasConstraintName("FK__Stitching__ShopC__36B12243");
            });

            modelBuilder.Entity<StitchingJobDetails>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.ItemCode })
                    .HasName("PK__Stitchin__A68A503C9AD00E32");

                entity.HasOne(d => d.ItemCodeNavigation)
                    .WithMany(p => p.StitchingJobDetails)
                    .HasForeignKey(d => d.ItemCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Stitching__ItemC__19DFD96B");
            });

            modelBuilder.Entity<StitchingUnitDepartmentHead>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.AssociatedDepartmentId })
                    .HasName("PK__Stitchin__DDE648143BC67C71");

                entity.HasOne(d => d.AssociatedDepartment)
                    .WithMany(p => p.StitchingUnitDepartmentHead)
                    .HasForeignKey(d => d.AssociatedDepartmentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Stitching__Assoc__267ABA7A");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.StitchingUnitDepartmentHead)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__StitchingUni__Id__25869641");
            });

            modelBuilder.Entity<StitchingUnitEmployee>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.StitchingUnitEmployee)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Stitching__Depar__1FCDBCEB");

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.StitchingUnitEmployee)
                    .HasForeignKey(d => d.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Stitching__Wareh__1ED998B2");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.SubCategoryName).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__SubCatego__Categ__2B3F6F97");
            });

            modelBuilder.Entity<SuitMeasurements>(entity =>
            {
                entity.HasKey(e => e.StitchingJobId)
                    .HasName("PK__SuitMeas__9E3F41C640DC6323");

                entity.Property(e => e.StitchingJobId).ValueGeneratedNever();

                entity.Property(e => e.OtherDetails)
                    .IsRequired()
                    .HasColumnName("Other_Details");

                entity.HasOne(d => d.StitchingJob)
                    .WithOne(p => p.SuitMeasurements)
                    .HasForeignKey<SuitMeasurements>(d => d.StitchingJobId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__SuitMeasu__Stitc__5BE2A6F2");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.JoiningDate).HasColumnType("date");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.UserTypeNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserType)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Users__UserType__70DDC3D8");

                entity.HasOne(d => d.WorksAtNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.WorksAt)
                    .HasConstraintName("FK__Users__WorksAt__22AA2996");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => new { e.WarehouseCode, e.AssociatedStitchingUnitCode })
                    .HasName("PK__Warehous__01EEE79FEF41B168");

                entity.HasOne(d => d.AssociatedStitchingUnitCodeNavigation)
                    .WithMany(p => p.WarehouseAssociatedStitchingUnitCodeNavigation)
                    .HasForeignKey(d => d.AssociatedStitchingUnitCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Warehouse__Assoc__1BFD2C07");

                entity.HasOne(d => d.WarehouseCodeNavigation)
                    .WithMany(p => p.WarehouseWarehouseCodeNavigation)
                    .HasForeignKey(d => d.WarehouseCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Warehouse__Wareh__1B0907CE");
            });
        }
    }
}