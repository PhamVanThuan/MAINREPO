using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.Validator_DAO
	/// </summary>
	public partial class Validator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Validator_DAO>, IValidator
	{
				public Validator(SAHL.Common.BusinessModel.DAO.Validator_DAO Validator) : base(Validator)
		{
			this._DAO = Validator;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.InitialValue
		/// </summary>
		public String InitialValue 
		{
			get { return _DAO.InitialValue; }
			set { _DAO.InitialValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.RegularExpression
		/// </summary>
		public String RegularExpression 
		{
			get { return _DAO.RegularExpression; }
			set { _DAO.RegularExpression = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.MinimumValue
		/// </summary>
		public Double? MinimumValue
		{
			get { return _DAO.MinimumValue; }
			set { _DAO.MinimumValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.MaximumValue
		/// </summary>
		public Double? MaximumValue
		{
			get { return _DAO.MaximumValue; }
			set { _DAO.MaximumValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.DomainField
		/// </summary>
		public IDomainField DomainField 
		{
			get
			{
				if (null == _DAO.DomainField) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDomainField, DomainField_DAO>(_DAO.DomainField);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DomainField = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DomainField = (DomainField_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.ErrorRepository
		/// </summary>
		public IErrorRepository ErrorRepository 
		{
			get
			{
				if (null == _DAO.ErrorRepository) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IErrorRepository, ErrorRepository_DAO>(_DAO.ErrorRepository);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ErrorRepository = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ErrorRepository = (ErrorRepository_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Validator_DAO.ValidatorType
		/// </summary>
		public IValidatorType ValidatorType 
		{
			get
			{
				if (null == _DAO.ValidatorType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValidatorType, ValidatorType_DAO>(_DAO.ValidatorType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValidatorType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValidatorType = (ValidatorType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


