using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicPlaylist.Data;
using MusicPlaylist.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MusicPlaylist.Services
{
    public class PlaylistItemService : IPlaylistItemService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Song[]> GetPlaylistItemAsync(IdentityUser user)
        {
            var items = await _context.Items
                .Where(x => x.UserId == user.Id)
                .ToArrayAsync();
            return items;
        }

        public async Task<bool> AddItemAsync(Song newItem, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.UserId = user.Id;

            if (!newItem.DueAt.HasValue) // if no value, default due in 3 days
            {
                newItem.DueAt = DateTimeOffset.Now;
            }

            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
        public async Task<bool> DeleteItemAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            _context.Items.Remove(item);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }

        public async Task<Object> searchItemAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();


            return item;
        }

        public async Task<bool> EditItemAsync(Song obj)
        {
            if (obj == null) return false;

            _context.Items.Update(obj);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }

    }
}