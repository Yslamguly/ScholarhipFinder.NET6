using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScholarhipFinderAPI.Models;

namespace ScholarhipFinderAPI.Data
{
	public class ApiDbContext : IdentityDbContext
	{
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Scholarship> Scholarships { get; set; }
		public virtual DbSet<ScholarshipCategory> ScholarshipCategories { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<WishList> WishLists { get; set; }
		public virtual DbSet<WishListItem> WishListItems { get; set; }
		public ApiDbContext(DbContextOptions<ApiDbContext> options):base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder){
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ScholarshipCategory>()
				.HasKey(sc => sc.Id);

			modelBuilder.Entity<ScholarshipCategory>()
				.HasOne(sc => sc.Scholarship)
				.WithMany(s => s.ScholarshipCategories)
				.HasForeignKey(sc => sc.ScholarshipId);

			modelBuilder.Entity<ScholarshipCategory>()
				.HasOne(sc => sc.Category)
				.WithMany(c => c.ScholarshipCategories)
				.OnDelete(DeleteBehavior.Cascade)
				.HasForeignKey(sc => sc.CategoryId);

			modelBuilder.Entity<User>()
				.HasKey(u => u.UserId);

			modelBuilder.Entity<WishList>()
				.HasKey(wl => wl.Id);

			modelBuilder.Entity<WishList>()
				.HasOne(wl => wl.User)
				.WithMany(u => u.WishLists)
				.OnDelete(DeleteBehavior.Cascade)
				.HasForeignKey(wl => wl.UserId);

			modelBuilder.Entity<WishListItem>()
				.HasKey(wli => wli.Id);

			modelBuilder.Entity<WishListItem>()
				.HasOne(wli => wli.WishList)
				.WithMany(wl => wl.WishListItems)
				.OnDelete(DeleteBehavior.Cascade)
				.HasForeignKey(wli => wli.WishListId);

			modelBuilder.Entity<WishListItem>()
				.HasOne(wli => wli.Scholarship)
				.WithMany(s => s.WishListItems)
				.OnDelete(DeleteBehavior.Cascade)
				.HasForeignKey(wli => wli.ScholarshipId);

			modelBuilder.Entity<Scholarship>()
				.HasKey(s => s.Id);

			modelBuilder.Entity<Category>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<Scholarship>()
				.HasMany(s => s.ScholarshipCategories)
				.WithOne(sc => sc.Scholarship);

			modelBuilder.Entity<Category>()
				.HasMany(c => c.ScholarshipCategories)
				.WithOne(sc => sc.Category);


		}
	}
}