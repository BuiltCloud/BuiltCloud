#region Using

using System.ComponentModel.DataAnnotations;

#endregion

namespace BuiltCloud.Dashboard.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
