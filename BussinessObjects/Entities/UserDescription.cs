namespace BussinessObjects.Entities
{
    public class UserDescription
    {

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? Description { get; set; }

        public string? WhyUseTravelMate { get; set; }

        public string? Interests { get; set; }

        public string? MusicMoviesBooks { get; set; }

        public string? WhatToShare { get; set; }
    }
}
