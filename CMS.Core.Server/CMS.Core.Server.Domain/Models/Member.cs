using System;
using CMS.Core.Server.Core.Models;
using CMS.Core.Server.Domain.Events;

namespace CMS.Core.Server.Domain.Models
{
    public class Member : BaseAggregateRoot<Member, Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime Dbo { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public int Gender { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private Member() { }
        public Member(Guid id,string firstName, string lastName, DateTime dbo, string email, string mobile,
            int gender, DateTime joinedAt): base(id)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentOutOfRangeException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentOutOfRangeException(nameof(lastName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentOutOfRangeException(nameof(email));
            if (dbo == DateTime.MinValue)
                throw new ArgumentOutOfRangeException(nameof(dbo));

            this.FirstName = firstName;
            this.LastName = lastName;
            this.Dbo = dbo;
            this.Email = email;
            this.Mobile = mobile;
            this.Gender = gender;
            this.JoinedAt = joinedAt;

            this.AddEvent(new MemberCreatedEvent(this));
        }

        public void UpdateMember(string firstName, string lastName, DateTime dbo, string email, string mobile,
            int gender)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentOutOfRangeException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentOutOfRangeException(nameof(lastName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentOutOfRangeException(nameof(email));
            if (dbo == DateTime.MinValue)
                throw new ArgumentOutOfRangeException(nameof(dbo));

            this.FirstName = firstName;
            this.LastName = lastName;
            this.Dbo = dbo;
            this.Email = email;
            this.Mobile = mobile;

            this.AddEvent(new MemberUpdatedEvent(this));
        }


        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case MemberCreatedEvent mc:
                    this.Id = mc.AggregateId;
                    this.FirstName = mc.FirstName;
                    this.LastName = mc.LastName;
                    this.Dbo = mc.Dbo;
                    this.Email = mc.Email;
                    this.Mobile = mc.Mobile;
                    this.Gender = mc.Gender;
                    this.JoinedAt = mc.JoinedAt;
                    break;
                case MemberUpdatedEvent mc:
                    this.FirstName = mc.FirstName;
                    this.LastName = mc.LastName;
                    this.Dbo = mc.Dbo;
                    this.Email = mc.Email;
                    this.Mobile = mc.Mobile;
                    this.Gender = mc.Gender;
                    break;

            }
        }

        

        public static Member Create(string firstName, string lastName, DateTime dbo, string email, string mobile,
            int gender, DateTime joinedAt)
        {
            return new Member()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Dbo = dbo,
                Email = email,
                Mobile = mobile,
                Gender = gender,
                JoinedAt = joinedAt
            };
        }
    }
}
