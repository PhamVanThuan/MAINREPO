using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers;
using WebApi.Hal;

namespace SAHL.Services.Query.Coordinators
{

    public interface IPagingLinksCoordinator
    {
        List<Link> CreatePagingLinks(IFindQuery query, int count, Type type);
        bool HasPagingComponents(IFindQuery query);
    }

    public class PagingLinksCoordinator : IPagingLinksCoordinator
    {

        private ILinkResolver linkResolver;

        public PagingLinksCoordinator(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public List<Link> CreatePagingLinks(IFindQuery query, int count, Type type)
        {
            List<Link> links = new List<Link>();
            if (HasPagingComponents(query))
            {
                int pageSize = query.PagedPart.PageSize;
                int currentPage = query.PagedPart.CurrentPage;
                int numberOfPages =  CalculateNumberOfPages(query, count);

                //first link
                links.Add(new Link("first", linkResolver.GetHref(type) + CreatePagingJson(1, pageSize, query.FullFilterString)));
                
                //last link 
                links.Add(new Link("last", linkResolver.GetHref(type) + CreatePagingJson(numberOfPages, pageSize, query.FullFilterString))); 

                //next link
                if (currentPage < numberOfPages)
                {
                    links.Add(new Link("next", linkResolver.GetHref(type) + CreatePagingJson((currentPage + 1), pageSize, query.FullFilterString))); 
                }

                //previous link
                if (currentPage > 1)
                {
                    links.Add(new Link("previous", linkResolver.GetHref(type) + CreatePagingJson((currentPage - 1), pageSize, query.FullFilterString)));
                }

                //all page links 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    links.Add(new Link(i.ToString(), linkResolver.GetHref(type) + CreatePagingJson(i, pageSize, query.FullFilterString)));
                }
            }

            return links;
        }

        private int CalculateNumberOfPages(IFindQuery query, int count)
        {
            int numberOfPages = count / query.PagedPart.PageSize;
            if ((count - (numberOfPages * query.PagedPart.PageSize)) > 0)
            {
                numberOfPages++;
            }
            return numberOfPages;
        }

        private string CreatePagingJson(int currentPage, int pageSize, string filter)
        {
            return "?paging={Paging: {currentPage: " + currentPage + ", pageSize: " + pageSize + "}}" + "&filter=" + filter; 
        }

        public bool HasPagingComponents(IFindQuery query)
        {
            if (query.PagedPart != null)
            {
                return query.PagedPart.PageSize != 0;
            }
            return false;

        }

    }
}