using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;
using dropShippingApp.Data;
using dropShippingApp.Models;
using dropShippingApp.Data.Repositories;
using dropShippingApp.ViewModels;
using Microsoft.Xrm.Sdk.Query;
using dropShippingApp.HelperUtilities;

namespace dropShippingApp.Controllers
{
    public class ProductController : Controller
    {
        private IRosterProductRepo rosterProductRepo;
        private ICustomProductRepo customProductRepo;
        private IProductSortRepo sortRepo;
        private IProductCategoryRepo categoryRepo;
        private IProductGroupRepo productGroupRepo;
        private ITeamRepo teamRepo;

        public ProductController(IRosterProductRepo rosterProductRepo,
            ICustomProductRepo customProductRepo,
            IProductSortRepo sortRepo,
            IProductCategoryRepo categoryRepo,
            IProductGroupRepo productGroupRepo,
            ITeamRepo teamRepo)
        {
            this.rosterProductRepo = rosterProductRepo;
            this.customProductRepo = customProductRepo;
            this.sortRepo = sortRepo;
            this.categoryRepo = categoryRepo;
            this.productGroupRepo = productGroupRepo;
            this.teamRepo = teamRepo;
        }
        //for testing controller.
        //public ProductController(IRosterProductRepo rosterProductRepo,IProductGroupRepo productGroupRepo, ITeamRepo teamRepo)
        //{
        //    this.teamRepo = teamRepo;
        //    this.productGroupRepo = productGroupRepo;
        //    this.rosterProductRepo = rosterProductRepo;
        //}

        public async Task<IActionResult> Index()
        {
            return View("Search", null);
        }

        public async Task<IActionResult> ViewProduct(int productGroupId)
        {
            try
            {
                // get product group
                var foundGroup = productGroupRepo.GetGroupById(productGroupId);

                // setup view model
                var viewProductVM = new ProductSelectionViewModel()
                {
                    ProductGroup = foundGroup
                };

                // send to view
                return View(viewProductVM);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0010",
                    Message = "An error occured while trying to view a product."
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> BackToFirstPage(int categoryId = -1, string searchTerm = null)
        {
            try
            {
                if (searchTerm != null)
                    return RedirectToAction("Search", new
                    {
                        searchString = searchTerm,
                        currentPage = 0
                    });
                else
                    return RedirectToAction("DisplayByCategory", new
                    {
                        categoryId = categoryId,
                        currentPage = 0
                    });
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0011",
                    Message = "An error occured while trying to return to get the page."
                };
                return View("Error", e);
            }
       
        }

        public async Task<IActionResult> NextPage(int currentPage, int categoryId = -1, string searchTerm = null)
        {
            try
            {
                if (searchTerm != null)
                    return RedirectToAction("Search", new
                    {
                        searchString = searchTerm,
                        currentPage = currentPage + 1
                    });
                else
                    return RedirectToAction("DisplayByCategory", new
                    {
                        categoryId = categoryId,
                        currentPage = currentPage + 1
                    });
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0011",
                    Message = "An error occured while trying to return to get the page."
                };
                return View("Error", e);
            }
 
        }

        public async Task<IActionResult> PreviousPage(int currentPage, int categoryId = -1, string searchTerm = null)
        {
            try
            {
                if (searchTerm != null)
                    return RedirectToAction("Search", new
                    {
                        searchString = searchTerm,
                        currentPage = currentPage - 1
                    });
                else
                    return RedirectToAction("DisplayByCategory", new
                    {
                        categoryId = categoryId,
                        currentPage = currentPage - 1
                    });
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0011",
                    Message = "An error occured while trying to return to get the page."
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> Search(string searchString, int currentPage = -1)
        {
            try
            {
                // search for products
                var foundGroups = SearchHelper.SearchByString<ProductGroup>(productGroupRepo.Groups, searchString);

                // create browse view model
                var browseVM = SearchHelper.CreateBrowseObject<ProductGroup>(
                        currentPage == -1 ? 0 : currentPage,
                        searchTerm: searchString,
                        queriedGroups: foundGroups);

                // return view
                return View("Search", browseVM);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0020",
                    Message = "An error occured while trying to search."
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> DisplayByCategory(int categoryId, int currentPage = -1)
        {
            try
            {
                // get products by category
                var categoryGroups = SearchHelper.FilterByCategory<ProductGroup>(productGroupRepo.Groups, categoryId);

                // get current category
                var category = categoryRepo.GetCategoryById(categoryId);

                // create browse view model
                var browseVM = SearchHelper.CreateBrowseObject<ProductGroup>(
                    currentPage == -1 ? 0 : currentPage,
                    productCategory: category,
                    queriedGroups: categoryGroups);

                // return view
                return View("Search", browseVM);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0012",
                    Message = "An error occured while trying to get the categories."
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> SortProductGroups(int sortId, int categoryId = -1, string searchTerm = null, int currentPage = -1)
        {
            try
            {
                // IMPORTANT: at no point will the user be allowed to search AND browse by category AT THE SAME TIME

                // get products by appropriate query
                var searchableList = productGroupRepo.Groups;
                var filteredGroups = categoryId != -1 ?
                    SearchHelper.FilterByCategory<ProductGroup>(searchableList, categoryId)
                        : SearchHelper.SearchByString<ProductGroup>(searchableList, searchTerm);

                // get sort and check sort type
                var foundSort = sortRepo.GetSortById(sortId);
                var productGroupSortArgument = 0;
                if (foundSort.SortName.ToUpper() == "LOWEST PRICE")
                {
                    productGroupSortArgument = -1;
                }
                else if (foundSort.SortName.ToUpper() == "HIGHEST PRICE")
                {
                    productGroupSortArgument = 1;
                }
                SearchHelper.SortGroupsByPrice(ref filteredGroups, sortBy: productGroupSortArgument);

                // create browse view model
                BrowseViewModel browseVM = null;
                if (categoryId != -1)
                {
                    // means user is browsing by category
                    var foundCategory = categoryRepo.GetCategoryById(categoryId);
                    browseVM = SearchHelper.CreateBrowseObject<ProductGroup>(
                        currentPage == -1 ? 0 : currentPage,
                        productCategory: foundCategory,
                        queriedGroups: filteredGroups);
                }
                else
                {
                    // user is browsing products THEY searched
                    browseVM = SearchHelper.CreateBrowseObject<ProductGroup>(
                        currentPage == -1 ? 0 : currentPage,
                        searchTerm: searchTerm,
                        queriedGroups: filteredGroups);
                }

                // return list
                return View("Search", browseVM);
            }
            catch
            {

                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0013",
                    Message = "An error occured while trying to sort."
                };
                return View("Error", e);
            }
          
        }
    }
}
