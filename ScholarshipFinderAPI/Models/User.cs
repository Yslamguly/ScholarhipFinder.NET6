using Microsoft.AspNetCore.Identity;

namespace ScholarhipFinderAPI.Models{
    public class User {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateAdded { get; set; }
		public DateTime DateUpdated { get; set; }
        public ICollection<WishList> WishLists { get; set; }

    }
}