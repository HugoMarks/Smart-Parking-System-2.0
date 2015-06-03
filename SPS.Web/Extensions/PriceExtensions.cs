using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class PriceExtensions
    {
        public static Price ToPrice(this PriceViewModel model)
        {
            return new Price
            {
                EndTime = TimeSpan.Parse(model.EndTime),
                StartTime = TimeSpan.Parse(model.StartTime),
                Value = decimal.Parse(model.Price),
                IsDefault = model.IsDefault
            };
        }

        public static PriceViewModel ToPriceViewModel(this Price price)
        {
            return new PriceViewModel
            {
                EndTime = price.EndTime.ToString(@"hh\:mm"),
                StartTime = price.StartTime.ToString(@"hh\:mm"),
                Price = price.Value.ToString(),
                IsDefault = price.IsDefault
            };
        }
    }
}