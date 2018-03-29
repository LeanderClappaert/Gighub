using System.Linq;
using System.Web.Http;
using Gighub.DTOs;
using Gighub.Models;
using Microsoft.AspNet.Identity;

namespace Gighub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exists = _context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId);

            if (exists)
            {
                return BadRequest("The attendance already exists.");
            }

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}
