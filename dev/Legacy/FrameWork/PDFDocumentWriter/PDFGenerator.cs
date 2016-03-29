//#define LOCAL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDFDocumentWriter.DataAccess;
using PDFDocumentWriter.Logging;
using PDFUtils.PDFWriting;
using SAHL.Common.DataAccess;

namespace PDFDocumentWriter
{
    public enum enReportStatementKey
    {
        LegalAgreement_Edge = 4004,
        LegalAgreement_InterestOnly = 4005,
        LegalAgreement_StandardVariable = 4006,
        LegalAgreement_VariFix = 4007,
        LegalAgreement_InterestOnly_OPTIN = 4008,
        LegalAgreement_Varifix_OPTIN_5year = 4009,
        LegalAgreement_Varifix_6month = 4011,
        LegalAgreement_Varifix_OPTIN_6month = 4012,
        PersonalLoanOffer = 7016,
        PersonalLoanLegalAgreements = 7017,
        PersonalLoanLegalAgreementsWithLifePolicy = 7021,
        PersonalLoanDisbursementLetter = 7019,
        PersonalLoanDisbursementLifeConditionsLetter = 7020
    }

    public class PDFGenerator
    {
        static Data.Lookup dsLookup = null;
        public static string TemplateBasePath = String.Empty;
        public static string OutputPath = String.Empty;

        static PDFGenerator()
        {
            try
            {
                LogSettingsClass lsl = new LogSettingsClass();

                LogPlugin.SeedLogSettings(lsl);
                LogPlugin.LogError("Logging Setup");
                lsl.AppName = string.Format("Document Engine");
                LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;
                if (section != null)
                {
                    lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
                    lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
                    lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
                    lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
                    lsl.FilePath = section.Logging["File"].path;
                }
                lsl.NumDaysToStore = 14;
                lsl.RollOverSizeInKB = 4096;
                LogPlugin.SeedLogSettings(lsl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Debug.WriteLine(ex.ToString());
            }
            dsLookup = new PDFDocumentWriter.Data.Lookup();
            DataAccess.DataAccess.GetLookupData(dsLookup);

            // use the control table values
            TemplateBasePath = dsLookup.Control.Rows[0]["ControlText"].ToString();
            //OutputPath = "c:\\temp\\";//
            OutputPath = dsLookup.Control.Rows[1]["ControlText"].ToString();
        }

        public string ReportName { get; set; }

        /// <summary>
        /// Generates a {LoanNumber}{OutName}.pdf for a given loannumber
        /// </summary>
        /// <param name="obj"></param>
        public string GenerateDocument(PDFGenerationObject obj)
        {
            string genericKey = obj.Params["AccountKey"].ToString();
            ReportName = string.Empty;

            List<string> errorMessages = new List<string>();

            // Check if an SPV movement needs to coccure before report can run
            switch (obj.ReportStatementKey)
            {
                case (int)enReportStatementKey.LegalAgreement_Edge:
                case (int)enReportStatementKey.LegalAgreement_InterestOnly:
                case (int)enReportStatementKey.LegalAgreement_StandardVariable:
                case (int)enReportStatementKey.LegalAgreement_VariFix:
                case (int)enReportStatementKey.LegalAgreement_Varifix_6month:
                case (int)enReportStatementKey.LegalAgreement_InterestOnly_OPTIN:
                case (int)enReportStatementKey.LegalAgreement_Varifix_OPTIN_5year:
                case (int)enReportStatementKey.LegalAgreement_Varifix_OPTIN_6month:
                    errorMessages = ValidateReportForSPVMovement(genericKey);
                    break;
                default:
                    break;
            }

            // if report is invalid then produce the error document and get outta here
            if (errorMessages.Count > 0)
            {
                return GenerateErrorDocument(Int32.Parse(genericKey), obj.ReportStatementKey, errorMessages);
            }

            // Check if there is a Life Policy and swap reportstatement to conditions report.
            CheckLifePolicyConditions(ref obj);

            // only do this for non-personal loan stuff
            if (obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanLegalAgreements
                && obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanDisbursementLifeConditionsLetter
                && obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanLegalAgreementsWithLifePolicy
                && obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanOffer
                && obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanDisbursementLetter)
            {
                // Check if its Varifix. Then work out whether its a 6month or 5 year one.
                CheckVarifixType(ref obj);

                // Check if its Interest Only and swap report statementkey accordingly.
                CheckInterestOnly(ref obj);

            }
            List<string> TempFilesToBeMerged = new List<string>();
            try
            {
                //goto TMP;

                LogPlugin.LogInfo("Getting ReportStatement");
                // get the lookup info from the dataset. (Rows that make up the template)
                DataRow[] arr = dsLookup.ReportStatementDocumentTemplate.Select(string.Format("ReportStatementKey={0}", obj.ReportStatementKey), "TemplateGenerationOrder ASC");

                // The reason I have done this in 2 passes is that when I populate the values in the template
                // when I save the doc it loses the setFields. If you call pdfStamper.Close() [There is no flush method] it
                // closes the underlying FileStream and you can no longer write to it.
                // So, 1: Generate Templates, 2: Stick em Together.

                #region Populate the fields in each template and save to temp folder


                string tempDirectory = Path.GetTempPath();

                for (int i = 0; i < arr.Length; i++)
                {
                    string Name = Path.Combine(tempDirectory, string.Format(@"{0}{1}.pdf", Thread.CurrentThread.GetHashCode(), i));

                    if (File.Exists(Name))
                    {
                        File.Delete(Name);
                    }
                    Data.Lookup.ReportStatementDocumentTemplateRow dr = (Data.Lookup.ReportStatementDocumentTemplateRow)arr[i];
                    ReportName = dr.ReportStatementRow.ReportName;
                    string TemplateName = string.Format("{0}{1}", TemplateBasePath, dr.DocumentTemplateRow.Path);
                    PdfReader reader = new PdfReader(TemplateName);
                    using (FileStream fs = new FileStream(Name, FileMode.Create))
                    {
                        // Only populate if there is a UI statement
                        string StatementName = string.Empty;
                        StatementName = dr.DocumentTemplateRow.StatementName;
                        PopulateFields(dr.DocumentTemplateRow.ApplicationName, StatementName, reader, fs, obj.Params);
                    }
                    TempFilesToBeMerged.Add(Name);
                    LogPlugin.LogInfo("Created Temp file:{0}", Name);
                }

                #endregion Populate the fields in each template and save to temp folder

                #region Do Signatures

            TMP:
                if (obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanOffer &&
                    obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanLegalAgreements &&
                    obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanLegalAgreementsWithLifePolicy &&
                    obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanDisbursementLifeConditionsLetter &&
                    obj.ReportStatementKey != (int)enReportStatementKey.PersonalLoanDisbursementLetter)
                {
                    LogPlugin.LogInfo("Beginning Sigs");
                    string SigName = Path.Combine(tempDirectory, string.Format(@"{0}Sigs.pdf", Thread.CurrentThread.GetHashCode()));
                    //this is particularely bad. We should have a particular report group for excluding Signatures
                    TempFilesToBeMerged.Add(SigName);
                    if (File.Exists(SigName))
                    {
                        File.Delete(SigName);
                    }
                    using (FileStream fs = new FileStream(SigName, FileMode.Create))
                    {
                        Document doc = new Document(PageSize.A4);
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                        doc.Open();
                        Data.DataSet dsRoles = GetSignatureData(obj.Params);
                        DoSignatures(doc, dsRoles);
                        doc.Close();
                    }
                    LogPlugin.LogInfo("Sigs Complete");
                }

                #endregion Do Signatures

                return MergeGenerateFiles(genericKey, ReportName, TempFilesToBeMerged);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                LogPlugin.LogError("Unable to generate document for Key:{0}{1}{2}", genericKey, Environment.NewLine, ex.ToString());
                throw;
            }
            finally
            {
                LogPlugin.LogInfo("Cleaning up Temp docs");
                // cleanup temp docs
                foreach (string Name in TempFilesToBeMerged)
                {
                    File.Delete(Name);
                }
            }
        }

        private string MergeGenerateFiles(string genericKey, string ReportName, List<string> TempFilesToBeMerged)
        {
            #region Merge all the temp docs together

            //LogPlugin.LogInfo("Merging TempDocs");
            //string OutName = string.Format(@"{0}\{1}{2}.pdf", OutputPath, ReportName, AccountKey);
            string OutName = string.Format(@"{0}{1}{2}.pdf", OutputPath, ReportName, genericKey);
            if (File.Exists(OutName))
            {
                File.Delete(OutName);
            }

            using (FileStream fs = new FileStream(OutName, FileMode.Create))
            {
                PdfReader reader = null;
                Document doc = null;
                PdfWriter writer = null;
                //int TotalPageCount = 0;
                int nPages = 0;
                PdfContentByte cb = null;

                HeaderFooter footerEvent = new HeaderFooter();

                foreach (string Name in TempFilesToBeMerged)
                {
                    reader = new PdfReader(Name);
                    nPages = reader.NumberOfPages;
                    if (null == doc)
                    {
                        doc = new Document(reader.GetPageSizeWithRotation(1));
                        writer = PdfWriter.GetInstance(doc, fs);
                        writer.SetBoxSize("footer", new iTextSharp.text.Rectangle(36, 54, 559, 788));
                        writer.PageEvent = footerEvent;
                        doc.Open();
                        cb = writer.DirectContent;
                    }
                    MergeInDoc(Name, nPages, doc, cb, reader, writer);
                    LogPlugin.LogInfo("Merged doc:{0}", Name);
                }
                doc.Close();
            }
            LogPlugin.LogInfo("Final Merge Complete");

            #endregion Merge all the temp docs together

            foreach (string Name in TempFilesToBeMerged)
            {
                File.Delete(Name);
            }
            return OutName;
        }

        public string GenerateDocument(List<PDFGenerationObject> pdfGenerationObjects)
        {
            List<string> docs = new List<string>();
            string uniqueKey = string.Format("batch_{0}", DateTime.Now.Ticks);
            try
            {
                foreach (var item in pdfGenerationObjects)
                {
                    docs.Add(GenerateDocument(item));
                }

                return MergeGenerateFiles(uniqueKey, ReportName, docs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                LogPlugin.LogError("Unable to generate batch document for Report:{0}{1}{2}", ReportName, Environment.NewLine, ex.ToString());
                throw;
            }
            finally
            {
                LogPlugin.LogInfo("Cleaning up Temp docs");
                // cleanup temp docs
                foreach (string Name in docs)
                {
                    if (File.Exists(Name))
                    {
                        File.Delete(Name);
                    }
                }
            }
        }

        private void MergeInDoc(string TemplateName, int nPages, Document doc, PdfContentByte cb,
           PdfReader reader, PdfWriter writer)
        {
            PdfImportedPage page;
            int CurrPage = 0, Rotation = 0;
            while (CurrPage < nPages)
            {
                //TotalPageCount++;
                //HeaderFooter footer = new HeaderFooter(new Phrase(string.Format("{0}", TotalPageCount)), false);
                //footer.PageNumber = TotalPageCount;
                //footer.Alignment = Element.ALIGN_CENTER;
                //doc.Footer = footer;
                // PDF reader is 1 based.
                CurrPage++;
                doc.SetPageSize(reader.GetPageSizeWithRotation(CurrPage));
                doc.NewPage();
                page = writer.GetImportedPage(reader, CurrPage);
                Rotation = reader.GetPageRotation(CurrPage);
                // our rotation should always be 0
                if (90 == Rotation || 270 == Rotation)
                {
                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, 0);//reader.GetPageSizeWithRotation(1).Height);
                }
                else
                {
                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                }
            }
        }

        private void AppendNaturalPeople(ref PDFTableUtils table, Data.DataSet ds)
        {
            int nApplicants = ds.Role.Rows.Count;
            for (int i = 0; i < nApplicants; i++)
            {
                Data.DataSet.LegalEntityRow row = (Data.DataSet.LegalEntityRow)ds.LegalEntity.Rows[i];
                AddLENPSignature(row, ref table);
            }
            //if (nApplicants == 1)
            //{
            //    Data.DataSet.LegalEntityRow row = (Data.DataSet.LegalEntityRow)ds.LegalEntity.Rows[0];
            //    AddLENPSignature(row, ref table);
            //}
            //else
            //{
            //    Data.DataSet.LegalEntityRow row = (Data.DataSet.LegalEntityRow)ds.LegalEntity.Rows[0];
            //    AddLENPSignature(row, ref table);
            //    row = (Data.DataSet.LegalEntityRow)ds.LegalEntity.Rows[1];
            //    AddLENPSignature(row, ref table);
            //}
        }

        private void AppendTrustCCCompany(ref PDFTableUtils table, Data.DataSet ds)
        {
            Data.DataSet.LegalEntityRow row = (Data.DataSet.LegalEntityRow)ds.LegalEntity.Rows[0];
            // Signed at ... on Row
            table.AddCell("Signed at:", 2);
            table.AddCell("on", 2);
            PhraseBuilder pb = new PhraseBuilder();
            pb.AddChunk("Witnesses:", FontFactory.GetFont(FontFactory.TIMES_BOLD, 10));
            table.AddCell(pb.BuildPhrase(), 4);
            table.AddCell("1.");
            table.AddCell("…………………………………….………");
            table.AddCell("");
            table.AddCell("…………………………………….………");
            table.AddCell("", 3);
            pb = new PhraseBuilder();
            pb.AddChunk("Signed on behalf of ");
            pb.AddChunk("{0}", FontFactory.GetFont(FontFactory.TIMES_BOLD, 10), row.RegisteredName);
            pb.AddChunk(" in its capacity as a Borrower, the signatory warranting his authority to sign");
            table.AddCell(pb.BuildPhrase());
            table.AddCell("2.");
            table.AddCell("…………………………………….………");
            table.AddCell("", 2);

            for (int i = 1; i < ds.LegalEntity.Rows.Count; i++)
            {
                row = (Data.DataSet.LegalEntityRow)ds.LegalEntity.Rows[i];
                AddLENPSignature(row, ref table);
            }
        }

        private void AddLENPSignature(Data.DataSet.LegalEntityRow row, ref PDFTableUtils table)
        {
            table.AddCell("Signed at:", 2);
            table.AddCell("on", 2);
            PhraseBuilder pb = new PhraseBuilder();
            pb.AddChunk("Witnesses:", FontFactory.GetFont(FontFactory.TIMES_BOLD, 10));
            table.AddCell(pb.BuildPhrase(), 4);
            table.AddCell("1.");
            table.AddCell("…………………………………….………");
            table.AddCell("");
            table.AddCell("…………………………………….………");
            table.AddCell("", 3);
            pb = new PhraseBuilder();
            pb.AddChunk("for ");
            pb.AddChunk("{0} {1}", FontFactory.GetFont(FontFactory.TIMES_BOLD, 10), row.FirstNames, row.Surname);
            pb.AddChunk(" (the Borrower), the signatory warranting his authority to sign");
            table.AddCell(pb.BuildPhrase());
            table.AddCell("2.");
            table.AddCell("…………………………………….………");
            table.AddCell("");
            pb = new PhraseBuilder();
            pb.AddChunk("Domicilium: ", FontFactory.GetFont(FontFactory.TIMES_BOLD, 10));
            pb.AddChunk("{0}", row.Domicilium);
            table.AddCell(pb.BuildPhrase());
        }

        private void DoSignatures(Document doc, Data.DataSet ds)
        {
            bool b = doc.NewPage();
            PDFTableUtils table = new PDFTableUtils(4);
            float[] headerwidths = { 5, 35, 5, 55 }; // percentage
            table.Table.SetWidths(headerwidths);
            table.Table.WidthPercentage = 100;
            table.Table.DefaultCell.BorderWidth = 0;
            table.Table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.Table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //table.AddCell("1 ");
            //table.AddCell("2 ");
            //table.AddCell("3 ");
            //table.AddCell("4 ");
            table.Table.HeaderRows = 0;// end of header

            // Signed at row
            table.AddCell("Signed at", 2);
            table.AddCell("on", 2);

            // Witnesses: Row
            PhraseBuilder pb = new PhraseBuilder();
            pb.AddChunk("Witnesses:", FontFactory.TIMES_BOLD);
            table.AddCell(pb.BuildPhrase(), 4);

            //// 1: ... Row
            table.AddCell("1:");
            table.AddCell("…………………………………….………");
            table.AddCell(" ");
            // add image here.
            byte[] byImg = new byte[0];
            using (Stream fsImg = Assembly.GetExecutingAssembly().GetManifestResourceStream("PDFDocumentWriter.Images.Signature.jpg"))
            {
                byImg = new byte[fsImg.Length];
                fsImg.Read(byImg, 0, (int)fsImg.Length);
            }
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byImg);
            img.ScalePercent(50, 50);

            PdfPCell cell = new PdfPCell(img);
            cell.Border = 0;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell.PaddingLeft = 60;
            table.Table.AddCell(cell);

            // blank with "for The Lender"
            table.AddCell(" ", 3);
            table.AddCell("…………………………………….………");

            // 2: ... Row
            table.AddCell("2:");
            table.AddCell("…………………………………….………");
            table.AddCell(" ");
            pb = new PhraseBuilder();
            pb.AddChunk("for ", FontFactory.GetFont(FontFactory.TIMES, 10));
            pb.AddChunk(string.Format("the Lender "), FontFactory.GetFont(FontFactory.TIMES_BOLD, 10));
            table.AddCell(pb.BuildPhrase(), 2);

            int LegalEntityTypeKey = Convert.ToInt32(ds.LegalEntity.Rows[0]["LegalEntityTypeKey"]);
            switch (LegalEntityTypeKey)
            {
                case 1:
                    {
                        // unknown
                        break;
                    }
                case 2:// LENP
                    {
                        AppendNaturalPeople(ref table, ds);
                        break;
                    }
                case 3:
                case 4:
                case 5:
                    {
                        // Company, Trust, CC
                        AppendTrustCCCompany(ref table, ds);
                        break;
                    }
            }

            doc.Add(table.Table);
        }

        private void PopulateFields(string applicationName, string statementName, PdfReader reader, FileStream fs, Dictionary<string, object> Params)
        {
            LogPlugin.LogInfo("PopulateFields (applicationName:{0},statementName:{1}", applicationName, statementName);

            PdfStamper pdfWriter = new PdfStamper(reader, fs);
            AcroFields fields = pdfWriter.AcroFields;
            PdfContentByte by = pdfWriter.GetOverContent(1);
            int AccountKey = Convert.ToInt32(Params["AccountKey"]);
            if (null != statementName)
            {
                // get the StatementKey for the required uiStatement
                int statementKey = -1;
                statementKey = DataAccess.DataAccess.GetStatementKey(applicationName, statementName, Properties.Settings.Default.RepositoryFind, DBMan.strConn);

                LogPlugin.LogInfo("uiStatementKey:{0}", statementKey);

                DataRow[] arr1 = dsLookup.uiStatement.Select(string.Format("StatementKey={0}", statementKey));
                Data.Lookup.uiStatementRow uiRow = (Data.Lookup.uiStatementRow)arr1[0];
                string Query = uiRow.Statement;

                System.Data.DataSet dsData = new DataSet();
                DataAccess.DataAccess.ExecuteUIStatement(dsData, Query, Params);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    LogPlugin.LogError("unable to get information for Account:{0}", AccountKey);
                    pdfWriter.FormFlattening = true;
                    pdfWriter.Close();
                    return;
                }
                DataTable dt = dsData.Tables[0];
                foreach (DataColumn dc in dt.Columns)
                {
                    string ColName = string.Empty;
                    try
                    {
                        ColName = dc.ColumnName;
                        fields.SetField(ColName, dt.Rows[0][ColName].ToString());
                    }
                    catch (Exception ex)
                    {
                        LogPlugin.LogWarning("{0}", ColName);
                    }
                }
                //InsertInitial(fields, pdfWriter);
            }
            pdfWriter.FormFlattening = true;
            pdfWriter.Close();
        }

        //private void InsertBarCode(AcroFields fields, PdfContentByte by, int AccountKey)
        //{
        //    string FileName = GenerateBarCode(AccountKey);
        //    float[] imageArea = fields.GetFieldPositions("BarCode");
        //    byte[] byImg = new byte[0];
        //    using (FileStream fsImg = File.OpenRead(FileName))
        //    {
        //        byImg = new byte[fsImg.Length];
        //        fsImg.Read(byImg, 0, (int)fsImg.Length);
        //    }
        //    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byImg);

        //    iTextSharp.text.Rectangle imageRect = new iTextSharp.text.Rectangle(imageArea[1], imageArea[2], imageArea[3], imageArea[4]);
        //    img.ScaleToFit(imageRect.Width, imageRect.Height);
        //    img.SetAbsolutePosition(imageArea[3] - img.ScaledWidth + (imageRect.Width - img.ScaledWidth) / 2, imageArea[2] + (imageRect.Height - img.ScaledHeight) / 2);
        //    by.AddImage(img);
        //}

        //private void InsertInitial(AcroFields fields, PdfStamper pdfWriter)
        //{
        //    string[] Keys = new string[fields.Fields.Keys.Count];
        //    fields.Fields.Keys.CopyTo(Keys, 0);
        //    foreach (object key in Keys)
        //    {
        //        if (key.ToString().StartsWith("SAHLInitial"))
        //        {
        //            string Key = key.ToString();
        //            string PageNo = Key.Substring(Key.Length - 2, 2);
        //            int PgNo = 1;
        //            int.TryParse(PageNo, out PgNo);
        //            PdfContentByte by = pdfWriter.GetOverContent(PgNo);
        //            float[] imageArea = fields.GetFieldPositions(Key);
        //            byte[] byImg = new byte[0];
        //            using (Stream fsImg = Assembly.GetExecutingAssembly().GetManifestResourceStream("PDFDocumentWriter.Images.Initials.jpg"))
        //            {
        //                byImg = new byte[fsImg.Length];
        //                fsImg.Read(byImg, 0, (int)fsImg.Length);
        //            }

        //            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byImg);

        //            iTextSharp.text.Rectangle imageRect = new iTextSharp.text.Rectangle(imageArea[1], imageArea[2], imageArea[3], imageArea[4]);
        //            img.ScaleToFit(imageRect.Width, imageRect.Height);
        //            img.SetAbsolutePosition(imageArea[3] - img.ScaledWidth + (imageRect.Width - img.ScaledWidth) / 2, imageArea[2] + (imageRect.Height - img.ScaledHeight) / 2);
        //            by.AddImage(img);
        //        }
        //    }
        //}

        private string GenerateBarCode(int AccountKey)
        {
            string FileName = string.Empty;
            Bitmap barCode = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(barCode);
            // Note this font needs to be copied from the Font folder to c:\windows\fonts
            using (System.Drawing.Font theeOfNine = new System.Drawing.Font("Free 3 of 9", 60, FontStyle.Regular, GraphicsUnit.Point))
            {
                SizeF dataSize = g.MeasureString(AccountKey.ToString(), theeOfNine);

                barCode = new Bitmap(barCode, dataSize.ToSize());
                g = Graphics.FromImage(barCode);
                g.Clear(System.Drawing.Color.White);

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                g.DrawString(AccountKey.ToString(), theeOfNine, new SolidBrush(System.Drawing.Color.Black), 0, 0);
                g.Flush();
            }
            g.Dispose();
            //barCode.Save(@".\Barcode.bmp");
            FileName = string.Format(@".\{0}Barcode.jpg", AccountKey);
            barCode.Save(FileName, ImageFormat.Jpeg);
            barCode.Dispose();
            return FileName;
        }

        private Data.DataSet GetSignatureData(Dictionary<string, object> Params)
        {
            int AccountKey = Convert.ToInt32(Params["AccountKey"]);
            Data.DataSet ds = new PDFDocumentWriter.Data.DataSet();

            string query = UIStatementRepository.GetStatement("Document Generation", "PDFGenerator_GetSignatureData");

            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add("@AccountKey", AccountKey);

            List<string> TableMappings = new List<string>();
            TableMappings.Add("Account");
            TableMappings.Add("LegalEntity");
            TableMappings.Add("Role");
            DBMan.FillMultiTable(ds, TableMappings, query, keys);

            return ds;
        }

        /// <summary>
        /// Returns a list of Fields in t PDFTemplate that can have values assigned to them
        /// </summary>
        /// <param name="Path">PAth to PDF on disk</param>
        /// <returns>List of Field Names</returns>
        public List<string> GetPDFKeys(string Path)
        {
            PdfReader pdf = new PdfReader(Path);
            var htFields = pdf.AcroFields.Fields;
            string[] Keys = new string[htFields.Keys.Count];
            htFields.Keys.CopyTo(Keys, 0);
            
            List<string> AllKeys = new List<string>();
            for (int i = 0; i < Keys.Length; i++)
            {
                AllKeys.Add(Keys[i].ToString());
            }
            return AllKeys;
        }

        private void CheckVarifixType(ref PDFGenerationObject obj)
        {
            if (obj.ReportStatementKey == (int)enReportStatementKey.LegalAgreement_VariFix) // 4007
            {
                /*
                 * Select fs.financialservicekey, t.TrancheTypeKey
from Financialservice fs
inner join Trade T on FS.TradeKey = T.TradeKey
where fs.FinancialserviceTypeKey = 2

Tranche Type
TrancheTypeKey = 2  (6 month reset)
TrancheTypeKey = 3  (5 year reset)

                 **/
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Select fs.financialservicekey, t.TrancheTypeKey ");
                sb.AppendLine("from Financialservice fs (nolock) ");
                sb.AppendLine("inner join Trade T (nolock) on FS.TradeKey = T.TradeKey ");
                sb.AppendFormat("where fs.FinancialserviceTypeKey = 2 and fs.AccountKey={0}", obj.Params["AccountKey"]);
                DataTable dt = new DataTable();
                DBMan.FillFromQuery(dt, sb.ToString(), null);
                int TranchTypeKey = Convert.ToInt32(dt.Rows[0]["TrancheTypeKey"]);
                if (TranchTypeKey == 2)
                    obj.ReportStatementKey = (int)enReportStatementKey.LegalAgreement_Varifix_6month; // 4011;
                else
                    obj.ReportStatementKey = (int)enReportStatementKey.LegalAgreement_VariFix; // 4007;
            }
        }

        private void CheckInterestOnly(ref PDFGenerationObject obj)
        {
            if (obj.ReportStatementKey == (int)enReportStatementKey.LegalAgreement_StandardVariable) // 4006
            {
                // check for interest only
                string parmDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select [2am].[dbo].[fIsLoanInterestOnlyActive] ({0},'{1}') as InterestOnlyActive", obj.Params["AccountKey"], parmDate);
                DataTable dt = new DataTable();
                DBMan.FillFromQuery(dt, sb.ToString(), null);

                bool interestOnly = Convert.ToBoolean(dt.Rows[0]["InterestOnlyActive"]);

                // if its interest only then switch report statement key
                if (interestOnly)
                    obj.ReportStatementKey = (int)enReportStatementKey.LegalAgreement_InterestOnly; // 4005;
            }
        }

        private void CheckLifePolicyConditions(ref PDFGenerationObject obj)
        {
            if (obj.ReportStatementKey == (int)enReportStatementKey.PersonalLoanDisbursementLetter) // 7019
            {
                // check for a life policy and include the conditions document template

                StringBuilder sb = new StringBuilder();

                //Will need to find way of looking up Life linked to PL

                sb.AppendFormat(@"select top 1 LP_FS.Accountkey from dbo.FinancialService LP_FS
                                    INNER JOIN dbo.FinancialService PL_FS on LP_FS.ParentFinancialServiceKey = PL_FS.FinancialServiceKey
                                    AND LP_FS.FinancialServiceTypekey = 11
                                    WHERE PL_FS.Accountkey = {0}", obj.Params["AccountKey"]);

                DataTable dt = new DataTable();
                DBMan.FillFromQuery(dt, sb.ToString(), null);

                if (dt.Rows.Count > 0)
                    obj.ReportStatementKey = (int)enReportStatementKey.PersonalLoanDisbursementLifeConditionsLetter; //7020
            }
            else if (obj.ReportStatementKey == (int)enReportStatementKey.PersonalLoanLegalAgreements) // 7017
            {
                // check for a life policy and include the conditions document template

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat(@"declare @OfferKey int
								  declare @LatestOfferInformationKey int

									-- Get the Latest Offer Information Key
									set @LatestOfferInformationKey = (
									select top 1
										OfferInformationKey
									from
											[2am].dbo.OfferInformation offerInformation
									join	[2am].dbo.Offer offer on offerInformation.OfferKey = offer.OfferKey and offer.ReservedAccountKey = {0}
									and		offerInformation.OfferInformationTypeKey = 3 -- Latest Accepted Offer Information
									order by OfferInsertDate desc)

									select (case when LifePremium > 0 then 1
											else 0
											end) as HasLifePremium from [2am].dbo.OfferInformationPersonalLoan where OfferInformationKey = @LatestOfferInformationKey", obj.Params["AccountKey"]);

                var hasLifePolicy = int.Parse(DBMan.ExecuteScalar(sb.ToString()).ToString());

                if (hasLifePolicy == 1)
                    obj.ReportStatementKey = (int)enReportStatementKey.PersonalLoanLegalAgreementsWithLifePolicy; //7021
            }
        }

        private List<string> ValidateReportForSPVMovement(string genericKey)
        {
            List<string> errorMessages = new List<string>();

            #region validate whether this loan needs to change SPV.

            int statementKey = -1;
            statementKey = DataAccess.DataAccess.GetStatementKey("Document Generation", "LegalAgreementDetermineCanPrint", Properties.Settings.Default.RepositoryFind, DBMan.strConn);

            DataRow[] arr1 = dsLookup.uiStatement.Select(string.Format("StatementKey={0}", statementKey));
            Data.Lookup.uiStatementRow uiRow = (Data.Lookup.uiStatementRow)arr1[0];
            string Query = uiRow.Statement;

            System.Data.DataSet dsData = new DataSet();
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("LoanNumber", genericKey);

            DataAccess.DataAccess.ExecuteUIStatement(dsData, Query, Params);

            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                if (dsData.Tables[0].Rows[0]["CanPrint"].ToString() == "0")
                {
                    errorMessages.Add("Loan number " + genericKey.ToString() + " must change spv before being signed");
                }
            }

            #endregion validate whether this loan needs to change SPV.

            return errorMessages;
        }

        private string GenerateErrorDocument(int genericKey, int reportStatementKey, List<string> errorMessages)
        {
            LogPlugin.LogInfo("Generate Error Document for Loan Number:{0}", genericKey);

            // get the reportstatment record so we can use the reportname in the documentname
            DataRow[] arr1 = dsLookup.ReportStatement.Select(string.Format("ReportStatementKey={0}", reportStatementKey));
            Data.Lookup.ReportStatementRow reportStatementRow = (Data.Lookup.ReportStatementRow)arr1[0];
            string reportName = reportStatementRow.ReportName;

            // define the name for the error document
            string errorDocName = string.Format(@"{0}{1}{2}.pdf", OutputPath, "Error_" + reportName, genericKey);

            // remove if already exists
            if (File.Exists(errorDocName))
                File.Delete(errorDocName);

            // create new error document
            using (FileStream fs = new FileStream(errorDocName, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // add a page
                doc.NewPage();

                //setup the table for the document
                PdfPTable table = new PdfPTable(1);
                float[] headerwidths = { 100 }; // percentage
                table.SetWidths(headerwidths);
                table.WidthPercentage = 100;
                table.DefaultCell.BorderWidth = 0;
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.HeaderRows = 0;

                PdfPCell cell; Phrase phrase;

                // insert cell for the document header row 2
                phrase = new Phrase(reportName, FontFactory.GetFont(FontFactory.TIMES, 15, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
                cell = new PdfPCell(phrase);
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Border = 0;
                table.AddCell(cell);

                // insert blank row
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES, 50)));
                cell.Border = 0;
                table.AddCell(cell);

                // insert cell for the document header row 1
                phrase = new Phrase("Document Errors", FontFactory.GetFont(FontFactory.TIMES, 22, iTextSharp.text.Font.NORMAL, BaseColor.RED));
                cell = new PdfPCell(phrase);
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell.BorderWidth = 0.5f;
                table.AddCell(cell);

                // insert blank row
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES, 50)));
                cell.Border = 0;
                table.AddCell(cell);

                // loop thru each error message and write to document
                int i = 1;
                foreach (string errorMessage in errorMessages)
                {
                    table.AddCell(i + ". " + errorMessage);
                    i++;
                }

                // add the table to the document
                doc.Add(table);

                // close the document
                doc.Close();
            }

            return errorDocName;
        }
    }


    class HeaderFooter : PdfPageEventHelper {
        
        int pagenumber;
 
       
        public override void OnStartPage(PdfWriter writer, Document document) {
            pagenumber++;
        }
 
        public override void OnEndPage(PdfWriter writer, Document document) {
            //iTextSharp.text.Rectangle rect = writer.GetBoxSize("footer");
            
            //ColumnText.ShowTextAligned(writer.DirectContent,
            //        Element.ALIGN_CENTER, new Phrase(String.Format("page {0}", pagenumber)),
            //        (rect.Left + rect.Right) / 2, rect.Bottom - 18, 0);
        }
    }
}