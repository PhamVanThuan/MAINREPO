
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO
	/// </summary>
    public partial class StageDefinitionComposite_WTF : BusinessModelBase<StageDefinitionComposite_WTF_DAO>, IStageDefinitionComposite_WTF
	{
        public StageDefinitionComposite_WTF(StageDefinitionComposite_WTF_DAO StageDefinitionComposite_WTF) : base(StageDefinitionComposite_WTF)
		{
            this._DAO = StageDefinitionComposite_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.UseThisDate
		/// </summary>
		public Boolean UseThisDate 
		{
			get { return _DAO.UseThisDate; }
			set { _DAO.UseThisDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.UseThisReason
		/// </summary>
		public Boolean UseThisReason 
		{
			get { return _DAO.UseThisReason; }
			set { _DAO.UseThisReason = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.StageDefinitionStageDefinitionGroup
		/// </summary>
        public IStageDefinitionStageDefinitionGroup_WTF StageDefinitionStageDefinitionGroup 
		{
			get
			{
				if (null == _DAO.StageDefinitionStageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStageDefinitionStageDefinitionGroup_WTF, StageDefinitionStageDefinitionGroup_WTF_DAO>(_DAO.StageDefinitionStageDefinitionGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StageDefinitionStageDefinitionGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.StageDefinitionStageDefinitionGroup = (StageDefinitionStageDefinitionGroup_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



