using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Utility.Helpers;

namespace BusinessObject.Entities.Base
{
    [Index(nameof(Id),IsUnique = true, Name = "Index_Id")]
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreatedTime = LastUpdatedTime = CoreHelper.SystemTimeNow;
        }
        
        [Key]
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastUpdatedBy { get; set; }
        public int? DeletedBy { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
    }
}
