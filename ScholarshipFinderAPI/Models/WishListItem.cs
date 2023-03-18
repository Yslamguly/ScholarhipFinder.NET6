namespace ScholarhipFinderAPI.Models{
    public class WishListItem:BaseEntity
    {
        public int WishListId { get; set; }  
        public WishList WishList { get; set; }
        public int ScholarshipId { get; set; }
        public Scholarship Scholarship { get; set; }
    }
}