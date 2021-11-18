﻿

using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Helpers;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services.Interfaces;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codecool.CodecoolShop.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderDao _orderDao;
        private readonly IProductOrderDao _productOrderDao;
        private readonly IProductDao _productDao;
        private readonly ICustomerDao _customerDao;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderServices(IOrderDao order, IProductDao product,
            IProductOrderDao productOrder, UserManager<IdentityUser> userManager,
            ICustomerDao customerDao)
        {
            _orderDao = order;
            _productDao = product;
            _productOrderDao = productOrder;
            _userManager = userManager;
            _customerDao = customerDao;
        }

        public void AddOrder(OrderViewDetails order)
        {
            int customerId = _customerDao.GetCustomerIdByEmail(order);

            DataAccessLayer.Model.Order newOrder = new()
            {
                OrderPlaced = DateTime.Now,
                CustomerId = customerId,
            };

            _orderDao.Add(newOrder);
        }

        public DataAccessLayer.Model.Order Get(int id)
        {
            return _orderDao.Get(id);
        }

        public void RemoveItem(int id)
        {
            _orderDao.RemoveItem(id);
        }

        public List<DataAccessLayer.Model.Order> GetAllItems()
        {
            return _orderDao.GetAll().ToList();
        }

        public List<DataAccessLayer.Model.Order> GetByUserId(int customerId)
        {
            return _orderDao.GetByCustomerId(customerId).ToList();
        }

        public int GetLatestOrderId()
        {
            DataAccessLayer.Model.Order latestOrder = _orderDao.GetLatestAddedOrder();
            return latestOrder.CustomerId;
        }

        public decimal GetTotalOrderValue(List<ProductOrder> orderItems)
        {
            return orderItems.Select(item => item.PricePerProduct * item.Quantity).Sum();
        }

        public List<ProductOrder> UpdateProductOrderPriceFromJson(OrderViewDetails order)
        {
            List<ProductOrder> orderItems = JsonHelper.Deserialize<List<ProductOrder>>(order.CartItems);

            foreach (var item in orderItems)
            {
                item.Product = _productDao.Get(item.ProductId);
                item.PricePerProduct = item.Product.Price;
            }

            return orderItems;
        }

        public void ChargeCustomer(OrderViewDetails order, decimal orderTotal)
        {
            ChargeService charges = new();

            charges.Create(new ChargeCreateOptions
            {
                Amount = (long)orderTotal * 100,
                Description = "Test Payment",
                Currency = "usd",
                Source = order.StripeToken,
            });
        }
    }
}
