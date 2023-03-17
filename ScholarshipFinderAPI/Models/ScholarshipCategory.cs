namespace ScholarhipFinderAPI.Models{
    public class ScholarshipCategory:BaseEntity{
        public ScholarshipCategory()
        {
            Scholarships = new HashSet<Scholarship>();
        }
        public int ScholarhipId { get; set; }
        public int CategoryId { get; set; }

        public virtual ICollection<Scholarship> Scholarships { get; set; }

    }
}