using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.DTO
{
    public class CreditCardForAddingDTO
    {
        [Required(ErrorMessage = "Credit Card Number Is Required ")] 
        [StringLength(20,MinimumLength = 16, ErrorMessage = " You Must Specify The 16-digit Credit Card Number With Spaces In Between \n")]
         public string creditCardNumber  { get; set; }
         [Required(ErrorMessage = "Credit Card Holder Is Required \n")]
         [StringLength(30,MinimumLength = 2, ErrorMessage = " You Must Insert The Name On Your Credit Card \n")]
        public string creditCardHolder  { get; set; }
        [Required(ErrorMessage = "Expiration Date Is Required \n")]
        [StringLength(5,MinimumLength = 5, ErrorMessage = " Use The Format 'MM/YY' For The Expiration Date Field \n")]
        public string expirationDate  {get; set;}
        [Required(ErrorMessage = "CVV Is Required \n")]
        [StringLength(3,MinimumLength = 3, ErrorMessage = " You Must Specify The 3-digit CVV On The Back Of Yopur Credit Card \n")]
        public string cvv  {get; set;}
    }
}