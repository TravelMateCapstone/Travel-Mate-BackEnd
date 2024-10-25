﻿using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Entities
{
    public class Profile
    {
        [Key]
        public int UserId { get; set; }

        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public DateTime? Birthdate { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }

        public string? WhyUseTravelMate { get; set; }

        public string? MusicMoviesBooks { get; set; }

        public string? WhatToShare { get; set; }
        public string? ImageUser { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
