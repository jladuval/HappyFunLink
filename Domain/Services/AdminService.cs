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

        private readonly IRepository<HappyLink> _happyLinks;

        private readonly IRepository<Link> _links;

        private readonly IUnitOfWork _uow;

        public AdminService(
            IRepository<Adjective> adjectives, 
            IRepository<Noun> nouns,
            IRepository<HappyLink> happyLinks,
            IRepository<Link> links,
            IUnitOfWork uow)
        {
            _adjectives = adjectives;
            _nouns = nouns;
            _happyLinks = happyLinks;
            _links = links;
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

        public void DeleteNoun(int id)
        {
            var noun = _nouns.FindById(id);
           _happyLinks.Delete(x => x.Word.Contains(noun.Word) && x.LastAccessed == null);
            _nouns.Delete(noun);
            _uow.Commit();
        }

        public void DeleteAdjective(int id)
        {
            var adj = _adjectives.FindById(id);
            _happyLinks.Delete(x => x.Word.Contains(adj.Word) && x.LastAccessed == null);
            _adjectives.Delete(adj);
            _uow.Commit();
        }
    }
}
