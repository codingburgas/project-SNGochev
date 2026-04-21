using System;
using EventBookingPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace EventBookingPlatform.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("EventBookingPlatform.Models.ApplicationUser", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime>("CreatedAt");
                b.Property<string>("Email").IsRequired().HasMaxLength(120);
                b.Property<string>("FirstName").IsRequired().HasMaxLength(60);
                b.Property<string>("LastName").IsRequired().HasMaxLength(60);
                b.Property<string>("PasswordHash").IsRequired();
                b.Property<string>("Role").IsRequired().HasMaxLength(20);

                b.HasKey("Id");
                b.HasIndex("Email").IsUnique();
                b.ToTable("Users");
            });

            modelBuilder.Entity("EventBookingPlatform.Models.Category", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime>("CreatedAt");
                b.Property<string>("Description").HasMaxLength(250);
                b.Property<string>("Name").IsRequired().HasMaxLength(60);

                b.HasKey("Id");
                b.HasIndex("Name").IsUnique();
                b.ToTable("Categories");
            });

            modelBuilder.Entity("EventBookingPlatform.Models.Event", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int>("Capacity");
                b.Property<int>("CategoryId");
                b.Property<DateTime>("CreatedAt");
                b.Property<string>("Description").HasMaxLength(1000);
                b.Property<DateTime>("EventDate");
                b.Property<decimal>("TicketPrice").HasColumnType("decimal(18,2)");
                b.Property<string>("Title").IsRequired().HasMaxLength(120);
                b.Property<string>("Venue").IsRequired().HasMaxLength(120);

                b.HasKey("Id");
                b.HasIndex("CategoryId");
                b.ToTable("Events");
            });

            modelBuilder.Entity("EventBookingPlatform.Models.Ticket", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int>("ApplicationUserId");
                b.Property<DateTime>("CreatedAt");
                b.Property<int>("EventId");
                b.Property<DateTime>("PurchasedAt");
                b.Property<int>("Quantity");
                b.Property<int>("Status");
                b.Property<decimal>("TotalPrice").HasColumnType("decimal(18,2)");
                b.Property<decimal>("UnitPrice").HasColumnType("decimal(18,2)");

                b.HasKey("Id");
                b.HasIndex("ApplicationUserId");
                b.HasIndex("EventId");
                b.ToTable("Tickets");
            });

            modelBuilder.Entity("EventBookingPlatform.Models.Event", b =>
            {
                b.HasOne("EventBookingPlatform.Models.Category", "Category")
                    .WithMany("Events")
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            modelBuilder.Entity("EventBookingPlatform.Models.Ticket", b =>
            {
                b.HasOne("EventBookingPlatform.Models.ApplicationUser", "ApplicationUser")
                    .WithMany("Tickets")
                    .HasForeignKey("ApplicationUserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("EventBookingPlatform.Models.Event", "Event")
                    .WithMany("Tickets")
                    .HasForeignKey("EventId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
        }
    }
}
