namespace HappyFunLink.Models.Admin
{
    using System.Collections.Generic;

    public class AdminModel
    {
        public UserModel NewUser { get; set; }

        public List<string> Adjectives { get; set; }

        public List<string> Nouns { get; set; }
    }
}