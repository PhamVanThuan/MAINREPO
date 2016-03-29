using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class CallbackReason : IEntityValidation, ICallbackReason, IDAOObject
	{
		private SAHL.Common.BusinessModel.DAO.CallbackReason_DAO _CallbackReason;
	public CallbackReason(SAHL.Common.BusinessModel.DAO.CallbackReason_DAO CallbackReason)
	{
		this._CallbackReason = CallbackReason;
	}
		/// <summary>
		/// 
		/// </summary>
		/// <returns><see cref="CallbackReason_DAO"/></returns>		
public object GetDAOObject()

		{
			return _CallbackReason;
		}
		/// <summary>
		/// 
		/// </summary>
		public String Description 
		{
			get { return _CallbackReason.Description; }
			set { _CallbackReason.Description = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 CallbackReasonKey 
		{
			get { return _CallbackReason.CallbackReasonKey; }
			set { _CallbackReason.CallbackReasonKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		private DAOEventList<Callback_DAO, ICallback, Callback> _Callbacks;
		/// <summary>
		/// 
		/// </summary>
		public IEventList<ICallback> Callbacks
		{
			get
			{
				if (null == _Callbacks) 
				{
					IDictionary<Type, Type> Types = new Dictionary<Type, Type>();
					Types.Add(typeof(Callback_DAO), typeof(Callback));
					_Callbacks = new DAOEventList<Callback_DAO, ICallback, Callback>(_CallbackReason.Callbacks, Types);
					_Callbacks.BeforeAdd += new EventListHandler(OnCallbacks_BeforeAdd);					
					_Callbacks.BeforeRemove += new EventListHandler(OnCallbacks_BeforeRemove);
				}
				return _Callbacks;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IOriginationSourceProduct OriginationSourceProduct 
		{
			get
			{
				if (null == _CallbackReason.OriginationSourceProduct) return null;				
				IDictionary<Type, Type> Types = new Dictionary<Type, Type>();
				Types.Add(typeof(OriginationSourceProduct_DAO), typeof(OriginationSourceProduct));
				return TypeHelper.GetInterfaceObject<OriginationSourceProduct_DAO, IOriginationSourceProduct, OriginationSourceProduct>(_CallbackReason.OriginationSourceProduct, Types);
			}

			set
			{
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CallbackReason.OriginationSourceProduct = (OriginationSourceProduct_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


