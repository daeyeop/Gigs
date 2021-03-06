﻿using System.Linq;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using GigHub.Dtos;


namespace GigHub.Controllers
{
    [Authorize] // to protect userId from outside when making Http request.
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userID = User.Identity.GetUserId(); // getting the currently logged in user from user object. wont pass userid for security purpose.
            var exists = _context.Attendances.Any(a => a.AttendeeId == userID && a.GigId == dto.GigId);
            if (exists)
                return BadRequest("The attendance already exists.");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userID
            };
            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}
