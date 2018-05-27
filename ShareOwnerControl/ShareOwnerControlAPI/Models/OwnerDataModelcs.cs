using System;
using ServiceStack.DataAnnotations;

namespace ShareOwnerControlAPI.Models
{
    public class OwnerDataModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public Guid ShareHolderId { get; set; }
    }
}
