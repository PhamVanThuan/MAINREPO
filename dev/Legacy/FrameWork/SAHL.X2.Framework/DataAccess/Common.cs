using System;
using System.Data;

namespace SAHL.X2.Framework.DataAccess
{
    public partial class Common
    {
        /// <summary>
        /// Copies the data in the source datarow to the target datarow.  The state of the rows are not cloned.
        /// </summary>
        /// <remarks>An Exception is thrown if source and target are not of the same type.</remarks>
        /// <param name="p_Source">The source of the data.</param>
        /// <param name="p_Target">The target of the data.</param>
        public static void CloneDataRow(DataRow p_Source, DataRow p_Target)
        {
            if (p_Source.GetType() != p_Target.GetType())
                throw new Exceptions.DataAccessException("Incompatible data types, cannot clone row");
            for (int i = 0; i < p_Target.ItemArray.Length; i++)
            {
                p_Target[i] = p_Source[i];
                //p_Target.ItemArray[i] = p_Source.ItemArray[i];
            }
        }

        /// <summary>
        /// Compares the values the data in the two rows.  They have to be the same type.
        /// The state of the rows are NOT considered in the comparison.  Only value types of
        /// Integer, float, String, datetime and Sbyte are evaluated.
        /// </summary>
        /// <param name="A">An Instance of System.Data.Datarow or derived type.</param>
        /// <param name="B">An Instance of the same type as A.</param>
        /// <returns>true if the values in A and B are equal, false otherwise.</returns>
        public static bool CompareDataRow(DataRow A, DataRow B)
        {
            if (A.GetType() != B.GetType())
                throw new Exceptions.DataAccessException("Incompatible data types, cannot compare row");
            if (A.ItemArray.Length != B.ItemArray.Length)
                return false;

            bool retval = true;
            for (int i = 0; i < A.ItemArray.Length; i++)
            {
                switch (A[i].GetType().ToString())
                {
                    case ("System.String"):
                        if (Convert.ToString(A[i]) != Convert.ToString(B[i]))
                            retval = false;
                        break;
                    case ("System.Decimal"):
                    case ("System.Single"):
                    case ("System.Double"):
                        if (Convert.ToDecimal(A[i]) != Convert.ToDecimal(B[i]))
                            retval = false;
                        break;
                    case ("System.Int16"):
                    case ("System.Int32"):
                    case ("System.Int64"):
                    case ("System.UInt16"):
                    case ("System.UInt32"):
                    case ("System.UInt64"):
                        if (Convert.ToInt64(A[i]) != Convert.ToInt64(B[i]))
                            retval = false;
                        break;
                    case ("System.DateTime"):
                        DateTime ADT = Convert.ToDateTime(A[i]);
                        DateTime BDT = Convert.ToDateTime(B[i]);
                        if (ADT.CompareTo(BDT) != 0)
                            retval = false;
                        break;
                    case ("System.SByte"):
                        if (Convert.ToSByte(A[i]) != Convert.ToSByte(B[i]))
                            retval = false;
                        break;
                    default:
                        throw new Exception(String.Format("Unsupported type {0}, cannot compare values", A[i].GetType().ToString()));
                }
                //if (A[i] != B[i])
                //    retval = false;
            }
            return retval;
        }
    }
}