using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Attorney;
using WebApi.Hal;

namespace SAHL.Services.Query.Coordinators
{

    public interface IPagingCoordinator
    {
        IListRepresentation ApplyPaging(IListRepresentation listRepresentation, IFindQuery query, Func<int> getCount, Type type, int queryRecordCount);
    }

    public class PagingCoordinator : IPagingCoordinator
    {

        private IPagingLinksCoordinator pagingLinksCoordinator;

        public PagingCoordinator(IPagingLinksCoordinator pagingLinksCoordinator)
        {
            this.pagingLinksCoordinator = pagingLinksCoordinator;
        }

        public IListRepresentation ApplyPaging(IListRepresentation listRepresentation, IFindQuery query, Func<int> getCount, Type type, int queryRecordCount)
        {

            SetListRepresentationHeader(listRepresentation, query, getCount, queryRecordCount);
                
            List<Link> links = pagingLinksCoordinator.CreatePagingLinks(query, listRepresentation.TotalCount.Value, type);

            if (links.Count <= 0)
            {
                return listRepresentation;
            }

            listRepresentation._paging = new PagingRepresentation()
            {
                Count = listRepresentation.TotalCount.Value,
                CurrentPage = query.PagedPart.CurrentPage,
                PageSize = query.PagedPart.PageSize
            };

            foreach (var link in links)
            {
                listRepresentation._paging._links.Add(link);
            }

            return listRepresentation;
        }

        private void SetListRepresentationHeader(IListRepresentation listRepresentation, IFindQuery query, Func<int> getCount, int queryRecordCount)
        {

            listRepresentation.TotalCount = getCount();
            
            if (IsLimitAppliedOnQuerySet(query))
            {
                listRepresentation.ListCount = listRepresentation.TotalCount;
            }
            else
            {
                listRepresentation.ListCount = listRepresentation.TotalCount;
                if (IsQueryResultNotLessThanLimit(listRepresentation, query))
                {
                    listRepresentation.ListCount = query.Limit.Count;
                    if (IsQueryResultLessThanLimitSetForQuery(query.Limit.Count, queryRecordCount))
                    {
                        listRepresentation.ListCount = queryRecordCount;
                    }
                }
            }

            if (IsPagingIncludedInQuery(query))
            {
                listRepresentation.ListCount = query.PagedPart.PageSize;
                if (IsRecordCountFromQueryLessThanThePageSize(query.PagedPart.PageSize, queryRecordCount))
                {
                    listRepresentation.ListCount = queryRecordCount;
                }
            }
        }

        private bool IsRecordCountFromQueryLessThanThePageSize(int queryPageSize, int queryRecordCount)
        {
            return queryRecordCount < queryPageSize;
        }

        private bool IsQueryResultLessThanLimitSetForQuery(int limitTotal, int queryRecordCount)
        {
            return queryRecordCount < limitTotal;
        }

        private bool IsPagingIncludedInQuery(IFindQuery query)
        {
            return query.PagedPart != null;
        }

        private bool IsQueryResultNotLessThanLimit(IListRepresentation listRepresentation, IFindQuery query)
        {
            return listRepresentation.TotalCount > query.Limit.Count;
        }

        private bool IsLimitAppliedOnQuerySet(IFindQuery query)
        {
            return query.Limit == null;
        }
    }
}