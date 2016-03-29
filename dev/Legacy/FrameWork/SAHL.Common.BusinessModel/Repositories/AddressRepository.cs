using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.BusinessModel.TypeMapper;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IAddressRepository))]
    public class AddressRepository : AbstractRepositoryBase, IAddressRepository
    {
        public AddressRepository()
        {
        }

        #region Old Code

        ///// <summary>
        ///// domain implementation of function [2AM]..fGetFormatedAddressDelimited
        ///// </summary>
        ///// <param name="address"></param>
        ///// <returns></returns>
        //public string fGetFormattedAddressDelimited(IAddress address, bool UseCarriageReturns)
        //{
        //    string delimeter = ",";

        //    if (UseCarriageReturns)
        //        delimeter = "\n";

        //    StringBuilder sb = new StringBuilder();

        //    switch (address.AddressFormat.Key)
        //    {
        //        case 1: //street
        //            AddressStreet street = address as AddressStreet;
        //            if (!String.IsNullOrEmpty(street.UnitNumber)) sb.Append(street.UnitNumber + delimeter);
        //            if (!String.IsNullOrEmpty(street.BuildingName))
        //            {
        //                if (!String.IsNullOrEmpty(street.BuildingNumber)) sb.Append(street.BuildingNumber + " ");
        //                sb.Append(street.BuildingName + delimeter);
        //            }
        //            if (!String.IsNullOrEmpty(street.StreetName))
        //            {
        //                if (!String.IsNullOrEmpty(street.StreetNumber)) sb.Append(street.StreetNumber + " ");
        //                sb.Append(street.StreetName + delimeter);
        //            }
        //            if (!String.IsNullOrEmpty(street.RRR_SuburbDescription)) sb.Append(street.RRR_SuburbDescription + delimeter);
        //            if (street.RRR_CityDescription != street.RRR_SuburbDescription && !String.IsNullOrEmpty(street.RRR_CityDescription))
        //                sb.Append(street.RRR_CityDescription + delimeter);
        //            if (!String.IsNullOrEmpty(street.RRR_ProvinceDescription)) sb.Append(street.RRR_ProvinceDescription + delimeter);
        //            if (!String.IsNullOrEmpty(street.RRR_CountryDescription)) sb.Append(street.RRR_CountryDescription);
        //            break;

        //        case 2: //box
        //            AddressBox box = address as AddressBox;
        //            if (!String.IsNullOrEmpty(box.BoxNumber)) sb.Append(String.Format("PO Box {0}{1}", box.BoxNumber, delimeter));
        //            if (!String.IsNullOrEmpty(box.PostOffice.Description)) sb.Append(box.PostOffice.Description + delimeter);

        //            if (box.RRR_CityDescription != box.PostOffice.Description && !String.IsNullOrEmpty(box.RRR_CityDescription))
        //                sb.Append(box.RRR_CityDescription + delimeter);

        //            if (!String.IsNullOrEmpty(box.RRR_ProvinceDescription)) sb.Append(box.RRR_ProvinceDescription + delimeter);
        //            if (!String.IsNullOrEmpty(box.RRR_PostalCode)) sb.Append(box.RRR_PostalCode + delimeter);
        //            if (!String.IsNullOrEmpty(box.RRR_CountryDescription)) sb.Append(box.RRR_CountryDescription);
        //            break;

        //        case 3: //postnet
        //            AddressPostnetSuite pnet = address as AddressPostnetSuite;
        //            if (!String.IsNullOrEmpty(pnet.SuiteNumber)) sb.Append(String.Format("PostNet Suite {0}{1}", pnet.SuiteNumber, delimeter));
        //            if (!String.IsNullOrEmpty(pnet.PrivateBagNumber)) sb.Append(String.Format("Private Bag {0}{1}", pnet.PrivateBagNumber, delimeter));
        //            if (!String.IsNullOrEmpty(pnet.PostOffice.Description)) sb.Append(pnet.PostOffice.Description + delimeter);

        //            if (pnet.RRR_CityDescription != pnet.PostOffice.Description && !String.IsNullOrEmpty(pnet.RRR_CityDescription))
        //                sb.Append(pnet.RRR_CityDescription + delimeter);

        //            if (!String.IsNullOrEmpty(pnet.RRR_ProvinceDescription)) sb.Append(pnet.RRR_ProvinceDescription + delimeter);
        //            if (!String.IsNullOrEmpty(pnet.RRR_PostalCode)) sb.Append(pnet.RRR_PostalCode + delimeter);
        //            if (!String.IsNullOrEmpty(pnet.RRR_CountryDescription)) sb.Append(pnet.RRR_CountryDescription);
        //            break;

        //        case 4: //private bag
        //            AddressPrivateBag bag = address as AddressPrivateBag;
        //            if (!String.IsNullOrEmpty(bag.PrivateBagNumber)) sb.Append(String.Format("Private Bag {0}{1}", bag.PrivateBagNumber, delimeter));
        //            if (!String.IsNullOrEmpty(bag.PostOffice.Description)) sb.Append(bag.PostOffice.Description + delimeter);

        //            if (bag.RRR_CityDescription != bag.PostOffice.Description && !String.IsNullOrEmpty(bag.RRR_CityDescription))
        //                sb.Append(bag.RRR_CityDescription + delimeter);

        //            if (!String.IsNullOrEmpty(bag.RRR_ProvinceDescription)) sb.Append(bag.RRR_ProvinceDescription + delimeter);
        //            if (!String.IsNullOrEmpty(bag.RRR_PostalCode)) sb.Append(bag.RRR_PostalCode + delimeter);
        //            if (!String.IsNullOrEmpty(bag.RRR_CountryDescription)) sb.Append(bag.RRR_CountryDescription);
        //           break;

        //        case 5: //free text
        //            AddressFreeText free = address as AddressFreeText;
        //            if (!String.IsNullOrEmpty(free.FreeText1)) sb.Append(free.FreeText1 + delimeter);
        //            if (!String.IsNullOrEmpty(free.FreeText2)) sb.Append(free.FreeText2 + delimeter);
        //            if (!String.IsNullOrEmpty(free.FreeText3)) sb.Append(free.FreeText3 + delimeter);
        //            if (!String.IsNullOrEmpty(free.FreeText4)) sb.Append(free.FreeText4 + delimeter);
        //            if (!String.IsNullOrEmpty(free.FreeText5)) sb.Append(free.FreeText5 + delimeter);
        //            if (!String.IsNullOrEmpty(free.RRR_CountryDescription)) sb.Append(free.RRR_CountryDescription);
        //           break;

        //        case 6: //cluster
        //            AddressClusterBox cluster = address as AddressClusterBox;
        //            if (!String.IsNullOrEmpty(cluster.ClusterBoxNumber)) sb.Append(String.Format("Cluster Box {0}{1}", cluster.ClusterBoxNumber, delimeter));
        //            if (!String.IsNullOrEmpty(cluster.PostOffice.Description)) sb.Append(cluster.PostOffice.Description + delimeter);

        //            if (cluster.RRR_CityDescription != cluster.PostOffice.Description && !String.IsNullOrEmpty(cluster.RRR_CityDescription))
        //                sb.Append(cluster.RRR_CityDescription + delimeter);

        //            if (!String.IsNullOrEmpty(cluster.RRR_ProvinceDescription)) sb.Append(cluster.RRR_ProvinceDescription + delimeter);
        //            if (!String.IsNullOrEmpty(cluster.RRR_PostalCode)) sb.Append(cluster.RRR_PostalCode + delimeter);
        //            if (!String.IsNullOrEmpty(cluster.RRR_CountryDescription)) sb.Append(cluster.RRR_CountryDescription);
        //           break;

        //        default:
        //            break;
        //   }

        //    return sb.ToString();
        //}

        #endregion Old Code

        #region IAddressRepository Members

        /// <summary>
        /// Implements <see cref="IAddressRepository.AddressExists"/>
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool AddressExists(ref IAddress address)
        {
            // make sure the address is valid before trying to do a search
            if (!address.ValidateEntity())
                return false;

            IAddressSearchCriteria criteria = new AddressSearchCriteria();
            criteria.ExactMatch = true;

            if (address is IAddressBox)
            {
                IAddressBox ab = address as IAddressBox;
                criteria.AddressFormat = AddressFormats.Box;
                criteria.BoxNumber = ab.BoxNumber;
                if (ab.PostOffice != null)
                    criteria.PostOfficeKey = ab.PostOffice.Key;
            }
            else if (address is IAddressClusterBox)
            {
                IAddressClusterBox acb = address as IAddressClusterBox;
                criteria.AddressFormat = AddressFormats.ClusterBox;
                criteria.ClusterBoxNumber = acb.ClusterBoxNumber;
                if (acb.PostOffice != null)
                    criteria.PostOfficeKey = acb.PostOffice.Key;
            }
            else if (address is IAddressFreeText)
            {
                IAddressFreeText aft = address as IAddressFreeText;
                criteria.AddressFormat = AddressFormats.FreeText;
                criteria.FreeTextLine1 = aft.FreeText1;
                criteria.FreeTextLine2 = aft.FreeText2;
                criteria.FreeTextLine3 = aft.FreeText3;
                criteria.FreeTextLine4 = aft.FreeText4;
                criteria.FreeTextLine5 = aft.FreeText5;
                if (aft.PostOffice != null)
                    criteria.PostOfficeKey = aft.PostOffice.Key;
            }
            else if (address is IAddressPostnetSuite)
            {
                IAddressPostnetSuite aps = address as IAddressPostnetSuite;
                criteria.AddressFormat = AddressFormats.PostNetSuite;
                criteria.PostnetSuiteNumber = aps.SuiteNumber;
                criteria.PrivateBagNumber = aps.PrivateBagNumber;
                if (aps.PostOffice != null)
                    criteria.PostOfficeKey = aps.PostOffice.Key;
            }
            else if (address is IAddressPrivateBag)
            {
                IAddressPrivateBag apb = address as IAddressPrivateBag;
                criteria.AddressFormat = AddressFormats.PrivateBag;
                criteria.PrivateBagNumber = apb.PrivateBagNumber;
                if (apb.PostOffice != null)
                    criteria.PostOfficeKey = apb.PostOffice.Key;
            }
            else if (address is IAddressStreet)
            {
                IAddressStreet ast = address as IAddressStreet;
                criteria.AddressFormat = AddressFormats.Street;
                criteria.UnitNumber = ast.UnitNumber;
                criteria.BuildingNumber = ast.BuildingNumber;
                criteria.BuildingName = ast.BuildingName;
                criteria.StreetNumber = ast.StreetNumber;
                criteria.StreetName = ast.StreetName;
                if (ast.Suburb != null)
                    criteria.SuburbKey = ast.Suburb.Key;
            }
            else
            {
                throw new NotSupportedException();
            }

            IEventList<IAddress> addresses = SearchAddresses(criteria, 1);
            if (addresses.Count == 0)
                return false;

            // address exists - update the reference object and return true

            address = addresses[0];
            return true;
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetAddressByKey"/>.
        /// </summary>
        /// <param name="addressKey"></param>
        /// <returns></returns>
        public IAddress GetAddressByKey(int addressKey)
        {
            Address_DAO address = Address_DAO.Find(addressKey);

            switch (address.AddressFormat.Key)
            {
                case (int)AddressFormats.Box:
                    return new AddressBox(address.As<AddressBox_DAO>());
                case (int)AddressFormats.ClusterBox:
                    return new AddressClusterBox(address.As<AddressClusterBox_DAO>());
                case (int)AddressFormats.FreeText:
                    return new AddressFreeText(address.As<AddressFreeText_DAO>());
                case (int)AddressFormats.PostNetSuite:
                    return new AddressPostnetSuite(address.As<AddressPostnetSuite_DAO>());
                case (int)AddressFormats.PrivateBag:
                    return new AddressPrivateBag(address.As<AddressPrivateBag_DAO>());
                case (int)AddressFormats.Street:
                    return new AddressStreet(address.As<AddressStreet_DAO>());
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets an <see cref="IAddressFormat"/> by a specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the IAddressFi=ormat.</param>
        /// <returns>An <see cref="IAddressFormat"/> object matching the supplied <c>key</c>.</returns>
        public IAddressFormat GetAddressFormatByKey(AddressFormats key)
        {
            return base.GetByKey<IAddressFormat, AddressFormat_DAO>((int)key);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetAddressTypeByKey"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IAddressType GetAddressTypeByKey(int key)
        {
            return base.GetByKey<IAddressType, AddressType_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetEmptyAddress"></see>.
        /// </summary>
        public IAddress GetEmptyAddress(Type type)
        {
            if (type == typeof(IAddressBox))
                return new AddressBox(new AddressBox_DAO());
            else if (type == typeof(IAddressClusterBox))
                return new AddressClusterBox(new AddressClusterBox_DAO());
            else if (type == typeof(IAddressFreeText))
                return new AddressFreeText(new AddressFreeText_DAO());
            else if (type == typeof(IAddressPostnetSuite))
                return new AddressPostnetSuite(new AddressPostnetSuite_DAO());
            else if (type == typeof(IAddressPrivateBag))
                return new AddressPrivateBag(new AddressPrivateBag_DAO());
            else if (type == typeof(IAddressStreet))
                return new AddressStreet(new AddressStreet_DAO());
            else
                throw new ArgumentException("Unsupported type provided.");
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetForeignCountryPostOffice"/>
        /// </summary>
        /// <param name="countryKey"></param>
        /// <returns></returns>
        public IPostOffice GetForeignCountryPostOffice(int countryKey)
        {
            Country_DAO country = Country_DAO.Find(countryKey);
            // foreign countries are stored on a 1 to 1 basis
            if (country.Provinces.Count == 0)
                return null;
            if (country.Provinces[0].Cities.Count == 0)
                return null;
            if (country.Provinces[0].Cities[0].PostOffices.Count == 0)
                return null;

            PostOffice_DAO postOffice = country.Provinces[0].Cities[0].PostOffices[0];
            return new PostOffice(postOffice);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetFailedLegalEntityAddressByKey(int)"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IFailedLegalEntityAddress GetFailedLegalEntityAddressByKey(int Key)
        {
            return base.GetByKey<IFailedLegalEntityAddress, FailedLegalEntityAddress_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetLegalEntityAddressByKey(int)"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ILegalEntityAddress GetLegalEntityAddressByKey(int Key)
        {
            return base.GetByKey<ILegalEntityAddress, LegalEntityAddress_DAO>(Key);
        }

        


        /// <summary>
        /// Implements <see cref="IAddressRepository.GetProvinceByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IProvince GetProvinceByKey(int Key)
        {
            return base.GetByKey<IProvince, Province_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetSuburbByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ISuburb GetSuburbByKey(int Key)
        {
            return base.GetByKey<ISuburb, Suburb_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetPostOfficeByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IPostOffice GetPostOfficeByKey(int Key)
        {
            return base.GetByKey<IPostOffice, PostOffice_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.GetPostOfficesByPrefix"/>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount">The maximum number of rows to return.  Set to -1 for no limit.</param>
        /// <returns></returns>
        public IReadOnlyEventList<IPostOffice> GetPostOfficesByPrefix(string prefix, int maxRowCount)
        {
            SimpleQuery<PostOffice_DAO> q = new SimpleQuery<PostOffice_DAO>(@"
                from PostOffice_DAO po
                where po.Description LIKE ?
                ",
                prefix + "%"
            );
            if (maxRowCount > 0)
                q.SetQueryRange(maxRowCount);

            List<PostOffice_DAO> postOffices = new List<PostOffice_DAO>(q.Execute());
            DAOEventList<PostOffice_DAO, IPostOffice, PostOffice> daoList = new DAOEventList<PostOffice_DAO, IPostOffice, PostOffice>(postOffices);
            return new ReadOnlyEventList<IPostOffice>(daoList);
        }

        /// <summary>
        /// Implements <see cref="IAddressRepository.SearchAddresses"/>
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IEventList<IAddress> SearchAddresses(IAddressSearchCriteria searchCriteria, int maxRowCount)
        {
            AddressSearchQuery searchQuery = null;

            switch (searchCriteria.AddressFormat)
            {
                case AddressFormats.Box:
                    searchQuery = new AddressBoxSearchQuery(searchCriteria, maxRowCount);
                    IList<AddressBox_DAO> boxAddresses = AddressBox_DAO.ExecuteQuery(searchQuery) as IList<AddressBox_DAO>;
                    return new DAOEventList<AddressBox_DAO, IAddress, AddressBox>(boxAddresses);
                case AddressFormats.ClusterBox:
                    searchQuery = new AddressClusterBoxSearchQuery(searchCriteria, maxRowCount);
                    IList<AddressClusterBox_DAO> clusterAddresses = AddressClusterBox_DAO.ExecuteQuery(searchQuery) as IList<AddressClusterBox_DAO>;
                    return new DAOEventList<AddressClusterBox_DAO, IAddress, AddressClusterBox>(clusterAddresses);
                case AddressFormats.FreeText:
                    searchQuery = new AddressFreeTextSearchQuery(searchCriteria, maxRowCount);
                    IList<AddressFreeText_DAO> freeTextAddresses = AddressFreeText_DAO.ExecuteQuery(searchQuery) as IList<AddressFreeText_DAO>;
                    return new DAOEventList<AddressFreeText_DAO, IAddress, AddressFreeText>(freeTextAddresses);
                case AddressFormats.PostNetSuite:
                    searchQuery = new AddressPostnetSuiteSearchQuery(searchCriteria, maxRowCount);
                    IList<AddressPostnetSuite_DAO> postnetAddresses = AddressPostnetSuite_DAO.ExecuteQuery(searchQuery) as IList<AddressPostnetSuite_DAO>;
                    return new DAOEventList<AddressPostnetSuite_DAO, IAddress, AddressPostnetSuite>(postnetAddresses);
                case AddressFormats.PrivateBag:
                    searchQuery = new AddressPrivateBagSearchQuery(searchCriteria, maxRowCount);
                    IList<AddressPrivateBag_DAO> privateBagAddresses = AddressPrivateBag_DAO.ExecuteQuery(searchQuery) as IList<AddressPrivateBag_DAO>;
                    return new DAOEventList<AddressPrivateBag_DAO, IAddress, AddressPrivateBag>(privateBagAddresses);
                case AddressFormats.Street:
                    searchQuery = new AddressStreetSearchQuery(searchCriteria, maxRowCount);
                    IList<AddressStreet_DAO> streetAddresses = AddressStreet_DAO.ExecuteQuery(searchQuery) as IList<AddressStreet_DAO>;
                    return new DAOEventList<AddressStreet_DAO, IAddress, AddressStreet>(streetAddresses);
                default:
                    throw new NotImplementedException("The AddressFormat on the criteria is not supported.");
            }
        }

        public IMailingAddress CreateEmptyMailingAddress()
        {
            return base.CreateEmpty<IMailingAddress, MailingAddress_DAO>();
            //return new MailingAddress(new MailingAddress_DAO());
        }

        public ILegalEntityAddress CreateEmptyLegalEntityAddress()
        {
            return base.CreateEmpty<ILegalEntityAddress, LegalEntityAddress_DAO>();
        }

        public void SaveMailingAddress(IMailingAddress mailingAddress)
        {
            base.Save<IMailingAddress, MailingAddress_DAO>(mailingAddress);
        }

        /// <summary>
        /// Saves an address.  This takes an address by reference, as if the address already exists we just return the
        /// existing address instead of creating a new wasted record.
        /// </summary>
        /// <param name="address"></param>
        public void SaveAddress(ref IAddress address)
        {
            //first clean the address
            if (address is IAddressBox)
            {
                IAddressBox ab = address as IAddressBox;

                if (ab.BoxNumber != null)
                    ab.BoxNumber = ab.BoxNumber.Trim();
                if (String.IsNullOrEmpty(ab.BoxNumber))
                    ab.BoxNumber = null;
            }
            else if (address is IAddressClusterBox)
            {
                IAddressClusterBox acb = address as IAddressClusterBox;

                if (acb.ClusterBoxNumber != null)
                    acb.ClusterBoxNumber = acb.ClusterBoxNumber.Trim();
                if (String.IsNullOrEmpty(acb.ClusterBoxNumber))
                    acb.ClusterBoxNumber = null;
            }
            else if (address is IAddressFreeText)
            {
                IAddressFreeText aft = address as IAddressFreeText;

                if (aft.FreeText1 != null)
                    aft.FreeText1 = aft.FreeText1.Trim();
                if (String.IsNullOrEmpty(aft.FreeText1))
                    aft.FreeText1 = null;

                if (aft.FreeText2 != null)
                    aft.FreeText2 = aft.FreeText2.Trim();
                if (String.IsNullOrEmpty(aft.FreeText2))
                    aft.FreeText2 = null;

                if (aft.FreeText3 != null)
                    aft.FreeText3 = aft.FreeText3.Trim();
                if (String.IsNullOrEmpty(aft.FreeText3))
                    aft.FreeText3 = null;

                if (aft.FreeText4 != null)
                    aft.FreeText4 = aft.FreeText4.Trim();
                if (String.IsNullOrEmpty(aft.FreeText4))
                    aft.FreeText4 = null;

                if (aft.FreeText5 != null)
                    aft.FreeText5 = aft.FreeText5.Trim();
                if (String.IsNullOrEmpty(aft.FreeText5))
                    aft.FreeText5 = null;
            }
            else if (address is IAddressPostnetSuite)
            {
                IAddressPostnetSuite aps = address as IAddressPostnetSuite;

                if (aps.SuiteNumber != null)
                    aps.SuiteNumber = aps.SuiteNumber.Trim();
                if (String.IsNullOrEmpty(aps.SuiteNumber))
                    aps.SuiteNumber = null;

                if (aps.PrivateBagNumber != null)
                    aps.PrivateBagNumber = aps.PrivateBagNumber.Trim();
                if (String.IsNullOrEmpty(aps.PrivateBagNumber))
                    aps.PrivateBagNumber = null;
            }
            else if (address is IAddressPrivateBag)
            {
                IAddressPrivateBag apb = address as IAddressPrivateBag;

                if (apb.PrivateBagNumber != null)
                    apb.PrivateBagNumber = apb.PrivateBagNumber.Trim();
                if (String.IsNullOrEmpty(apb.PrivateBagNumber))
                    apb.PrivateBagNumber = null;
            }
            else if (address is IAddressStreet)
            {
                IAddressStreet ast = address as IAddressStreet;

                if (ast.UnitNumber != null)
                    ast.UnitNumber = ast.UnitNumber.Trim();
                if (String.IsNullOrEmpty(ast.UnitNumber))
                    ast.UnitNumber = null;

                if (ast.BuildingNumber != null)
                    ast.BuildingNumber = ast.BuildingNumber.Trim();
                if (String.IsNullOrEmpty(ast.BuildingNumber))
                    ast.BuildingNumber = null;

                if (ast.BuildingName != null)
                    ast.BuildingName = ast.BuildingName.Trim();
                if (String.IsNullOrEmpty(ast.BuildingName))
                    ast.BuildingName = null;

                if (ast.StreetNumber != null)
                    ast.StreetNumber = ast.StreetNumber.Trim();
                if (String.IsNullOrEmpty(ast.StreetNumber))
                    ast.StreetNumber = null;

                if (ast.StreetName != null)
                    ast.StreetName = ast.StreetName.Trim();
                if (String.IsNullOrEmpty(ast.StreetName))
                    ast.StreetName = null;
            }
            else
            {
                throw new NotSupportedException();
            }

            // if the address value passed in is a new address, do a search to see if the address
            // exists - if it does the address variable will be populated with the existing address
            // and we don't have to do anything, otherwise it's a new address and we need to save it
            if (address.Key == 0 && AddressExists(ref address))
                return;

            base.Save<IAddress, Address_DAO>(address);
        }

        public IApplicationMailingAddress CreateEmptyApplicationMailingAddress()
        {
            return base.CreateEmpty<IApplicationMailingAddress, ApplicationMailingAddress_DAO>();
        }

        public void SaveApplicationMailingAddress(IApplicationMailingAddress mailingAddress)
        {
            base.Save<IApplicationMailingAddress, ApplicationMailingAddress_DAO>(mailingAddress);
        }

        public void SaveLegalEntityAddress(ILegalEntityAddress legalEntityAddress)
        {
            base.Save<ILegalEntityAddress, LegalEntityAddress_DAO>(legalEntityAddress);
        }

        public void CleanDirtyAddress(IFailedLegalEntityAddress dirtyAddress)
        {
            IDAOObject idaoDirtyAddress = dirtyAddress as IDAOObject;
            FailedLegalEntityAddress_DAO dao = (FailedLegalEntityAddress_DAO)idaoDirtyAddress.GetDAOObject();
            // update the relevant cleaned column, depending on the whether it's postal or street
            if (dao.FailedPostalMigration != null)
                dao.PostalIsCleaned = true;
            else
                dao.IsCleaned = true;

            dao.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        #endregion IAddressRepository Members
    }
}