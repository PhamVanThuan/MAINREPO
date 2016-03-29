using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO
    /// </summary>
    public partial class AllocationMandateOperator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO>, IAllocationMandateOperator
    {
        public AllocationMandateOperator(SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO AllocationMandateOperator)
            : base(AllocationMandateOperator)
        {
            this._DAO = AllocationMandateOperator;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.Order
        /// </summary>
        public Int32 Order
        {
            get { return _DAO.Order; }
            set { _DAO.Order = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.AllocationOperator
        /// </summary>
        public IOperator AllocationOperator
        {
            get
            {
                if (null == _DAO.AllocationOperator) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOperator, Operator_DAO>(_DAO.AllocationOperator);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AllocationOperator = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AllocationOperator = (Operator_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.AllocationMandateSet
        /// </summary>
        public IAllocationMandateSet AllocationMandateSet
        {
            get
            {
                if (null == _DAO.AllocationMandateSet) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAllocationMandateSet, AllocationMandateSet_DAO>(_DAO.AllocationMandateSet);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AllocationMandateSet = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AllocationMandateSet = (AllocationMandateSet_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.AllocationMandate
        /// </summary>
        public IAllocationMandate AllocationMandate
        {
            get
            {
                if (null == _DAO.AllocationMandate) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAllocationMandate, AllocationMandate_DAO>(_DAO.AllocationMandate);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AllocationMandate = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AllocationMandate = (AllocationMandate_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}