using System.ComponentModel.DataAnnotations;

namespace  EduFlow.Models;

public class User
{
    public Guid Id {get; set;} = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Email {get; set;} = string.Empty;

    [Required]
    public string PasswordHash {get; set;} = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName {get; set;} = string.Empty;

    [Required]
    public string Role {get; set;} = "Student";
    public Guid TenantId {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

}