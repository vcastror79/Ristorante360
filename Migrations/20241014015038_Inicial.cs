using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ristorante360.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Category_Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Client_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Full_Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Phone_Number = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Client_Id);
                });

            migrationBuilder.CreateTable(
                name: "Error",
                columns: table => new
                {
                    Error_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    Error_Message = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Exception_Message = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Error", x => x.Error_Id);
                });

            migrationBuilder.CreateTable(
                name: "Order_Status",
                columns: table => new
                {
                    Order_Status_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado_Pedido", x => x.Order_Status_Id);
                });

            migrationBuilder.CreateTable(
                name: "Order_Type",
                columns: table => new
                {
                    Order_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Pedido", x => x.Order_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Method",
                columns: table => new
                {
                    Payment_Method_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Pago", x => x.Payment_Method_Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Supplies_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Supplies_Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit_Type",
                columns: table => new
                {
                    Unit_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit_Type", x => x.Unit_Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Product_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Product_Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Product_Id);
                    table.ForeignKey(
                        name: "FK_Product_Category",
                        column: x => x.Category_Id,
                        principalTable: "Category",
                        principalColumn: "Category_Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Surname = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TokenRecovery = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.User_Id);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id");
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Inventory_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Supplies_Id = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Unit_Id = table.Column<int>(type: "int", nullable: false),
                    Lote = table.Column<int>(type: "int", nullable: false),
                    Minimum_Amount = table.Column<int>(type: "int", nullable: false),
                    Admission_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expiration_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Inventory_Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Supplies",
                        column: x => x.Supplies_Id,
                        principalTable: "Supplies",
                        principalColumn: "Supplies_Id");
                    table.ForeignKey(
                        name: "FK_Inventory_UnitType",
                        column: x => x.Unit_Id,
                        principalTable: "Unit_Type",
                        principalColumn: "Unit_Id");
                });

            migrationBuilder.CreateTable(
                name: "Supplies_For_Product",
                columns: table => new
                {
                    Supplies_Product_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Supplies_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false),
                    Unit_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitTypeUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies_For_Product", x => x.Supplies_Product_Id);
                    table.ForeignKey(
                        name: "FK_Supplies_For_Product_Product",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "Product_Id");
                    table.ForeignKey(
                        name: "FK_Supplies_For_Product_Supplies",
                        column: x => x.Supplies_Id,
                        principalTable: "Supplies",
                        principalColumn: "Supplies_Id");
                    table.ForeignKey(
                        name: "FK_Supplies_For_Product_UnitType",
                        column: x => x.Unit_Id,
                        principalTable: "Unit_Type",
                        principalColumn: "Unit_Id");
                    table.ForeignKey(
                        name: "FK_Supplies_For_Product_Unit_Type_UnitTypeUnitId",
                        column: x => x.UnitTypeUnitId,
                        principalTable: "Unit_Type",
                        principalColumn: "Unit_Id");
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Log_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Log_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Module = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Log_Id);
                    table.ForeignKey(
                        name: "FK_Log_User",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Order_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Order_Status_Id = table.Column<int>(type: "int", nullable: false),
                    Order_Type_Id = table.Column<int>(type: "int", nullable: true),
                    Payment_Method_Id = table.Column<int>(type: "int", nullable: true),
                    Total_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Client_Id = table.Column<int>(type: "int", nullable: true),
                    Order_Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Order_Id);
                    table.ForeignKey(
                        name: "FK_Order_Client",
                        column: x => x.Client_Id,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                    table.ForeignKey(
                        name: "FK_Order_Order_Status",
                        column: x => x.Order_Status_Id,
                        principalTable: "Order_Status",
                        principalColumn: "Order_Status_Id");
                    table.ForeignKey(
                        name: "FK_Order_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuppliesId = table.Column<int>(type: "int", nullable: false),
                    NotificationMessage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Inventory_SuppliesId",
                        column: x => x.SuppliesId,
                        principalTable: "Inventory",
                        principalColumn: "Inventory_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_product",
                columns: table => new
                {
                    Order_Product_Id = table.Column<int>(type: "int", nullable: false),
                    Order_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_product", x => x.Order_Product_Id);
                    table.ForeignKey(
                        name: "FK_order_product_Order",
                        column: x => x.Order_Id,
                        principalTable: "Order",
                        principalColumn: "Order_Id");
                    table.ForeignKey(
                        name: "FK_order_product_Product",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "Product_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Supplies_Id",
                table: "Inventory",
                column: "Supplies_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Unit_Id",
                table: "Inventory",
                column: "Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Log_User_Id",
                table: "Log",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SuppliesId",
                table: "Notifications",
                column: "SuppliesId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Client_Id",
                table: "Order",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Order_Status_Id",
                table: "Order",
                column: "Order_Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_User_Id",
                table: "Order",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_product_Order_Id",
                table: "Order_product",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_product_Product_Id",
                table: "Order_product",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_Id",
                table: "Product",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_For_Product_Product_Id",
                table: "Supplies_For_Product",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_For_Product_Supplies_Id",
                table: "Supplies_For_Product",
                column: "Supplies_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_For_Product_Unit_Id",
                table: "Supplies_For_Product",
                column: "Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_For_Product_UnitTypeUnitId",
                table: "Supplies_For_Product",
                column: "UnitTypeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_Id",
                table: "User",
                column: "Role_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Error");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Order_product");

            migrationBuilder.DropTable(
                name: "Order_Type");

            migrationBuilder.DropTable(
                name: "Payment_Method");

            migrationBuilder.DropTable(
                name: "Supplies_For_Product");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "Unit_Type");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Order_Status");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
