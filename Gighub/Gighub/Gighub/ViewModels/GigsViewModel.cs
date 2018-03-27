using System.Collections.Generic;
using Gighub.Models;

namespace Gighub.ViewModels
{
    public class GigsViewModel
    {
        public IEnumerable<Gig> UpComingGigs { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
    }
}