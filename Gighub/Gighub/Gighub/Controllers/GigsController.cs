using System;
using System.Data.Entity;
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
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("Create", viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();

            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now)
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var viewModel = new GigsViewModel()
            {
                UpComingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };

            return View("Gigs", viewModel);
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            /*
                This used to be 2 calls to the DB to get the artist and genre for the currently logged in user. 
                But by changing the Domain Model (adding FK's GenreId and ArtistId) these calls are unnecessary.
                --> When loading the view, everything is loaded using foreign keys.

                var artistId = User.Identity.GetUserId();
                var artist = _context.Users.Single(u => u.Id == artistId);
                var genre = _context.Genres.Single(g => g.Id == viewModel.Genre);
            */

            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }


            var gig = new Gig
            {
                /*
                    Artist = artist,
                    Genre = genre,
                 */

                /*
                    Parsing is too detailed for a Controller.
                    A Controller is like a manager, coördination only.

                    DateTime = DateTime.Parse($"{viewModel.Date} {viewModel.Time}"),
                */

                ArtistId = User.Identity.GetUserId(),
                GenreId = viewModel.Genre,
                DateTime = viewModel.GetDateTime(),
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs"); //temporary send the user to the homepage.
        }
    }
}