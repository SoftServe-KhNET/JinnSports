using System.ComponentModel.DataAnnotations;

namespace JinnSports.WEB.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }
}