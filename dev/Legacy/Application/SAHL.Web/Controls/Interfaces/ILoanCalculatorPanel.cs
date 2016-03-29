namespace SAHL.Web.Controls.Interfaces
{
    /// <summary>
    /// Interface for the <see cref="LoanCalculatorPanel"/> control so we can easily mock the control in testing.
    /// </summary>
    public interface ILoanCalculatorPanel
    {
        string ConfirmedBy { get; set; }

    }
}
