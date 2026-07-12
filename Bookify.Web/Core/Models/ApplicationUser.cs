namespace Bookify.Web.Core.Models
{
    [Index(nameof(Email) , IsUnique = true)]
    [Index(nameof(UserName) , IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string? UpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string? LastUpdatedById { get; set; }

        public bool IsDeleted { get; set; }
    }
}
