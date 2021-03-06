﻿using dropShippingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dropShippingApp.Data.Repositories.RealRepos
{
    public class RealCartRepo : ICartRepo
    {
        private ApplicationDbContext context;
        public RealCartRepo(ApplicationDbContext c)
        {
            this.context = c;
        }

        public async Task UpdateCart(Cart cart)
        {
            this.context.Carts.Update(cart);
            await this.context.SaveChangesAsync();
        }

        public List<Cart> GetCarts
        {
            get
            {
                var carts = this.context.Carts
                    .Include(cart => cart.CartItems) // get custom product
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .Include(cart => cart.CartItems) // get custom product tags
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                    .Include(cart => cart.CartItems) // get custom pricing history
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.PricingHistory)
                    .Include(cart => cart.CartItems) // get roster base color
                        .ThenInclude(cartItem => cartItem.ProductSelection) 
                         .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.BaseColor)
                    .Include(cart => cart.CartItems) // get roster base size
                        .ThenInclude(cartItem => cartItem.ProductSelection) 
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.BaseSize)
                    .Include(cart => cart.CartItems) // get pricing history for roster product
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.PricingHistory)
                    .Include(cart => cart.CartItems) // get roster product group
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                    .Include(cart => cart.CartItems) // get roster product category
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                        .ThenInclude(group => group.Category)
                    .Include(cart => cart.CartItems) // get roster product tags
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                        .ThenInclude(group => group.ProductTags)
                    .ToList();
                return carts;
            }
        }

        public async Task<Cart> FindCartById(int cartId)
        {
            return this.context.Carts
                    .Include(cart => cart.CartItems) // get custom product
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .Include(cart => cart.CartItems) // get custom product tags
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                    .Include(cart => cart.CartItems) // get custom pricing history
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.PricingHistory)
                    .Include(cart => cart.CartItems) // get roster base color
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                         .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.BaseColor)
                    .Include(cart => cart.CartItems) // get roster base size
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.BaseSize)
                    .Include(cart => cart.CartItems) // get pricing history for roster product
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.PricingHistory)
                    .Include(cart => cart.CartItems) // get roster product group
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                    .Include(cart => cart.CartItems) // get roster product category
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                        .ThenInclude(group => group.Category)
                    .Include(cart => cart.CartItems) // get roster product tags
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                        .ThenInclude(group => group.ProductTags)
                    .ToList()
                    .Find(cart => cart.CartID == cartId);
        }

        public async Task<Cart> FindCartByItemId(int itemId)
        {
            var foundCart = this.context.Carts
                .Include(cart => cart.CartItems) // get custom product
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .Include(cart => cart.CartItems) // get custom product tags
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                    .Include(cart => cart.CartItems) // get custom pricing history
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.PricingHistory)
                    .Include(cart => cart.CartItems) // get roster base color
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                         .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.BaseColor)
                    .Include(cart => cart.CartItems) // get roster base size
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.BaseSize)
                    .Include(cart => cart.CartItems) // get pricing history for roster product
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.PricingHistory)
                    .Include(cart => cart.CartItems) // get roster product group
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                    .Include(cart => cart.CartItems) // get roster product category
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                        .ThenInclude(group => group.Category)
                    .Include(cart => cart.CartItems) // get roster product tags
                        .ThenInclude(cartItem => cartItem.ProductSelection)
                        .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                        .ThenInclude(baseProduct => baseProduct.RosterGroup)
                        .ThenInclude(group => group.ProductTags)
                    .ToList()
                    .Find(cart => 
                        cart.CartItems.Find(item => item.CartItemID == itemId) != null);
            return foundCart;
        }

        public async Task AddCart(Cart cart)
        {
            this.context.Carts.Add(cart);
            await this.context.SaveChangesAsync();
        }

        public async Task<Cart> RemoveCartById(int cartId)
        {
            var foundCart = this.context.Carts.ToList()
                .Find(cart => cart.CartID == cartId);
            this.context.Carts.Remove(foundCart);
            await this.context.SaveChangesAsync();
            return foundCart;
        }

        public async Task AddCartItem(CartItem item)
        {
            this.context.CartItems.Add(item);
            await this.context.SaveChangesAsync();
        }

        public async Task<CartItem> RemoveCartItem(int itemId)
        {
            var foundItem = this.context.CartItems.ToList()
                .Find(item => item.CartItemID == itemId);
            this.context.CartItems.Remove(foundItem);
            await this.context.SaveChangesAsync();
            return foundItem;
        }

        public async Task UpdateCartItem(CartItem item)
        {
            this.context.CartItems.Update(item);
            await this.context.SaveChangesAsync();
        }

        public async Task<CartItem> GetCartItemById(int cartId)
        {
            return this.context.CartItems
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                .Include(cartItem => cartItem.ProductSelection)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.PricingHistory)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .ThenInclude(baseProduct => baseProduct.BaseColor)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .ThenInclude(baseProduct => baseProduct.BaseSize)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .ThenInclude(baseProduct => baseProduct.PricingHistory)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .ThenInclude(baseProduct => baseProduct.RosterGroup)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .ThenInclude(baseProduct => baseProduct.RosterGroup)
                    .ThenInclude(group => group.Category)
                .Include(cartItem => cartItem.ProductSelection)
                    .ThenInclude(selectedProduct => selectedProduct.BaseProduct)
                    .ThenInclude(baseProduct => baseProduct.RosterGroup)
                    .ThenInclude(group => group.ProductTags)
                .ToList()
                .Find(item => item.CartItemID == cartId);
        }
    }
}
