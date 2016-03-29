namespace Automation.DataModels
{
    public class LegalEntityContactDetails : IDataModel
    {
        public string EmailAddress { get; set; }

        public PhoneNumber WorkPhoneNumber { get; set; }

        public PhoneNumber FaxNumber { get; set; }

        public PhoneNumber HomePhoneNumber { get; set; }

        public PhoneNumber CellphoneNumber { get; set; }
    }

    public class PhoneNumber
    {
        private string value;

        public PhoneNumber(string number)
        {
            this.value = number;
        }

        public string Code
        {
            get
            {
                return value.Split(' ')[0];
            }
        }

        public string Number
        {
            get
            {
                return value.Split(' ')[1];
            }
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}