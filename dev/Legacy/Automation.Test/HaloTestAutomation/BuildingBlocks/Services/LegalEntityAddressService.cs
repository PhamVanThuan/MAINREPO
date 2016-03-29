using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class LegalEntityAddressService : _2AMDataHelper, ILegalEntityAddressService
    {
        public Automation.DataModels.LegalEntityAddress GetRandomLegalEntityAddress(string legalentityIdnumber = "", string legalentityRegisteredName = "",
                string legalentityRegistrationNumber = "")
        {
            var legalentityAddress = base.GetRandomLegalEntityAddress(legalentityIdnumber, legalentityRegisteredName, legalentityRegistrationNumber);
            Assert.NotNull(legalentityAddress, "no legal entity with registration number:{0}, id number: {1},registered name:{2}",
                                                            legalentityRegistrationNumber, legalentityIdnumber, legalentityRegisteredName);
            var legalentity = base.GetLegalEntity(legalentityIdnumber, legalentityRegistrationNumber, legalentityRegisteredName, "", 0);
            var address = base.GetAddresses(addresskey: legalentityAddress.AddressKey).FirstOrDefault();
            legalentityAddress.LegalEntity = legalentity;
            legalentityAddress.Address = address;
            return legalentityAddress;
        }

        public IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetLegalEntityDomiciliumAddress(int leKey, GeneralStatusEnum domiciliumStatus)
        {
            var domicilium = (from dom in base.GetLegalEntityDomiciliumAddresses(leKey)
                              where dom.LegalEntityKey == leKey && dom.GeneralStatusKey == domiciliumStatus
                              select dom);
            return domicilium;
        }

        /// <summary>
        /// If you need a new address assign the address to the LegalEntityAddress.Address Model. If you already have an addressKey assign it to the LegalEntityAddress.AddressKey property
        /// and the link will be created using that addressKey
        /// </summary>
        /// <param name="legalEntityAddress"></param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityAddress InsertLegalEntityAddress(Automation.DataModels.LegalEntityAddress legalEntityAddress)
        {
            if (legalEntityAddress.AddressKey == 0)
            {
                var address = this.InsertAddress(legalEntityAddress.Address);
                legalEntityAddress.AddressKey = address.AddressKey;
            }
            if (legalEntityAddress.LegalEntity.LegalEntityKey == 0)
            {
                legalEntityAddress.LegalEntity.LegalEntityKey = legalEntityAddress.LegalEntityKey;
            }           
            return base.InsertLegalEntityAddress(legalEntityAddress);
        }

        public Automation.DataModels.LegalEntityAddress InsertLegalEntityAddressByAddressType(int leKey, AddressFormatEnum addressFormat, AddressTypeEnum addressType, GeneralStatusEnum generalStatus)
        {
            var emptyAddress = new Automation.DataModels.Address
            {
                AddressFormatKey = addressFormat,
                RRR_CountryDescription = "South Africa",
                RRR_ProvinceDescription = "Kwazulu-natal",
                UserID = @"SAHL\ClintonS",
                ChangeDate = DateTime.Now.ToString(Formats.DateTimeFormatSQL)
            };
            var random = new Random();
            switch (addressFormat)
            {
                case AddressFormatEnum.Street:

                    emptyAddress.StreetNumber = random.Next(0, 10000).ToString();
                    emptyAddress.StreetName = string.Format("Test{0} Street", random.Next(0, 1000).ToString());
                    emptyAddress.SuburbKey = 9357;
                    emptyAddress.RRR_CityDescription = "Durban";
                    emptyAddress.RRR_SuburbDescription = "La Lucia Ridge";
                    emptyAddress.RRR_PostalCode = "4051";
                    break;

                case AddressFormatEnum.Box:
                    emptyAddress.BoxNumber = random.Next(0, 10000).ToString();
                    emptyAddress.PostOfficeDescription = "La Lucia";
                    emptyAddress.RRR_CityDescription = "Umhlanga";
                    emptyAddress.RRR_SuburbDescription = "La Lucia";
                    emptyAddress.RRR_PostalCode = "4153";
                    break;

                case AddressFormatEnum.PostNetSuite:
                    emptyAddress.BoxNumber = string.Format(@"X{0}", random.Next(0, 10000).ToString());
                    emptyAddress.PostOfficeDescription = "La Lucia Ridge";
                    emptyAddress.RRR_CityDescription = "Durban";
                    emptyAddress.RRR_SuburbDescription = "La Lucia Ridge";
                    emptyAddress.RRR_PostalCode = "4019";
                    emptyAddress.SuiteNumber = random.Next(0, 10000).ToString();
                    break;

                case AddressFormatEnum.PrivateBag:
                    emptyAddress.BoxNumber = random.Next(0, 10000).ToString();
                    emptyAddress.PostOfficeDescription = "La Lucia Ridge";
                    emptyAddress.RRR_CityDescription = "Durban";
                    emptyAddress.RRR_PostalCode = "4019";
                    break;

                case AddressFormatEnum.FreeText:
                    emptyAddress.PostOfficeDescription = "South Georgia and The South Sandwich Islands";
                    emptyAddress.RRR_ProvinceDescription = "South Georgia and The South Sandwich Islands";
                    emptyAddress.RRR_CountryDescription = "South Georgia and The South Sandwich Islands";
                    emptyAddress.RRR_CityDescription = "South Georgia and The South Sandwich Islands";
                    emptyAddress.RRR_SuburbDescription = "South Georgia and The South Sandwich Islands";
                    emptyAddress.Line1 = string.Format(@"Line {0}", random.Next(0, 10000).ToString()); ;
                    emptyAddress.Line2 = string.Format(@"Line {0}", random.Next(0, 10000).ToString()); ;
                    emptyAddress.Line3 = string.Format(@"Line {0}", random.Next(0, 10000).ToString()); ;
                    emptyAddress.Line4 = string.Format(@"Line {0}", random.Next(0, 10000).ToString()); ;
                    emptyAddress.Line5 = string.Format(@"Line {0}", random.Next(0, 10000).ToString()); ;
                    break;

                case AddressFormatEnum.ClusterBox:
                    emptyAddress.BoxNumber = random.Next(10000, 20000).ToString();
                    emptyAddress.PostOfficeDescription = "La Lucia Ridge";
                    emptyAddress.RRR_CityDescription = "Durban";
                    emptyAddress.RRR_SuburbDescription = "La Lucia Ridge";
                    emptyAddress.RRR_PostalCode = "4019";
                    break;
            }
            var legalEntityAddress = new Automation.DataModels.LegalEntityAddress
            {
                Address = emptyAddress,
                LegalEntityKey = leKey,
                LegalEntity = new Automation.DataModels.LegalEntity { LegalEntityKey = leKey },
                GeneralStatusKey = generalStatus,
                AddressTypeKey = addressType
            };
            return this.InsertLegalEntityAddress(legalEntityAddress);
        }

        public Dictionary<string, int> GetLegalEntityAddressesByAccountKey(int accountkey)
        {
            var formattedAddresses = new Dictionary<string, int>();
            var results = base.GetLegalEntityAddressesByAccountKey(accountkey);
            int generalstatuskey = 0;
            string address = "";
            foreach (var row in results)
            {
                generalstatuskey = row.Column("generalstatuskey").GetValueAs<int>();
                address = row.Column("address").Value;
                if (!formattedAddresses.ContainsKey(address))
                    formattedAddresses.Add(address, generalstatuskey);
            }
            return formattedAddresses;
        }
    }
}