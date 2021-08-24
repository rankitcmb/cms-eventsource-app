using System;
using CMS.Core.Server.Core.Models;
using CMS.Core.Server.Domain.Models;

namespace CMS.Core.Server.Domain.Events
{
    public class MemberCreatedEvent : BaseDomainEvent<Member, Guid>
    {
        public MemberCreatedEvent() { }

        public MemberCreatedEvent(Member member):base(member)
        {
            FirstName = member.FirstName;
            LastName = member.LastName;
            Dbo = member.Dbo;
            Email = member.Email;
            Mobile = member.Mobile;
            Gender = member.Gender;
            JoinedAt = member.JoinedAt;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dbo { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}