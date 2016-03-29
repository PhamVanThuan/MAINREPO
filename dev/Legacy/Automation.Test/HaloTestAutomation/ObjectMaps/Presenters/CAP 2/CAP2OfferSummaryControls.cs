using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CAP2OfferSummaryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button SubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNTUReason")]
        protected SelectList NTUReason { get; set; }

        [FindBy(Id = "ctl00_Main_PromotionClient")]
        protected CheckBox Promotion { get; set; }

        [FindBy(Id = "ctl00_Main_ddlPaymentOption")]
        protected SelectList PaymentOption { get; set; }

        [FindBy(Id = "ctl00_valSummary_Body")]
        protected Div divValidationSummaryBody { get; set; }

        protected ElementCollection listErrorMessages
        {
            get
            {
                Div validationSummaryBody = divValidationSummaryBody;
                return validationSummaryBody.ElementsWithTag("LI");
            }
        }

        protected TableRow gridCAPOptions(string CapType)
        {
            TableCellCollection CapOptions = base.Document.Table("ctl00_Main_CapOfferGrid").TableCells;
            return CapOptions.Filter(Find.ByText(CapType))[0].ContainingTableRow;
        }

        protected TableRow CapOptionOne()
        {
            return base.Document.TableRow("ctl00$Main$CapOfferGrid_0");
        }

        protected TableRow CapOptionTwo()
        {
            return base.Document.TableRow("ctl00$Main$CapOfferGrid_1");
        }

        protected TableRow CapOptionThree()
        {
            return base.Document.TableRow("ctl00$Main$CapOfferGrid_2");
        }

        protected bool gridCAPOptionExists(string CapType)
        {
            return base.Document.Table("ctl00_Main_CapOfferGrid").TableCells.Exists(Find.ByText(CapType));
        }

        [FindBy(Value = "Add Offer")]
        protected Button AddOffer { get; set; }
    }
}