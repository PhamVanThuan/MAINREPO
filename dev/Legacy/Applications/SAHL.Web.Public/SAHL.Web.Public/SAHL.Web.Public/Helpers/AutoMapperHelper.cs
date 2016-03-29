using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Public.Models;

namespace SAHL.Web.Public.Helpers
{
    public class AutoMapperHelper
    {
        public static void SetUp()
        {
            AutoMapper.Mapper.CreateMap<AddNoteDetailViewModel, AttorneyService.NoteDetail>();
            AutoMapper.Mapper.CreateMap<NoteDetailViewModel, AttorneyService.NoteDetail>();
            AutoMapper.Mapper.CreateMap<AttorneyService.NoteDetail, NoteDetailViewModel>();
            AutoMapper.Mapper.CreateMap<AttorneyService.DebtCounselling, SearchResultViewModel>();
            AutoMapper.Mapper.CreateMap<AttorneyService.LegalEntity, LegalEntityViewModel>();
			AutoMapper.Mapper.CreateMap<AttorneyService.ReportParameter, ReportParamsViewModel>();
            AutoMapper.Mapper.CreateMap<AttorneyService.Proposal, ProposalViewModel>();
        }
    }
}