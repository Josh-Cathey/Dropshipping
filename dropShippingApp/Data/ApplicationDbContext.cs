﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using dropShippingApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace dropShippingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<CustomProduct> CustomProducts { get; set; }
        public DbSet<RosterProduct> RosterProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PricingHistory> PricingHistories { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<QuestionMessage> QuestionMessages { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamCreationRequest> TeamCreationRequests { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<TeamCategory> TeamCategories { get; set; }
        public DbSet<ProductSort> ProductSorts { get; set; }
        public DbSet<TeamSort> TeamSorts { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<RosterGroup> RosterGroups { get; set; }
        public DbSet<ImgurConfig> ImgurConfiguration { get; set; }
        public DbSet<ImgurPhotoData> SavedImgurPhotos { get; set; }
    }   
}
