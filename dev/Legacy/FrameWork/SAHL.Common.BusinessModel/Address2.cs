using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    public partial class Address : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Address_DAO>, IAddress
    {
        /// <summary>
        /// Gets a formatted description of the address.
        /// </summary>
        /// <param name="delimiter">The delimiter to use to split up the address components.</param>
        /// <returns>The address as a readable string.</returns>
        public virtual string GetFormattedDescription(AddressDelimiters delimiter)
        {
            return GetFormattedDescription(new List<string>(), delimiter);
        }

        /// <summary>
        /// Used internally to build up the formatted address string.
        /// </summary>
        /// <param name="addressLines">The address lines comprising the address.</param>
        /// <param name="delimiter">The delimiter to use to split up the address components.</param>
        /// <returns>The address as a readable string.</returns>
        /// <seealso cref="GetFormattedDescription(AddressDelimiters)"/>
        protected virtual string GetFormattedDescription(List<String> addressLines, AddressDelimiters delimiter)
        {
            StringBuilder sb = new StringBuilder();

            string delim = "";
            // Set up the Delimiter
            if (delimiter == AddressDelimiters.CarriageReturn)
                delim = Environment.NewLine;
            else if (delimiter == AddressDelimiters.Comma)
                delim = ", ";
            else if (delimiter == AddressDelimiters.HtmlLineBreak)
                delim = "<br />";
            else
                delim = " ";

            // Build the Formatted Address string
            foreach (string line in addressLines)
            {
                if (line != null && line.Trim().Length > 0)
                {
                    if (sb.Length > 0) sb.Append(delim);
                    sb.Append(line.Trim());
                }
            }

            // Format the address into sentence case (ie: each new work starts with a capital)
            return new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(sb.ToString().ToLower());
        }

        //{
        //string _FormattedAddress = "";
        //List<string> _addressLines = new List<string>();
        //string _delimiter = "";

        //switch (AddressFormat.Key)
        //{
        //    case 2: // Box
        //    case 3: // PostNet Suite
        //    case 4: // Private Bag
        //    case 6: // Cluster Box
        //        if (AddressFormat.Key == 3)
        //        {
        //            _addressLines.Add(AddressFormat.Description + " " + SuiteNumber);
        //            _addressLines.Add("Private Bag " + BoxNumber);
        //        }
        //        else if (AddressFormat.Key == 2)
        //            _addressLines.Add("P O Box " + BoxNumber);
        //        else
        //            _addressLines.Add(AddressFormat.Description + " " + BoxNumber);

        //        _addressLines.Add(PostOffice.Description);
        //        // Only add City is different Post Office Description
        //        if (RRR_CityDescription != PostOffice.Description)
        //            _addressLines.Add(RRR_CityDescription);
        //        _addressLines.Add(RRR_ProvinceDescription);
        //        _addressLines.Add(RRR_PostalCode);
        //        _addressLines.Add(RRR_CountryDescription);
        //        break;
        //    case 5: // Free Text
        //        _addressLines.Add(FreeText1);
        //        _addressLines.Add(FreeText2);
        //        _addressLines.Add(FreeText3);
        //        _addressLines.Add(FreeText4);
        //        _addressLines.Add(FreeText5);
        //        _addressLines.Add(RRR_CountryDescription);
        //        break;
        //    default:
        //        break;
        //}

        //// Build the Formatted Address string
        //int _count = 0;
        //foreach (string _line in _addressLines)
        //{
        //    if (_line != null && _line.Trim().Length > 1)
        //    {
        //        if (_count == 0)
        //            _FormattedAddress += _line.Trim();
        //        else
        //            _FormattedAddress += _delimiter + _line.Trim();

        //        _count++;
        //    }
        //}

        //// Format the address into sentence case (ie: each new work starts with a capital)
        //return new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(_FormattedAddress.ToLower());
        //return "";
        //}

        /// <summary>
        /// The format which the address is in (Street/Box/Postnet Suite etc). The Address_DAO base class is discriminated according
        /// to this column.
        /// </summary>
        public IAddressFormat AddressFormat
        {
            get
            {
                if (null == _DAO.AddressFormat)
                {
                    ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                    IAddressRepository addrRepo = RepositoryFactory.GetRepository<IAddressRepository>();

                    if (this is IAddressStreet)
                        return addrRepo.GetAddressFormatByKey(AddressFormats.Street);
                    else if (this is IAddressBox)
                        return addrRepo.GetAddressFormatByKey(AddressFormats.Box);
                    else if (this is IAddressPostnetSuite)
                        return addrRepo.GetAddressFormatByKey(AddressFormats.PostNetSuite);
                    else if (this is IAddressPrivateBag)
                        return addrRepo.GetAddressFormatByKey(AddressFormats.PrivateBag);
                    else if (this is IAddressFreeText)
                        return addrRepo.GetAddressFormatByKey(AddressFormats.FreeText);
                    else if (this is IAddressClusterBox)
                        return addrRepo.GetAddressFormatByKey(AddressFormats.ClusterBox);
                    else
                        throw new NotSupportedException("Unsupported Address Format");
                }
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAddressFormat, AddressFormat_DAO>(_DAO.AddressFormat);
            }
        }

        protected virtual void OnLegalEntityAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnAssetLiabilities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnAssetLiabilities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnAttorneys_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnAttorneys_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnMailingAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnMailingAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnProperties_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnProperties_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnValuatorAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnValuatorAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}