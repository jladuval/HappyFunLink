using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Interfaces;
using Entities;

namespace Domain.Services {
	public class LinkService {
		public LinkService(IRepository<Link> links) {

		}

		public string GetHappyLink(string originalLink) {

			return "unimplemented";
		}
	}
}
