using System;
using System.Xml;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.RCS.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using SAHL.Common.Security;
using SAHL.Common.Utils;
using System.IO;

namespace SAHL.Web.Views.RCS.Presenters
{
    /// <summary>
    /// Presenter for RCSUpload 
    /// </summary>
    public class RCSUpload : SAHLCommonBasePresenter<IRCSUpload>
    {
		private bool fileUploaded;
		private bool replacementNeeded;
		private List<string> missing = new List<string>();
		private List<string> extra = new List<string>();
		private Dictionary<string, List<string>> invalid = new Dictionary<string, List<string>>();
		private IXsdAbstraction xsdAbs;
		private DataTable DT;
		private int fileKey;

        public RCSUpload(IRCSUpload view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.UploadClick += new EventHandler(UploadClick);
            _view.ReplaceClick += new EventHandler(ReplaceClick);
            _view.CancelClick += new EventHandler(CancelClick);
            _view.SubmitClick += new EventHandler(SubmitClick);
			_view.ResultsClick += new EventHandler(ResultsClick);

            _view.ResultsButtonEnabled = false;
            _view.ReplaceButtonEnabled = false;

            if (PrivateCacheData.ContainsKey("fileUploaded"))
            {
                fileUploaded = Convert.ToBoolean(PrivateCacheData["fileUploaded"]);
            }

            if (fileUploaded == true)
            {
                _view.UploadButtonEnabled = false;
                _view.FileNameEnabled = false;
                _view.FileNameValEnabled = false;

                if (PrivateCacheData.ContainsKey("DT"))
                {
                    DT = PrivateCacheData["DT"] as DataTable;
                }

                if (PrivateCacheData.ContainsKey("xsdAbs"))
                {
                    xsdAbs = PrivateCacheData["xsdAbs"] as IXsdAbstraction;
                }

                BindReplacePanel();

                _view.SubmitButtonEnabled = true;

                if (PrivateCacheData.ContainsKey("FileNameDisplayText"))
                {
                    _view.FileNameDisplayText = PrivateCacheData["FileNameDisplayText"] as string;
                }
                _view.FileNameDisplayVisible = true;
                _view.FileNameVisible = false;
            }
            else
            {
                _view.SubmitButtonEnabled = false;
                _view.FileNameDisplayVisible = false;
                _view.FileNameVisible = true;
            }

            if (PrivateCacheData.ContainsKey("replacementNeeded"))
            {
                replacementNeeded = Convert.ToBoolean(PrivateCacheData["replacementNeeded"]);
            }

            if (replacementNeeded == true)
            {
                _view.ReplacementTableVisible = true;
                _view.SubmitButtonEnabled = false;
                _view.ReplaceButtonEnabled = true;
            }
            else
            {
                _view.ReplacementTableVisible = false;
            }

            IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
            if (importFileRepo != null)
            {
                IReadOnlyEventList<IImportFile> importFiles = importFileRepo.GetImportHistory();
                if (importFiles != null)
                {
                    _view.BindUploadHistoryGrid(importFiles);
                }
                else
                {
                    // TODO throw an exception GetImportHistory returned nothing
                }
            }
            else
            {
                // TODO throw an exception GetRepository failed
            }
        }

		protected void CancelClick(object sender, EventArgs e)
        {
			try
			{
				string uploadPath = "";

				PrivateCacheData.Clear();

				_view.ResultsButtonEnabled = false;
				_view.ReplaceButtonEnabled = false;
				_view.SubmitButtonEnabled = false;
				_view.FileNameDisplayVisible = false;
				_view.FileNameEnabled = true;
				_view.FileNameVisible = true;
				_view.ReplacementTableVisible = false;
				_view.UploadButtonEnabled = true;
				_view.FileNameDisplayText = "";

				IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
				if (importFileRepo != null)
				{
					uploadPath = importFileRepo.GetRCSUploadExportPath();
					if (uploadPath.Length > 0)
					{
						if (importFileRepo.ClearRCSUploadExportPath(uploadPath))
						{
							_view.Navigator.Navigate("Cancel");
						}
						else
						{
							// TODO throw an exception ClearRCSUploadExportPath failed
						}
					}
					else
					{
						// TODO throw an exception RCSUploadExportPath directory is ""
					}
				}
				else
				{
					// TODO throw an exception GetRepository failed
				}
			}
			catch
			{

			}
        }

		protected void UploadClick(object sender, EventArgs e)
        {
			try
			{
				string uploadPath = "", uploadFile = _view.UploadFileFileName;

				if (uploadFile.Length > 0)
				{
					IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
					if (importFileRepo != null)
					{
						uploadPath = importFileRepo.GetRCSUploadExportPath();
						if (uploadPath.Length > 0)
						{
							if (importFileRepo.ClearRCSUploadExportPath(uploadPath))
							{
								if (_view.SaveUploadFile(uploadPath + uploadFile))
								{
									importFileRepo.LoadRCS_CSVFile(uploadPath, uploadFile, ref DT, ref xsdAbs, ref missing, ref extra, ref invalid);

									if (PrivateCacheData.ContainsKey("DT"))
										PrivateCacheData.Remove("DT");
									PrivateCacheData.Add("DT", DT);
									if (PrivateCacheData.ContainsKey("xsdAbs"))
										PrivateCacheData.Remove("xsdAbs");
									PrivateCacheData.Add("xsdAbs", xsdAbs);
									if (PrivateCacheData.ContainsKey("fileUploaded"))
										PrivateCacheData.Remove("fileUploaded");
									PrivateCacheData.Add("fileUploaded", true);
									if (invalid == null || invalid.Count == 0)
									{
										if (PrivateCacheData.ContainsKey("replacementNeeded"))
											PrivateCacheData.Remove("replacementNeeded");
										PrivateCacheData.Add("replacementNeeded", false);
										_view.SubmitButtonEnabled = true;
										_view.ReplaceButtonEnabled = false;
									}
									else
									{
										if (PrivateCacheData.ContainsKey("replacementNeeded"))
											PrivateCacheData.Remove("replacementNeeded");
										PrivateCacheData.Add("replacementNeeded", true);
										if (PrivateCacheData.ContainsKey("invalid"))
											PrivateCacheData.Remove("invalid");
										PrivateCacheData.Add("invalid", invalid);
										_view.SubmitButtonEnabled = false;
										_view.ReplaceButtonEnabled = true;
									}
									if (PrivateCacheData.ContainsKey("uploadFile"))
										PrivateCacheData.Remove("uploadFile");
									PrivateCacheData.Add("uploadFile", uploadFile);

									_view.UploadButtonEnabled = false;
									_view.FileNameVisible = false;
									_view.FileNameEnabled = false;
									_view.FileNameValEnabled = false;
									_view.FileNameDisplayText = uploadFile;
									_view.FileNameDisplayVisible = true;

									BindReplacePanel();
									_view.ReplacementTableVisible = true;
								}
								else
								{
									// TODO throw an exception SaveUploadFile failed
								}
							}
							else
							{
								// TODO throw an exception ClearRCSUploadExportPath failed
							}
						}
						else
						{
							// TODO throw an exception RCSUploadExportPath directory is ""
						}
					}
					else
					{
						// TODO throw an exception GetRepository failed
					}
				}
				else
				{
					_view.FileNameValEnabled = true;
					_view.FileNameValIsValid = false;
					_view.SubmitButtonEnabled = false;
				}
			}
			catch
			{

			}
        }

		protected void ReplaceClick(object sender, EventArgs e)
        {
			try
			{
				if (PrivateCacheData.ContainsKey("DT"))
				{
					DT = PrivateCacheData["DT"] as DataTable;
				}

				if (PrivateCacheData.ContainsKey("xsdAbs"))
				{
					xsdAbs = PrivateCacheData["xsdAbs"] as IXsdAbstraction;
				}

				if (xsdAbs == null || DT == null)
					return;

				if (PrivateCacheData.ContainsKey("invalid"))
				{
					invalid = PrivateCacheData["invalid"] as Dictionary<string, List<string>>;
				}

				List<string> xcolumns = new List<string>();
				List<string> xbad = new List<string>();
				List<string> xreplace = new List<string>();

				//add the new replacements
				foreach (string key in invalid.Keys)
				{
					List<string> values = invalid[key];

					foreach (string s in values)
					{
						string column = Convert.ToString(key);
						string bad = Convert.ToString(s);
						string controlName = "ctl00$Main$" + column + "_" + s;
						string replace = _view.GetReplaceValues(controlName);
						if (replace.Length == 0)
							return;

						bool found = false;

						for (int k = 0; k < xcolumns.Count; k++)
						{
							if (xcolumns[k] == column && xbad[k] == bad)
							{
								xreplace[k] = replace;
								found = true;
								break;
							}
						}

						if (!found)
						{
							xcolumns.Add(column);
							xbad.Add(bad);
							xreplace.Add(replace);
						}
					}
				}

				//write the data replacements out and replace the data
				for (int i = 0; i < xcolumns.Count; i++)
				{
					if (xreplace[i].Length != 0 && xreplace[i] != xbad[i])
					{
						if (!DT.Columns.Contains(xcolumns[i]))
							DT.Columns.Add(xcolumns[i]);

						foreach (DataRow dRow in DT.Rows)
						{
							object obj = dRow[xcolumns[i]];

							if (obj != null && obj.ToString() == xbad[i])
							{
								dRow[xcolumns[i]] = xreplace[i];
							}
						}
					}
				}

				IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
				if (importFileRepo != null)
				{
					string xmlFileName = importFileRepo.GenerateXML(DT, ref missing, ref extra, ref invalid);

					if (xmlFileName.Length > 0)
					{
						string uploadFile = "";
						IImportFile importFile = importFileRepo.CreateEmptyImportFile();

						if (PrivateCacheData.ContainsKey("uploadFile"))
						{
							uploadFile = Convert.ToString(PrivateCacheData["uploadFile"]);
						}

						if (uploadFile.Length > 0)
						{
							StreamReader XML_Data = IOUtils.GetStreamFromFile(xmlFileName);
							if (!(XML_Data.EndOfStream))
							{
								importFile.XmlData = XML_Data.ReadToEnd();
								XML_Data.Close();
							}
							importFile.FileName = uploadFile;
							importFile.FileType = "Actual";
							importFile.Status = "Imported";
							importFile.DateImported = System.DateTime.Now;
							importFile.UserID = SAHLPrincipal.GetCurrent().Identity.Name;
							fileKey = importFileRepo.ImportDataFromXML(xmlFileName, importFile);
							if (fileKey > 0)
							{
								if (PrivateCacheData.ContainsKey("replacementNeeded"))
									PrivateCacheData.Remove("replacementNeeded");
								PrivateCacheData.Add("replacementNeeded", false);
								if (PrivateCacheData.ContainsKey("fileKey"))
									PrivateCacheData.Remove("fileKey");
								PrivateCacheData.Add("fileKey", fileKey);
								_view.ReplaceButtonEnabled = false;
								_view.SubmitButtonEnabled = false;
								_view.RegisterClientScripts("alert('Import Completed !');");
								_view.ResultsButtonEnabled = true;
							}
							else
							{
								//throw new Exception("Error executing stored RCS Import Procedure");
								_view.ReplaceButtonEnabled = false;
								_view.SubmitButtonEnabled = false;
								_view.RegisterClientScripts("alert('Import Failed!');");
								_view.ResultsButtonEnabled = false;
							}
						}
						else
						{
							//could not get upload file name
							_view.ReplaceButtonEnabled = false;
							_view.SubmitButtonEnabled = false;
							_view.RegisterClientScripts("alert('Import Failed!');");
							_view.ResultsButtonEnabled = false;
						}
					}
					else
					{
						//GenerateXML failed
						_view.ReplaceButtonEnabled = false;
						_view.SubmitButtonEnabled = false;
						_view.RegisterClientScripts("alert('Import Failed!');");
						_view.ResultsButtonEnabled = false;
					}
				}
			}
			catch
			{

			}
			finally
			{
				string uploadPath = "";
				IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
				if (importFileRepo != null)
				{
					uploadPath = importFileRepo.GetRCSUploadExportPath();
					if (uploadPath.Length > 0)
					{
						if (!importFileRepo.ClearRCSUploadExportPath(uploadPath))
						{
							// TODO throw an exception ClearRCSUploadExportPath failed
						}
					}
					else
					{
						// TODO throw an exception RCSUploadExportPath directory is ""
					}

					IReadOnlyEventList<IImportFile> importFiles = importFileRepo.GetImportHistory();
					if (importFiles != null)
					{
						_view.BindUploadHistoryGrid(importFiles);
					}
					else
					{
						// TODO throw an exception GetImportHistory returned nothing
					}
				}
				else
				{
					// TODO throw an exception GetRepository failed
				}
			}
        }

		protected void SubmitClick(object sender, EventArgs e)
        {
			try
			{
				if (PrivateCacheData.ContainsKey("invalid"))
				{
					invalid = PrivateCacheData["invalid"] as Dictionary<string, List<string>>;
				}

				if (invalid == null || invalid.Count == 0)
				{
					if (PrivateCacheData.ContainsKey("replacementNeeded"))
						PrivateCacheData.Remove("replacementNeeded");
					PrivateCacheData.Add("replacementNeeded", false);
					IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
					if (importFileRepo != null)
					{
						string xmlFileName = importFileRepo.GenerateXML(DT, ref missing, ref extra, ref invalid);

						if (xmlFileName.Length > 0)
						{
							IImportFile importFile = importFileRepo.CreateEmptyImportFile();

							importFile.FileName = _view.FileNameDisplayText; ;
							importFile.FileType = "Actual";
							importFile.Status = "Imported Stage1";
							importFile.DateImported = System.DateTime.Now;
							importFile.UserID = SAHLPrincipal.GetCurrent().Identity.Name;

							StreamReader XML_Data = IOUtils.GetStreamFromFile(xmlFileName);

							if (!(XML_Data.EndOfStream))
							{
								importFile.XmlData = XML_Data.ReadToEnd();
								XML_Data.Close();
							}
							fileKey = importFileRepo.ImportDataFromXML(xmlFileName, importFile);
							if (fileKey > 0)
							{
								if (PrivateCacheData.ContainsKey("fileKey"))
									PrivateCacheData.Remove("fileKey");
								PrivateCacheData.Add("fileKey", fileKey);
								_view.ReplaceButtonEnabled = false;
								_view.SubmitButtonEnabled = false;
								_view.RegisterClientScripts("alert('Import Completed!');");
								_view.ResultsButtonEnabled = true;
							}
							else
							{
								//throw new Exception("Error executing stored RCS Import Procedure");
								_view.ReplaceButtonEnabled = false;
								_view.SubmitButtonEnabled = false;
								_view.RegisterClientScripts("alert('Import Failed!');");
								_view.ResultsButtonEnabled = false;
							}
							//}
							//else
							//{
							//    throw new Exception("Error reading xml file");
							//}
						}
						else
						{
							//GenerateXML failed
						}
					}
				}
				else
				{
					_view.ReplacementTableVisible = true;
					_view.ReplaceButtonEnabled = true;
					_view.SubmitButtonEnabled = false;
					if (PrivateCacheData.ContainsKey("replacementNeeded"))
						PrivateCacheData.Remove("replacementNeeded");
					PrivateCacheData.Add("replacementNeeded", true);
					_view.RegisterClientScripts("alert('Invalid Data Found. Please replace before continuing');");
				}
			}
			catch
			{

			}
			finally
			{
				string uploadPath = "";
				IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
				if (importFileRepo != null)
				{
					uploadPath = importFileRepo.GetRCSUploadExportPath();
					if (uploadPath.Length > 0)
					{
						if (!importFileRepo.ClearRCSUploadExportPath(uploadPath))
						{
							// TODO throw an exception ClearRCSUploadExportPath failed
						}
					}
					else
					{
						// TODO throw an exception RCSUploadExportPath directory is ""
					}

					IReadOnlyEventList<IImportFile> importFiles = importFileRepo.GetImportHistory();
					if (importFiles != null)
					{
						_view.BindUploadHistoryGrid(importFiles);
					}
					else
					{
						// TODO throw an exception GetImportHistory returned nothing
					}
				}
				else
				{
					// TODO throw an exception GetRepository failed
				}
			}
        }

		protected void ResultsClick(object sender, EventArgs e)
        {
			try
			{
				if (PrivateCacheData.ContainsKey("fileKey"))
				{
					fileKey = Convert.ToInt32(PrivateCacheData["fileKey"]);
				}

				if (fileKey > 0)
				{
					IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
					if (importFileRepo != null)
					{
						IReadOnlyEventList<IImportLegalEntity> importLegalEntity = importFileRepo.GetImportResultsByFileKey(fileKey);
						if (importLegalEntity != null)
						{
							_view.ViewResults(importLegalEntity);
						}
						else
						{
							_view.RegisterClientScripts("alert('No Results Found !');");
						}
					}
				}
				else
				{
					_view.RegisterClientScripts("alert('Could not retrieve file key !');");
				}
			}
			catch
			{
				
			}
        }

        private void BindReplacePanel()
        {
			try
			{
				TableRow tr;
				TableCell tc;

				if (invalid == null || invalid.Count == 0)
				{
					tr = new TableRow();
					tc = new TableCell();

					tc.Text = "No Replacements need to be made";
					tr.Cells.Add(tc);
					_view.ReplaceListRowsAdd = tr;
				}
				else
				{
					tr = new TableRow();
					tr.CssClass = "TableHeaderB";
					tr.Style.Add(HtmlTextWriterStyle.Height, "25px");
					tc = new TableCell();
					tc.Style.Add(HtmlTextWriterStyle.Width, "25%");
					SAHLLabel invalidColHead = new SAHLLabel();
					invalidColHead.Font.Bold = true;
					invalidColHead.Text = "Column Name";
					tc.Controls.Add(invalidColHead);
					tr.Cells.Add(tc);

					tc = new TableCell();
					tc.Style.Add(HtmlTextWriterStyle.Width, "25%");
					SAHLLabel invalidValueHead = new SAHLLabel();
					invalidValueHead.Font.Bold = true;
					invalidValueHead.Text = "Invalid Value";
					tc.Controls.Add(invalidValueHead);
					tr.Cells.Add(tc);

					tc = new TableCell();
					tc.Style.Add(HtmlTextWriterStyle.Width, "50%");
					SAHLLabel replaceValueHead = new SAHLLabel();
					replaceValueHead.Font.Bold = true;
					replaceValueHead.Text = "Replacement Value";
					tc.Controls.Add(replaceValueHead);
					tr.Cells.Add(tc);
					_view.ReplaceListRowsAdd = tr;

					int rowCount = 0;
					foreach (string key in invalid.Keys)
					{

						List<string> values = invalid[key];

						foreach (string s in values)
						{
							tr = new TableRow();
							if ((rowCount % 2) == 0)
								tr.CssClass = "TableRowA";
							else
								tr.CssClass = "TableRowA2";
							rowCount++;

							tc = new TableCell();
							tc.Style.Add(HtmlTextWriterStyle.Width, "25%");
							SAHLLabel invalidCol = new SAHLLabel();
							invalidCol.Text = key;
							tc.Controls.Add(invalidCol);
							tr.Cells.Add(tc);

							tc = new TableCell();
							tc.Style.Add(HtmlTextWriterStyle.Width, "25%");
							SAHLLabel invalidValue = new SAHLLabel();
							invalidValue.Text = s;
							tc.Controls.Add(invalidValue);
							tr.Cells.Add(tc);

							tc = new TableCell();
							tc.Style.Add(HtmlTextWriterStyle.Width, "50%");
							SAHLDropDownList replaceValues = new SAHLDropDownList();
							replaceValues.Style.Add(HtmlTextWriterStyle.Width, "95%");
							//Bind Controls *****
							replaceValues.ID = key + "_" + s;
							BindDropDownControl(replaceValues, key, xsdAbs);
							tc.Controls.Add(replaceValues);
							tr.Cells.Add(tc);
							_view.ReplaceListRowsAdd = tr;
						}
					}
				}
			}
			catch
			{

			}
        }

		private void BindDropDownControl(SAHLDropDownList dropDownControl, string invalidName, IXsdAbstraction xsdAbs)
		{
			try
			{
				string InvalidType = invalidName.Substring(0, invalidName.Length - 3);
				IImportFileRepository importFileRepo = RepositoryFactory.GetRepository<IImportFileRepository>();
				if (importFileRepo != null)
				{
					ISimpleNode node = importFileRepo.GetSimpleNode(InvalidType, xsdAbs);
					if (node != null && node.Name == InvalidType)
					{
						if (node.Enumerations != null && node.Enumerations.Count > 0)
						{
							foreach (string s in node.Enumerations)
							{
								_view.DropControlItemsAdd(dropDownControl, s);
							}
						}
					}
				}
			}
			catch
			{

			}
		}
    }
}