using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    //DbContext : EntityFramework nesnesidir ve uygulama ve veritabanı arasındaki bağlantı için köprü görevi görür.
    // Nuget EntityFrameworkCore - EntityFrameworkCoreSqlServer(MsSql kullanacağımız için) -
    public class PersonsDbContext : DbContext
    {
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
        }
    }
}
