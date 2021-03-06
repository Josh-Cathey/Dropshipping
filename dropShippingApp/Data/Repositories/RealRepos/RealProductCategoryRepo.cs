﻿using dropShippingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dropShippingApp.Data.Repositories.RealRepos
{
    public class RealProductCategoryRepo : IProductCategoryRepo
    {
        private ApplicationDbContext context;

        public RealProductCategoryRepo(ApplicationDbContext c)
        {
            this.context = c;
        }

        public List<ProductCategory> GetCategories 
        {
            get 
            {
                return this.context.ProductCategories.ToList();
            }
        }

        public ProductCategory GetCategoryById(int categoryId)
        {
            return this.context.ProductCategories.ToList()
                .Find(category => category.ProductCategoryID == categoryId);
        }

        public async Task AddCategory(ProductCategory category)
        {
            this.context.ProductCategories.Add(category);
            await this.context.SaveChangesAsync();
        }

        public async Task<ProductCategory> RemoveCategory(int categoryId)
        {
            var foundCategory = this.context.ProductCategories.ToList()
                .Find(category => category.ProductCategoryID == categoryId);
            this.context.ProductCategories.Remove(foundCategory);
            await this.context.SaveChangesAsync();
            return foundCategory;
        }

        public async Task UpdateCategory(ProductCategory category)
        {
            this.context.ProductCategories.Update(category);
            await this.context.SaveChangesAsync();
        }
    }
}
