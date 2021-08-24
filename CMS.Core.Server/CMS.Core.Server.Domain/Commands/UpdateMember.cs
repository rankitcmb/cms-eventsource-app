using System;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Core;
using CMS.Core.Server.Domain.Models;
using MediatR;

namespace CMS.Core.Server.Domain.Commands
{
    public class UpdateMember : INotification
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dbo { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }

        public UpdateMember(Guid guid,string firstName, string lastName, DateTime dbo, string email, string mobile, int gender)
        {
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Dbo = dbo;
            Email = email;
            Mobile = mobile;
            Gender = gender;
        }
    }

    public class UpdateMemberHandler : INotificationHandler<UpdateMember>
    {
        private readonly IEventsService<Member, Guid> _memberEventsService;
        public UpdateMemberHandler(IEventsService<Member, Guid> memberEventsService)
        {
            _memberEventsService = memberEventsService;
        }
        public async Task Handle(UpdateMember notification, CancellationToken cancellationToken)
        {
            var member = await _memberEventsService.RehydrateAsync(notification.Guid);

            if (member == null)
                throw new ArgumentOutOfRangeException(nameof(UpdateMember.Guid), "Invalid Member Guid");
            
            member.UpdateMember(notification.FirstName,notification.LastName,notification.Dbo,notification.Email,notification.Mobile,notification.Gender);

            await _memberEventsService.PersistAsync(member);
        }
    }
}