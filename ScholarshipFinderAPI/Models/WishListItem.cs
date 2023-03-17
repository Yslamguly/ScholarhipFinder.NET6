namespace ScholarhipFinderAPI.Models{
    public class WishListItem:BaseEntity
    {
        public int WishListId { get; set; }     
        public int ScholarshipId { get; set; }
    }
}