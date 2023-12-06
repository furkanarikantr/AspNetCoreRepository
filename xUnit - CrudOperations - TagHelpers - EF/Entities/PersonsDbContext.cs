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
    public class PersonsDbContext : DbContext
    {
        //Db bağlantısı burada gösteriliyor. Fakat biz program.cs içerisinde yaptık. base ile options'ı oradan aldık.
        public PersonsDbContext(DbContextOptions options) : base(options)
        {
        }

        //DbSet : EntityFramework'te belirli bir veritabanı tablosunu temsil eder ve veritabanındaki tabloları eşler.
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set;}

        //Veritabanı modelinin nasıl oluşturulacağını belirlediğimiz yerdir. Tablolaları yapılandırmamızı sağlar.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Country'lerden oluşacak tablo adını Countries yapıyoruz. Bunu yazmasaydık, yukarıdan yine Countries olucaktı.
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
            //Seed to persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }
    }
}

/*
    Veritabanını Ef ile oluşturmak, güncellemek ve yönetmek için context'in olduğu Entityframework.Tools nuget paketi ekle.
    Veritabanını Ef ile oluşturmak, güncellemek ve yönetmek için uygulama katmanına Entityframework.Desing nuget paketi ekle.
    Paketlerden sonra veritabanını oluşturmak ve eklemek için => Add-Migration Initial
    Veritabanı oluşturma işlemleri bittikten sonra Update-Database komutu ile veritabanı güncelleyip işlemleri gerçekleştiriyoruz.
*/