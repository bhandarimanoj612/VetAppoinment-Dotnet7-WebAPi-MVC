using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VetAppoinment.Models.DbContext;
using VetAppoinment.Models.Dtos.Appoinment;
using VetAppoinment.Models.Entities;

namespace VetAppoinment.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            return _context.Appointments.ToList();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public ActionResult<Appointment> GetAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointments
        [HttpPost]
        public ActionResult<Appointment> PostAppointment(AppointmentCreateDTO appointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = new Appointment
            {
                VetName = appointmentDTO.VetName,
                AppointmentDate = appointmentDTO.AppointmentDate,
                Description = appointmentDTO.Description,
                Price = appointmentDTO.Price,
                Image= appointmentDTO.Image,//use image url for now 
                PhoneNumber = appointmentDTO.PhoneNumber,
                Email = appointmentDTO.Email,
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public IActionResult PutAppointment(int id, AppointmentUpdateDTO appointmentDTO)
        {
            if (id != appointmentDTO.Id)
            {
                return BadRequest();
            }

            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.VetName = appointmentDTO.VetName;
            appointment.AppointmentDate = appointmentDTO.AppointmentDate;
            appointment.Description = appointmentDTO.Description;
            appointment.Price = appointmentDTO.Price;
            appointment.PhoneNumber = appointmentDTO.PhoneNumber;
            appointment.Email = appointmentDTO.Email;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
