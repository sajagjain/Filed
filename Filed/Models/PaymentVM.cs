using Filed.Services.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Filed.Models
{
    public class PaymentVM
    {
        [Required(ErrorMessage = "This Field is Required")]
        [CreditCard]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string CardHolder { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [FutureDate]
        public DateTime ExpirationDate { get; set; }

        [Range(100, 999)]
        public string SecurityCode { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        //Assuming nothing would cost more than 1 trillion
        [Range(0.0, 1000000000000.0)]
        public decimal Amount { get; set; }
    }
}