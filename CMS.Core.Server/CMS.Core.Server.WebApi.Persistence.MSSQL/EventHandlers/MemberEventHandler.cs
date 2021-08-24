using System;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.Domain.Events;
using CMS.Core.Server.WebApi.Core.Entity;
using CMS.Core.Server.WebApi.Persistence.MSSQL.Repository;
using MediatR;

namespace CMS.Core.Server.WebApi.Persistence.MSSQL.EventHandlers
{
    public class MemberEventHandler: INotificationHandler<EventReceived<MemberCreatedEvent>>,INotificationHandler<EventReceived<MemberUpdatedEvent>>
    {
        private readonly MemberRepository _memberRepository;
        public MemberEventHandler(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task Handle(EventReceived<MemberCreatedEvent> notification, CancellationToken cancellationToken)
        {

            var memberEvent = notification.Event;

            var dto = new Core.Entity.Member(memberEvent.AggregateId, memberEvent.FirstName, memberEvent.LastName, memberEvent.Dbo, memberEvent.Email,
                memberEvent.Mobile, memberEvent.Gender, memberEvent.JoinedAt);

            await _memberRepository.AddAsync(dto);

        }

        public async Task Handle(EventReceived<MemberUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var memberEvent = notification.Event;

            var dto = new Core.Entity.Member(memberEvent.AggregateId, memberEvent.FirstName, memberEvent.LastName, memberEvent.Dbo, memberEvent.Email,
                memberEvent.Mobile, memberEvent.Gender, DateTime.MinValue);

            await _memberRepository.UpdateAsync(dto);

        }
    }
}