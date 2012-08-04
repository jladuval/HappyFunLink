namespace HappyFunLink.Models.Admin
{
    using System.Collections.Generic;

    public class AdminModel
    {
        public UserModel NewUser { get; set; }

        public List<AdjectiveModel> Adjectives { get; set; }

        public List<NounModel> Nouns { get; set; }

        public string NewNoun { get; set;  }

        public string NewAdjective { get; set; }
    }
}