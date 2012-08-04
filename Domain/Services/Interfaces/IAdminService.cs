namespace Domain.Services.Interfaces
{
    using System.Collections.Generic;

    using Entities;

    public interface IAdminService
    {
        List<Adjective> GetAllAdjectives();

        List<Noun> GetAllNouns();

        void InsertAdjectives(List<Adjective> adjectives);

        void InsertAdjective(Adjective adjective);

        void InsertNouns(List<Noun> nouns);

        void InsertNoun(Noun noun);
    }
}
