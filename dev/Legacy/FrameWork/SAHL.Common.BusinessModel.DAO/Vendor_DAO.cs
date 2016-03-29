using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Vendor", Schema = "dbo", Lazy = true)]
    public class Vendor_DAO : DB_2AM<Vendor_DAO>
    {
        private int _key;

        private string _vendorCode;

        private OrganisationStructure_DAO _organisationStructure;

        private LegalEntity_DAO _legalEntity;

        private GeneralStatus_DAO _generalStatus;

        private Vendor_DAO _parent;

        [PrimaryKey(PrimaryKeyType.Native, "VendorKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get 
            { 
                return this._key; 
            }
            set 
            { 
                this._key = value; 
            }
        }

        [Property("VendorCode", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("VendorCode is a mandatory field")]
        public virtual string VendorCode
        {
            get
            {
                return this._vendorCode;
            }
            set
            {
                this._vendorCode = value;
            }
        }

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure is a mandatory field")]
        public virtual OrganisationStructure_DAO OrganisationStructure
        {
            get 
            { 
                return _organisationStructure; 
            }
            set 
            { 
                _organisationStructure = value; 
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("ParentKey", NotNull = false)]
        public virtual Vendor_DAO Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }
    }
}
