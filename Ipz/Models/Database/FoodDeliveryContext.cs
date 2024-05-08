using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ipz_server.Models.Database;

public partial class FoodDeliveryContext : DbContext
{
    public FoodDeliveryContext()
    {
    }

    public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderInformation> OrderInformations { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.DishId).HasName("dishes_pkey");

            entity.ToTable("dishes");

            entity.Property(e => e.DishId)
                .ValueGeneratedNever()
                .HasColumnName("dish_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasMany(d => d.Restaurants).WithMany(p => p.Dishes)
                .UsingEntity<Dictionary<string, object>>(
                    "RestaurantsDish",
                    r => r.HasOne<Restaurant>().WithMany()
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_restaurants_dishes_restaurant_id"),
                    l => l.HasOne<Dish>().WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_restaurants_dishes_dish_id"),
                    j =>
                    {
                        j.HasKey("DishId", "RestaurantId").HasName("restaurants_dishes_pkey");
                        j.ToTable("restaurants_dishes");
                        j.IndexerProperty<Guid>("DishId").HasColumnName("dish_id");
                        j.IndexerProperty<Guid>("RestaurantId").HasColumnName("restaurant_id");
                    });
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.Property(e => e.LocationId)
                .ValueGeneratedNever()
                .HasColumnName("location_id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Street)
                .HasMaxLength(200)
                .HasColumnName("street");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("money")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_restaurant_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_user_id");
        });

        modelBuilder.Entity<OrderInformation>(entity =>
        {
            entity.HasKey(e => e.OrderInformationsId).HasName("order_informations_pkey");

            entity.ToTable("order_informations");

            entity.Property(e => e.OrderInformationsId)
                .ValueGeneratedNever()
                .HasColumnName("order_informations_id");
            entity.Property(e => e.DishId).HasColumnName("dish_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Dish).WithMany(p => p.OrderInformations)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_informations_dish_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderInformations)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_informations_order_id");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("restaurants_pkey");

            entity.ToTable("restaurants");

            entity.HasIndex(e => e.LocationId, "unique_restaurants_location_id").IsUnique();

            entity.Property(e => e.RestaurantId)
                .ValueGeneratedNever()
                .HasColumnName("restaurant_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Location).WithOne(p => p.Restaurant)
                .HasForeignKey<Restaurant>(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_restaurants_location_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.LocationId, "unique_users_location_id").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Location).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.LocationId)
                .HasConstraintName("fk_users_location_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_role_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
