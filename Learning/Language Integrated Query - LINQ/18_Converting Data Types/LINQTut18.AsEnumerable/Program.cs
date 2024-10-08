﻿using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQTut18.AsEnumerable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShippingList<Shipping> shippings = ShippingRepository.AllAsShippingList;
            
            var todayShipping = shippings.Where(x => x.ShippingDate == DateTime.Today);

            todayShipping.Process("today's shipping using the ShippingList<T> with a custom Where method I created (ShippingList<T> Where)");
            



            var todayShipping2 = shippings.AsEnumerable().Where(x => x.ShippingDate == DateTime.Today);

            todayShipping2.Process("Today's shipping using IEnumerable<T> Where");

            Console.ReadKey();
        }
    }
   
}
