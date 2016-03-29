using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="ACBBank_DAO"/> domain entity.
    /// </summary>
    public class ACBBankHelper : BaseHelper<ACBBank_DAO>
    {

        /// <summary>
        /// The primary key for ACBBanks is NOT auto-generated - provide a large key here 
        /// and increment it after each creation.
        /// </summary>
        private static int _bankKey = 4200;

        /// <summary>
        /// Creates a new <see cref="ACBBank_DAO"/> entity.
        /// </summary>
        /// <returns>A new ACBBank_DAO entity (not yet persisted).</returns>
        public ACBBank_DAO Create()
        {
            ACBBank_DAO bank = new ACBBank_DAO();
            bank.ACBBankDescription = String.Format("Test Bank [{0}]", _bankKey.ToString());
            bank.Key = _bankKey;

            CreatedEntities.Add(bank);
            _bankKey++;

            return bank;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (ACBBank_DAO bank in CreatedEntities)
            {
                TestBase.DeleteRecord("ACBBank", "ACBBankCode", bank.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
