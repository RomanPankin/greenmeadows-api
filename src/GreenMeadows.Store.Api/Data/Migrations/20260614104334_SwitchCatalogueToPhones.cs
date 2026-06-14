using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMeadows.Store.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SwitchCatalogueToPhones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "6.7\" flagship smartphone, 256GB, triple camera.", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=600&q=80&auto=format", "Aurora X1", 899.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "6.1\" mid-range smartphone, 128GB, dual camera.", "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=600&q=80&auto=format", "Nimbus 5", 499.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "6.0\" budget smartphone, 64GB, long battery life.", "https://images.unsplash.com/photo-1598327105666-5b89351aff97?w=600&q=80&auto=format", "Pixela Lite", 299.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "6.8\" pro smartphone, 512GB, 120Hz display.", "https://images.unsplash.com/photo-1605236453806-6ff36851218e?w=600&q=80&auto=format", "Quantum Pro", 1099.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "5.4\" compact smartphone, 128GB, lightweight.", "https://images.unsplash.com/photo-1567581935884-3349723552ca?w=600&q=80&auto=format", "Eon Mini", 349.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "6.5\" smartphone, 256GB, fast wireless charging.", "https://images.unsplash.com/photo-1580910051074-3eb694886505?w=600&q=80&auto=format", "Solis Z", 749.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Stainless steel spade with an ash handle.", "https://placehold.co/300x300?text=Spade", "Garden Spade", 24.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Frost-resistant 30cm terracotta planter.", "https://placehold.co/300x300?text=Pot", "Terracotta Pot (Large)", 12.50m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "1kg pollinator-friendly native wildflower blend.", "https://placehold.co/300x300?text=Seeds", "Wildflower Seed Mix", 8.75m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Galvanised steel watering can with a rose head.", "https://placehold.co/300x300?text=Can", "Watering Can (5L)", 18.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Bypass secateurs with a non-slip grip.", "https://placehold.co/300x300?text=Shears", "Pruning Shears", 15.25m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Recycled-plastic compost converter.", "https://placehold.co/300x300?text=Compost", "Compost Bin (220L)", 39.99m });
        }
    }
}
