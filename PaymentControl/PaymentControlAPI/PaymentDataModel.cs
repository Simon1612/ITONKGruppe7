using System;
using ServiceStack.DataAnnotations;

namespace PaymentControlAPI
{
    public class PaymentDataModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public Guid CounterParty { get; set; }
        public string PaymentType { get; set; }
        public int PaymentAmount { get; set; }
    }
}
