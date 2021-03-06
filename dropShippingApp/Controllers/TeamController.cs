using dropShippingApp.Data.Repositories;
using dropShippingApp.HelperUtilities;
using dropShippingApp.Models;
using Microsoft.AspNetCore.Identity;
using dropShippingApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using dropShippingApp.APIModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace dropShippingApp.Controllers
{
    public class TeamController : Controller
    {


        private UserManager<AppUser> userManager;
        private ICustomProductRepo customProductRepo;
        private IProductGroupRepo productGroupRepo;
        private ITeamRepo teamRepo;
        private ITeamSortRepo teamSortRepo;
        private ITeamCategoryRepo categoryRepo;
        private IOrderRepo orderRepo;
        private IUserRepo userRepo;
        private ILocationRepo locationRepo;
        private ITeamCreationReqRepo teamRequestRepo;
        private ITagRepo tagRepo;
        private IPricingRepo pricingRepo;
        private IRosterGroupRepo rosterGroupRepo;
        private IConfiguration configuration;
        private IImgurRepo imgurConfigRepo;
        private IImgurPhotoRepo imgurPhotoRepo;
        private IRosterProductRepo rosterProductRepo;

        public TeamController(
            ITeamRepo teamRepo,
            ITeamSortRepo sortRepo,
            ITeamCategoryRepo categoryRepo,
            IOrderRepo orderRepo,
            IUserRepo userRepo,
            ILocationRepo locationRepo,
            ICustomProductRepo customProductRepo,
            IProductGroupRepo productGroupRepo,
            UserManager<AppUser> userManager,
            ITeamCreationReqRepo teamRequestRepo,
            ITagRepo tagRepo,
            IPricingRepo pricingRepo,
            IRosterGroupRepo rosterGroupRepo,
            IConfiguration configuration,
            IImgurRepo imgurConfigRepo,
            IImgurPhotoRepo imgurPhotoRepo,
            IRosterProductRepo rosterProductRepo)
        {
            this.teamRepo = teamRepo;
            this.teamSortRepo = sortRepo;
            this.categoryRepo = categoryRepo;
            this.orderRepo = orderRepo;
            this.userRepo = userRepo;
            this.locationRepo = locationRepo;
            this.customProductRepo = customProductRepo;
            this.productGroupRepo = productGroupRepo;
            this.userManager = userManager;
            this.teamRequestRepo = teamRequestRepo;
            this.tagRepo = tagRepo;
            this.pricingRepo = pricingRepo;
            this.rosterGroupRepo = rosterGroupRepo;
            this.configuration = configuration;
            this.imgurConfigRepo = imgurConfigRepo;
            this.imgurPhotoRepo = imgurPhotoRepo;
            this.rosterProductRepo = rosterProductRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var teamList = teamRepo.GetTeams;
                return View(teamList);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0014",
                    Message = "An error occured while trying get the team."
                };
                return View("Error", e);
            }
       
        }

        public async Task<IActionResult> Browse()
        {
            return View("Search", null);
        }

        public async Task<IActionResult> ViewTeam(int teamId)
        {
            try
            {
                Team foundTeam = teamRepo.FindTeamById(teamId);
                return View(foundTeam);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0014",
                    Message = "An error occured while trying get the team."
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
                // search for teams
                var foundTeams = SearchHelper.SearchByString<Team>(teamRepo.GetTeams, searchString);

                // create browse view model
                var browseVM = SearchHelper.CreateBrowseObject<Team>(
                    currentPage == -1 ? 0 : currentPage,
                    searchTerm: searchString,
                    queriedTeams: foundTeams);

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
                var categoryTeams = SearchHelper.FilterByCategory<Team>(teamRepo.GetTeams, categoryId);

                // get current category
                var category = categoryRepo.GetCategoryById(categoryId);

                // create browse view model
                var browseVM = SearchHelper.CreateBrowseObject<Team>(
                    currentPage == -1 ? 0 : currentPage,
                    teamCategory: category,
                    queriedTeams: categoryTeams);

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

        public async Task<IActionResult> SortTeams(int sortId, int categoryId = -1, string searchTerm = null, int currentPage = -1)
        {
            try
            {
                // IMPORTANT: at no point will the user be allowed to search AND browse by category AT THE SAME TIME

                // get products by appropriate query
                var filteredTeams = categoryId != -1 ?
                    SearchHelper.FilterByCategory<Team>(teamRepo.GetTeams, categoryId)
                    : SearchHelper.SearchByString<Team>(teamRepo.GetTeams, searchTerm);

                // get sort and check sort type
                var foundSort = teamSortRepo.GetSortById(sortId);
                if (foundSort.SortName.ToUpper() == "OLDEST")
                {
                    filteredTeams.Sort((team1, team2) => team1.DateJoined.CompareTo(team2.DateJoined));
                }
                else if (foundSort.SortName.ToUpper() == "NEWEST")
                {
                    filteredTeams.Sort((team1, team2) => team2.DateJoined.CompareTo(team1.DateJoined));
                }
                else if (foundSort.SortName.ToUpper() == "MOST POPULAR")
                {
                    SearchHelper.SortByMostPopular<Team>(ref filteredTeams, orderRepo.GetOrders);
                }

                // create browse view model
                BrowseViewModel browseVM = null;
                if (categoryId != -1)
                {
                    // means user is browsing by category
                    var foundCategory = categoryRepo.GetCategoryById(categoryId);
                    browseVM = SearchHelper.CreateBrowseObject<Team>(
                        currentPage == -1 ? 0 : currentPage,
                        teamCategory: foundCategory,
                        queriedTeams: filteredTeams);
                }
                else
                {
                    // user is browsing products THEY searched
                    browseVM = SearchHelper.CreateBrowseObject<Team>(
                        currentPage == -1 ? 0 : currentPage,
                        searchTerm: searchTerm,
                        queriedTeams: filteredTeams);
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

        public async Task<IActionResult> TeamBannerUpload()
        {
            // get user data
            AppUser user = await userRepo.GetUserDataAsync(HttpContext.User);
            if (user != null)
            {
                // check if imgur ID is in use
                var teamData = user.ManagedTeam;
                var bannerData = new ImgurUploadRequest();
                if (teamData.BannerImageData != null)
                {
                       // get imgur data, set link 
                       bannerData.LinkToImage = "https://i.imgur.com/" + teamData.BannerImageData.PhotoID + ".jpg";
                }
                return View("UploadBanner", bannerData);
            }
            return RedirectToAction("TeamManagement");
        }

        [HttpPost]
        public async Task<IActionResult> TeamBannerUpload(ImgurUploadRequest imageData)
        {
            if(ModelState.IsValid && imageData.Image != null)
            {
                // get user data
                AppUser user = await userRepo.GetUserDataAsync(HttpContext.User);
                if (user != null)
                {
                    // setup image data object
                    var userTeam = user.ManagedTeam;
                    var imagurConfig = imgurConfigRepo.GetConfig;
                    imageData.Type = "base64";
                    imageData.Title = userTeam.Name + "_teamBanner";
                    imageData.Description = "Team banner image";

                    // delete old image if exists
                    if (userTeam.BannerImageData != null)
                    {
                        var oldPhotoDeleteHash = user.ManagedTeam.BannerImageData.DeleteHash;
                        var accessToken = imagurConfig.AccessToken;
                        var imageDeleteData = ImagurAuth.DeleteImage(oldPhotoDeleteHash, accessToken);
                        var deleteResponse = JsonConvert.DeserializeObject<ImgurBasicUploadResponse>(imageDeleteData.Content);
                    }

                    // upload new image, parse result
                    var imageDataResponse = ImagurAuth.AddImage(imageData, configuration["ImgurCredentials:ClientID"]);
                    var responseBody = JsonConvert.DeserializeObject<ImgurUploadResponse>(imageDataResponse.Content);
                    
                    // get new image ID and set to team in DB
                    if (responseBody.status == 200)
                    {
                        if (userTeam.BannerImageData != null)
                        {
                            // update existing image data
                            var updatedBannerModel = userTeam.BannerImageData;
                            updatedBannerModel.PhotoID = responseBody.data.id;
                            updatedBannerModel.DeleteHash = responseBody.data.deletehash;

                            // save to database
                            await imgurPhotoRepo.UpdatePhoto(updatedBannerModel);
                        }
                        else
                        {
                            // create and set image data
                            var newBannerModel = new ImgurPhotoData()
                            {
                                PhotoID = responseBody.data.id,
                                DeleteHash = responseBody.data.deletehash
                            };

                            // save to database and add to team
                            await imgurPhotoRepo.AddPhoto(newBannerModel);
                            userTeam.BannerImageData = newBannerModel;
                        }
                    } 
                    else
                        return RedirectToAction("TeamBannerUpload");
                    // update team data
                    await teamRepo.UpdateTeam(userTeam);
                }
                else
                {
                    return RedirectToAction("TeamBannerUpload");
                }
            }
            return RedirectToAction("TeamManagement");
        }

        public async Task<IActionResult> TeamSettings()
        {
            ViewBag.DefaultFooter = false;
            try
            {
                var user = await userRepo.GetUserDataAsync(HttpContext.User);
                Team usersTeam = user.ManagedTeam;
                if (user == null && user.ManagedTeam == null)
                {
                    // If user is null, redirect to a Team/Index
                    ErrorViewModel e = new ErrorViewModel
                    {
                        RequestId = "DKS-0014",
                        Message = "An error occured while trying to get the settings"
                    };
                    return View("Error", e);
                }

                // verify users role, after roles are set up
                // get the users team
                TeamSettingsViewModel teamSettings = new TeamSettingsViewModel();
                teamSettings.TeamID = usersTeam.TeamID;
                teamSettings.Name = usersTeam.Name;
                teamSettings.Country = usersTeam.Country;
                teamSettings.Providence = usersTeam.Providence;
                teamSettings.StreetAddress = usersTeam.StreetAddress;
                teamSettings.ZipCode = usersTeam.ZipCode;
                teamSettings.CorporatePageURL = usersTeam.CorporatePageURL;
                teamSettings.BusinessEmail = usersTeam.BusinessEmail;
                teamSettings.PhoneNumber = usersTeam.PhoneNumber;
                teamSettings.Description = usersTeam.Description;

                return View("TeamSettings", teamSettings);

            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0014",
                    Message = "An error occured while trying to get the settings"
                };
                return View("Error", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeamSettings(int id, TeamSettingsViewModel teamSettings)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Team foundTeam = teamRepo.FindTeamById(id);
                    foundTeam.Name = teamSettings.Name;
                    foundTeam.Country = teamSettings.Country;
                    foundTeam.Providence = teamSettings.Providence;
                    foundTeam.StreetAddress = teamSettings.StreetAddress;
                    foundTeam.ZipCode = teamSettings.ZipCode;
                    foundTeam.CorporatePageURL = teamSettings.CorporatePageURL;
                    foundTeam.BusinessEmail = teamSettings.BusinessEmail;
                    foundTeam.PhoneNumber = teamSettings.PhoneNumber;
                    foundTeam.Description = teamSettings.Description;

                    await teamRepo.UpdateTeam(foundTeam);

                    var teamMangementVM = new TeamManagementVM()
                    {
                        LifeTimeSales = 0,
                        MonthlySales = 0,
                        WeeklySales = 0,
                        Team = foundTeam,
                        OfferedProductGroups = null
                    };
                    ViewBag.DefaultFooter = false;
                    return View("TeamManagement", teamMangementVM);
                }
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0014",
                    Message = "An error occured while trying to change the settings"
                };
                return View("Error", e);
            }
            return View();
        }

        public async Task<IActionResult> TeamManagement()
        {
            // display the main management page for teams
            // verify users role, after roles are set up
            // get the user's team
            var user = await userRepo.GetUserDataAsync(HttpContext.User);
            var teamMangementVM = new TeamManagementVM()
            {
                LifeTimeSales = 0,
                MonthlySales = 0,
                WeeklySales = 0,
                Team = user.ManagedTeam,
                OfferedProductGroups = null
            };
            ViewBag.DefaultFooter = false;
            return View("TeamManagement", teamMangementVM);
        }

        public async Task<IActionResult> AddGroup()
        {
            try
            {
                var tagList = tagRepo.GetTags;
                var rosterGroupList = rosterGroupRepo.ProductGroups;
                var createGroupVM = new CreateGroupVM()
                {
                    DatabaseTags = tagList,
                    RosterGroups = rosterGroupList
                };
                return View("AddGroup", createGroupVM);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0016",
                    Message = "An error occured while trying to add a group"
                };
                return View("Error", e);
            }
   
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(CreateGroupVM createdGroupModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // valid user
                    var user = await userRepo.GetUserDataAsync(HttpContext.User);
                    if (user != null)
                    {
                        // get base roster group
                        var rosterGroup = rosterGroupRepo.GetGroupById(createdGroupModel.SelectedRosterGroupID);
                        // create product group
                        var newGroup = new ProductGroup()
                        {
                            Title = createdGroupModel.Title,
                            Description = createdGroupModel.Description,
                            GeneralThumbnail = createdGroupModel.GeneralThumbnail,
                            PrintDesignPNG = createdGroupModel.PrintDesignPNG,
                            BaseGroupModelNumber = rosterGroup.ModelNumber
                        };


                        // check new tags
                        var tagList = tagRepo.GetTags;
                        if (createdGroupModel.NewTagName != null)
                        {
                            // add existing tag from DB
                            var existingTag = tagList.Find(tag => tag.TagLine.ToUpper() == createdGroupModel.NewTagName.ToUpper());
                            if (existingTag == null)
                            {
                                // create tag and add to DB if it does not exist
                                var newTag = new Tag()
                                {
                                    TagLine = createdGroupModel.NewTagName
                                };
                                await tagRepo.AddTag(newTag);
                                existingTag = newTag;
                            }
                            newGroup.ProductTags.Add(existingTag);
                        }
                        else if (createdGroupModel.ExistingTagID != null)
                        {
                            // get existing tag from DB, add to group
                            var existingTag = tagList.Find(tag => tag.TagID == createdGroupModel.ExistingTagID);
                            newGroup.ProductTags.Add(existingTag);
                        }

                        // save new group to DB
                        await productGroupRepo.AddProductGroup(newGroup);

                        // update user team data
                        user.ManagedTeam.ProductGroups.Add(newGroup);
                        await userManager.UpdateAsync(user);

                        return RedirectToAction("TeamManagement");
                    }
                }
                ModelState.AddModelError(nameof(CreateGroupVM.Title), "Please make sure to fill out all required fields");
                return View("AddGroup", createdGroupModel);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0016",
                    Message = "An error occured while trying to add a group"
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> RemoveGroup(int SelectedGroupID)
        {
            try
            {
                // get user and select the group attached to its team
                var user = await userRepo.GetUserDataAsync(HttpContext.User);
                if (user != null)
                {
                    // check if group exists, for secure removal
                    var doesGroupExist = user.ManagedTeam.ProductGroups
                        .Exists(group => group.ProductGroupID == SelectedGroupID);
                    if (doesGroupExist)
                        await productGroupRepo.RemoveProductGroup(SelectedGroupID);
                }
                return RedirectToAction("TeamManagement");
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0017",
                    Message = "An error occured while trying to remove a group"
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> UpdateGroup(int SelectedGroupID)
        {
            try
            {
                // get user and select the group attached to its team
                var user = await userRepo.GetUserDataAsync(HttpContext.User);
                if (user != null)
                {
                    // get the group and return view
                    var foundGroup = user.ManagedTeam.ProductGroups
                        .Find(group => group.ProductGroupID == SelectedGroupID);

                    if (foundGroup == null)
                        return RedirectToAction("TeamManagement");

                    // create updated group VM from found group
                    var updateGroupVM = new UpdateGroupVM()
                    {
                        GroupID = foundGroup.ProductGroupID,
                        Title = foundGroup.Title,
                        Description = foundGroup.Description,
                        ThumbnailURL = foundGroup.GeneralThumbnail,
                        DatabaseTags = tagRepo.GetTags,
                        ExistingGroupTags = foundGroup.ProductTags
                    };

                    return View("ModifyGroup", updateGroupVM);
                }
                return RedirectToAction("TeamManagement");
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0018",
                    Message = "An error occured while trying to update a group"
                };
                return View("Error", e);
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGroup(UpdateGroupVM updatedGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userRepo.GetUserDataAsync(HttpContext.User);
                    if (user != null)
                    {
                        // get the group
                        var foundGroup = user.ManagedTeam.ProductGroups
                            .Find(group => group.ProductGroupID == updatedGroup.GroupID);
                        // apply updated properties
                        foundGroup.Description = updatedGroup.Description;
                        foundGroup.Title = updatedGroup.Title;
                        foundGroup.GeneralThumbnail = updatedGroup.ThumbnailURL;

                        // check new tags
                        var tagList = tagRepo.GetTags;
                        if (updatedGroup.NewTagName != null)
                        {
                            // add existing tag from DB
                            var existingTag = tagList.Find(tag => tag.TagLine.ToUpper() == updatedGroup.NewTagName.ToUpper());
                            if (existingTag == null)
                            {
                                // create tag and add to DB if it does not exist
                                var newTag = new Tag()
                                {
                                    TagLine = updatedGroup.NewTagName
                                };
                                await tagRepo.AddTag(newTag);
                                existingTag = newTag;
                            }
                            var groupTags = foundGroup.ProductTags;
                            groupTags.Add(existingTag);
                            foundGroup.ProductTags = groupTags;
                        }
                        else if (updatedGroup.ExistingTagID != null)
                        {
                            // get existing tag from DB, add to group
                            var existingTag = tagList.Find(tag => tag.TagID == updatedGroup.ExistingTagID);
                            var groupTags = foundGroup.ProductTags;
                            groupTags.Add(existingTag);
                            foundGroup.ProductTags = groupTags;
                        }
                        // check remove tag
                        if (updatedGroup.RemovedTagID != null)
                        {
                            // find and remove tag in list
                            var groupTags = foundGroup.ProductTags;
                            var removedTagIndex = groupTags.FindIndex(tag => tag.TagID == updatedGroup.RemovedTagID);
                            groupTags.RemoveAt(removedTagIndex);
                            foundGroup.ProductTags = groupTags;
                        }

                        // update group in DB
                        await productGroupRepo.UpdateProductGroup(foundGroup);
                        return RedirectToAction("TeamManagement");
                    }
                }
                ModelState.AddModelError(nameof(UpdateGroupVM.Title), "Please make sure to fill out all required fields");
                return View("ModifyGroup", updatedGroup);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0018",
                    Message = "An error occured while trying to update a group"
                };
                return View("Error", e);
            }

        }

        public async Task<IActionResult> UpdateTeamProduct(CustomProductSelectionCardVM selectedProductModel)
        {
            try
            {
                // redirects to team product management page
                var user = await userRepo.GetUserDataAsync(HttpContext.User);
                if (user != null)
                {
                    // get group and product data
                    var selectedGroup = user.ManagedTeam.ProductGroups
                        .Find(group => group.ProductGroupID == selectedProductModel.SelectedGroupID);
                    var selectedProduct = selectedGroup.ChildProducts
                        .Find(product => product.CustomProductID == selectedProductModel.SelectedProductID);
                    if (selectedGroup != null && selectedProduct != null)
                    {
                        var modifyProductVM = new UpdateProductVM()
                        {
                            ProductId = selectedProductModel.SelectedProductID,
                            GroupId = selectedProductModel.SelectedGroupID,
                            ProductData = selectedProduct,
                            CurrentPrice = selectedProduct.CurrentPrice,
                            LinkToImage = selectedProduct.ProductPhotoData == null ? null : String.Concat("https://i.imgur.com/", selectedProduct.ProductPhotoData.PhotoID, ".jpg")
                        };
                        return View("ModifyProduct", modifyProductVM);
                    }
                }
                return RedirectToAction("TeamManagement");
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0019",
                    Message = "An error occured while trying to update a team product"
                };
                return View("Error", e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeamProduct(UpdateProductVM updatedProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // redirects to team product management page
                    var user = await userRepo.GetUserDataAsync(HttpContext.User);
                    if (user != null)
                    {
                        // get target product
                        var foundProduct = user.ManagedTeam.ProductGroups
                            .Find(group => group.ProductGroupID == updatedProduct.GroupId).ChildProducts
                            .Find(product => product.CustomProductID == updatedProduct.ProductId);

                        // check and update imgur photo data where appropriate
                        if (updatedProduct.PhotoData != null)
                        {
                            // setup imgur request object
                            var imagurConfig = imgurConfigRepo.GetConfig;
                            var imageRequestData = new ImgurUploadRequest
                            {
                                Image = updatedProduct.PhotoData,
                                Type = "base64",
                                Title = "Product" + foundProduct.CustomProductID.ToString() + "_ProductVariantImage",
                                Description = "Product variant image"
                            };

                            if (foundProduct.ProductPhotoData != null)
                            {
                                var alreadyHasPhotoData = false;

                                // delete old image if exists
                                if (foundProduct.ProductPhotoData != null)
                                {
                                    alreadyHasPhotoData = true;
                                    var oldPhotoDeleteHash = foundProduct.ProductPhotoData.DeleteHash;
                                    var accessToken = imagurConfig.AccessToken;
                                    var imageDeleteData = ImagurAuth.DeleteImage(oldPhotoDeleteHash, accessToken);
                                    var deleteResponse = JsonConvert.DeserializeObject<ImgurBasicUploadResponse>(imageDeleteData.Content);
                                }

                                // upload new imgur photo, update photo data in DB
                                var imageDataResponse = ImagurAuth.AddImage(imageRequestData, configuration["ImgurCredentials:ClientID"]);
                                var responseBody = JsonConvert.DeserializeObject<ImgurUploadResponse>(imageDataResponse.Content);

                                if (alreadyHasPhotoData)
                                {
                                    // apply changes in DB and update
                                    foundProduct.ProductPhotoData.PhotoID = responseBody.data.id;
                                    foundProduct.ProductPhotoData.DeleteHash = responseBody.data.deletehash;
                                    await imgurPhotoRepo.UpdatePhoto(foundProduct.ProductPhotoData);
                                }
                                else
                                {
                                    // make new photo object
                                    var photoData = new ImgurPhotoData
                                    {
                                        PhotoID = responseBody.data.id,
                                        DeleteHash = responseBody.data.deletehash
                                    };
                                    // save to Db, add to found product
                                    await imgurPhotoRepo.AddPhoto(photoData);
                                    foundProduct.ProductPhotoData = photoData;
                                }
                            }
                        }

                        // check if pricing has changed
                        if (updatedProduct.CurrentPrice != null && foundProduct.CurrentPrice != updatedProduct.CurrentPrice)
                        {
                            var newPricingHistory = new PricingHistory()
                            {
                                DateChanged = DateTime.Now,
                                NewPrice = (decimal)updatedProduct.CurrentPrice
                            };

                            // save pricing history to DB
                            await pricingRepo.AddHistory(newPricingHistory);
                            foundProduct.AddPricingHistory(newPricingHistory);
                        }

                        // save product changes in DB
                        await customProductRepo.UpdateCustomProduct(foundProduct);
                        return RedirectToAction("TeamManagement");
                    }
                }
                ModelState.AddModelError(nameof(UpdateProductVM.CurrentPrice), "Please make sure to fill fields with appropriate values");
                return View("ModifyGroup", updatedProduct);
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0019",
                    Message = "An error occured while trying to update a team product"
                };
                return View("Error", e);
            }
        }

        public async Task<IActionResult> AddTeamProduct(int SelectedGroupID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get user and team data
                    var user = await userRepo.GetUserDataAsync(HttpContext.User);
                    var group = user.ManagedTeam.ProductGroups.Find(group => group.ProductGroupID == SelectedGroupID);

                    // get all roster products, filter the ones with the group model number passed in by user
                    var availbleRosterList = new List<RosterProduct>();
                    foreach (var product in rosterProductRepo.GetRosterProducts)
                    {
                        // only include if the roster product is in the product family AND is not already a part of the custom group
                        if (product.RosterGroup.ModelNumber == group.BaseGroupModelNumber && !group.ChildProducts.Exists(existingProduct => existingProduct.BaseProduct.RosterProductID == product.RosterProductID))
                            availbleRosterList.Add(product);
                    }

                    // build create product view model
                    var createProductVM = new CreateProductVM
                    {
                        AvailableBaseProducts = availbleRosterList,
                        GroupId = SelectedGroupID
                    };

                    // return view
                    return View("AddProduct", createProductVM);
                }
                return RedirectToAction("TeamManagement");
            }
            catch
            {
                // todo fix catch logic
                return RedirectToAction("TeamManagement");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTeamProduct(CreateProductVM newProductData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get user and team data
                    var user = await userRepo.GetUserDataAsync(HttpContext.User);
                    var targetGroup = user.ManagedTeam.ProductGroups
                        .Find(group => group.ProductGroupID == newProductData.GroupId);

                    // get the selected base product
                    var selectedProduct = await rosterProductRepo.GetRosterProductById(newProductData.SelectedBaseProduct);

                    // create imgur upload request view model
                    var imageData = new ImgurUploadRequest
                    {
                        Image = newProductData.ProductPhoto,
                        Type = "base64",
                        Title = user.ManagedTeam.Name + "_ProductVariantImage",
                        Description = "Product variant image"
                    };

                    // upload new image, parse result
                    var imageDataResponse = ImagurAuth.AddImage(imageData, configuration["ImgurCredentials:ClientID"]);
                    var responseBody = JsonConvert.DeserializeObject<ImgurUploadResponse>(imageDataResponse.Content);

                    // get new image ID and set to team in DB
                    if (responseBody.status == 200)
                    {
                        /*
                        *   Add imgur photo data to DB
                        *   Create initial pricing history for product
                        *   Create new custom product, add to DB
                        *   Add to appropriate group and update
                        *   Update product group with new custom product
                        */
                        var photoData = new ImgurPhotoData
                        {
                            PhotoID = responseBody.data.id,
                            DeleteHash = responseBody.data.deletehash
                        };
                        await imgurPhotoRepo.AddPhoto(photoData);

                        var initialPricingHist = new PricingHistory
                        {
                            NewPrice = newProductData.InitialPrice,
                            DateChanged = DateTime.Now
                        };
                        await pricingRepo.AddHistory(initialPricingHist);

                        var newProduct = new CustomProduct()
                        {
                            BaseProduct = selectedProduct,
                            ProductPhotoData = photoData,
                            IsProductActive = true
                        };
                        newProduct.AddPricingHistory(initialPricingHist);
                        await customProductRepo.AddCustomProduct(newProduct);

                        var childProducts = targetGroup.ChildProducts;
                        childProducts.Add(newProduct);
                        targetGroup.ChildProducts = childProducts;
                        await productGroupRepo.UpdateProductGroup(targetGroup);
                    }
                    else
                        return RedirectToAction("AddTeamProduct",new {
                            groupId = newProductData.GroupId
                        });
                }
              return RedirectToAction("TeamManagement");
            }
          
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0021",
                    Message = "An error occured while trying to remove a product"
                };
                return View("Error", e);
            }
        }

        public async Task<IActionResult> RemoveTeamProduct(int SelectedGroupID, int SelectedProductID)
        {
            if (ModelState.IsValid)
            {
                // redirects to team product management page
                var user = await userRepo.GetUserDataAsync(HttpContext.User);
                if(user != null)
                {
                    try
                    {
                        // get product to be removed
                        var foundProduct = user.ManagedTeam.ProductGroups
                            .Find(group => group.ProductGroupID == SelectedGroupID).ChildProducts
                            .Find(product => product.CustomProductID == SelectedProductID);

                        // remove from group
                        var foundGroup = user.ManagedTeam.ProductGroups
                            .Find(group => group.ProductGroupID == SelectedGroupID);
                        foundGroup.ChildProducts.Remove(foundProduct);
                        await productGroupRepo.UpdateProductGroup(foundGroup);

                        // remove product from DB
                        await customProductRepo.RemoveCustomProduct(foundProduct.CustomProductID);
                    }
                    catch(Exception error)
                    {
                        return RedirectToAction("TeamManagement");
                    }
                }
            }
            return RedirectToAction("TeamManagement");
        }

        public async Task<IActionResult> TeamReq()
        {
            ViewBag.DefaultFooter = false;
            var user = await userRepo.GetUserDataAsync(HttpContext.User);
            var requestModel = new TeamCreationRequest
            {
                AlreadyHasTeam = user.ManagedTeam != null ? true : false
            };
            return View(requestModel);
        }

        [HttpPost]
        public async Task<IActionResult> TeamReq(TeamCreationRequest request, string country, string province)
        {
            try
            {
                MyWordFilter filter = new MyWordFilter();
                if (ModelState.IsValid)
                {
                    if (filter.BadWords(request.TeamDescription) == false && filter.BadWords(request.TeamName) == false
                       && filter.BadWords(request.BusinessEmail) == false && filter.BadWords(request.CorporatePageURL) == false
                        && filter.BadWords(request.StreetAddress) == false)
                    {
                        List<Country> countries = locationRepo.Countries;
                        Country myCountry = countries.First(c => c.CountryName.ToLower() == country.ToLower());

                        List<Province> provinces = locationRepo.Provinces;
                        Province myProv = provinces.First(p => p.ProvienceAbbreviation.ToLower() == province.ToLower());
                        TeamCreationRequest req = new TeamCreationRequest
                        {
                            TeamName = request.TeamName,
                            TeamDescription = request.TeamDescription,
                            BusinessEmail = request.BusinessEmail,
                            PhoneNumber = request.PhoneNumber,
                            CorporatePageURL = request.CorporatePageURL,
                            StreetAddress = request.StreetAddress,
                            Country = myCountry,
                            Providence = myProv,
                            ZipCode = request.ZipCode
                        };

                        await teamRequestRepo.AddReq(req);

                        return View("ReqConfirm");
                    }
                }
                return View("TeamReq");
            }
            catch
            {
                ErrorViewModel e = new ErrorViewModel
                {
                    RequestId = "DKS-0022",
                    Message = "An error occured while trying to submit a team request"
                };
                return View("Error", e);
            }

        }

        // HEADLESS API METHODS
        public async Task<IActionResult> GetProductsByGroupId(int id)
        {
            
            var user = await userRepo.GetUserDataAsync(HttpContext.User);
            if(user != null)
            {
                // get product from DB
                // return product list
                var productGroup = user.ManagedTeam.ProductGroups.Find(group => group.ProductGroupID == id);
                return Ok(productGroup.ChildProducts);
            }
            return BadRequest();
        }
    }
}
