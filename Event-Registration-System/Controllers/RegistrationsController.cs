using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Event_Registration_System.Data;
using Event_Registration_System.Models;
using Event_Registration_System.Services;

namespace Event_Registration_System.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly MainDBContext _context;
        private readonly MailjetService _mailjetService;

        public RegistrationsController(MainDBContext context, MailjetService mailjetService)
        {
            _context = context;
            _mailjetService = mailjetService;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            var mainDBContext = _context.Registration.Include(r => r.Event);
            return View(await mainDBContext.ToListAsync());
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Registration == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.RegistrationId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationId,ParticipantName,Email,EventId")] Registration model)
        {
            //if (ModelState.IsValid)
            //{
                var result = await _mailjetService.SendEmail(
                    model.ParticipantName,
                    model.Email,          
                    $"<h2>Dear {model.ParticipantName},</h2><p>You have successfully registered for the event.</p>"
                );

                if (result)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    ViewBag.SuccessMessage = "Registration completed successfully. A confirmation email has been sent.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "There was a problem sending the email. Please try again.");
                }
            //}

            // Reload EventId dropdown if something went wrong
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", model.EventId);
            return View(model);
        }


        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registration == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", registration.EventId);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistrationId,ParticipantName,Email,EventId")] Registration registration)
        {
            if (id != registration.RegistrationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.RegistrationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", registration.EventId);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registration == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.RegistrationId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registration == null)
            {
                return Problem("Entity set 'MainDBContext.Registration'  is null.");
            }
            var registration = await _context.Registration.FindAsync(id);
            if (registration != null)
            {
                _context.Registration.Remove(registration);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
          return (_context.Registration?.Any(e => e.RegistrationId == id)).GetValueOrDefault();
        }
    }
}
