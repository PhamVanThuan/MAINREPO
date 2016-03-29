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
	/// This is derived from Valuation_DAO. When instantiated it represents a SAHL Manual Valuation.
	/// </summary>
	public partial class ValuationDiscriminatedSAHLManual : Valuation, IValuationDiscriminatedSAHLManual
	{
		protected new SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedSAHLManual_DAO _DAO;
		public ValuationDiscriminatedSAHLManual(SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedSAHLManual_DAO ValuationDiscriminatedSAHLManual) : base(ValuationDiscriminatedSAHLManual)
		{
			this._DAO = ValuationDiscriminatedSAHLManual;
		}
		/// <summary>
		/// A SAHL Manual Valuation can only have one Main Building.
		/// </summary>
		public IValuationMainBuilding ValuationMainBuilding 
		{
			get
			{
				if (null == _DAO.ValuationMainBuilding) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationMainBuilding, ValuationMainBuilding_DAO>(_DAO.ValuationMainBuilding);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationMainBuilding = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationMainBuilding = (ValuationMainBuilding_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// A SAHL Manual Valuation can have many outbuildings.
		/// </summary>
		private DAOEventList<ValuationOutbuilding_DAO, IValuationOutbuilding, ValuationOutbuilding> _ValuationOutBuildings;
		/// <summary>
		/// A SAHL Manual Valuation can have many outbuildings.
		/// </summary>
		public IEventList<IValuationOutbuilding> ValuationOutBuildings
		{
			get
			{
				if (null == _ValuationOutBuildings) 
				{
					if(null == _DAO.ValuationOutBuildings)
						_DAO.ValuationOutBuildings = new List<ValuationOutbuilding_DAO>();
					_ValuationOutBuildings = new DAOEventList<ValuationOutbuilding_DAO, IValuationOutbuilding, ValuationOutbuilding>(_DAO.ValuationOutBuildings);
					_ValuationOutBuildings.BeforeAdd += new EventListHandler(OnValuationOutBuildings_BeforeAdd);					
					_ValuationOutBuildings.BeforeRemove += new EventListHandler(OnValuationOutBuildings_BeforeRemove);					
					_ValuationOutBuildings.AfterAdd += new EventListHandler(OnValuationOutBuildings_AfterAdd);					
					_ValuationOutBuildings.AfterRemove += new EventListHandler(OnValuationOutBuildings_AfterRemove);					
				}
				return _ValuationOutBuildings;
			}
		}
		/// <summary>
		/// A SAHL Manual Valuation can only have one Total Combined Thatch Value.
		/// </summary>
		public IValuationCombinedThatch ValuationCombinedThatch 
		{
			get
			{
				if (null == _DAO.ValuationCombinedThatch) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationCombinedThatch, ValuationCombinedThatch_DAO>(_DAO.ValuationCombinedThatch);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationCombinedThatch = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationCombinedThatch = (ValuationCombinedThatch_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// A SAHL Manual Valuation can have many improvements associated to it. e.g. Tennis Court, Walls or a Pool.
		/// </summary>
		private DAOEventList<ValuationImprovement_DAO, IValuationImprovement, ValuationImprovement> _ValuationImprovements;
		/// <summary>
		/// A SAHL Manual Valuation can have many improvements associated to it. e.g. Tennis Court, Walls or a Pool.
		/// </summary>
		public IEventList<IValuationImprovement> ValuationImprovements
		{
			get
			{
				if (null == _ValuationImprovements) 
				{
					if(null == _DAO.ValuationImprovements)
						_DAO.ValuationImprovements = new List<ValuationImprovement_DAO>();
					_ValuationImprovements = new DAOEventList<ValuationImprovement_DAO, IValuationImprovement, ValuationImprovement>(_DAO.ValuationImprovements);
					_ValuationImprovements.BeforeAdd += new EventListHandler(OnValuationImprovements_BeforeAdd);					
					_ValuationImprovements.BeforeRemove += new EventListHandler(OnValuationImprovements_BeforeRemove);					
					_ValuationImprovements.AfterAdd += new EventListHandler(OnValuationImprovements_AfterAdd);					
					_ValuationImprovements.AfterRemove += new EventListHandler(OnValuationImprovements_AfterRemove);					
				}
				return _ValuationImprovements;
			}
		}
		/// <summary>
		/// A SAHL Manual Valuation can only have one Cottage.
		/// </summary>
		public IValuationCottage ValuationCottage 
		{
			get
			{
				if (null == _DAO.ValuationCottage) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationCottage, ValuationCottage_DAO>(_DAO.ValuationCottage);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationCottage = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationCottage = (ValuationCottage_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ValuationOutBuildings = null;
			_ValuationImprovements = null;
			
		}
	}
}


