using Filed.Services.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Filed.Services.Data.DataModels
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [CreditCard]
        public string CreditCardNumber { get; set; }

        [Required]
        public string CardHolder { get; set; }

        [Required]
        [FutureDate]
        public DateTime ExpirationDate { get; set; }

        [Range(100, 999)]
        public string SecurityCode { get; set; }

        [Required]
        //Assuming nothing would cost more than 1 trillion
        [Range(0.0, 1000000000000.0)]
        public decimal Amount { get; set; }

        public List<PaymentState> PaymentState { get; set; }

    }
}