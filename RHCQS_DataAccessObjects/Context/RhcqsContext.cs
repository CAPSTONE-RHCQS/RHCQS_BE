using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RHCQS_DataAccessObjects.Models;

namespace RHCQS_DataAccessObjects;

public partial class RhcqsContext : DbContext
{
    public RhcqsContext()
    {
    }

    public RhcqsContext(DbContextOptions<RhcqsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AssignTask> AssignTasks { get; set; }

    public virtual DbSet<BactchPayment> BactchPayments { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<ConstructionItem> ConstructionItems { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DesignTemplate> DesignTemplates { get; set; }

    public virtual DbSet<EquimentItem> EquimentItems { get; set; }

    public virtual DbSet<FinalQuotation> FinalQuotations { get; set; }

    public virtual DbSet<FinalQuotationItem> FinalQuotationItems { get; set; }

    public virtual DbSet<HouseDesignDrawing> HouseDesignDrawings { get; set; }

    public virtual DbSet<HouseDesignVersion> HouseDesignVersions { get; set; }

    public virtual DbSet<InitialQuotation> InitialQuotations { get; set; }

    public virtual DbSet<InitialQuotationItem> InitialQuotationItems { get; set; }

    public virtual DbSet<Labor> Labors { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialSection> MaterialSections { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageDetail> PackageDetails { get; set; }

    public virtual DbSet<PackageHouse> PackageHouses { get; set; }

    public virtual DbSet<PackageLabor> PackageLabors { get; set; }

    public virtual DbSet<PackageMaterial> PackageMaterials { get; set; }

    public virtual DbSet<PackageQuotation> PackageQuotations { get; set; }

    public virtual DbSet<PackageType> PackageTypes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<QuotationItem> QuotationItems { get; set; }

    public virtual DbSet<QuotationLabor> QuotationLabors { get; set; }

    public virtual DbSet<QuotationMaterial> QuotationMaterials { get; set; }

    public virtual DbSet<QuotationSection> QuotationSections { get; set; }

    public virtual DbSet<QuotationUtility> QuotationUtilities { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubConstructionItem> SubConstructionItems { get; set; }

    public virtual DbSet<SubTemplate> SubTemplates { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TemplateItem> TemplateItems { get; set; }

    public virtual DbSet<UtilitiesItem> UtilitiesItems { get; set; }

    public virtual DbSet<UtilitiesSection> UtilitiesSections { get; set; }

    public virtual DbSet<Utility> Utilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(60);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<AssignTask>(entity =>
        {
            entity.ToTable("AssignTask");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.AssignTasks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignTask_Account");
        });

        modelBuilder.Entity<BactchPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InstallmentPayment");

            entity.ToTable("BactchPayment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentPhase).HasColumnType("datetime");
            entity.Property(e => e.Percents).HasMaxLength(5);

            entity.HasOne(d => d.Contract).WithMany(p => p.BactchPayments)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BactchPayment_Contract");

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.BactchPayments)
                .HasForeignKey(d => d.FinalQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BactchPayment_DetailedQuotation");

            entity.HasOne(d => d.IntitialQuotation).WithMany(p => p.BactchPayments)
                .HasForeignKey(d => d.IntitialQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BactchPayment_InitialQuotation");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blog_Account");
        });

        modelBuilder.Entity<ConstructionItem>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContractCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TaxCode).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasMaxLength(50);

            entity.HasOne(d => d.Project).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contract_Project");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(60);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<DesignTemplate>(entity =>
        {
            entity.ToTable("DesignTemplate");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<EquimentItem>(entity =>
        {
            entity.ToTable("EquimentItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Note)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Unit).HasMaxLength(50);

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.EquimentItems)
                .HasForeignKey(d => d.FinalQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquimentItem_FinalQuotation");
        });

        modelBuilder.Entity<FinalQuotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OfficialQuotation");

            entity.ToTable("FinalQuotation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
            entity.Property(e => e.Version).HasMaxLength(10);

            entity.HasOne(d => d.Account).WithMany(p => p.FinalQuotations)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailedQuotation_Account");

            entity.HasOne(d => d.Project).WithMany(p => p.FinalQuotations)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailedQuotation_Project");

            entity.HasOne(d => d.Promotion).WithMany(p => p.FinalQuotations)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_DetailedQuotation_Promotion");

            entity.HasOne(d => d.QuotationUtilities).WithMany(p => p.FinalQuotations)
                .HasForeignKey(d => d.QuotationUtilitiesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailedQuotation_QuoationUltities");
        });

        modelBuilder.Entity<FinalQuotationItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DetailedQuotationItem");

            entity.ToTable("FinalQuotationItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.Weight).HasMaxLength(50);

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.FinalQuotationItems)
                .HasForeignKey(d => d.FinalQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailedQuotationItem_DetailedQuotation");
        });

        modelBuilder.Entity<HouseDesignDrawing>(entity =>
        {
            entity.ToTable("HouseDesignDrawing");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.AssignTask).WithMany(p => p.HouseDesignDrawings)
                .HasForeignKey(d => d.AssignTaskId)
                .HasConstraintName("FK_HouseDesignDrawing_AssignTask");

            entity.HasOne(d => d.Project).WithMany(p => p.HouseDesignDrawings)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HouseDesignDrawing_Project");
        });

        modelBuilder.Entity<HouseDesignVersion>(entity =>
        {
            entity.ToTable("HouseDesignVersion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.HouseDesignDrawing).WithMany(p => p.HouseDesignVersions)
                .HasForeignKey(d => d.HouseDesignDrawingId)
                .HasConstraintName("FK_HouseDesignVersion_HouseDesignDrawing");
        });

        modelBuilder.Entity<InitialQuotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PreliminaryQuotation");

            entity.ToTable("InitialQuotation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.InitialQuotations)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_InitialQuotation_Account");

            entity.HasOne(d => d.Project).WithMany(p => p.InitialQuotations)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InitialQuotation_Project");
        });

        modelBuilder.Entity<InitialQuotationItem>(entity =>
        {
            entity.ToTable("InitialQuotationItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasMaxLength(10);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.ConstructionItem).WithMany(p => p.InitialQuotationItems)
                .HasForeignKey(d => d.ConstructionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InitialQuotationItem_ConstructionItems");

            entity.HasOne(d => d.InitialQuotation).WithMany(p => p.InitialQuotationItems)
                .HasForeignKey(d => d.InitialQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InitialQuotationItem_InitialQuotation");
        });

        modelBuilder.Entity<Labor>(entity =>
        {
            entity.ToTable("Labor");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Material");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Shape).HasMaxLength(50);
            entity.Property(e => e.Size).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.MaterialType).WithMany(p => p.Materials)
                .HasForeignKey(d => d.MaterialTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_MaterialType");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Materials)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_Supplier");
        });

        modelBuilder.Entity<MaterialSection>(entity =>
        {
            entity.ToTable("MaterialSection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialSections)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaterialSection_Material");
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.ToTable("MaterialType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.HouseDesignDrawing).WithMany(p => p.Media)
                .HasForeignKey(d => d.HouseDesignDrawingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Media_HouseDesignDrawing");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Reciver).HasMaxLength(50);
            entity.Property(e => e.Sender).HasMaxLength(50);
            entity.Property(e => e.Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("Package");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.PackageName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.PackageType).WithMany(p => p.Packages)
                .HasForeignKey(d => d.PackageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Package_PackageType");
        });

        modelBuilder.Entity<PackageDetail>(entity =>
        {
            entity.ToTable("PackageDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Package).WithMany(p => p.PackageDetails)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageDetail_Package");
        });

        modelBuilder.Entity<PackageHouse>(entity =>
        {
            entity.ToTable("PackageHouse");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.DesignTemplate).WithMany(p => p.PackageHouses)
                .HasForeignKey(d => d.DesignTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageHouse_DesignTemplate");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageHouses)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageHouse_Package");
        });

        modelBuilder.Entity<PackageLabor>(entity =>
        {
            entity.ToTable("PackageLabor");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Labor).WithMany(p => p.PackageLabors)
                .HasForeignKey(d => d.LaborId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageLabor_Labor");

            entity.HasOne(d => d.PackageDetail).WithMany(p => p.PackageLabors)
                .HasForeignKey(d => d.PackageDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageLabor_PackageDetail");
        });

        modelBuilder.Entity<PackageMaterial>(entity =>
        {
            entity.ToTable("PackageMaterial");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.MaterialSection).WithMany(p => p.PackageMaterials)
                .HasForeignKey(d => d.MaterialSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageMaterial_MaterialSection");

            entity.HasOne(d => d.PackageDetail).WithMany(p => p.PackageMaterials)
                .HasForeignKey(d => d.PackageDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageMaterial_PackageMaterial");
        });

        modelBuilder.Entity<PackageQuotation>(entity =>
        {
            entity.ToTable("PackageQuotation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.InitialQuotation).WithMany(p => p.PackageQuotations)
                .HasForeignKey(d => d.InitialQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotation_InitialQuotation");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageQuotations)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotation_Package");
        });

        modelBuilder.Entity<PackageType>(entity =>
        {
            entity.ToTable("PackageType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.BatchPayment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BatchPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_BactchPayment");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_PaymentType");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.ToTable("PaymentType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(5)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Project_Customer");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AvailableTime).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<QuotationItem>(entity =>
        {
            entity.ToTable("QuotationItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectComponent).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.QuotationSection).WithMany(p => p.QuotationItems)
                .HasForeignKey(d => d.QuotationSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationItem_QuotationSection");
        });

        modelBuilder.Entity<QuotationLabor>(entity =>
        {
            entity.ToTable("QuotationLabor");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Labor).WithMany(p => p.QuotationLabors)
                .HasForeignKey(d => d.LaborId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationLabor_Labor");

            entity.HasOne(d => d.QuotationItem).WithMany(p => p.QuotationLabors)
                .HasForeignKey(d => d.QuotationItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationLabor_QuotationItem");
        });

        modelBuilder.Entity<QuotationMaterial>(entity =>
        {
            entity.ToTable("QuotationMaterial");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Unit).HasMaxLength(50);

            entity.HasOne(d => d.Material).WithMany(p => p.QuotationMaterials)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationMaterial_Material");

            entity.HasOne(d => d.QuotationItem).WithMany(p => p.QuotationMaterials)
                .HasForeignKey(d => d.QuotationItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationMaterial_QuotationItem");
        });

        modelBuilder.Entity<QuotationSection>(entity =>
        {
            entity.ToTable("QuotationSection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Package).WithMany(p => p.QuotationSections)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationSection_Package");
        });

        modelBuilder.Entity<QuotationUtility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UltilitiesDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.InitialQuotation).WithMany(p => p.QuotationUtilities)
                .HasForeignKey(d => d.InitialQuotationId)
                .HasConstraintName("FK_QuotationUtilities_InitialQuotation");

            entity.HasOne(d => d.UltilitiesItem).WithMany(p => p.QuotationUtilities)
                .HasForeignKey(d => d.UltilitiesItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuoationUltities_UltilitiesItem");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SubConstructionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RoughCategory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.ConstructionItems).WithMany(p => p.SubConstructionItems)
                .HasForeignKey(d => d.ConstructionItemsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubConstructionItems_ConstructionItems");
        });

        modelBuilder.Entity<SubTemplate>(entity =>
        {
            entity.ToTable("SubTemplate");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Size)
                .HasMaxLength(9)
                .IsFixedLength();

            entity.HasOne(d => d.DesignTemplate).WithMany(p => p.SubTemplates)
                .HasForeignKey(d => d.DesignTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubTemplate_DesignTemplate");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConstractPhone).HasMaxLength(11);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TemplateItem>(entity =>
        {
            entity.ToTable("TemplateItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.ConstructionItem).WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.ConstructionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TemplateItem_ConstructionItems");

            entity.HasOne(d => d.SubTemplate).WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.SubTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TemplateItem_SubTemplate");
        });

        modelBuilder.Entity<UtilitiesItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UltilitiesItem");

            entity.ToTable("UtilitiesItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Section).WithMany(p => p.UtilitiesItems)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UltilitiesItem_UltilitiesSection");
        });

        modelBuilder.Entity<UtilitiesSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UltilitiesSection");

            entity.ToTable("UtilitiesSection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Ultilities).WithMany(p => p.UtilitiesSections)
                .HasForeignKey(d => d.UltilitiesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UltilitiesSection_Ultilities");
        });

        modelBuilder.Entity<Utility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Ultilities");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
