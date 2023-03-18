namespace ScholarhipFinderAPI.Models{
    public class ScholarshipCategory:BaseEntity{
        public int ScholarshipId { get; set; }
        public Scholarship Scholarship {get;set;}
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}