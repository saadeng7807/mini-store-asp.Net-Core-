using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  // call EntityFrameWork 

using mini_store.Models;

namespace mini_store.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser> // EntityFramework
    {
            public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
            {
                
            }


        public DbSet<Product> products{get;set;}
        public DbSet<Items> items {get;set;}

        public DbSet<Costumers> costumers {get;set;}
          public DbSet<Details> details {get;set;}

           public DbSet<Categories> categories {get;set;}

           public DbSet<Image> images {get;set;}


       
    }

  
}


