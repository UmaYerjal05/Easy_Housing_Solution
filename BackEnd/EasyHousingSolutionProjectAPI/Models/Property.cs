using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class Property
{
    public int PropertyId { get; set; }

    public string PropertyName { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public string PropertyOption { get; set; } = null!;

    public string? Description { get; set; }

    public string Address { get; set; } = null!;

    public decimal PriceRange { get; set; }

    public decimal InitialDeposit { get; set; }

    public string Landmark { get; set; } = null!;

    [DefaultValue(false)]
    public bool IsActive { get; set; } = false;

    public int? SellerId { get; set; }
    


}
