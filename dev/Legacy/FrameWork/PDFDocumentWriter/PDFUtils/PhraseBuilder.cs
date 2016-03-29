using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;

namespace PDFUtils.PDFWriting
{
    public class PhraseBuilder
    {
        List<Chunk> chunks = new List<Chunk>();
        Font f;

        public PhraseBuilder()
        {
            chunks.Clear();
            f = FontFactory.GetFont(FontFactory.TIMES, 10);
        }

        public Font DefaultFont
        {
            get { return f; }
            set { f = value; }
        }

        public void AddChunk(string Text, params object[] args)
        {
            chunks.Add(new Chunk(string.Format(Text, args), f));
        }

        public void AddChunk(string Text, Font f, params object[] args)
        {
            Chunk c = new Chunk(string.Format(Text, args));
            c.Font = f;
            chunks.Add(c);
        }

        public Phrase BuildPhrase()
        {
            Phrase p = new Phrase();
            p.AddRange(chunks);
            return p;
        }
    }
}
