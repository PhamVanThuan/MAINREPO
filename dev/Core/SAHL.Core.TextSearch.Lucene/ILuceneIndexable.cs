using Lucene.Net.Documents;

namespace SAHL.Core.TextSearch.Lucene
{
    public interface ILuceneIndexable
    {
        int DocumentId { get; set; }

        Document GetDocument();
    }
}