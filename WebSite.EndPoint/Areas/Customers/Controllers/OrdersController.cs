﻿using Application.Orders.CustomerOrdersServices;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.EndPoint.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class OrdersController : Controller
    {
        private readonly ICustomerOrdersService customerOrdersService;
        private readonly UserManager<User> userManager;

        public OrdersController(ICustomerOrdersService customerOrdersService
            , UserManager<User> userManager)
        {
            this.customerOrdersService = customerOrdersService;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = userManager.GetUserAsync(User).Result;
            var orders = customerOrdersService.GetMyOrder(user.Id);
            return View(orders);
        }
        [Route("Customers/orders/ChangeOrederStat/{orderId}/{status}")]
        public async Task<IActionResult> ChangeOrederStat(int orderId, int status)
        {
            var user = userManager.GetUserAsync(User).Result;
           await customerOrdersService.chengeOrederStatuse(orderId, status, user.Id);
            return RedirectToAction("Index");
        }
        [Route("Customers/Orders/OrderDetails/{PaymentId}")]
        public IActionResult OrderDetails(Guid PaymentId)
        {
           var model = customerOrdersService.GetCustomerOrderDitales(PaymentId);
            return View(model);
        }
    }
}
