using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Dashboard;

public class VwShipmentStatusDistribution
{
    public enShipmentStatus Status { get; set; }
    public int Count { get; set; }
}