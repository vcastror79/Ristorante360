﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ristorante360Admin.Models;

#nullable disable

namespace Ristorante360.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ristorante360Admin.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Category_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Category_Name");

                    b.HasKey("CategoryId")
                        .HasName("PK_Categoria");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Client_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"));

                    b.Property<string>("Address")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Full_Name");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Phone_Number");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("ClientId")
                        .HasName("PK_Cliente");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Error", b =>
                {
                    b.Property<int>("ErrorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Error_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ErrorId"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("Date_Time");

                    b.Property<string>("ErrorMessage")
                        .HasMaxLength(4000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(4000)")
                        .HasColumnName("Error_Message");

                    b.Property<string>("ExceptionMessage")
                        .HasMaxLength(4000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(4000)")
                        .HasColumnName("Exception_Message");

                    b.HasKey("ErrorId");

                    b.ToTable("Error", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Inventory", b =>
                {
                    b.Property<int>("InventoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Inventory_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InventoryId"));

                    b.Property<DateTime>("AdmissionDate")
                        .HasColumnType("datetime")
                        .HasColumnName("Admission_Date");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("Amount");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("Expiration_Date");

                    b.Property<int>("Lote")
                        .HasColumnType("int")
                        .HasColumnName("Lote");

                    b.Property<int>("MinimumAmount")
                        .HasColumnType("int")
                        .HasColumnName("Minimum_Amount");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("SuppliesId")
                        .HasColumnType("int")
                        .HasColumnName("Supplies_Id");

                    b.Property<int>("UnitId")
                        .HasColumnType("int")
                        .HasColumnName("Unit_Id");

                    b.HasKey("InventoryId")
                        .HasName("PK_Inventario");

                    b.HasIndex("SuppliesId");

                    b.HasIndex("UnitId");

                    b.ToTable("Inventory", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Log_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogId"));

                    b.Property<string>("Detail")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime")
                        .HasColumnName("Log_Date");

                    b.Property<string>("Module")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    b.HasKey("LogId");

                    b.HasIndex("UserId");

                    b.ToTable("Log", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("NotificationMessage")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SuppliesId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("SuppliesId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Order_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int")
                        .HasColumnName("Client_Id");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime")
                        .HasColumnName("Order_Date");

                    b.Property<string>("OrderSpecifications")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Order_Specifications");

                    b.Property<int>("OrderStatusId")
                        .HasColumnType("int")
                        .HasColumnName("Order_Status_Id");

                    b.Property<int?>("OrderTypeId")
                        .HasColumnType("int")
                        .HasColumnName("Order_Type_Id");

                    b.Property<int?>("PaymentMethodId")
                        .HasColumnType("int")
                        .HasColumnName("Payment_Method_Id");

                    b.Property<decimal?>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Total_Amount");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    b.HasKey("OrderId")
                        .HasName("PK_Pedido");

                    b.HasIndex("ClientId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.OrderProduct", b =>
                {
                    b.Property<int>("OrderProductId")
                        .HasColumnType("int")
                        .HasColumnName("Order_Product_Id");

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("Order_Id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("Product_Id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.HasKey("OrderProductId")
                        .HasName("PK_order_product");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("Order_product", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Order_Status_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderStatusId"));

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("OrderStatusId")
                        .HasName("PK_Estado_Pedido");

                    b.ToTable("Order_Status", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.OrderType", b =>
                {
                    b.Property<int>("OrderTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Order_Type_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderTypeId"));

                    b.Property<string>("Type")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("OrderTypeId")
                        .HasName("PK_Tipo_Pedido");

                    b.ToTable("Order_Type", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Payment_Method_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"));

                    b.Property<string>("Type")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.HasKey("PaymentMethodId")
                        .HasName("PK_Tipo_Pago");

                    b.ToTable("Payment_Method", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Product_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("Category_Id");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Product_Description");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Product_Name");

                    b.HasKey("ProductId")
                        .HasName("PK_Producto");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Role_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("Role_Name");

                    b.HasKey("RoleId")
                        .HasName("PK_Rol");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.SuppliesForProduct", b =>
                {
                    b.Property<int>("SuppliesProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Supplies_Product_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SuppliesProductId"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("Product_Id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SuppliesId")
                        .HasColumnType("int")
                        .HasColumnName("Supplies_Id");

                    b.Property<int>("UnitId")
                        .HasColumnType("int")
                        .HasColumnName("Unit_Id");

                    b.Property<int?>("UnitTypeUnitId")
                        .HasColumnType("int");

                    b.HasKey("SuppliesProductId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SuppliesId");

                    b.HasIndex("UnitId");

                    b.HasIndex("UnitTypeUnitId");

                    b.ToTable("Supplies_For_Product", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Supply", b =>
                {
                    b.Property<int>("SuppliesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Supplies_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SuppliesId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("SuppliesId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.UnitType", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Unit_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UnitId"));

                    b.Property<string>("Unit")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UnitId");

                    b.ToTable("Unit_Type", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<bool>("IsTemporaryPassword")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("Role_Id");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("TokenRecovery")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("UserId")
                        .HasName("PK_Usuario");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Inventory", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Supply", "oSupplies")
                        .WithMany("Inventories")
                        .HasForeignKey("SuppliesId")
                        .IsRequired()
                        .HasConstraintName("FK_Inventory_Supplies");

                    b.HasOne("Ristorante360Admin.Models.UnitType", "oUnitType")
                        .WithMany("Inventories")
                        .HasForeignKey("UnitId")
                        .IsRequired()
                        .HasConstraintName("FK_Inventory_UnitType");

                    b.Navigation("oSupplies");

                    b.Navigation("oUnitType");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Log", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Log_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Notification", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Inventory", "Inventory")
                        .WithMany("Notifications")
                        .HasForeignKey("SuppliesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Order", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK_Order_Client");

                    b.HasOne("Ristorante360Admin.Models.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Order_Status");

                    b.HasOne("Ristorante360Admin.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("OrderStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.OrderProduct", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_order_product_Order");

                    b.HasOne("Ristorante360Admin.Models.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_order_product_Product");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Product", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Category", "oCategory")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_Product_Category");

                    b.Navigation("oCategory");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.SuppliesForProduct", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Product", "oProduct")
                        .WithMany("SuppliesForProducts")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_Supplies_For_Product_Product");

                    b.HasOne("Ristorante360Admin.Models.Supply", "oSupplies")
                        .WithMany("SuppliesForProducts")
                        .HasForeignKey("SuppliesId")
                        .IsRequired()
                        .HasConstraintName("FK_Supplies_For_Product_Supplies");

                    b.HasOne("Ristorante360Admin.Models.UnitType", "oUnitType")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .IsRequired()
                        .HasConstraintName("FK_Supplies_For_Product_UnitType");

                    b.HasOne("Ristorante360Admin.Models.UnitType", null)
                        .WithMany("SuppliesForProducts")
                        .HasForeignKey("UnitTypeUnitId");

                    b.Navigation("oProduct");

                    b.Navigation("oSupplies");

                    b.Navigation("oUnitType");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.User", b =>
                {
                    b.HasOne("Ristorante360Admin.Models.Role", "oRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_User_Role");

                    b.Navigation("oRole");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Client", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Inventory", b =>
                {
                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.OrderStatus", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Product", b =>
                {
                    b.Navigation("OrderProducts");

                    b.Navigation("SuppliesForProducts");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.Supply", b =>
                {
                    b.Navigation("Inventories");

                    b.Navigation("SuppliesForProducts");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.UnitType", b =>
                {
                    b.Navigation("Inventories");

                    b.Navigation("SuppliesForProducts");
                });

            modelBuilder.Entity("Ristorante360Admin.Models.User", b =>
                {
                    b.Navigation("Logs");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
