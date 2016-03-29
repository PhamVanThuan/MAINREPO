using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class PaymentReviewUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dtePaymentReceivedDate")]
        public TextField txtPaymentReceived;

        [FindBy(Id = "ctl00_Main_txtPaymentReceivedAmount_txtRands")]
        public TextField txtPaymentReceivedAmountRands;

        [FindBy(Id = "ctl00_Main_txtPaymentReceivedAmount_txtCents")]
        public TextField txtPaymentReceivedAmountCents;

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        public Button btnUpdate;

        [FindBy(Id = "ctl00_Main_dteTermReviewDate")]
        public TextField txtTermReviewDate;
    }
}