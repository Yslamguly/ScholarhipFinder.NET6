using Microsoft.AspNetCore.Identity;

namespace ScholarhipFinderAPI.Models{
    public class User : BaseEntity{
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<WishList> WishLists { get; set; }

    }
}