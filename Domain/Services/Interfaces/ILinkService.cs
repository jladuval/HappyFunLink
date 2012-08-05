namespace Domain.Services.Interfaces 
{
	public interface ILinkService 
    {
		string GetHappyLink(string originalLink);

	    string GetOriginalLink(string happyLink);
	}
}
