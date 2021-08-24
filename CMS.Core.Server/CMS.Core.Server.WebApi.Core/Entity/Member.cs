using System;

namespace CMS.Core.Server.WebApi.Core.Entity
{
    public class Member
    {
        public Member(Guid aggregateId, string firstName, string lastName, DateTime dbo, string email, string mobile, int gender, DateTime joinedAt)
        {
            AggregateId = aggregateId;
            FirstName = firstName;
            LastName = lastName;
            Dbo = dbo;
            Email = email;
            Mobile = mobile;
            Gender = gender;
            JoinedAt = joinedAt;
        }

        public Guid AggregateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dbo { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}