using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECR.Domain.Models;
[Table(nameof(Login))]
public partial class Login {
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Name Name { get; set; } = null!;

    public string? Position { get; set; }

    public byte[]? Photo { get; set; } = [];
}
