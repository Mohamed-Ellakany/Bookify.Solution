using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }  

        public DateTime? LastUpdatedOn { get; set; }
        public int? UpdatedById { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedById { get; set; }


    }
}
