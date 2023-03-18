namespace ScholarhipFinderAPI.Models{
    public class User : BaseEntity{
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }

        public ICollection<WishList> WishLists { get; set; }

    }
}