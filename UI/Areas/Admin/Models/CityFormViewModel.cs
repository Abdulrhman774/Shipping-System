using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace UI.Areas.Admin.Models;

public class CityFormViewModel
{
    public Guid Id { get; set; }
    public string CityAname { get; set; } = string.Empty;
    public string CityEname { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id != Guid.Empty;
    
    public List<SelectListItem> Countries { get; set; } = new();
}
