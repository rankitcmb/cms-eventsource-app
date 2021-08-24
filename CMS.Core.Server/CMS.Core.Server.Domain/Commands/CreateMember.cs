using System;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Core;
using CMS.Core.Server.Domain.Events;
using CMS.Core.Server.Domain.Models;
using MediatR;

namespace CMS.Core.Server.Domain.Commands
{
    public class CreateMember : INotification
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dbo { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public DateTime JoinedAt { get; set; }


        public CreateMember(Guid id, string firstName, string lastName, DateTime dbo, string email, string mobile,
            int gender, DateTime joinedAt) {

            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Dbo = dbo;
            this.Email = email;
            this.Mobile = mobile;
            this.Gender = gender;
            this.JoinedAt = joinedAt;
        }
    }

    public class CreateMemberHandler : INotificationHandler<CreateMember>
    {
        private readonly IEventsService<Member, Guid> _eventsService;

        public CreateMemberHandler(IEventsService<Member, Guid> eventsService)
        {
            _eventsService = eventsService;
        }

        public async Task Handle(CreateMember notification, CancellationToken cancellationToken)
        {
            var member = new Member(notification.Id,
                notification.FirstName,
                notification.LastName,
                notification.Dbo,
                notification.Email,
                notification.Mobile,
                notification.Gender,
                notification.JoinedAt);

            await _eventsService.PersistAsync(member);
            
        }
    }

}