namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IApplicationProduct
    {
        IApplication Application { get; }

        void DisposeProduct();

        SAHL.Common.Globals.Products ProductType { get; }
    }
}