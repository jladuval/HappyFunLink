namespace HappyFunLink.Models.Home
{
    using System.ComponentModel.DataAnnotations;

    public class LinkModel
    {
        [Required]
        public string OriginalLink { get; set; }

        public string HappyLink { get; set; }
    }
}