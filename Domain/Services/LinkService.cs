using System;
using System.Linq;
using Data.Interfaces;
using Entities;

namespace Domain.Services {
    using System.Data.Entity;
    using System.Security.Policy;
    using System.Web;

    using Domain.Services.Interfaces;

    public class LinkService : ILinkService {
	    private readonly IRepository<Link> _links;

	    private readonly IRepository<HappyLink> _happyLinks;

	    private readonly IUnitOfWork _unitOfWork;

	    public LinkService(IRepository<Link> links, IRepository<HappyLink> happyLinks, IUnitOfWork unitOfWork)
	    {
	        _links = links;
	        _happyLinks = happyLinks;
	        _unitOfWork = unitOfWork;
	    }

        public string GetOriginalLink(string happyLink)
        {
            return _links.Find(x => x.HappyLink.Word == happyLink).Select(x => x.OriginalLink).SingleOrDefault();
        }

	    public string GetHappyLink(string originalLink)
	    {
	        var happyLink = _links.Find(x => x.OriginalLink == originalLink).Include(x => x.HappyLink).SingleOrDefault()
	                        ?? AssignNewHappyLink(originalLink);

	        happyLink.HappyLink.LastAccessed = DateTime.Now;
	                        
			_unitOfWork.Commit();
	        return happyLink.HappyLink.Word;
	    }

	    private Link AssignNewHappyLink(string originalLink)
	    {
            if(!originalLink.Trim().Contains("://"))
            {
                originalLink = "http://" + originalLink;
            }
	        var happyLink = new Link
	            {
	                HappyLink = _happyLinks.Find(x => x.LastAccessed == null).OrderBy(r => Guid.NewGuid()).FirstOrDefault() ?? OhLordWeRanOutOfLinksFuckFuckShitFuck(),
                    OriginalLink = originalLink
	            };
	        _links.Create(happyLink);
	        return happyLink;
	    }

        private HappyLink OhLordWeRanOutOfLinksFuckFuckShitFuck()
        {
            var emergencyLink = new HappyLink { LastAccessed = DateTime.Now, Word = Guid.NewGuid().ToString() };
            _happyLinks.Create(emergencyLink);
            emergencyLink.LastAccessed = DateTime.Now;
            return emergencyLink;
        }
	}
}
