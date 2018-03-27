using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Gighub.Models;
using Gighub.ViewModels;
using Microsoft.AspNet.Identity;

namespace Gighub.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult GetArtist()
        {
            var userId = User.Identity.GetUserId();

            var artists = _context.Followings
                .Where(a => a.FollowerId == userId)
                .Include(g => g.Followee)
                .ToList();

            var viewModel = new ArtistsViewModel()
            {
                Artists = artists,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Artists I'm following"
            };

            return View(viewModel);
        }
    }
}