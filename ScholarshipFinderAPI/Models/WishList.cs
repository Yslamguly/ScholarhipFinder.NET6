namespace ScholarhipFinderAPI.Models{
    public class WishList:BaseEntity
    {
        public WishList()
        {
            Users = new HashSet<User>();
            WishListItems = new HashSet<WishListItem>();


        }
        public int UserId { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }

    }
}