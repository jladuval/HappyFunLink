namespace Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Data.Interfaces;

    using Domain.Services.Interfaces;

    using Entities;

    public class AdminService : IAdminService
    {
        private readonly IRepository<Adjective> _adjectives;

        private readonly IRepository<Noun> _nouns;

        private readonly IUnitOfWork _uow;

        public AdminService(
            IRepository<Adjective> adjectives, 
            IRepository<Noun> nouns,
            IUnitOfWork uow)
        {
            _adjectives = adjectives;
            _nouns = nouns;
            _uow = uow;
        }

        public List<Adjective> GetAllAdjectives()
        {
            return _adjectives.GetAll().ToList();
        }

        public List<Noun> GetAllNouns()
        {
            return _nouns.GetAll().ToList();
        }

        public void InsertAdjectives(List<Adjective> adjectives)
        {
            adjectives.ForEach(x => _adjectives.Create(x));
            _uow.Commit();
        }

        public void InsertAdjective(Adjective adjective)
        {
            _adjectives.Create(adjective);
            _uow.Commit();
        }

        public void InsertNouns(List<Noun> nouns)
        {
            nouns.ForEach(x => _nouns.Create(x));
            _uow.Commit();
        }

        public void InsertNoun(Noun noun)
        {
            _nouns.Create(noun);
            _uow.Commit();
        }
    }
}
