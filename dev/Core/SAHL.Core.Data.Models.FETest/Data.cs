using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class DataDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DataDataModel(string securityGroup, string archiveDate, decimal? dataContainer, decimal? backupVolume, string overlay, decimal? sTOR, string gUID, string extension, string key1, string key2, string key3, string key4, string key5, string key6, string key7, string key8, string msgTo, string msgFrom, string msgSubject, DateTime? msgReceived, DateTime? msgSent, string key9, string key10, string key11, string key12, string key13, string key14, string key15, string key16, string title, string originalFilename)
        {
            this.securityGroup = securityGroup;
            this.archiveDate = archiveDate;
            this.dataContainer = dataContainer;
            this.backupVolume = backupVolume;
            this.Overlay = overlay;
            this.STOR = sTOR;
            this.GUID = gUID;
            this.Extension = extension;
            this.Key1 = key1;
            this.Key2 = key2;
            this.Key3 = key3;
            this.Key4 = key4;
            this.Key5 = key5;
            this.Key6 = key6;
            this.Key7 = key7;
            this.Key8 = key8;
            this.msgTo = msgTo;
            this.msgFrom = msgFrom;
            this.msgSubject = msgSubject;
            this.msgReceived = msgReceived;
            this.msgSent = msgSent;
            this.Key9 = key9;
            this.Key10 = key10;
            this.Key11 = key11;
            this.Key12 = key12;
            this.Key13 = key13;
            this.Key14 = key14;
            this.Key15 = key15;
            this.Key16 = key16;
            this.Title = title;
            this.OriginalFilename = originalFilename;
        
        }
        [JsonConstructor]
        public DataDataModel(decimal iD, string securityGroup, string archiveDate, decimal? dataContainer, decimal? backupVolume, string overlay, decimal? sTOR, string gUID, string extension, string key1, string key2, string key3, string key4, string key5, string key6, string key7, string key8, string msgTo, string msgFrom, string msgSubject, DateTime? msgReceived, DateTime? msgSent, string key9, string key10, string key11, string key12, string key13, string key14, string key15, string key16, string title, string originalFilename)
        {
            this.ID = iD;
            this.securityGroup = securityGroup;
            this.archiveDate = archiveDate;
            this.dataContainer = dataContainer;
            this.backupVolume = backupVolume;
            this.Overlay = overlay;
            this.STOR = sTOR;
            this.GUID = gUID;
            this.Extension = extension;
            this.Key1 = key1;
            this.Key2 = key2;
            this.Key3 = key3;
            this.Key4 = key4;
            this.Key5 = key5;
            this.Key6 = key6;
            this.Key7 = key7;
            this.Key8 = key8;
            this.msgTo = msgTo;
            this.msgFrom = msgFrom;
            this.msgSubject = msgSubject;
            this.msgReceived = msgReceived;
            this.msgSent = msgSent;
            this.Key9 = key9;
            this.Key10 = key10;
            this.Key11 = key11;
            this.Key12 = key12;
            this.Key13 = key13;
            this.Key14 = key14;
            this.Key15 = key15;
            this.Key16 = key16;
            this.Title = title;
            this.OriginalFilename = originalFilename;
        
        }		

        public decimal ID { get; set; }

        public string securityGroup { get; set; }

        public string archiveDate { get; set; }

        public decimal? dataContainer { get; set; }

        public decimal? backupVolume { get; set; }

        public string Overlay { get; set; }

        public decimal? STOR { get; set; }

        public string GUID { get; set; }

        public string Extension { get; set; }

        public string Key1 { get; set; }

        public string Key2 { get; set; }

        public string Key3 { get; set; }

        public string Key4 { get; set; }

        public string Key5 { get; set; }

        public string Key6 { get; set; }

        public string Key7 { get; set; }

        public string Key8 { get; set; }

        public string msgTo { get; set; }

        public string msgFrom { get; set; }

        public string msgSubject { get; set; }

        public DateTime? msgReceived { get; set; }

        public DateTime? msgSent { get; set; }

        public string Key9 { get; set; }

        public string Key10 { get; set; }

        public string Key11 { get; set; }

        public string Key12 { get; set; }

        public string Key13 { get; set; }

        public string Key14 { get; set; }

        public string Key15 { get; set; }

        public string Key16 { get; set; }

        public string Title { get; set; }

        public string OriginalFilename { get; set; }

        public void SetKey(decimal key)
        {
            this.ID =  key;
        }
    }
}