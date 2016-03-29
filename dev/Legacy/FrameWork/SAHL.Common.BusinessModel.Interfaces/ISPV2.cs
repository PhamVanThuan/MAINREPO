namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface ISPV
    {
        bool AllowTermChange { get; }

        bool AllowFurtherLending { get; }
    }
}