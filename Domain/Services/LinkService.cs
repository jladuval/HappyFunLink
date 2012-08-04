using System;
using System.Linq;
using Data.Interfaces;
using Entities;

namespace Domain.Services {
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

	    public string GetHappyLink(string originalLink)
	    {
	        var happyLink = _links.Find(x => x.OriginalLink == originalLink).SingleOrDefault();
	        if(happyLink == null)
	        {
	            happyLink = AssignNewHappyLink(originalLink);
	        }
	        else
	        {
	            happyLink.HappyLink.LastAccessed = DateTime.Now;
	        }
	                        
			_unitOfWork.Commit();
	        return happyLink.HappyLink.Text;
	    }

	    private Link AssignNewHappyLink(string originalLink)
	    {
	        var happyLink = new Link
	            {
	                HappyLink = _happyLinks.Find(x => x.LastAccessed == null).FirstOrDefault() ?? OhLordWeRanOutOfLinksFuckFuckShitFuck(),
                    OriginalLink = originalLink
	            };
	        _links.Create(happyLink);
	        return happyLink;
	    }

        private HappyLink OhLordWeRanOutOfLinksFuckFuckShitFuck()
        {
            var emergencyLink = _happyLinks.GetAll().OrderByDescending(x => x.LastAccessed).FirstOrDefault();
            if(emergencyLink == null)
            {
                emergencyLink = new HappyLink { LastAccessed = DateTime.Now, Text = new Guid().ToString() };
                _happyLinks.Create(emergencyLink);
            }
            emergencyLink.LastAccessed = DateTime.Now;
            return emergencyLink;
        }
	}
}
