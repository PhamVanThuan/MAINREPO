using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PDFUtils.PDFWriting
{
    public class PDFTableUtils
    {
        PdfPTable table;
        int _BorderWidth = 0;

        public int BorderWidth
        {
            get { return _BorderWidth; }
            set { _BorderWidth = value; }
        }

        public PDFTableUtils(int nCols)
        {
            table = new PdfPTable(nCols);
        }

        public PdfPTable Table
        {
            get
            {
                return table;
            }
        }

        public void AddCell(PdfPCell cell)
        {
            table.AddCell(cell);
        }

        public void AddCell(Phrase p)
        {
            AddCell(p, 1);
        }

        public void AddCell(Phrase p, int ColSpan)
        {
            PdfPCell cell = new PdfPCell(p);
            cell.Colspan = ColSpan;
            cell.BorderWidth = _BorderWidth;
            table.AddCell(cell);
        }

        public void AddCell(string Text, int ColSpan)
        {
            PhraseBuilder pb = new PhraseBuilder();
            pb.AddChunk(Text);
            PdfPCell cell = new PdfPCell(pb.BuildPhrase());
            cell.BorderWidth = _BorderWidth;
            cell.Colspan = ColSpan;
            table.AddCell(cell);
        }

        public void AddCell(string Text)
        {
            AddCell(Text, 1);
        }
    }
}
