using System.ComponentModel.DataAnnotations;

public class UpdateProfileDto
{
    [Required]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters")]
    public string Name { get; set; }
}