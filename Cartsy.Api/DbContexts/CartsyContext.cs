using Microsoft.EntityFrameworkCore;
using Cartsy.Api.Entities;

namespace Cartsy.Api.DbContexts;

public class CartsyContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<LegalCustomer> LegalCustomers { get; set; } = null!;
    public DbSet<NaturalCustomer> NaturalCustomers { get; set; } = null!;

    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<Consumer> Consumers { get; set; } = null!;


    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<StoreService> StoreServices { get; set; } = null!;
    public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemType> ItemTypes { get; set; } = null!;
    public DbSet<AdditionalServices> AdditionalServices { get; set; } = null!;


    public CartsyContext(DbContextOptions<CartsyContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var customer = modelBuilder.Entity<Customer>();
        
        customer
            .ToTable("Customers")
            .HasDiscriminator(c => c.TypeDiscriminator)
            .HasValue<LegalCustomer>(1)
            .HasValue<NaturalCustomer>(2);

        customer
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        

        customer
            .Property(p => p.Email)
            .HasMaxLength(100)
            .IsRequired();

        customer
            .Property(p => p.CellPhone)
            .HasMaxLength(13)
            .IsFixedLength()
            .IsRequired();

        customer
            .Property(p => p.HomePhone)
            .HasMaxLength(13)
            .IsFixedLength();

        customer
            .Property(c => c.Status);

        customer
            .HasOne(c => c.Address)
            .WithOne(a => a.Customer)
            .HasForeignKey<Customer>(c => c.AddressId);

        customer
            .HasOne(c => c.Store)
            .WithOne(s => s.Customer)
            .HasForeignKey<Store>(c => c.CustomerId);


        var consumer = modelBuilder.Entity<Consumer>();

        consumer
            .Property(c => c.CPF)
            .IsFixedLength()
            .HasMaxLength(11)
            .IsRequired();
        
        consumer
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        consumer
            .Property(p => p.Email)
            .HasMaxLength(100)
            .IsRequired();

        consumer
            .Property(p => p.CellPhone)
            .HasMaxLength(13)
            .IsFixedLength()
            .IsRequired();

        consumer
            .Property(p => p.HomePhone)
            .HasMaxLength(13)
            .IsFixedLength();
        
        consumer
            .Property(c => c.Status);

        consumer
            .HasOne(p => p.Address)
            .WithOne(a => a.Consumer)
            .HasForeignKey<Consumer>(c => c.AddressId);

        var naturalCustomer = modelBuilder.Entity<NaturalCustomer>();

        naturalCustomer.Property(c => c.CPF)
            .IsFixedLength()
            .HasMaxLength(11)
            .IsRequired();
        naturalCustomer
            .Property(p => p.Type)
            .IsRequired();

        var legalCustomer = modelBuilder.Entity<LegalCustomer>();

        legalCustomer.Property(c => c.CNPJ)
            .IsFixedLength()
            .HasMaxLength(14)
            .IsRequired();
        
        legalCustomer
            .Property(p => p.Type)
            .IsRequired();


        
        var address = modelBuilder.Entity<Address>();

       

        address
            .Property(s => s.State)
            .HasMaxLength(50)
            .IsRequired();
        
        address
            .Property(s => s.UF)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();
        

        address
            .Property(c => c.City)
            .HasMaxLength(100)
            .IsRequired();

        address
            .Property(a => a.Number)
            .IsRequired();

        address
            .Property(a => a.Street)
            .HasMaxLength(50)
            .IsRequired();

        address
            .Property(a => a.CEP)
            .HasMaxLength(8)
            .IsFixedLength()
            .IsRequired();

        var store = modelBuilder.Entity<Store>();

        store
            .HasOne(s => s.Address)
            .WithOne(a => a.Store)
            .HasForeignKey<Store>(s => s.AddressId);

        store
            .HasMany(s => s.Services)
            .WithMany(s => s.Stores)
            .UsingEntity<StoreService>(ss => ss.ToTable("StoresServices")
                .Property(ss => ss.CreatedAt).HasDefaultValueSql("NOW()")
            );

        store
            .Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        store
            .HasMany(s => s.Items)
            .WithOne(i => i.Store)
            .HasForeignKey(s => s.StoreId);

        var order = modelBuilder.Entity<Order>();

        order
            .HasMany(o => o.Items)
            .WithMany(i => i.Orders)
            .UsingEntity<OrderItem>(oi => oi.ToTable("OrdersItems")
                .Property(oi => oi.CreatedAt).HasDefaultValueSql("NOW()")
            );

        order
            .HasOne(o => o.Store)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.StoreId);

        order
            .HasOne(o => o.Consumer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ConsumerId);

        order
            .HasOne(o => o.Status)
            .WithMany(os => os.Orders)
            .HasForeignKey(o => o.ConsumerId);

        order
            .Property(o => o.Price)
            .HasPrecision(2, 10)
            .IsRequired();

        order
            .Property(o => o.DateDelivered)
            .HasDefaultValue(null);

        var orderStatus = modelBuilder.Entity<OrderStatus>();

        orderStatus.Property(os => os.Status)
            .HasMaxLength(50)
            .IsRequired();

        var item = modelBuilder.Entity<Item>();

        item
            .HasOne(i => i.Type)
            .WithMany(it => it.Items)
            .HasForeignKey(i => i.TypeId);

        item
            .Property(i => i.Description)
            .HasMaxLength(100);

        item
            .Property(i => i.Name)
            .HasMaxLength(50)
            .IsRequired();

        item
            .Property(i => i.Price)
            .HasPrecision(2, 10)
            .IsRequired();

        item
            .Property(i => i.Stock)
            .IsRequired();

        var additionalServices = modelBuilder.Entity<AdditionalServices>();

        additionalServices
            .Property(asvc => asvc.Price)
            .HasPrecision(2, 10)
            .IsRequired();

        additionalServices
            .Property(asvc => asvc.Type)
            .HasMaxLength(50)
            .IsRequired();

        additionalServices
            .Property(asvc => asvc.Service)
            .IsRequired();

        var itemType = modelBuilder.Entity<ItemType>();

        itemType
            .Property(it => it.Type)
            .HasMaxLength(50)
            .IsRequired();

        itemType
            .Property(it => it.Tax)
            .IsRequired();
        

        modelBuilder.Entity<Address>().HasData(
            new Address { Id = 1, CEP = "12345678", Number = 123, City = "Los Angeles", State = "California",Street = "Main St", UF = "CA"},
            new Address { Id = 2, CEP = "87654321", Number = 456, City = "New York City", State = "New York", Street = "Broadway Ave", UF = "NY"},
            new Address { Id = 3, CEP = "98765432", Number = 789, City = "Houston", State = "Texas", Street = "Oak St", UF = "TX"});
        
        modelBuilder.Entity<Consumer>().HasData(
            new Consumer
            {
                Id = 1,
                Name = "Linus Torvalds",
                CellPhone = "123-456-7890",
                HomePhone = "987-654-3210",
                Email = "linus@example.com",
                Status = true,
                AddressId = 1,
                CPF = "73473943096"
            },
            new Consumer
            {
                Id = 2,
                Name = "rafa pava",
                CellPhone = "123-456-7890",
                HomePhone = "987-654-3210",
                Email = "rafa@pava.com",
                Status = true,
                AddressId = 2,
                CPF = "73473943096"
            },
            new Consumer
            {
                Id = 3,
                Name = "ti ago",
                CellPhone = "123-456-7890",
                HomePhone = "987-654-3210",
                Email = "ti@ago.com",
                Status = true,
                AddressId = 3,
                CPF = "73473943096"
            });
        
        
        modelBuilder.Entity<AdditionalServices>().HasData(
            new AdditionalServices { Id = 1, Price = 10.99, Type = "Shipping", Service = "Express Shipping"},
            new AdditionalServices { Id = 2, Price = 5.00, Type = "Packaging" , Service = "Gift Wrapping"},
            new AdditionalServices { Id = 3, Price = 2.50, Type = "Warranty" ,Service = "Extended Warranty"});

        modelBuilder.Entity<ItemType>().HasData(
            new ItemType { Id = 1, Type = "Electronics", Tax = 10 },
            new ItemType { Id = 2, Type = "Clothing", Tax = 5 },
            new ItemType { Id = 3, Type = "Books", Tax = 0 });

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Status = "Pending" },
            new OrderStatus { Id = 2, Status = "Processing" },
            new OrderStatus { Id = 3, Status = "Shipped" },
            new OrderStatus { Id = 4, Status = "Delivered" });
        
        
        
        modelBuilder.Entity<LegalCustomer>().HasData(

            new LegalCustomer
            {
                Id = 2,
                Name = "Microsoft Corp",
                CellPhone = "234-567-8901",
                HomePhone = "876-543-2109",
                Email = "info@microsoft.com",
                Type = "legal", // LegalCustomer's discriminator value
                TypeDiscriminator = 1,
                Status = true,
                AddressId = 2,
                CNPJ = "12345678901234"
            });
        modelBuilder.Entity<NaturalCustomer>().HasData(

            new NaturalCustomer
            {
                Id = 3,
                Name = "Alice Johnson",
                CellPhone = "345-678-9012",
                HomePhone = "765-432-1098",
                Email = "alice@example.com",
                TypeDiscriminator = 2, // NaturalCustomer's discriminator value
                Type = "natural",
                Status = true,
                AddressId = 3,
                CPF = "98765432109"
            });
        
        modelBuilder.Entity<Store>().HasData(
            new Store { Id = 2, Name = "FashionHub", AddressId = 2, CustomerId = 2},
            new Store { Id = 3, Name = "Book Haven", AddressId = 3, CustomerId = 3});

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Smartphone", Price = 499.99, Description = "High-end smartphone", Stock = 50, StoreId = 1, TypeId = 1 },
            new Item { Id = 2, Name = "T-Shirt", Price = 19.99, Description = "Cotton crew-neck t-shirt", Stock = 100, StoreId = 2, TypeId = 2 },
            new Item { Id = 3, Name = "Sci-Fi Novel", Price = 12.99, Description = "Bestselling science fiction book", Stock = 30, StoreId = 3, TypeId = 3 });

        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, Price = 499.99, StoreId = 1, StatusId = 1, ConsumerId = 1, DateCreated = DateTime.UtcNow },
            new Order { Id = 2, Price = 19.99, StoreId = 2, StatusId = 2, ConsumerId = 2, DateCreated = DateTime.UtcNow },
            new Order { Id = 3, Price = 12.99, StoreId = 3, StatusId = 3, ConsumerId = 3, DateCreated = DateTime.UtcNow });
        

        modelBuilder.Entity<StoreService>().HasData(
            new StoreService {StoreId = 1, ServicesId = 1, CreatedAt = DateTime.UtcNow },
            new StoreService {StoreId = 2, ServicesId = 2, CreatedAt = DateTime.UtcNow },
            new StoreService {StoreId = 3, ServicesId = 3, CreatedAt = DateTime.UtcNow });

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { ItemId = 1, OrderId = 1, CreatedAt = DateTime.UtcNow },
            new OrderItem { ItemId = 2, OrderId = 2, CreatedAt = DateTime.UtcNow },
            new OrderItem { ItemId = 3, OrderId = 3, CreatedAt = DateTime.UtcNow }
                );

        base.OnModelCreating(modelBuilder);
    }

}