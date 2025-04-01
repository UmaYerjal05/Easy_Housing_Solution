using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class Buyer
{
    
    public int BuyerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public int UserId { get; set; }

    public User User { get; set; }

   
}
