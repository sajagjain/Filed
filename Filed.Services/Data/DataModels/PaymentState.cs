using System;
using System.ComponentModel.DataAnnotations;

namespace Filed.Services.Data.DataModels
{
    public class PaymentState
    {
        [Key]
        public int Id { get; set; }
        public string PayState { get; set; }
        public DateTime Created { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
