using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class SaveUsageRecordViewModelExtensions
    {
        public static UsageRecord ToUsageRecord(this SaveUsageRecordViewModel model)
        {
            return new UsageRecord
            {
                EnterDateTime = DateTime.Parse(model.EnterDateTime),
                ExitDateTime = DateTime.Parse(model.ExitDateTime),
                Parking = new Parking { CNPJ = model.ParkingCNPJ },
                Tag = new Tag { Id = model.Tag },
                TotalValue = decimal.Parse(model.TotalCash),
                TotalHours = long.Parse(model.TotalHours)
            };
        }
    }
}