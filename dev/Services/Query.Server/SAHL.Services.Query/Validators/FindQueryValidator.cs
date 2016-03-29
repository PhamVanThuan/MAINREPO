using System.Collections;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers;

namespace SAHL.Services.Query.Validators
{
    public class FindQueryValidator : IFindQueryValidator
    {
        public void IsValid(IFindQuery findQuery)
        {
            IsValidPagingRequest(findQuery);   
        }

        private void IsValidPagingRequest(IFindQuery findQuery)
        {
            if (IsPagedPartSet(findQuery))
            {
                if (IsOrderBySet(findQuery))
                {
                    AddError(findQuery, "Paging request requires an order by clause.");
                }

                if (findQuery.PagedPart.CurrentPage < 1)
                {
                    AddError(findQuery, "Page size can't be 0 or negative.");
                }

            }
        }

        private void AddError(IFindQuery findQuery, string error)
        {
            if (findQuery.Errors != null)
            {
                findQuery.Errors.Add(error);    
            }
        }

        private bool IsOrderBySet(IFindQuery findQuery)
        {
            if (findQuery.OrderBy != null)
            {
                return findQuery.OrderBy.Count == 0;    
            }
            return false;
        }

        private bool IsPagedPartSet(IFindQuery findQuery)
        {
            return findQuery.PagedPart != null;
        }
    }

    public interface IFindQueryValidator
    {
        void IsValid(IFindQuery findQuery);
    }

}