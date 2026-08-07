using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Dashboard;

public class VwMonthlyVolume
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string? MonthName { get; set; }
    public int ShipmentCount { get; set; }
}