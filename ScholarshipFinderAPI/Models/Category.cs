namespace ScholarhipFinderAPI.Models{
    public class Category:BaseEntity
    {
        public Category()
        {
            ScholarshipCategories = new HashSet<ScholarshipCategory>();
        }
        public string Name { get; set; } = "";


        public virtual ICollection<ScholarshipCategory> ScholarshipCategories { get; set; }

    }
}