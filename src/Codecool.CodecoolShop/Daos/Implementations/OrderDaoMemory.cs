﻿using Codecool.CodecoolShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class OrderDaoMemory : IOrderDao
    {
        private List<Item> data = new List<Item>();
        private static OrderDaoMemory instance = null;

        public static OrderDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new OrderDaoMemory();
            }

            return instance;
        }

        public int GetTotalValue()
        {
            throw new NotImplementedException();
        }

        public void Add(Item item)
        {
            throw new NotImplementedException();
        }

        public void Update(Item item, int quantity)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(int productId)
        {
            throw new NotImplementedException();
        }

        public Item Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetAll()
        {
            return data;
        }

        public int GetTotalQuantity()
        {
            return data.Select(x => x.Quantity).Sum();
        }
    }
}
