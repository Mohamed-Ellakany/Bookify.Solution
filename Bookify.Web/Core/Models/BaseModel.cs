namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        public DateTime? LastUpdatedOn { get; set; }
        public string? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        
    }
}
