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
	/// SAHL.Common.BusinessModel.DAO.Note_DAO
	/// </summary>
	public partial class Note : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Note_DAO>, INote
	{
		
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.NoteDetails
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnNoteDetails_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.NoteDetails
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnNoteDetails_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.NoteDetails
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnNoteDetails_AfterAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.NoteDetails
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnNoteDetails_AfterRemove(ICancelDomainArgs args, object Item)
		{

		}
	}
}


