using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Test.Data
{
    public class TestModel 
    {
        [Required]
        public int Id { get; set; }

        [StringLength(10)]
        public string Key { get; set; }

        [MyPhoneNumber]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^[2-9]\d{2}-\d{3}-\d{4}$")]
        public string AlternatePhoneNumber { get; set; }

        [MinLength (2)]
        public string MinLengthTwo { get; set; }
        
        [MaxLength (2)]
        public string MaxLengthTwo { get; set; }

        [Country (AllowCountry="ZA")]
        public string Country { get; set; }

        [CreditCard]
        public string CreditCardNumber { get; set; }

        [FileExtensions(Extensions = ("exe"))]
        public string FileName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }


    }

    public class MyPhoneNumberAttribute : RegularExpressionAttribute
    {
        public MyPhoneNumberAttribute() : base(@"^[2-9]\d{2}-\d{3}-\d{4}$") { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CountryAttribute : ValidationAttribute
    {
        public string AllowCountry { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value.ToString() != AllowCountry)
            {
                return false;
            }

            return true;
        }
    }

}
