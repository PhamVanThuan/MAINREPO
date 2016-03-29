using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="ACBBranch_DAO"/> domain entity.
    /// </summary>
    public class ACBBranchHelper : BaseHelper<ACBBranch_DAO>
    {

        /// <summary>
        /// The primary key for ACBBranches is NOT auto-generated - provide a large key here 
        /// and increment it after each creation.
        /// </summary>
        private static int _branchKey = 42000;

        /// <summary>
        /// Creates a new <see cref="ACBBranch_DAO"/> object, that has not yet been persisted to the database.
        /// </summary>
        /// <param name="bank">The bank to which the branch belongs.</param>
        /// <returns></returns>
        public ACBBranch_DAO Create(ACBBank_DAO bank)
        {
            ACBBranch_DAO branch = new ACBBranch_DAO();
            branch.ACBBranchDescription = String.Format("Test Branch [{0}]", _branchKey.ToString());
            branch.ACBBank = bank;
            branch.ActiveIndicator = '0';
            branch.Key = _branchKey.ToString();

            CreatedEntities.Add(branch);
            _branchKey++;

            return branch;
        }

        /// <summary>
        /// Deletes all branches created using the helper.
        /// </summary>
        public override void Dispose()
        {
            foreach (ACBBranch_DAO branch in CreatedEntities)
            {
                TestBase.DeleteRecord("ACBBranch", "ACBBranchCode", branch.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
