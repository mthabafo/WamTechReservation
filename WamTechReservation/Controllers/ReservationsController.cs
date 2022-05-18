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
    /// <summary>
    /// Performs CRUD operations for reservations
    /// </summary>
    public class ReservationsController : Controller
    {
        // Stores Instance of ReservationDbContext
        private ReservationsDbContext _reservationsDbContext;

        public ReservationsController(ReservationsDbContext reservationsDbContext)
        {
            _reservationsDbContext = reservationsDbContext;
        }


        /// <summary>
        /// Lists all current reservations 
        /// </summary>
        /// <returns>A list of reservations</returns>
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            return View(await _reservationsDbContext.Reservations.ToListAsync());
        }


        /// <summary>
        /// Displays a view for creating a reservation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Creates a new reservation and post to the database
        /// </summary>
        /// <param name="reservation">reservation</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(Reservation reservation)
        {
            if (ValidateDates(reservation))
            {
                try
                {
                    _reservationsDbContext.Add(reservation);
                    await _reservationsDbContext.SaveChangesAsync();
                    TempData["AlertMessage"] = "Reservation Created Successfully..!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }

            return View();
        }


        /// <summary>
        /// Displays a view for updating a reservation
        /// </summary>
        /// <param name="id">Reservation Id</param>
        /// <returns>A view for updating a reservation</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _reservationsDbContext.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync());
        }


        /// <summary>
        /// Updates a reservation and post to the database
        /// </summary>
        /// <param name="id">Reservation Id</param>
        /// <param name="reservation">Reservation</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (ValidateDates(reservation))
            {
                try
                {
                    _reservationsDbContext.Entry(reservation).State = EntityState.Modified;
                    await _reservationsDbContext.SaveChangesAsync();
                    TempData["AlertMessage"] = "Reservation Updated Successfully..!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(reservation);
                }
            }
            return View(reservation);
        }


        /// <summary>
        /// Validate Start, End Dates and displays the error message in the fields
        /// </summary>
        /// <param name="reservation">Reservation</param>
        /// <returns>True is dates are valid and false if not</returns>
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


        /// <summary>
        /// Displays a view for deleting a reservation
        /// </summary>
        /// <param name="id">Reservation Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _reservationsDbContext.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync());
        }


        /// <summary>
        /// Deletes a selected reservation
        /// </summary>
        /// <param name="id">Reservation Id</param>
        /// <param name="reservation">Reservation</param>
        /// <returns></returns>
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
                    TempData["AlertMessage"] = "Reservation Deleted Successfully..!";
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
