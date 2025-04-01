using System;
using System.Collections.Generic;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class WishList
{
    public int WishListId { get; set; }

    public int BuyerId { get; set; }

    public int PropertyId { get; set; }

   
}
