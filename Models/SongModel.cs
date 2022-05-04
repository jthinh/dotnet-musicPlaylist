using System;
using System.ComponentModel.DataAnnotations;

namespace MusicPlaylist.Models
{
    public class Song
    {
        public string? UserId { get; set; }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Artist { get; set; }
        public DateTimeOffset? DueAt { get; set; }
    }
}