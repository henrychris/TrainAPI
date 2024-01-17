using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace TrainAPI.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [Key]
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Role { get; set; }
}