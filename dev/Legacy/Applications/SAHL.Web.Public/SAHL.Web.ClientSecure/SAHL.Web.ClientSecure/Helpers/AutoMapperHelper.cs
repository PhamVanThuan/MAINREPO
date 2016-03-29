using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.ClientSecure.Models;

namespace SAHL.Web.ClientSecure.Helpers
{
    public class AutoMapperHelper
    {
        public static void SetUp()
        {
            //AutoMapper.Mapper.CreateMap<AddNoteDetailViewModel, AuthenticationService.NoteDetail>();
            //AutoMapper.Mapper.CreateMap<NoteDetailViewModel, AuthenticationService.NoteDetail>();
            //AutoMapper.Mapper.CreateMap<AuthenticationService.NoteDetail, NoteDetailViewModel>();
            //AutoMapper.Mapper.CreateMap<AuthenticationService.DebtCounselling, SearchResultViewModel>();
            //AutoMapper.Mapper.CreateMap<AuthenticationService.LegalEntity, LegalEntityViewModel>();
            AutoMapper.Mapper.CreateMap<ClientSecureService.ReportParameter, ReportParamsViewModel>();
        }
    }
}