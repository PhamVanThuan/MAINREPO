using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using SAHL.Common.Service.Interfaces.DataSets;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel.Activation;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using System.Web;
using System.Security.Principal;
using SAHL.Common.Security;
using SAHL.Common.CacheData;
using System.Threading;
using System.Xml.Schema;
using System.IO;
using SAHL.Common.Globals;
using System.Reflection;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Logging;


namespace SAHL.Web.Services.Internal
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Valuation : ValuationBase, IValuation
	{
		public DataSet SubmitCompletedValuationLightstone(int uniqueID, string xmlData)
		{
			return InternalProcessForWorkflow("SubmitCompletedValuationLightstone", "EXTAssesmentCompleteLS", uniqueID, xmlData, ValuationStatuses.Complete);
		}

		public DataSet SubmitRejectedValuationLightstone(int uniqueID, string xmlData)
		{
			return InternalProcessForWorkflow("SubmitRejectedValuationLightstone", "EXTAssesmentRejectedLS", uniqueID, xmlData, ValuationStatuses.Returned);
		}

		public DataSet SubmitAmendedValuationLightstone(int UniqueID, string XMLData)
		{
			return InternalProcessForWorkflow("SubmitAmendedValuationLightstone", "EXTAssesmentAmendedLS", UniqueID, XMLData, ValuationStatuses.Complete);
		}

		private DataSet InternalProcessForWorkflow(string methodName, string x2Flag, int xmlHistoryID, string incomingLightstoneValuationXML, ValuationStatuses valuationStatusToSet)
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add("Method", methodName);
			parameters.Add("UniqueID", xmlHistoryID);
			LogPlugin.Logger.LogInfoMessage(methodName, String.Format("Method: {0}, UniqueID={1}", methodName, xmlHistoryID), parameters);
			string errorMessage = null;
			if (xmlHistoryID < 1)
			{
				errorMessage = "UniqueID parameter must be greater than 0";
				LogPlugin.Logger.LogErrorMessage(methodName, String.Format("The UniqueID parameter is invalid: {0}", xmlHistoryID), parameters);
				throw new Exception(errorMessage);
			}

			if (String.IsNullOrEmpty(incomingLightstoneValuationXML))
			{
				errorMessage = "XMLData parameter may not be null or empty";
				LogPlugin.Logger.LogErrorMessage(methodName, String.Format("The XMLData parameter is null or empty"), parameters);
				throw new Exception(errorMessage);
			}

			string principal = String.Format("{0}_{1}", xmlHistoryID, DateTime.Now.ToFileTime());
			SetupPrincipal(principal);
			SPC.DomainMessages.Clear();

			var incomingPhysicalValuation = new PhysicalValuation();
			SAHL.Common.BusinessModel.Interfaces.IValuation valuation = null;
			IXMLHistory xmlHistory = null;

			var lightstoneValuationResultDataSet = GetLightstoneValuationResultData(xmlHistoryID);

			TransactionScope transactionScope = null;
			using (transactionScope = new TransactionScope(OnDispose.Rollback))
			{
				try
				{
					var stringReader = new System.IO.StringReader(incomingLightstoneValuationXML);
					incomingPhysicalValuation.ReadXml(stringReader, XmlReadMode.Auto);

					xmlHistory = propertyRepository.GetXMLHistoryByKey(xmlHistoryID);

					if (xmlHistory == null)
					{
						errorMessage = "UniqueID not found in SAHL database";
						LogPlugin.Logger.LogErrorMessage(methodName, String.Format("The PropertyRepository.GetXMLHistoryByKey method failed to get an XMLHistory record with the given XMLHistoryKey (UniqueID): {0}", xmlHistoryID), parameters);
						throw new Exception();
					}

					//Amendment
					var xmlDocument = XDocument.Parse(xmlHistory.XmlData);
					XElement previousXmlHistoryID = xmlDocument.Root.Descendants().Where(x => x.Name.LocalName == "PreviousUniqueID").FirstOrDefault();

					//if PreviousUniqueID exists and has a value, it's an amendment, so regular complete is not allowed
					if (previousXmlHistoryID != null && !String.IsNullOrEmpty(previousXmlHistoryID.Value))
					{
						if (methodName == "SubmitCompletedValuationLightstone")
						{
							errorMessage = "SubmitCompletedValuationLightstone called on an Amendment. Please call the SubmitAmendedValuationLightstone method.";
							LogPlugin.Logger.LogErrorMessage(methodName, String.Format("SubmitCompletedValuationLightstone called on an Amendment"), parameters);
							throw new Exception();
						}
					}
					else
					{
						if (methodName == "SubmitAmendedValuationLightstone")
						{
							errorMessage = "SubmitAmendedValuationLightstone called on an original valuation. Please call the SubmitCompletedValuationLightstone method.";
							LogPlugin.Logger.LogErrorMessage(methodName, String.Format("SubmitAmendedValuationLightstone called on an original valuation"), parameters);
							throw new Exception();
						}
					}

					int valuationKey = -1;
					IInstance workflowInstance = null;
					if (xmlHistory.GenericKeyType.Key == (int)GenericKeyTypes.Offer)
					{
						workflowInstance = x2Repository.GetLatestInstanceForGenericKey(xmlHistory.GenericKey, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);
						var workflowInstanceData = x2Repository.GetX2DataForInstance(workflowInstance);

						if (workflowInstanceData != null && workflowInstanceData.Data != null && workflowInstanceData.Data.Rows.Count > 0)
						{
							var workflowInstanceRow = workflowInstanceData.Data.Rows[0];

							if (workflowInstanceRow["ValuationKey"] != DBNull.Value)
								valuationKey = Convert.ToInt32(workflowInstanceRow["ValuationKey"]);
						}
						if (valuationKey < 1)
							throw new Exception(String.Format("The ValuationKey field of the X2Data is invalid: {0}", valuationKey));
						valuation = propertyRepository.GetValuationByKey(valuationKey);
					}
					else
					{
						//Non-Workflow stuff
						var property = propertyRepository.GetPropertyByAccountKey(xmlHistory.GenericKey);
						valuation = property.Valuations.FirstOrDefault(val => val.ValuationStatus.Key == (int)ValuationStatuses.Pending);
					}

					if (valuation == null)
						throw new Exception(String.Format("No Valuation record found matching the ValuationKey field ({0}) of the X2Data for the case", valuationKey));

					if (valuation.ValuationDataProviderDataService.Key != (int)ValuationDataProviderDataServices.LightstonePhysicalValuation)
						throw new Exception(String.Format("The Valuation record ({0}) is not of type LightstonePhysicalValuation", valuationKey));

					if (valuation.IsActive)
					{
						errorMessage = String.Format("The Valuation record ({0}) is already Active", valuationKey);
						throw new Exception(errorMessage);
					}

					valuation.Data = CreateXMLHistoryData(methodName, incomingLightstoneValuationXML, lightstoneValuationResultDataSet);

					//if any of the following are null, fail the whole process:
					// ValuationAmount
					// Valuator
					// HOCRoof
					// ValuationHOCValue

					//Not a Rejected Lightstone Valuation
					if (methodName != "SubmitRejectedValuationLightstone")
					{
						if (incomingPhysicalValuation.APPLICATION_INFORMATION.Count > 0)
						{
							string valCompany = incomingPhysicalValuation.APPLICATION_INFORMATION[0].ASSESSMENT_COMPANY;
							valuation.Valuator = propertyRepository.GetValuatorByDescription(valCompany);

							if (valuation.Valuator == null)
							{
								errorMessage = String.Format("Invalid value for ASSESSMENT_COMPANY: {0}", valCompany);
								throw new Exception(errorMessage);
							}

							if (!incomingPhysicalValuation.APPLICATION_INFORMATION[0].IsDATE_OF_ASSESSMENTNull())
								valuation.ValuationDate = incomingPhysicalValuation.APPLICATION_INFORMATION[0].DATE_OF_ASSESSMENT;
						}
						else
						{
							errorMessage = String.Format("APPLICATION_INFORMATION has no records");
							throw new Exception(errorMessage);
						}

						if (incomingPhysicalValuation.REPLACEMENT_COST.Count > 0)
						{
							HOCRoofs roofType;

							char[] roofChars = incomingPhysicalValuation.REPLACEMENT_COST[0].ROOF_TYPE.ToLower().ToCharArray();

							roofChars[0] = char.ToUpper(roofChars[0]);

							if (Enum.TryParse(new string(roofChars), out roofType))
							{
								string rtKey = Convert.ToString((int)roofType);

								if (lookupRepository.HOCRoof.ObjectDictionary.ContainsKey(rtKey))
									valuation.HOCRoof = lookupRepository.HOCRoof.ObjectDictionary[rtKey];
								else
								{
									errorMessage = String.Format("Invalid value for ROOF_TYPE: {0}", incomingPhysicalValuation.REPLACEMENT_COST[0].ROOF_TYPE);
									throw new Exception(errorMessage);
								}
							}
							else
							{
								errorMessage = String.Format("Invalid value for ROOF_TYPE: {0}", incomingPhysicalValuation.REPLACEMENT_COST[0].ROOF_TYPE);
								throw new Exception(errorMessage);
							}
						}
						else
						{
							errorMessage = String.Format("REPLACEMENT_COST has no records");
							throw new Exception(errorMessage);
						}

						if (incomingPhysicalValuation.INSURANCE_VALUE.Count > 0)
						{
							valuation.ValuationAmount = incomingPhysicalValuation.INSURANCE_VALUE[0].PRESENT_MARKET_VALUE;
							valuation.ValuationHOCValue = incomingPhysicalValuation.INSURANCE_VALUE[0].TOTAL_REPLACEMENTINSURANCE_VALUE;

							double thatchAmount = incomingPhysicalValuation.INSURANCE_VALUE[0].IsREPLACEMENT_THATCHNull() ? 0 : incomingPhysicalValuation.INSURANCE_VALUE[0].REPLACEMENT_THATCH;

							valuation.HOCConventionalAmount = valuation.ValuationHOCValue - thatchAmount;
							valuation.HOCThatchAmount = thatchAmount;
							valuation.HOCShingleAmount = 0;
						}
						else
						{
							errorMessage = String.Format("INSURANCE_VALUE has no records");
							throw new Exception(errorMessage);
						}
					}

					IValuationStatus status = lookupRepository.ValuationStatus.SingleOrDefault(x => x.Key == (int)valuationStatusToSet);
					valuation.ValuationStatus = status;
					valuation.ValuationEscalationPercentage = 20;

					// setup rule exclusions
					IList<RuleExclusionSets> exclusionSets = new List<RuleExclusionSets>();
					exclusionSets.Add(RuleExclusionSets.ValuationMainBuildingView);
					SPC.ExclusionSets.Add(exclusionSets[0]);

					if (xmlHistory.GenericKeyType.Key == (int)GenericKeyTypes.Account &&
						status.Key == (int)ValuationStatuses.Complete)
					{
						valuation.IsActive = true;
					}

					// save the valuation
					propertyRepository.SaveValuation(valuation);

					//Add the External Activity
					if (xmlHistory.GenericKeyType.Key == (int)GenericKeyTypes.Offer)
					{
						TriggerExternalActivity(workflowInstance.ID, x2Flag);
					}
					transactionScope.VoteCommit();

					// send email to origional consultant
					if (valuation != null && xmlHistory.GenericKeyType.Key == (int)GenericKeyTypes.Account)
					{
						// parse consultant correctly
						string consultant = valuation.ValuationUserID;

						if (consultant.IndexOf(@"SAHL\") < 0)
							consultant = @"SAHL\" + consultant;

						// get email address of consultant
						IADUser adUser = organisationStructureRepository.GetAdUserForAdUserName(consultant);
						if (adUser != null && adUser.LegalEntity != null)
						{
							string emailAddress = adUser.LegalEntity.EmailAddress;
							if (!String.IsNullOrEmpty(emailAddress))
							{
								// build up email 
								ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.ValuationConsultantUpdateNotification);

								string subject = String.Format(template.Subject, xmlHistory.GenericKey);
								string body = String.Format(template.Template, xmlHistory.GenericKey);

								// send email
								messageService.SendEmailInternal("halo@sahomeloans.com", emailAddress, String.Empty, String.Empty, subject, body);
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (transactionScope != null)
						transactionScope.VoteRollBack();

					StringBuilder sb = new StringBuilder();

					if (DomainValidationFailed)
					{
						SPC.DomainMessages.Clear();
						sb.AppendLine("Cleared");

						foreach (IDomainMessage dm in SPC.DomainMessages)
						{
							sb.AppendLine(dm.Message);
							sb.AppendLine("WTF");
						}
					}

					string domainError = String.Format("{0} UniqueID={1} {2}\n", methodName, xmlHistoryID, ex.Message);
					if (!String.IsNullOrEmpty(sb.ToString()))
						domainError += "\nDomain Messages:\n " + sb.ToString();

					LogPlugin.Logger.LogErrorMessage(methodName, domainError, parameters);

					if (String.IsNullOrEmpty(errorMessage))
						errorMessage = String.Format("{0} UniqueID={1} - HALO Error - please notify SAHL", methodName, xmlHistoryID);
				}
				finally
				{
					DataRow row = lightstoneValuationResultDataSet.Tables[0].Rows[0];

					row["ErrorMessage"] = errorMessage;

					if (!String.IsNullOrEmpty(errorMessage))
						row["Successful"] = "False";

					if (xmlHistory != null)
					{
						string xml = CreateXMLHistoryData(methodName, incomingLightstoneValuationXML, lightstoneValuationResultDataSet);
						SAHL.Common.Service.XMLHistory.InsertXMLHistory(xml, xmlHistory.GenericKey, xmlHistory.GenericKeyType.Key);
					}
				}
			}
			//Attempt to update the HOC
			if (xmlHistory != null &&
 				valuation != null &&
				xmlHistory.GenericKeyType.Key == (int)GenericKeyTypes.Account &&
				valuation.IsActive &&
				valuation.ValuationStatus.Key == (int)ValuationStatuses.Complete)
			{
				UpdateHOCFromValuation(propertyRepository, valuation, xmlHistory.GenericKey);
			}

			return lightstoneValuationResultDataSet;
		}

		private void UpdateHOCFromValuation(IPropertyRepository propertyRepository, Common.BusinessModel.Interfaces.IValuation valuation, int accountKey)
		{
			//Recalc HOC
			using (var hocTransaction = new TransactionScope(TransactionMode.New))
			{
				try
				{
					propertyRepository.ValuationUpdateHOC(valuation, GenericKeyTypes.Account, accountKey);
					hocTransaction.VoteCommit();
				}
				catch (DomainValidationException domainValidationException)
				{
					ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.HOCUpdateFailedOnValuationRequest);

					var errorMessages = String.Join(", ", domainValidationException.DomainErrorMessages);
					var warningMessages = String.Join(", ", domainValidationException.DomainWarningMessages);

					string subject = String.Format(template.Subject, accountKey);
					string body = String.Format(template.Template, errorMessages + warningMessages);

					messageService.SendEmailInternal("halo@sahomeloans.com", template.DefaultEmail, String.Empty, String.Empty, subject, body);
					SPC.DomainMessages.Clear();
					hocTransaction.VoteRollBack();
				}
				catch (Exception ex)
				{
					var parameters = new Dictionary<string, object>();
					parameters.Add("AccountKey", accountKey);
					LogPlugin.Logger.LogErrorMessageWithException(MethodBase.GetCurrentMethod().Name, String.Format("Unable to update HOC from Valuation for Account: {0}", accountKey), ex, parameters);
				}
			}
		}

		private DataSet GetLightstoneValuationResultData(int uniqueID)
		{
			LightstoneWebServiceResult resultDS = new LightstoneWebServiceResult();
			LightstoneWebServiceResult.ResultsRow resultRow = resultDS.Results.NewResultsRow();
			resultRow.UniqueID = uniqueID.ToString();
			resultRow.Successful = "True";
			resultDS.Results.AddResultsRow(resultRow);

			DataSet ds = new DataSet("Response");
			DataTable dt = new DataTable(resultDS.Results.TableName);

			foreach (DataColumn dc in resultDS.Results.Columns)
				dt.Columns.Add(dc.ColumnName, dc.DataType);

			DataRow row = dt.NewRow();
			row["UniqueID"] = resultRow.UniqueID;
			row["Successful"] = resultRow.Successful;
			row["ErrorMessage"] = resultRow.ErrorMessage;
			dt.Rows.Add(row);
			ds.Tables.Add(dt);

			return ds;
		}

		private string CreateXMLHistoryData(string methodName, string incomingLightstoneValuationXML, DataSet lightstoneValuationResultDataSet)
		{
			return String.Format("<SAHL.{0}>\n<Request>\n{1}\n</Request>\n{2}\n</SAHL.{3}>", methodName, incomingLightstoneValuationXML, lightstoneValuationResultDataSet.GetXml(), methodName);
		}
	}
}
