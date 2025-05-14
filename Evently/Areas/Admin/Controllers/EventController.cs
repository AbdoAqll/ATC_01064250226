using BusinessLogic.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Utilities;

namespace Evently.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing events in the admin area.
    /// Provides functionality for viewing, creating, editing, and deleting events, along with handling event image uploads.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = StaticRoles.AdminRole)]
    public class EventController : Controller
    {
        private readonly IServicesProvider _servicesProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="servicesProvider">The service provider to access event-related services.</param>
        /// <param name="webHostEnvironment">The environment to access the web host directory for image uploads.</param>
        public EventController(IServicesProvider servicesProvider, IWebHostEnvironment webHostEnvironment)
        {
            _servicesProvider = servicesProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Displays a list of events, with an optional filter for a specific date.
        /// </summary>
        /// <param name="date">An optional date filter for events (defaults to all events if not provided).</param>
        /// <returns>A view displaying the list of events.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(DateTime? date = null)
        {
            var events = await _servicesProvider.EventService.GetAllEvents(
                predicate: date.HasValue ? e => e.Date.Date == date.Value.Date : null,
                includeProperties: "EventTranslations"
            );
            ViewBag.SelectedDate = date;
            return View(events);
        }

        /// <summary>
        /// Displays the event creation form.
        /// </summary>
        /// <returns>A view with an empty <see cref="EventCreateUpdateViewModel"/> for creating a new event.</returns>
        [HttpGet]
        public IActionResult Create() => View(new EventCreateUpdateViewModel());

        /// <summary>
        /// Handles the creation of a new event.
        /// </summary>
        /// <param name="model">The view model containing event details.</param>
        /// <param name="ImageFile">The image file to be uploaded for the event.</param>
        /// <returns>A redirect to the event list page upon successful creation, or stays on the form if validation fails.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateUpdateViewModel model, IFormFile ImageFile)
        {
            if (!ModelState.IsValid) return View(model);

            model.ImageUrl = await HandleImageUpload(ImageFile, model.ImageUrl);

            await _servicesProvider.EventService.CreateEvent(model);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the details of a specific event.
        /// </summary>
        /// <param name="id">The ID of the event to display.</param>
        /// <returns>A view displaying the event details.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var eventDetails = await _servicesProvider.EventService.GetFirstEvent(
                predicate: e => e.Id == id,
                includeProperties: "EventTranslations");

            return View(eventDetails);
        }

        /// <summary>
        /// Displays the event edit form with existing event data.
        /// </summary>
        /// <param name="id">The ID of the event to edit.</param>
        /// <returns>A view displaying the edit form pre-filled with event data.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventEntity = await _servicesProvider.EventService.GetFirstEvent(
                predicate: e => e.Id == id,
                includeProperties: "EventTranslations");

            var model = await _servicesProvider.EventService.MapEventToUpdateViewModel(eventEntity);
            return View("Create", model);
        }

        /// <summary>
        /// Handles updating an existing event.
        /// </summary>
        /// <param name="model">The updated event details.</param>
        /// <param name="ImageFile">The new image file for the event.</param>
        /// <returns>A redirect to the event list page upon successful update, or stays on the form if validation fails.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventCreateUpdateViewModel model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View("Create", model);

            // Handle the image upload for updates
            model.ImageUrl = await HandleImageUpload(ImageFile, model.ImageUrl);

            await _servicesProvider.EventService.UpdateEvent(model);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes an event and removes its associated image.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>A JSON response indicating success or failure of the delete operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var eventToDelete = await _servicesProvider.EventService.GetFirstEvent(predicate: e => e.Id == id);

            if (eventToDelete == null)
                return Json(new { success = false, message = "Event not found" });

            var result = await _servicesProvider.EventService.DeleteEvent(id);
            if (result)
            {
                // Remove associated image file from server
                await DeleteEventImage(eventToDelete.ImageUrl);
                return Json(new { success = true, message = "Delete successful" });
            }

            return Json(new { success = false, message = "Error while deleting" });
        }

        #region Helper Methods

        /// <summary>
        /// Handles image file upload and returns the URL of the uploaded image.
        /// Deletes the existing image if a new one is uploaded.
        /// </summary>
        /// <param name="imageFile">The image file to upload.</param>
        /// <param name="existingImageUrl">The URL of the existing image to replace (if any).</param>
        /// <returns>The URL of the uploaded image.</returns>
        private async Task<string> HandleImageUpload(IFormFile imageFile, string existingImageUrl)
        {
            if (imageFile == null) return existingImageUrl;

            string rootPath = _webHostEnvironment.WebRootPath;
            string uploadsFolder = Path.Combine(rootPath, "images", "Events");
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName + extension);

            // Ensure the directory exists
            Directory.CreateDirectory(uploadsFolder);

            // Delete the old image if it exists
            if (!string.IsNullOrEmpty(existingImageUrl))
            {
                string oldImagePath = Path.Combine(rootPath, existingImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            // Save the new file
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return @"\images\Events\" + fileName + extension;
        }

        /// <summary>
        /// Deletes an event image from the server.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to delete.</param>
        private async Task DeleteEventImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string rootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(rootPath, imageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        #endregion
    }
}
