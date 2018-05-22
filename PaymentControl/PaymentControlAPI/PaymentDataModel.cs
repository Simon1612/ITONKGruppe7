using System;

namespace PaymentControlAPI
{
    public class PaymentDataModel
    {
        public PaymentDataModel()
        {
        }

        public int Id { get; set; }
        public Guid CounterParty { get; set; }
        public string PaymentType { get; set; }
        public int PaymentAmount { get; set; }
    }
}
