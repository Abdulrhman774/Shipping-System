using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Dashboard;

public class VwFinancial
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string? MonthName { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
}