﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RHCQS_DataAccessObjects.Models;

namespace RHCQS_DataAccessObjects.Context;

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

    public virtual DbSet<BatchPayment> BatchPayments { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<ConstructionItem> ConstructionItems { get; set; }

    public virtual DbSet<ConstructionWork> ConstructionWorks { get; set; }

    public virtual DbSet<ConstructionWorkResource> ConstructionWorkResources { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DesignPrice> DesignPrices { get; set; }

    public virtual DbSet<DesignTemplate> DesignTemplates { get; set; }

    public virtual DbSet<EquipmentItem> EquipmentItems { get; set; }

    public virtual DbSet<FinalQuotation> FinalQuotations { get; set; }

    public virtual DbSet<FinalQuotationItem> FinalQuotationItems { get; set; }

    public virtual DbSet<HouseDesignDrawing> HouseDesignDrawings { get; set; }

    public virtual DbSet<HouseDesignVersion> HouseDesignVersions { get; set; }

    public virtual DbSet<InitialQuotation> InitialQuotations { get; set; }

    public virtual DbSet<InitialQuotationItem> InitialQuotationItems { get; set; }

    public virtual DbSet<Labor> Labors { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialSection> MaterialSections { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageHouse> PackageHouses { get; set; }

    public virtual DbSet<PackageLabor> PackageLabors { get; set; }

    public virtual DbSet<PackageMapPromotion> PackageMapPromotions { get; set; }

    public virtual DbSet<PackageMaterial> PackageMaterials { get; set; }

    public virtual DbSet<PackageQuotation> PackageQuotations { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<QuotationItem> QuotationItems { get; set; }

    public virtual DbSet<QuotationUtility> QuotationUtilities { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<SubConstructionItem> SubConstructionItems { get; set; }

    public virtual DbSet<SubTemplate> SubTemplates { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TemplateItem> TemplateItems { get; set; }

    public virtual DbSet<UtilitiesItem> UtilitiesItems { get; set; }

    public virtual DbSet<UtilitiesSection> UtilitiesSections { get; set; }

    public virtual DbSet<UtilityOption> UtilityOptions { get; set; }

    public virtual DbSet<WorkTemplate> WorkTemplates { get; set; }

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

            entity.HasOne(d => d.Account).WithMany(p => p.AssignTasks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignTask_Account");

            entity.HasOne(d => d.Project).WithMany(p => p.AssignTasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_AssignTask_Project");
        });

        modelBuilder.Entity<BatchPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InstallmentPayment");

            entity.ToTable("BatchPayment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.BatchPayments)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_BactchPayment_Contract");

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.BatchPayments)
                .HasForeignKey(d => d.FinalQuotationId)
                .HasConstraintName("FK_BactchPayment_FinalQuotation");

            entity.HasOne(d => d.InitialQuotation).WithMany(p => p.BatchPayments)
                .HasForeignKey(d => d.InitialQuotationId)
                .HasConstraintName("FK_BactchPayment_InitialQuotation");

            entity.HasOne(d => d.Payment).WithMany(p => p.BatchPayments)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BatchPayment_Payment_FK");
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
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ConstructionWork>(entity =>
        {
            entity.ToTable("ConstructionWork");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Unit)
                .HasMaxLength(5)
                .IsFixedLength();
            entity.Property(e => e.WorkName).HasMaxLength(500);

            entity.HasOne(d => d.Construction).WithMany(p => p.ConstructionWorks)
                .HasForeignKey(d => d.ConstructionId)
                .HasConstraintName("FK_ConstructionWork_ConstructionItems");
        });

        modelBuilder.Entity<ConstructionWorkResource>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("UpdateWorkTemplateCostsResources");
                    tb.HasTrigger("trg_UpdateWorkTemplateCosts");
                });

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.ConstructionWork).WithMany(p => p.ConstructionWorkResources)
                .HasForeignKey(d => d.ConstructionWorkId)
                .HasConstraintName("FK_ConstructionWorkResources_ConstructionWork");

            entity.HasOne(d => d.Labor).WithMany(p => p.ConstructionWorkResources)
                .HasForeignKey(d => d.LaborId)
                .HasConstraintName("FK_ConstructionWorkResources_MaterialSection1");

            entity.HasOne(d => d.MaterialSection).WithMany(p => p.ConstructionWorkResources)
                .HasForeignKey(d => d.MaterialSectionId)
                .HasConstraintName("FK_ConstructionWorkResources_MaterialSection");
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
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaxCode).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(100);
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
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Customer_Account");
        });

        modelBuilder.Entity<DesignPrice>(entity =>
        {
            entity.ToTable("DesignPrice");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DesignTemplate>(entity =>
        {
            entity.ToTable("DesignTemplate");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<EquipmentItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EquimentItem");

            entity.ToTable("EquipmentItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(50);

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.EquipmentItems)
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

            entity.HasOne(d => d.Project).WithMany(p => p.FinalQuotations)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinalQuotation_Project");

            entity.HasOne(d => d.Promotion).WithMany(p => p.FinalQuotations)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_FinalQuotation_Promotion");
        });

        modelBuilder.Entity<FinalQuotationItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DetailedQuotationItem");

            entity.ToTable("FinalQuotationItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.FinalQuotationItems)
                .HasForeignKey(d => d.FinalQuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinalQuotationItem_FinalQuotation");
        });

        modelBuilder.Entity<HouseDesignDrawing>(entity =>
        {
            entity.ToTable("HouseDesignDrawing");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.HouseDesignDrawings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_HouseDesignDrawing_Account");

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
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.HouseDesignDrawing).WithMany(p => p.HouseDesignVersions)
                .HasForeignKey(d => d.HouseDesignDrawingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HouseDesignVersion_HouseDesignDrawing");
        });

        modelBuilder.Entity<InitialQuotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PreliminaryQuotation");

            entity.ToTable("InitialQuotation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(5);

            entity.HasOne(d => d.Project).WithMany(p => p.InitialQuotations)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InitialQuotation_Project");

            entity.HasOne(d => d.Promotion).WithMany(p => p.InitialQuotations)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_InitialQuotation_Promotion");
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
            entity.ToTable("Labor", tb => tb.HasTrigger("UpdateWorkTemplateCostsLabor"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Material", tb => tb.HasTrigger("UpdateWorkTemplateCosts"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Shape).HasMaxLength(50);
            entity.Property(e => e.Size).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(20);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.MaterialSection).WithMany(p => p.Materials)
                .HasForeignKey(d => d.MaterialSectionId)
                .HasConstraintName("Material_MaterialSection_FK");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Materials)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_Supplier");
        });

        modelBuilder.Entity<MaterialSection>(entity =>
        {
            entity.ToTable("MaterialSection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.DesignTemplate).WithMany(p => p.Media)
                .HasForeignKey(d => d.DesignTemplateId)
                .HasConstraintName("Media_DesignTemplate_FK");

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.Media)
                .HasForeignKey(d => d.FinalQuotationId)
                .HasConstraintName("FK_Media_FinalQuotation");

            entity.HasOne(d => d.HouseDesignVersion).WithMany(p => p.Media)
                .HasForeignKey(d => d.HouseDesignVersionId)
                .HasConstraintName("FK_Media_HouseDesignVersion");

            entity.HasOne(d => d.InitialQuotation).WithMany(p => p.Media)
                .HasForeignKey(d => d.InitialQuotationId)
                .HasConstraintName("FK_Media_InitialQuotation");

            entity.HasOne(d => d.Payment).WithMany(p => p.Media)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Media_Payment");

            entity.HasOne(d => d.SubTemplate).WithMany(p => p.Media)
                .HasForeignKey(d => d.SubTemplateId)
                .HasConstraintName("Media_SubTemplate_FK");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SendAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Message_Account");

            entity.HasOne(d => d.Room).WithMany(p => p.Messages)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_Room");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("Package");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.PackageName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(20);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
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

            entity.HasOne(d => d.Package).WithMany(p => p.PackageLabors)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageLabor_Package");
        });

        modelBuilder.Entity<PackageMapPromotion>(entity =>
        {
            entity.ToTable("PackageMapPromotion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageMapPromotions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageMapPromotion_Package");

            entity.HasOne(d => d.Promotion).WithMany(p => p.PackageMapPromotions)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageMapPromotion_Promotion");
        });

        modelBuilder.Entity<PackageMaterial>(entity =>
        {
            entity.ToTable("PackageMaterial");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Material).WithMany(p => p.PackageMaterials)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("PackageMaterial_Material_FK");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageMaterials)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageMaterial_Package");
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

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentPhase).HasColumnType("datetime");
            entity.Property(e => e.Unit).HasMaxLength(10);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

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
            entity.Property(e => e.CustomerName).HasMaxLength(50);
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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Project_Customer");

            entity.HasOne(d => d.DesignPrice).WithMany(p => p.Projects)
                .HasForeignKey(d => d.DesignPriceId)
                .HasConstraintName("FK_Project_DesignPrice");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ExpTime).HasColumnType("datetime");
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Unit).HasMaxLength(10);
        });

        modelBuilder.Entity<QuotationItem>(entity =>
        {
            entity.ToTable("QuotationItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.FinalQuotationItem).WithMany(p => p.QuotationItems)
                .HasForeignKey(d => d.FinalQuotationItemId)
                .HasConstraintName("QuotationItem_FinalQuotationItem_FK");

            entity.HasOne(d => d.WorkTemplate).WithMany(p => p.QuotationItems)
                .HasForeignKey(d => d.WorkTemplateId)
                .HasConstraintName("FK_QuotationItem_WorkTemplate");
        });

        modelBuilder.Entity<QuotationUtility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UltilitiesDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FinalQuotationId).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.FinalQuotation).WithMany(p => p.QuotationUtilities)
                .HasForeignKey(d => d.FinalQuotationId)
                .HasConstraintName("QuotationUtilities_FinalQuotation_FK");

            entity.HasOne(d => d.InitialQuotation).WithMany(p => p.QuotationUtilities)
                .HasForeignKey(d => d.InitialQuotationId)
                .HasConstraintName("FK_QuotationUtilities_InitialQuotation");

            entity.HasOne(d => d.UtilitiesItem).WithMany(p => p.QuotationUtilities)
                .HasForeignKey(d => d.UtilitiesItemId)
                .HasConstraintName("FK_QuotationUtilities_UtilitiesItem");

            entity.HasOne(d => d.UtilitiesSection).WithMany(p => p.QuotationUtilities)
                .HasForeignKey(d => d.UtilitiesSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationUtilities_UtilitiesSection");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.RoomReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK_Room_Account1");

            entity.HasOne(d => d.Sender).WithMany(p => p.RoomSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Room_Account");
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
            entity.Property(e => e.Code).HasMaxLength(5);
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

            entity.HasOne(d => d.SubConstruction).WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.SubConstructionId)
                .HasConstraintName("TemplateItem_SubConstructionItems_FK");

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
            entity.Property(e => e.Unit).HasMaxLength(100);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");

            entity.HasOne(d => d.Utilities).WithMany(p => p.UtilitiesSections)
                .HasForeignKey(d => d.UtilitiesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UltilitiesSection_Ultilities");
        });

        modelBuilder.Entity<UtilityOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Ultilities");

            entity.ToTable("UtilityOption");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpsDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<WorkTemplate>(entity =>
        {
            entity.ToTable("WorkTemplate", tb => tb.HasTrigger("trg_CalculateTotalCost"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasColumnType("datetime");

            entity.HasOne(d => d.ContructionWork).WithMany(p => p.WorkTemplates)
                .HasForeignKey(d => d.ContructionWorkId)
                .HasConstraintName("FK_WorkTemplate_ConstructionWork");

            entity.HasOne(d => d.Package).WithMany(p => p.WorkTemplates)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_WorkTemplate_Package");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
