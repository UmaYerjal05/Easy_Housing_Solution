using System;
using System.Collections.Generic;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class User
{
    public int UserId { get; set; } // Primary key for user table
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? UserType { get; set; } //admin,Buyer,Seller



}
