using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Services.Internal.DataModel;
using SAHL.Common.BusinessModel.Interfaces;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Services.Internal.DataModel
{
    public class AutoMapperHelper
    {
        public static void SetUp()
        {
            // Create custom Mapping
            AutoMapper.Mapper.CreateMap<SAHL.Common.BusinessModel.Interfaces.INoteDetail, SAHL.Web.Services.Internal.DataModel.NoteDetail>().ConvertUsing<NoteDetailConverter>();
            AutoMapper.Mapper.CreateMap<SAHL.Common.BusinessModel.Interfaces.IDebtCounselling, SAHL.Web.Services.Internal.DataModel.DebtCounselling>().ConvertUsing<DebtCounsellingConverter>();
            AutoMapper.Mapper.CreateMap<SAHL.Common.BusinessModel.Interfaces.IRole, SAHL.Web.Services.Internal.DataModel.LegalEntity>().ConvertUsing<RoleToLegalEntityConverter>();
            AutoMapper.Mapper.CreateMap<SAHL.Common.BusinessModel.Interfaces.ILegalEntity, SAHL.Web.Services.Internal.DataModel.LegalEntity>().ConvertUsing<LegalEntityToLegalEntityConverter>();
            AutoMapper.Mapper.CreateMap<SAHL.Common.BusinessModel.Interfaces.IReportParameter, SAHL.Web.Services.Internal.DataModel.ReportParameter>().ConvertUsing<ReportParameterConverter>();
            AutoMapper.Mapper.CreateMap<SAHL.Common.BusinessModel.Interfaces.IProposal, SAHL.Web.Services.Internal.DataModel.Proposal>().ConvertUsing<ProposalConverter>();
        }
    }

    /// <summary>
    /// Note Detail Converter
    /// </summary>
    public class NoteDetailConverter : AutoMapper.TypeConverter<SAHL.Common.BusinessModel.Interfaces.INoteDetail, SAHL.Web.Services.Internal.DataModel.NoteDetail>
    {
        protected override NoteDetail ConvertCore(Common.BusinessModel.Interfaces.INoteDetail source)
        {
            SAHL.Web.Services.Internal.DataModel.NoteDetail nd = new SAHL.Web.Services.Internal.DataModel.NoteDetail();
            nd.LegalEntityKey = source.LegalEntity.Key;
            nd.LegalEntityDisplayName = source.LegalEntity.DisplayName;
            nd.InsertedDate = source.InsertedDate;
            nd.Key = source.Key;
            nd.NoteText = source.NoteText;
            nd.WorkflowState = source.WorkflowState;
            nd.Tag = source.Tag;
            return nd;
        }
    }

    /// <summary>
    /// Report Parameter Converter
    /// </summary>
    public class ReportParameterConverter : AutoMapper.TypeConverter<SAHL.Common.BusinessModel.Interfaces.IReportParameter, SAHL.Web.Services.Internal.DataModel.ReportParameter>
    {
        protected override ReportParameter ConvertCore(Common.BusinessModel.Interfaces.IReportParameter source)
        {
            SAHL.Web.Services.Internal.DataModel.ReportParameter rp = new SAHL.Web.Services.Internal.DataModel.ReportParameter();
            rp.ReportParameterName = source.ParameterName;
            rp.ParameterTypeKey = source.ReportParameterType.Key;
            return rp;
        }
    }


    /// <summary>
    /// Debt Counselling Resolver
    /// </summary>
    public class DebtCounsellingConverter : AutoMapper.TypeConverter<SAHL.Common.BusinessModel.Interfaces.IDebtCounselling, SAHL.Web.Services.Internal.DataModel.DebtCounselling>
    {
        /// <summary>
        /// Resolve Core
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override DebtCounselling ConvertCore(Common.BusinessModel.Interfaces.IDebtCounselling source)
        {
            DebtCounselling debtCounselling = new DebtCounselling
            {
                DiaryDate = source.DiaryDate,
                AccountKey = source.Account.Key,
                DebtCounsellingKey = source.Key,
                LegalEntitiesOnAccount = AutoMapper.Mapper.Map<IList<ILegalEntity>, List<LegalEntity>>(source.Clients)
            };
            return debtCounselling;
        }
    }

    /// <summary>
    /// IRole to Legal Entity Converter
    /// </summary>
    public class RoleToLegalEntityConverter : AutoMapper.TypeConverter<SAHL.Common.BusinessModel.Interfaces.IRole, SAHL.Web.Services.Internal.DataModel.LegalEntity>
    {
        /// <summary>
        /// Resolve Core
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override LegalEntity ConvertCore(Common.BusinessModel.Interfaces.IRole source)
        {
            LegalEntity legalEntity = new LegalEntity
            {
                DisplayName = source.LegalEntity.DisplayName,
                IDNumber = source.LegalEntity is ILegalEntityNaturalPerson ? ((ILegalEntityNaturalPerson)source.LegalEntity).IDNumber : String.Empty
            };
            return legalEntity;
        }
    }

    /// <summary>
    /// ILegalEntity to Legal Entity Converter
    /// </summary>
    public class LegalEntityToLegalEntityConverter : AutoMapper.TypeConverter<SAHL.Common.BusinessModel.Interfaces.ILegalEntity, SAHL.Web.Services.Internal.DataModel.LegalEntity>
    {
        /// <summary>
        /// Resolve Core
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override LegalEntity ConvertCore(Common.BusinessModel.Interfaces.ILegalEntity source)
        {
            LegalEntity legalEntity = new LegalEntity
            {
                DisplayName = source.DisplayName,
                IDNumber = source is ILegalEntityNaturalPerson ? ((ILegalEntityNaturalPerson)source).IDNumber : String.Empty
            };
            return legalEntity;
        }
    }

    ///
    /// <summary>
    /// Proposal Converter
    /// </summary>
    public class ProposalConverter : AutoMapper.TypeConverter<SAHL.Common.BusinessModel.Interfaces.IProposal, SAHL.Web.Services.Internal.DataModel.Proposal>
    {
        /// <summary>
        /// Resolve Core
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override Proposal ConvertCore(Common.BusinessModel.Interfaces.IProposal source)
        {
            Proposal proposal = new Proposal
            {
                Accepted = source.Accepted,
                CreateDate = source.CreateDate,
                DebtCounsellingKey = source.DebtCounselling.Key,
                HOCInclusive = source.HOCInclusive,
                LegalEntityDisplayName = source.ADUser.LegalEntity != null ? source.ADUser.LegalEntity.DisplayName : "System",
                LifeInclusive = source.LifeInclusive,
                ProposalKey = source.Key,
                ProposalStatus = source.ProposalStatus.Description,
                ProposalType = source.ProposalType.Description,
                ReviewDate = source.ReviewDate
            };
            return proposal;
        }
    }
}