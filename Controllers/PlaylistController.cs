using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicPlaylist.Services;
using MusicPlaylist.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace MusicPlaylist.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistItemService _playlistItemService;
        private readonly UserManager<IdentityUser> _userManager;
        public PlaylistController(IPlaylistItemService playlistItemService,
        UserManager<IdentityUser> userManager)
        {
            _playlistItemService = playlistItemService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge(); // force user to login
            // again if their information is missing

            var items = await _playlistItemService.GetPlaylistItemAsync(currentUser);
            var model = new PlaylistViewModel()
            {
                Items = items
            };

            return View(model);
        }
        public IActionResult AddItem_()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Song newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var successful = await _playlistItemService.AddItemAsync(newItem, currentUser);
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditItem_(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var model = await _playlistItemService.searchItemAsync(id, currentUser);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(Song obj)
        {
            if (ModelState.IsValid)
            {
                var success = await _playlistItemService.EditItemAsync(obj);
                if (!success)
                {
                    return BadRequest("Could not update song.");
                }
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var successful = await _playlistItemService.DeleteItemAsync(id, currentUser);
            if (!successful)
            {
                return BadRequest("Could not mark item as done.");
            }

            return RedirectToAction("Index");
        }

    }
}