﻿using Application.Users;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.EndPoint.Utilities;

namespace WebSite.EndPoint.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class AddressController : Controller
    {
        private readonly IUserAddressService userAddressService;

        public AddressController(IUserAddressService userAddressService)
        {
            this.userAddressService = userAddressService;
        }
        public IActionResult Index()
        {
            var addresses = userAddressService.GetAddress(ClaimUtility.GetUserId(User));
            return View(addresses);
        }

        public IActionResult AddNewAddress()
        {
            return View(new AddUserAddressDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(
            ErrorMessage = "عبارت امنیتی را به درستی وارد نمائید",
            CaptchaGeneratorLanguage = Language.Persian,
            CaptchaGeneratorDisplayMode = DisplayMode.NumberToWord)]
        public IActionResult AddNewAddress(AddUserAddressDto address)
        {
            if (!ModelState.IsValid)
            {
                var query = from state in ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;

                var errorList = query.ToList();
                ViewBag.error = errorList;
                return View(address);
            }
            string userId = ClaimUtility.GetUserId(User);
            address.UserId = userId;
            userAddressService.AddnewAddress(address);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            string userId = ClaimUtility.GetUserId(User);
            return View(userAddressService.GetForEdit(id, userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(
            ErrorMessage = "عبارت امنیتی را به درستی وارد نمائید",
            CaptchaGeneratorLanguage = Language.Persian,
            CaptchaGeneratorDisplayMode = DisplayMode.NumberToWord)]
        public IActionResult Edit(AddUserAddressDto address)
        {
            string userId = ClaimUtility.GetUserId(User);
            address.UserId = userId;
            userAddressService.EditAddress(address);
            return RedirectToAction(nameof(Index));
        }
    }
}
