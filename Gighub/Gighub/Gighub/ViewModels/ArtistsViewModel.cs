using System.Collections.Generic;
using Gighub.Models;

namespace Gighub.ViewModels
{
    public class ArtistsViewModel
    {
        public IEnumerable<Following> Artists { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
    }
}