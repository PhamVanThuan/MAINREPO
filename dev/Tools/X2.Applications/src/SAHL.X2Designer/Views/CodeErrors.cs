using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SAHL.X2Designer.CodeGen;
using WeifenLuo.WinFormsUI;

namespace SAHL.X2Designer.Views
{
    public partial class CodeErrors : DockContent
    {
        CompilerErrorCollection m_Errors;
        List<Line> m_Lines;

        public CodeErrors()
        {
            InitializeComponent();
            m_Lines = new List<Line>();
        }

        public void ShowErrors(CompilerErrorCollection Errors)
        {
            m_Errors = new CompilerErrorCollection(Errors);
            ClearErrors();
            SplitErrors();
            RefreshErrors();
        }

        public void ClearErrors()
        {
            m_Lines.Clear();
        }

        private void RefreshErrors()
        {
            listViewErrors.Items.Clear();
            for (int i = 0; i < m_Errors.Count; i++)
            {
                ErrorSource ErrorSource = GetErrorSource(m_Errors[i]);
                if (ErrorSource != null)
                {
                    string ErrorSourceString = GetErrorSourceString(ErrorSource.Cp);

                    if (!m_Errors[i].IsWarning)
                    {
                        if (toolStripButtonErrors.Checked == true)
                        {
                            listViewErrors.Items.Add(new ListViewItem(new string[] { "", listViewErrors.Items.Count.ToString(), ErrorSourceString, m_Errors[i].ErrorText, m_Errors[i].Line.ToString(), m_Errors[i].Column.ToString() }));
                            listViewErrors.Items[listViewErrors.Items.Count - 1].Tag = ErrorSource;
                        }
                    }
                    else
                    {
                        if (m_Errors[i].IsWarning)
                        {
                            if (toolStripButtonErrors.Checked == true)
                            {
                                listViewErrors.Items.Add(new ListViewItem(new string[] { "", listViewErrors.Items.Count.ToString(), ErrorSourceString, m_Errors[i].ErrorText, m_Errors[i].Line.ToString(), m_Errors[i].Column.ToString() }));
                                listViewErrors.Items[listViewErrors.Items.Count - 1].Tag = ErrorSource;
                            }
                        }
                    }
                }
                else if (m_Errors[i].ErrorNumber != "CS0162")
                {
                    listViewErrors.Items.Add(new ListViewItem(new string[] { "", listViewErrors.Items.Count.ToString(), null, m_Errors[i].ErrorText, m_Errors[i].Line.ToString(), m_Errors[i].Column.ToString() }));
                }
            }
        }

        private void toolStripButtonErrors_Click(object sender, EventArgs e)
        {
            MainForm.App.BuildCode(false);
            MainForm.App.setStatusBar("Ready");

            RefreshErrors();
        }

        private void toolStripButtonWarnings_Click(object sender, EventArgs e)
        {
            RefreshErrors();
        }

        private void SplitErrors()
        {
            StringReader Sr = new StringReader(X2Generator.CurrentCode);
            string LineStr = Sr.ReadLine();
            int CharCount = 0;
            while (LineStr != null)
            {
                Line L = new Line();
                L.Start = CharCount;
                L.End = CharCount + LineStr.Length + 1;
                L.Data = LineStr;
                CharCount += LineStr.Length;
                m_Lines.Add(L);
                LineStr = Sr.ReadLine();
            }
        }

        private ErrorSource GetErrorSource(CompilerError Error)
        {
            // first get the code depth based on line number
            Line L = null;
            if (Error.Line < m_Lines.Count && Error.Line > 0)
                L = m_Lines[Error.Line - 1];

            if (L != null)
            {
                L.Start += (2 * (Error.Line - 1));
                L.End += (2 * (Error.Line - 1));
                for (int i = 0; i < X2Generator.CodeSections.Count; i++)
                {
                    string CodeSectionData = X2Generator.CodeSections[i].Node.GetCodeSectionData(X2Generator.CodeSections[i].CodeSection);

                    if (X2Generator.CodeSections[i].SectionStart <= L.Start && X2Generator.CodeSections[i].SectionEnd >= L.End)
                    {
                        if (X2Generator.CodeSections[i].Node != null)
                        {
                            int Offset = L.Start - X2Generator.CodeSections[i].SectionStart;

                            string Data = X2Generator.CurrentCode.Substring(X2Generator.CodeSections[i].SectionStart, Offset);
                            int LineCnt = Data.Split(new char[] { '\r', '\n' }).Length;

                            if (Offset > LineCnt)
                                Offset -= LineCnt;
                            return new ErrorSource(X2Generator.CodeSections[i], Offset);
                        }
                    }
                }
            }

            return null;
        }

        private string GetErrorSourceString(CodePosition ErrorSource)
        {
            if (ErrorSource != null && ErrorSource.Node != null)
            {
                return ErrorSource.Node.Text + " - " + ErrorSource.CodeSection;
            }

            return "";
        }

        private void listViewErrors_DoubleClick(object sender, EventArgs e)
        {
            if (listViewErrors.SelectedItems.Count == 1)
            {
                ErrorSource Source = listViewErrors.SelectedItems[0].Tag as ErrorSource;

                // set focus to the item that caused the error and set its codesection
                MainForm.App.GetCurrentView().Selection.Clear();
                if (Source != null)
                {
                    MainForm.App.GetCurrentView().Selection.Add(Source.Cp.Node);
                }
                MainForm.App.ShowCodeViewWindow(Source, true);
            }
        }
    }

    public class Line
    {
        public int Start;
        public int End;
        public string Data;
    }

    public class ErrorSource
    {
        public CodePosition Cp;
        public int Offset;

        public ErrorSource(CodePosition pCp, int pOffset)
        {
            Cp = pCp;
            Offset = pOffset;
        }
    }
}