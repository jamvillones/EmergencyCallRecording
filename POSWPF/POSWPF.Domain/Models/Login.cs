using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
namespace ECR.Domain.Models;

[Table(nameof(Login))]
[Index(nameof(Username), IsUnique = true)]
public partial class Login {
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Name Name { get; set; } = null!;

    public string? Position { get; set; }

    public byte[]? Photo { get; set; } = [];
}
