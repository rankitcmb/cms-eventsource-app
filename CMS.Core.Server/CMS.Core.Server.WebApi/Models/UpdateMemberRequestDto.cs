using System;

namespace CMS.Core.Server.WebApi.Models
{
    public class UpdateMemberRequestDto
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dbo { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
    }
}