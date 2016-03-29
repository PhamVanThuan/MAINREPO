//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.DataAccess.Workers
{
    public partial class CommonWorker
    {
        /// <summary>
        /// Fills the table with the results of the ControlGet query in the database, given
        /// all the parameters.
        /// </summary>
        /// <param name="p_Context">A context with a valid data connection.</param>
        /// <param name="p_Control">Table to be populated</param>
        public static void GetControlInfo(ITransactionContext p_Context, ControlConstants p_Control)
        {
            try
            {
                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Fill it.
                WorkerHelper.Fill(p_Control.Control, "RCS", "ControlGet", p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the row of data from the Control table where ControlDescription = search value
        /// </summary>
        /// <param name="p_control"></param>
        /// <param name="p_searchDesc"></param>
        public static ControlConstants.ControlRow GetControlValue(ControlConstants p_control, string p_searchDesc)
        {
            DataRow[] m_Rows = p_control.Control.Select(" ControlDescription = '" + p_searchDesc + "'");
            if (m_Rows.Length > 0)
                return (ControlConstants.ControlRow)m_Rows[0];
            else
                return null;
        }
    }
}