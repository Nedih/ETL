# Test Assessment Deliverables

## 1. SQL scripts used for creating the database and tables.

    The Database was created with EntityFramework Core using the Code-First approach, where the model was designed after the task description. The AppDbContext for Database creation is:

        public class AppDbContext : DbContext
        {
            public DbSet<Record> Records { get; set; } 

            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ETLDatabase;Trusted_Connection=True;");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Record>()
                    .HasKey(r => new { r.TpepPickupDatetime, r.TpepDropoffDatetime, r.PassengerCount });
            }
        }

    The model snapshot:
        modelBuilder.Entity("ETL.Models.Record", b =>
        {
            b.Property<DateTime>("TpepPickupDatetime")
                .HasColumnType("datetime2")
                .HasColumnOrder(0);

            b.Property<DateTime>("TpepDropoffDatetime")
                .HasColumnType("datetime2")
                .HasColumnOrder(1);

            b.Property<int>("PassengerCount")
                .HasColumnType("int")
                .HasColumnOrder(2);

            b.Property<int>("DOLocationID")
                .HasColumnType("int");

            b.Property<decimal>("FareAmount")
                .HasColumnType("decimal(18,2)");

            b.Property<int>("PULocationID")
                .HasColumnType("int");

            b.Property<string>("StoreAndFwdFlag")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<decimal>("TipAmount")
                .HasColumnType("decimal(18,2)");

            b.Property<double>("TripDistance")
                .HasColumnType("float");

            b.HasKey("TpepPickupDatetime", "TpepDropoffDatetime", "PassengerCount");

            b.ToTable("Records");
        });

## 2. Number of rows in your table after running the program.

    The table has 29889 rows, 111 duplicate records were removed

## 3. Any comments on any assumptions made.

    - Code-First approach with EFCore was used instead raw SQL or alternatives like ADO.NET, Dapper or NHibernate.
    - Records with negative Fare values were permitted as a possible refund.
    - Missing passangers count fields were set to 0 to records loss due to a possible mistake or a confusing format of data.
    - Since input data is considered to be from an unsafe source fields were validated, and since EF Core protects the DB from SQL Query Injections no additional protection was implemented.
    - The fields `pickup_datetime`, `dropoff_datetime`, and `passenger_count`are the Composite Primary Key since the task is filter out any duplicates based on these 3 fields.

## 4. Assume your program will be used on much larger data files. Describe in a few sentences what you would change if you knew it would be used for a 10GB CSV input file.
    
    - The program already has Bulk operations and some asynchrony implemented.
    - For now data from .csv is being loaded into the memory and it works fast, for bigger files there should be another approach - to process only 1 record at once, so the memory won't be overloaded. For small and medium files such approach would be working slower.
    - Since all the tasks will have a bigger execution time with a larger data - most of the methods should use asynchrony and parallel programming, whereas for small tasks the time needed for maintaining asynchrony and threads could add a big portion of time to a task exection time
