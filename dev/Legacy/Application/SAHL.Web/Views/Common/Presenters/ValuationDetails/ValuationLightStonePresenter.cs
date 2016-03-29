using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationLightStonePresenter : SAHLCommonBasePresenter<IValuationLightstoneView>
    {

        private IPropertyRepository propertyRepository;
        private DataTable dsValuations = new DataTable();
        private DataSet dsLightStone = new DataSet();
        private IValuation valuations;
        private int valuationkey;

        public ValuationLightStonePresenter(IValuationLightstoneView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // View.ddlMapProviderSelectedIndexChanged += ddlMapProviderSelectedIndexChanged;
            View.btnBackClicked += btnBack_Clicked;
            View.SetpnlOwners = true;

        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
            propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();

            valuationkey = Convert.ToInt32(GlobalCacheData["ValuationKey"]);
            valuations = propertyRepository.GetValuationByKey(valuationkey);
            //GlobalCacheData.Remove("ValuationKey");

            //if (!GlobalCacheData.ContainsKey("ValuationDS"))
            PopulateValuationsDS(valuations);
            // else
            //   dsValuations = GlobalCacheData["ValuationDS"] as DataTable;

            //  if (!PrivateCacheData.ContainsKey("LightstoneDS"))
            InitialiseXMLDataset(dsValuations);
            //else
            //     dsLightStone = PrivateCacheData["LightstoneDS"] as DataSet;

            SetupInterface();

        }


        void btnBack_Clicked(object sender, EventArgs e)
        {
            View.Navigator.Navigate("Back");
        }


        void SetupInterface()
        {

            if (dsLightStone != null)
            {
                // Parse the Dataset Data into the relevant Tables;
                DataTable ValuationDT = dsLightStone.Tables["Valuation"];
                //DataTable MapsDT = dsLightStone.Tables["Maps"];
                DataTable OwnersDT = dsLightStone.Tables["Owners"];
                DataTable CompSalesDT = dsLightStone.Tables["CompSales"];
                DataTable PropertyDT = dsLightStone.Tables["Property"];

                // Set all of the label values from the dataset
                DataRow valuationrow = ValuationDT.Rows[0];
                //DataRow maprow = MapsDT.Rows[0];
                DataRow propertyrow = PropertyDT.Rows[0];

                if (FieldDataExists(valuationrow,"TrackingNumber")) View.SetlblReferenceNumberValue = Convert.ToString(valuationrow["TrackingNumber"]).Trim();
                if (FieldDataExists(dsValuations.Rows[0], "ValuationDate")) View.SetlblAutomatedValueDateValue = dsValuations.Rows[0]["ValuationDate"].ToString();
                if (FieldDataExists(valuationrow, "PredictedPrice")) View.SetlblEstimatedMarketValue = Convert.ToDouble(valuationrow["PredictedPrice"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(valuationrow, "SafetyScore")) View.SetlblConfidenceScoreValue = Convert.ToDouble(valuationrow["SafetyScore"]).ToString(SAHL.Common.Constants.NumberFormat); // This is safety score in the DB - may be incorrect...
                if (FieldDataExists(valuationrow, "PredictedHigh")) View.SetlblExpectedHighValue = Convert.ToDouble(valuationrow["PredictedHigh"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(valuationrow, "PredictedLow")) View.SetlblExpectedLowValue = Convert.ToDouble(valuationrow["PredictedLow"]).ToString(SAHL.Common.Constants.CurrencyFormat);

                if (FieldDataExists(valuationrow, "PreviousSalesDate"))
                {
                    string DateString = Convert.ToString(valuationrow["PreviousSalesDate"]);
                    DateTime ReturnDate = ConvertStringToDate(DateString);
                    View.SetlblLastSaleDateValue = ReturnDate.ToString(SAHL.Common.Constants.DateFormat);
                }

                if (FieldDataExists(propertyrow, "Size")) View.SetlblERFSizeValue = Convert.ToString(propertyrow["Size"]).Trim() + " m2";
               
                if (FieldDataExists(valuationrow, "PreviousSalesPrice")) View.SetlblLastSalePriceValue = Convert.ToDouble(valuationrow["PreviousSalesPrice"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(valuationrow, "AQG")) View.SetlblAreaQualityGradeValue = Convert.ToString(valuationrow["AQG"]).Trim();
                if (FieldDataExists(valuationrow, "AE")) View.SetlblAreaExposureValue = Convert.ToString(valuationrow["AE"]).Trim();
                if (FieldDataExists(valuationrow, "Result")) View.SetlblAutomatedValuationDecisionValue = Convert.ToString(valuationrow["Result"]).Trim();

                // Bind the Grids
                View.BindgrdOwners(OwnersDT);
                View.BindgrdComparativeSales(CompSalesDT);

                //TODO there may be no URL Manage This
                // string MAPURLGOOGLE = "0";
                // string MAPURLMAPX = "0";
                // if (maprow[1].ToString().Length > 0)
                // {
                //     // Set the Map URL's
                //     MAPURLGOOGLE = maprow[1].ToString();
                //     MAPURLMAPX = maprow[2].ToString();
                // }

                // GlobalCacheData.Remove("MAPURLGOOGLE");
                // GlobalCacheData.Add("MAPURLGOOGLE", MAPURLGOOGLE, new List<ICacheObjectLifeTime>());
                // GlobalCacheData.Remove("MAPURLMAPX");
                // GlobalCacheData.Add("MAPURLMAPX", MAPURLMAPX, new List<ICacheObjectLifeTime>());

                //if (!PrivateCacheData.ContainsKey("LightstoneLoaded"))
                //    SetMapURL(MAPURLGOOGLE);

                PrivateCacheData.Remove("LightstoneLoaded");
                PrivateCacheData.Add("LightstoneLoaded", true);

            }
        }

        //static void SetMapURL(string MAPURL)
        //{
        //    // TODO if business require maps at a later stage
        //    //string url;
        //    //if (MAPURL == "0")
        //    //    url = "<b>No Image Available</b>";
        //    //else
        //    //    url = "<iframe id='MapiFrame' frameborder='0' width='100%' height='100%' scrolling='auto' src='" + MAPURL + "'></iframe>";

        //    //View.SetMapLiteral = url;
        //}

        // void ddlMapProviderSelectedIndexChanged(object sender, EventArgs e)
        // {
        //if (View.GetdllMapProviderIndex == 1)
        //    SetMapURL(Convert.ToString(GlobalCacheData["MAPURLGOOGLE"]));
        //if (View.GetdllMapProviderIndex == 2)
        //    SetMapURL(Convert.ToString(GlobalCacheData["MAPURLMAPX"]));

        //View.GetdllMapProviderIndex = View.GetdllMapProviderIndex;
        //}


        void PopulateValuationsDS(IValuation Valuations)
        {
            // Setup the Valuations Table 
            dsValuations.Reset();
            dsValuations.Columns.Add("Key");
            dsValuations.Columns.Add("DataServiceProviderKey");
            dsValuations.Columns.Add("Valuer");
            dsValuations.Columns.Add("ValuationDate");
            dsValuations.Columns.Add("ValuationAmount");
            dsValuations.Columns.Add("HOCValuation");
            dsValuations.Columns.Add("ValuationPurpose");
            dsValuations.Columns.Add("RequestedBy");
            dsValuations.Columns.Add("ValuationType");
            dsValuations.Columns.Add("XMLData");
            dsValuations.TableName = "Valuations";

            DataRow valRow = dsValuations.NewRow();
            valRow["Key"] = Valuations.Key;
            valRow["DataServiceProviderKey"] = Valuations.ValuationDataProviderDataService.DataProviderDataService.Key;
            valRow["Valuer"] = Valuations.Valuator.LegalEntity.DisplayName;
            valRow["ValuationDate"] = Convert.ToString(Valuations.ValuationDate.ToShortDateString());
            valRow["ValuationAmount"] = Convert.ToString(Valuations.ValuationAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
            valRow["HOCValuation"] = Convert.ToString(Valuations.ValuationHOCValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
            valRow["ValuationPurpose"] = ""; // offer.ApplicationType.Description;
            valRow["RequestedBy"] = Valuations.ValuationUserID;
            valRow["ValuationType"] = Valuations.ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description;
            valRow["XMLData"] = Valuations.Data;
            dsValuations.Rows.Add(valRow);

            GlobalCacheData.Remove("ValuationDS");
            GlobalCacheData.Add("ValuationDS", dsValuations, new List<ICacheObjectLifeTime>());

        }


        void InitialiseXMLDataset(DataTable DSValuations)
        {
            string XMLData = null;
            for (int i = 0; i < DSValuations.Rows.Count; i++)
                if (Convert.ToUInt32(DSValuations.Rows[i][0]) == valuationkey)
                {
                    XMLData = DSValuations.Rows[i][9].ToString();
                    break;
                }

            if (XMLData != null)
            {
                System.IO.StringReader TextReader = new System.IO.StringReader(XMLData);
                dsLightStone.ReadXml(TextReader, XmlReadMode.Auto);
            }

			dsLightStone.Tables["CompSales"].Columns.Add("FullAddress");
            try
            {
                // Add a Full Address row to the table for the Grid display

				for (int i = 0; i < dsLightStone.Tables["CompSales"].Rows.Count; i++)
                {
                    // string buildAddress = "";
                    StringBuilder buildAddress = new StringBuilder();
					if (dsLightStone.Tables["CompSales"].Columns.Contains("Unit"))
                    {
						if (FieldDataExists(dsLightStone.Tables["CompSales"].Rows[i], "Unit")) buildAddress.Append(dsLightStone.Tables["CompSales"].Rows[i]["Unit"]).Append(" ");
						if (FieldDataExists(dsLightStone.Tables["CompSales"].Rows[i], "Sectional_Title")) buildAddress.Append(dsLightStone.Tables["CompSales"].Rows[i]["Sectional_Title"]).Append(" ");
						if (FieldDataExists(dsLightStone.Tables["CompSales"].Rows[i], "Suburb")) buildAddress.Append(dsLightStone.Tables["CompSales"].Rows[i]["Suburb"]);
                    }
                    else
                    {
                        // Some of these fields may not exist in the XML
						if (FieldDataExists(dsLightStone.Tables["CompSales"].Rows[i], "STREET_NUMBER")) buildAddress.Append(dsLightStone.Tables["CompSales"].Rows[i]["STREET_NUMBER"]).Append(" ");
						if (FieldDataExists(dsLightStone.Tables["CompSales"].Rows[i], "STREET_NAME")) buildAddress.Append(dsLightStone.Tables["CompSales"].Rows[i]["STREET_NAME"]).Append(" ");
						if (FieldDataExists(dsLightStone.Tables["CompSales"].Rows[i], "STREET_TYPE")) buildAddress.Append(dsLightStone.Tables["CompSales"].Rows[i]["STREET_TYPE"]);
                    }
					dsLightStone.Tables["CompSales"].Rows[i]["FullAddress"] = buildAddress.ToString();
                }
            }
            catch (Exception ex)
            {
                View.Messages.Add(new Warning("Unable to create the Lightstone Dataset. Please contact your Administrator. Reason :" + ex.Message, "Unable to create the Lightstone Dataset. Please contact your Administrator. Reason :" + ex.Message));
            }

            PrivateCacheData.Remove("LightstoneDS");
            PrivateCacheData.Add("LightstoneDS", dsLightStone);
        }

        /// <summary>
        /// Converts the Lightstone date format to a DateTime value
        /// </summary>
        /// <param name="stringDate"></param>
        /// <returns></returns>
        private static DateTime ConvertStringToDate(string stringDate)
        {
            int year = Convert.ToInt32(stringDate.Substring(0, 4));
            int mm = Convert.ToInt32(stringDate.Substring(4, 2));
            int dd = Convert.ToInt32(stringDate.Substring(6, 2));

            return new DateTime(year, mm, dd);
        }

        /// <summary>
        /// Checks if a Field Exists in a dataset
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool FieldDataExists(DataRow dataRow, string fieldName)
        {
            try
            {
                if (dataRow[fieldName] == null) return false;
                return dataRow.GetColumnError(fieldName).Length <= 0;
            }
            catch (Exception)
            {
                // This will happen if the field does not exist...
                return false;
            }
           
        }
    }
}
