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
	/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO
	/// </summary>
	public partial class ExpenseTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO>, IExpenseTypeGroup
	{
				public ExpenseTypeGroup(SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO ExpenseTypeGroup) : base(ExpenseTypeGroup)
		{
			this._DAO = ExpenseTypeGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Fee
		/// </summary>
		public Boolean? Fee
		{
			get { return _DAO.Fee; }
			set { _DAO.Fee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Expense
		/// </summary>
		public Boolean? Expense
		{
			get { return _DAO.Expense; }
			set { _DAO.Expense = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.ExpenseTypes
		/// </summary>
		private DAOEventList<ExpenseType_DAO, IExpenseType, ExpenseType> _ExpenseTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.ExpenseTypes
		/// </summary>
		public IEventList<IExpenseType> ExpenseTypes
		{
			get
			{
				if (null == _ExpenseTypes) 
				{
					if(null == _DAO.ExpenseTypes)
						_DAO.ExpenseTypes = new List<ExpenseType_DAO>();
					_ExpenseTypes = new DAOEventList<ExpenseType_DAO, IExpenseType, ExpenseType>(_DAO.ExpenseTypes);
					_ExpenseTypes.BeforeAdd += new EventListHandler(OnExpenseTypes_BeforeAdd);					
					_ExpenseTypes.BeforeRemove += new EventListHandler(OnExpenseTypes_BeforeRemove);					
					_ExpenseTypes.AfterAdd += new EventListHandler(OnExpenseTypes_AfterAdd);					
					_ExpenseTypes.AfterRemove += new EventListHandler(OnExpenseTypes_AfterRemove);					
				}
				return _ExpenseTypes;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ExpenseTypes = null;
			
		}
	}
}


