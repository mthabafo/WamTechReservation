using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WamTechReservation.Models;

namespace WamTechReservation.Controllers
{
    public class ReservationsController : Controller
    {
        private ReservationsDbContext _reservationsDbContext;

        public ReservationsController(ReservationsDbContext reservationsDbContext)
        {
            _reservationsDbContext = reservationsDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            return View(await _reservationsDbContext.Reservations.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public bool ValidateDates(Reservation reservation) 
        {
            if (reservation.StartDate <= DateTime.Today)
            {
                ModelState.AddModelError("StartDate", "Date must be a future date");
                return false;
            }
            else if (reservation.EndDate < reservation.StartDate)
            {
                ModelState.AddModelError("EndDate", "Date cannot be less than StartDate");
                return false;
            }
            else
                return true;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Reservation reservation)
        {
            if (ValidateDates(reservation))
            {
                try
                {
                    _reservationsDbContext.Add(reservation);
                    await _reservationsDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _reservationsDbContext.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (ValidateDates(reservation))
            {
                try
                {
                    _reservationsDbContext.Entry(reservation).State = EntityState.Modified;
                    await _reservationsDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(reservation);
                }
            }
            return View(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _reservationsDbContext.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        // POST: ReservationsController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id, Reservation reservation)
        {
            try
            {
                var currentReservation = _reservationsDbContext.Reservations.Where(x => x.Id == reservation.Id).FirstOrDefault();

                if (currentReservation != null)
                {
                    _reservationsDbContext.Remove(currentReservation);
                    await _reservationsDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }                  
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
            }
            return View(reservation);
        }
    }
}
