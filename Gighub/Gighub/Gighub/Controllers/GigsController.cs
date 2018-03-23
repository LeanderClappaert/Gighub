using System;
using System.Linq;
using System.Web.Mvc;
using Gighub.Models;
using Gighub.ViewModels;
using Microsoft.AspNet.Identity;

namespace Gighub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            /*
                This used to be 2 calls to the DB to get the artist and genre for the currently logged in user. 
                But by changing the Domain Model (adding FK's GenreId and ArtistId) these calls are unnecessary.
                --> When loading the view, everything is loaded using 1 call to the DB.

                var artistId = User.Identity.GetUserId();
                var artist = _context.Users.Single(u => u.Id == artistId);
                var genre = _context.Genres.Single(g => g.Id == viewModel.Genre);
            */

            var gig = new Gig
            {
                /*
                    Artist = artist,
                    Genre = genre,
                 */

                ArtistId = User.Identity.GetUserId(),
                DateTime = DateTime.Parse($"{viewModel.Date} {viewModel.Time}"),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home"); //temporary send the user to the homepage.
        }
    }
}