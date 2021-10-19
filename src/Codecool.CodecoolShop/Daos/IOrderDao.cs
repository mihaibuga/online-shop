﻿using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos
{
    public interface IOrderDao : IDao<Item>
    {
        decimal GetTotalValue();
        void Update(Item item, int quantity);
    }
}
