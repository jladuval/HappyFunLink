using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Interfaces {
	public interface ILinkService {
		string GetGeneratedLink(string originalLink);
	}
}
