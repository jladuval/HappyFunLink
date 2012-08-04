using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities {
	public class Link {
		public int Id { get; set; }
		public string OriginalLink { get; set; }
		public HappyLink HappyLink { get; set; }
	}
}
