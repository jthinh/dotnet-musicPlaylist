using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlaylist.Models;
using Microsoft.AspNetCore.Identity;

namespace MusicPlaylist.Services
{
    public interface IPlaylistItemService
    {
        Task<Song[]> GetPlaylistItemAsync(IdentityUser user);
        Task<bool> AddItemAsync(Song newItem, IdentityUser user);
        Task<bool> DeleteItemAsync(Guid id, IdentityUser user);
        Task<bool> EditItemAsync(Song obj);
        Task<Object> searchItemAsync(Guid id, IdentityUser user);
    }
}