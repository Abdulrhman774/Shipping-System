using Domain.Shared;

namespace Domain.Entities;

public partial class TbSetting : BaseEntity
{
    public double? KiloMeterRate { get; set; }

    public double? KilooGramRate { get; set; }
}
