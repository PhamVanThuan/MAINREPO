namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This will call the termchange stored procedure to create testcases
        /// </summary>
        /// <returns></returns>
        public void CreateTermChangeTestCases()
        {
            SQLStoredProcedure Procedure = new SQLStoredProcedure { Name = "test.CreateTermChangeTestCases" };
            dataContext.ExecuteStoredProcedure(Procedure);
        }
    }
}