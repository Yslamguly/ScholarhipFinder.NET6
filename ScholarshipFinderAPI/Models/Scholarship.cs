namespace ScholarhipFinderAPI.Models{
    public class Scholarship:BaseEntity
    {
        public Scholarship()
        {
            WishListItems = new HashSet<WishListItem>();
            ScholarshipCategories = new HashSet<ScholarshipCategory>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }

        public ICollection<ScholarshipCategory> ScholarshipCategories { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }
    }
}