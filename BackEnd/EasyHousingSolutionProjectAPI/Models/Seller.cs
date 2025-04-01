using System;
using System.Collections.Generic;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class Seller
{
    public int SellerId { get; set; }

    public string UserName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int StateId { get; set; }

    public int CityId { get; set; }

    public string EmailId { get; set; } = null!;

    //Foreign key to user
    public int UserId { get; set; }

    public User User { get; set; }=null!;

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
