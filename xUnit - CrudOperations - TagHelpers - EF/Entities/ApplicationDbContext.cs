using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    //DbContext : EntityFramework nesnesidir ve uygulama ve veritabanı arasındaki bağlantı için köprü görevi görür.
    // Nuget EntityFrameworkCore - EntityFrameworkCoreSqlServer(MsSql kullanacağımız için)
    public class ApplicationDbContext : DbContext
    {
        //Db bağlantısı burada gösteriliyor. Fakat biz program.cs içerisinde yaptık. base ile options'ı oradan aldık.
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //DbSet : EntityFramework'te belirli bir veritabanı tablosunu temsil eder ve veritabanındaki tabloları eşler.
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set;}

        //Veritabanı modelinin nasıl oluşturulacağını belirlediğimiz yerdir. Tablolaları yapılandırmamızı sağlar.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Tablo isimleri ile projemizde Entity isimlerinin eşleşmesini yapıyoruz.
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //SeedData => Veritabanına başlangıç verilerinin eklenmesini sağlar.
            //Seed to countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            //Fluent API
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TIN")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345")
                ;

            //TIN sütunundaki değerlerin benzersiz olmasını sağlar.
            //modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

            //Burada CHK_TIN diye bir işlem tanımlanır. TIN uzunluğu 8 olmayan değerleri kabul etmez, 8 uzunluğunda olan değerleri kabul eder.
            modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len(TIN) = 8");

            //Tablo İlişkilendirme
            //modelBuilder.Entity<Person>(temp =>
            //{
            //    temp.HasOne<Country>(c => c.Country).WithMany(p => p.Persons).HasForeignKey(p => p.CountryId);
            //});
        }

        //Migration ile veritabanına eklediğimiz StoreProcedure'u çağırıyoruz.
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@PersonId", person.PersonId),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryId", person.CountryId),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
                new SqlParameter("@TIN", person.TIN)
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, " +
                "@Address, @ReceiveNewsLetters, @TIN", parameters);
        }
    }
}

/*
    Veritabanını Ef ile oluşturmak, güncellemek ve yönetmek için context'in olduğu Entityframework.Tools nuget paketi ekle.
    Veritabanını Ef ile oluşturmak, güncellemek ve yönetmek için uygulama katmanına Entityframework.Desing nuget paketi ekle.
    Paketlerden sonra veritabanını oluşturmak ve eklemek için => Add-Migration Initial
    Veritabanı oluşturma işlemleri bittikten sonra Update-Database komutu ile veritabanı güncelleyip işlemleri gerçekleştiriyoruz.
*/

/*
    Birden fazla veya karmaşık veritabanı işlemi gerçekleştirmek istediğimizde EF'de StoredProcedure kullanılır.
        Performansı arttırır.
        Güvenliği arttırır
        Ağ trafiğini yormadan çalışırlar
    Insert-Update-Delete işlemleri için DbContext.Database.ExecuteSqlRaw kullanılır.
*/

/*
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Tablo isimleri ile projemizde Entity isimlerinin eşleşmesini yapıyoruz.
            modelBuilder.Entity<ModelClass>( ).ToTable("table_name", schema: "schema_name");
 
            //Tablo isimleri ile projemizde Entity isimlerinin eşleşmesini görüntü için eşliyoruz. ??
            modelBuilder.Entity<ModelClass>( ).ToView("view_name", schema: "schema_name");
 
            //DbContext'teki veritabanının tüm tablolar için geçerli varsayılan şemayı belirtir.
            modelBuilder.HasDefaultSchema("schema_name");
        }
    
 
*/