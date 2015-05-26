using SPS.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Common
{
    public static class BillingHelper
    {
        public static IList<IGrouping<string, Billing>> GetBillingsGrouppedByParking(DateTime start, DateTime end)
        {
            var records = BusinessManager.Instance.UsageRecords.FindAll();
            var billings = records.Where(r => r.EnterDateTime.Date >= start.Date && r.EnterDateTime.Date <= end.Date)
                .Select(r => new Billing
                {
                    DateTime = r.EnterDateTime.Date,
                    ParkingName = r.Parking.Name,
                    TotalHours = r.TotalHours,
                    TotalValue = r.TotalValue
                }).ToList();

            return billings.GroupBy(b => b.ParkingName).ToList();
        }
        public static IList<Billing> GetBillingsForParking(string parkingCNPJ, DateTime start, DateTime end)
        {
            var records = BusinessManager.Instance.UsageRecords.FindAll();
            var billings = records.Where(r => r.Parking.CNPJ == parkingCNPJ && (r.EnterDateTime.Date >= start.Date && r.EnterDateTime.Date <= end.Date))
                .Select(r => new Billing
                {
                    DateTime = r.EnterDateTime.Date,
                    ParkingName = r.Parking.Name,
                    TotalHours = r.TotalHours,
                    TotalValue = r.TotalValue
                }).ToList();

            return billings.ToList();
        }
    }

    public class Billing
    {
        public DateTime DateTime { get; set; }

        public string ParkingName { get; set; }

        public decimal TotalValue { get; set; }

        public float TotalHours { get; set; }
    }
}