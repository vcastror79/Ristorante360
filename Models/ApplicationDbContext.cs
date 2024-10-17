using System;
using System.Collections.Generic;
using Ristorante360Admin.Models;
using Microsoft.EntityFrameworkCore;
using Ristorante360Admin.Models;

namespace Ristorante360Admin.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderType> OrderTypes { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SuppliesForProduct> SuppliesForProducts { get; set; }

    public virtual DbSet<Supply> Supplies { get; set; }
    public virtual DbSet<UnitType> UnitTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Categoria");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Category_Name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK_Cliente");

            entity.ToTable("Client");

            entity.Property(e => e.ClientId).HasColumnName("Client_Id");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Full_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
        });
        modelBuilder.Entity<Error>(entity =>
        {
            entity.ToTable("Error");
            entity.HasKey(e => e.ErrorId);

            entity.Property(e => e.ErrorId).HasColumnName("Error_Id");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("Date_Time");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Error_Message");
            entity.Property(e => e.ExceptionMessage)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Exception_Message");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK_Inventario");

            entity.ToTable("Inventory");

            entity.Property(e => e.InventoryId).HasColumnName("Inventory_Id");
            entity.Property(e => e.AdmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Admission_Date");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("Expiration_Date");
            entity.Property(e => e.Amount).HasColumnName("Amount");
            entity.Property(e => e.UnitId).HasColumnName("Unit_Id");
            entity.Property(e => e.MinimumAmount).HasColumnName("Minimum_Amount");
            entity.Property(e => e.SuppliesId).HasColumnName("Supplies_Id");
            entity.Property(e => e.Lote).HasColumnName("Lote");

            entity.HasOne(d => d.oUnitType)
                .WithMany(p => p.Inventories)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_UnitType");

            entity.HasOne(d => d.oSupplies)
                .WithMany(p => p.Inventories)
                .HasForeignKey(d => d.SuppliesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Supplies");
        });


        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log");

            entity.Property(e => e.LogId).HasColumnName("Log_Id");
            entity.Property(e => e.Detail)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LogDate)
                .HasColumnType("datetime")
                .HasColumnName("Log_Date");
            entity.Property(e => e.Module)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_User");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Pedido");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("Order_Id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderStatusId).HasColumnName("Order_Status_Id");

            // Permitir que los campos acepten valores nulos
            entity.Property(e => e.OrderTypeId).HasColumnName("Order_Type_Id").IsRequired(false);
            entity.Property(e => e.PaymentMethodId).HasColumnName("Payment_Method_Id").IsRequired(false);
            entity.Property(e => e.TotalAmount).HasColumnName("Total_Amount").IsRequired(false);
            entity.Property(e => e.ClientId).HasColumnName("Client_Id").IsRequired(false); ;
            entity.Property(e => e.OrderSpecifications).HasColumnName("Order_Specifications").IsRequired(false); ;

            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Client");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Order_Status");


        });


        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.OrderProductId).HasName("PK_order_product");
            entity.ToTable("Order_product");

            entity.Property(e => e.OrderProductId)
                .ValueGeneratedNever()
                .HasColumnName("Order_Product_Id");

            entity.Property(e => e.OrderId).HasColumnName("Order_Id");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.Quantity).HasColumnName("Quantity"); // Mapeo del campo Quantity

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_product_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_product_Product");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PK_Estado_Pedido");

            entity.ToTable("Order_Status");

            entity.Property(e => e.OrderStatusId).HasColumnName("Order_Status_Id");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderType>(entity =>
        {
            entity.HasKey(e => e.OrderTypeId).HasName("PK_Tipo_Pedido");

            entity.ToTable("Order_Type");

            entity.Property(e => e.OrderTypeId).HasColumnName("Order_Type_Id");
            entity.Property(e => e.Type)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK_Tipo_Pago");

            entity.ToTable("Payment_Method");

            entity.Property(e => e.PaymentMethodId).HasColumnName("Payment_Method_Id");
            entity.Property(e => e.Type)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Producto");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Product_Description");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Product_Name");

            entity.HasOne(d => d.oCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Rol");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<SuppliesForProduct>(entity =>
        {
            entity.HasKey(e => e.SuppliesProductId);

            entity.ToTable("Supplies_For_Product");

            entity.Property(e => e.SuppliesProductId)
                .HasColumnName("Supplies_Product_Id");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.SuppliesId).HasColumnName("Supplies_Id");
            entity.Property(e => e.UnitId).HasColumnName("Unit_Id");

            entity.HasOne(d => d.oProduct).WithMany(p => p.SuppliesForProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplies_For_Product_Product");

            entity.HasOne(d => d.oSupplies).WithMany(p => p.SuppliesForProducts)
                .HasForeignKey(d => d.SuppliesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplies_For_Product_Supplies");

            entity.HasOne(d => d.oUnitType)
                .WithMany()
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplies_For_Product_UnitType");
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.HasKey(e => e.SuppliesId);

            entity.Property(e => e.SuppliesId).HasColumnName("Supplies_Id");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });
        modelBuilder.Entity<UnitType>(entity =>
        {
            entity.HasKey(e => e.UnitId);

            entity.ToTable("Unit_Type");

            entity.Property(e => e.UnitId).HasColumnName("Unit_Id");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Usuario");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.Surname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TokenRecovery)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.oRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.NotificationId);
            entity.Property(n => n.NotificationId).UseIdentityColumn(); // Configurar como columna con identidad
            entity.Property(n => n.NotificationMessage).HasMaxLength(200);
            entity.Property(n => n.CreatedDate).IsRequired();
            entity.Property(n => n.IsRead).IsRequired();

            entity.HasOne(n => n.Inventory)
                .WithMany(i => i.Notifications)
                .HasForeignKey(n => n.SuppliesId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
