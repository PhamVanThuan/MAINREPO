using SAHL.Core.UI.Halo.Common;
using System.Collections.Generic;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public abstract class AbstractAddressStreetPageModelBase : AbstractAddressPageModelBase
    {
        private int selectedCountryKey;

        public AbstractAddressStreetPageModelBase()
        {
            this.AddressStatuses = new NamedKey[] { new NamedKey("Active", 1), new NamedKey("Inactive", 2) };
            this.SelectedAddressStatusKey = 1;

            this.Countries = new NamedKey[] { new NamedKey("South Africa", 1) };
            this.SelectedCountryKey = 1;

            this.Provinces = new NamedKey[] { new NamedKey("- Please select - ", 0), new NamedKey("North West", 1), new NamedKey("Western Cape",2), new NamedKey("Mpumalanga", 3),
                                                            new NamedKey("Kwazulu-natal", 4), new NamedKey("Northern Cape", 5), new NamedKey("Eastern Cape", 6),
                                                            new NamedKey("Limpopo", 7), new NamedKey("Gauteng", 8), new NamedKey("Free State", 9)};
            this.SelectedProvinceKey = 0;
        }

        public IEnumerable<NamedKey> AddressStatuses { get; protected set; }

        public int SelectedAddressStatusKey { get; set; }

        public string UnitNumber { get; set; }

        public string BuildingNo { get; set; }

        public string BuildingName { get; set; }

        public string StreetNo { get; set; }

        public string StreetName { get; set; }

        public IEnumerable<NamedKey> Countries { get; protected set; }

        public int SelectedCountryKey { get; set; }

        public IEnumerable<NamedKey> Provinces { get; protected set; }

        public int SelectedProvinceKey { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }
    }
}