﻿using Application.Interfaces.Contexts;
using Application.Orders.CustomerOrdersServices;
using Application.Orders.Dto;
using Domain.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.AdminOrderServices
{
    public interface IAdminOrdersService
    {
        OderDitalesForAdminDto GetAdminOrderDitales(Guid PaymentId);
        List<MyOrderDto> GetShopAdminOrder(string searchkey,int orderStatus);
    }

    public class AdminOrdersService : IAdminOrdersService
    {

        private readonly IDataBaseContext context;

        public AdminOrdersService(IDataBaseContext context)
        {
            this.context = context;
        }

        public OderDitalesForAdminDto GetAdminOrderDitales(Guid PaymentId)
        {
            var payment = context.Payments.Where(p => p.Id == PaymentId)
                 .Include(p => p.Order).ThenInclude(p => p.OrderItems)
                 .FirstOrDefault();
            if (payment!=null)
            {
                var model = new OderDitalesForAdminDto
                {
                    Address=payment.Order.Address,
                    Amount=payment.Amount,
                    OrderId=payment.Order.Id,
                    OrederItems=payment.Order.OrderItems.Select(o=> new OrederItemsForOrderDto {
                    Id=o.Id,
                    CatalogItemid=o.CatalogItemId,
                    ProductName=o.ProductName,
                    UnitPrice=o.UnitPrice,
                    Units=o.Units
                 
                    }).ToList(),
                    PaymentId= PaymentId                   
                };
                return model;
            }
            return null;
        }

        public List<MyOrderDto> GetShopAdminOrder(string searchkey, int orderStatus)
        {


            var orders = context.Orders.Where(p=>((int)p.OrderStatus)== orderStatus)
                .Include(p => p.OrderItems)
                .OrderByDescending(p => p.Id).AsQueryable();
            if (!String.IsNullOrEmpty(searchkey))
            {
                orders = orders.Where(p => p.Address.ReciverName.Contains(searchkey.Trim())
                 || p.Address.PhoneNumber.Contains(searchkey.Trim())
                  || p.Address.PostalAddress.Contains(searchkey.Trim())).AsQueryable();
            }
            var result = orders.ToList()
                .Select(p => new MyOrderDto
                {
                    Id = p.Id,
                    Date = p.ZamanSabt,
                    OrderStatus = p.OrderStatus,
                    PaymentStatus = p.PaymentStatus,
                    Price = p.TotalPrice(),
                    PaymentId=context.Payments.FirstOrDefault(m=>m.OrderId==p.Id)?.Id

                }).ToList();
            return result;
        }
    }
}