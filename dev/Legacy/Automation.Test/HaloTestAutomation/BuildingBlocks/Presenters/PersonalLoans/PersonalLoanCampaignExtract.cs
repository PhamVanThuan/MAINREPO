using BuildingBlocks.Assertions;
using ObjectMaps.Presenters.PersonalLoans;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanCampaignExtract : PersonalLoanCampaignExtractControls
    {
        public void ClickImport()
        {
            base.ImportButton.Click();
        }

        public void ClickBrowse()
        {
            base.BrowseButton.Click();
        }

        public void CampaignExtractPageAssertions()
        {
            WatiNAssertions.AssertFieldsExist(new List<Element>() { BrowseButton, FileNameLabel });
        }

        public void UploadFile(string filePath)
        {
            base.BrowseButton.Set(filePath);
            base.ImportButton.Click();
        }
    }
}