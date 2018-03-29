using System.Linq;
using System.Web.Http;
using Gighub.DTOs;
using Gighub.Models;
using Microsoft.AspNet.Identity;

namespace Gighub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exists = _context.Followings.Any(f => f.FolloweeId == userId && f.FolloweeId == dto.FolloweeId);
            
            if (exists)
            {
                return BadRequest("Following already exists.");
            }

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };

            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }
    }
}