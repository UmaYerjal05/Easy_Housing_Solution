using System;
using System.Collections.Generic;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int StateId { get; set; }

    
}
