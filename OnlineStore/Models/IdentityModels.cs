using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineStore.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Улица/микрорайон")]
        public string Street { get; set; }

        [Display(Name = "Дом")]
        public string House { get; set; }

        [Display(Name = "Квартира")]
        public string Flat { get; set; }

        [Display(Name = "Почтовый индекс")]
        public string ZIP { get; set; }

        [Display(Name = "Имя")]
        public string FName { get; set; }

        [Display(Name = "Фамилия")]
        public string LName{ get; set; }
        //[Display(Name = "Корзина")]
        //public Cart Cart { get; set; } = new Cart();
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<GPU> GPU { get; set; }
        public DbSet<CPU> CPU { get; set; }
        public DbSet<HDD> HDD { get; set; }
        public DbSet<SSD> SSD { get; set; }
        public DbSet<RAM> RAM { get; set; }
        public DbSet<Cart> Cart{ get; set; }
        public DbSet<History> History{ get; set; }

        public System.Data.Entity.DbSet<OnlineStore.ViewModels.GPUViewModel> GPUViewModels { get; set; }
    }
}