using System;

namespace CMS.Core.Server.Domain.Models
{
    public class ChipType
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public int OrderNo { get; set; }
        public bool IsActive { get; set; }

        private ChipType() {}

        public ChipType(Guid id, decimal value,int orderNo)
        {
            Id = id;
            Value = value;
            OrderNo = orderNo;
        }

        public static ChipType Create(decimal value,int orderNo)
        {
            return new ChipType(Guid.NewGuid(), value,orderNo);
        }
    }
}