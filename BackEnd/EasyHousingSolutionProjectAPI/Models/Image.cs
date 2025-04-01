using System;
using System.Collections.Generic;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public int? PropertyId { get; set; }

    public string? ImageName { get; set; }

    
}
