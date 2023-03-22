namespace ScholarhipFinderAPI.Models{
    public class WishList:BaseEntity
    {
        public WishList()
        {
            // Users = new HashSet<User>();
            WishListItems = new HashSet<WishListItem>();
        }
        public string UserId { get; set; }

        public virtual User User { get; set; }
        // public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }

    }
}