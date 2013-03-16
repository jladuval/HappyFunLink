namespace HappyFunLink.Models.Home
{
    using System.ComponentModel.DataAnnotations;

    public class LinkModel
    {
        [Required(ErrorMessage="Please enter a link")]
        [StringLength(1000, ErrorMessage = "Link must be under 1000 characters")]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "Invalid characters in link")] // No spaces
        public string OriginalLink { get; set; }

        public string HappyLink { get; set; }
    }
}