using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.Views;
using System.Reflection;
using System;
using SAHL.Tools.Workflow.Common.ReferenceChecking;
using NuGet;
using System.Collections.Generic;

namespace SAHL.X2Designer.XML
{
	internal class XMLSaveDocument
	{
		public XMLSaveDocument(string fileName)
		{
			MainForm.App.Cursor = Cursors.WaitCursor;
			string xmlName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".xml";
			string configFileName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".config";
			ProcessView PV = MainForm.App.GetCurrentView();

			XmlWriterSettings xSettings = new XmlWriterSettings();
			xSettings.OmitXmlDeclaration = false;
			xSettings.Indent = true;
			xSettings.Encoding = Encoding.UTF8;

			XmlWriter xWriter = XmlWriter.Create(xmlName, xSettings);

			xWriter.WriteStartElement("ProcessName");
			xWriter.WriteAttributeString("Name", PV.Name);
			xWriter.WriteAttributeString("ProductVersion", "0.3");
			xWriter.WriteAttributeString("MapVersion", MainForm.App.GetCurrentView().Document.MapVersion);
			xWriter.WriteAttributeString("Retrieved", "false");

            xWriter.WriteAttributeString("Legacy", MainForm.App.GetCurrentView().Document.IsLegacy.ToString());
            xWriter.WriteAttributeString("ViewableOnUserInterfaceVersion", MainForm.App.GetCurrentView().Document.HaloV3Viewable ? "3" : "2");

			#region WorkFlowS

			xWriter.WriteStartElement("WorkFlows"); // + WorkFlows
			for (int x = 0; x < PV.Document.WorkFlows.Length; x++)
			{
				if (PV.Document.WorkFlows[x].WorkFlowName.Length < 1)
				{
					continue;
				}
				if (PV.Document.WorkFlows[x] != PV.Document.CurrentWorkFlow)
				{
					PV.Document.WorkFlows[x].Expand();
				}
				xWriter.WriteStartElement("WorkFlow");  // + WorkFlow
				xWriter.WriteAttributeString("WorkFlowName", PV.Document.WorkFlows[x].WorkFlowName.ToString());
				xWriter.WriteAttributeString("LocationX", PV.Document.WorkFlows[x].Location.X.ToString());
				xWriter.WriteAttributeString("LocationY", PV.Document.WorkFlows[x].Location.Y.ToString());
				xWriter.WriteAttributeString("GenericKeyTypeKey", PV.Document.WorkFlows[x].GenericKeyTypeKey.ToString());

				#region //Custom Variables

				xWriter.WriteStartElement("CustomVariables"); // + CustomVariables
				for (int r = 0; r < MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables.Count; r++)
				{
					xWriter.WriteStartElement("CustomVariable"); // + CustomVariable
					xWriter.WriteAttributeString("Name", MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables[r].Name);
					xWriter.WriteAttributeString("Length", MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables[r].Length.ToString());
					CustomVariableTypeTypeConvertor mConvertor = new CustomVariableTypeTypeConvertor();

					string myVar = mConvertor.ConvertToString(MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables[r].Type);
					xWriter.WriteAttributeString("Type", myVar);
					xWriter.WriteEndElement(); // - CustomVariable
				}
				xWriter.WriteEndElement(); // - CustomVariables

				#endregion //Custom Variables

				#region //External Activities

				xWriter.WriteStartElement("ExternalActivities"); // + ExternalActivities
				for (int r = 0; r < MainForm.App.GetCurrentView().Document.WorkFlows[x].ExternalActivityCollection.Count; r++)
				{
					xWriter.WriteStartElement("ExternalActivity"); // + ExternalActivity
					xWriter.WriteAttributeString("Name", MainForm.App.GetCurrentView().Document.WorkFlows[x].ExternalActivityCollection[r].ExternalActivity);
					xWriter.WriteAttributeString("Description", MainForm.App.GetCurrentView().Document.WorkFlows[x].ExternalActivityCollection[r].Description);
					xWriter.WriteEndElement(); // - ExternalActivity
				}
				xWriter.WriteEndElement(); // - ExternalActivities

				#endregion //External Activities

				#region //Custom Forms

				xWriter.WriteStartElement("CustomForms"); // + CustomForms
				for (int r = 0; r < MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms.Count; r++)
				{
					xWriter.WriteStartElement("CustomForm"); // + CustomForm
					xWriter.WriteAttributeString("Name", MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms[r].Name);
					xWriter.WriteAttributeString("Description", MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms[r].Description);
					xWriter.WriteEndElement(); // - CustomForm
				}
				xWriter.WriteEndElement(); // - CustomForms

				#endregion //Custom Forms

				#region InvisibleAnchorNode

				foreach (GoObject o in PV.Document.WorkFlows[x])
				{
					if (o is InvisibleAnchorNode)
					{
						xWriter.WriteStartElement("InvisibleAnchorNode"); // + InvisibleAnchorNode
						InvisibleAnchorNode mInvisibleAnchorNode = o as InvisibleAnchorNode;
						PointF convertPoint = mInvisibleAnchorNode.Position;

						xWriter.WriteAttributeString("LocationX", convertPoint.X.ToString());
						xWriter.WriteAttributeString("LocationY", convertPoint.Y.ToString());
						xWriter.WriteEndElement(); // - InvisibleAnchorNode                    }
						break;
					}
				}

				#endregion InvisibleAnchorNode

				#region ClapperBoard

				foreach (GoObject o in PV.Document.WorkFlows[x])
				{
					if (o is ClapperBoard)
					{
						xWriter.WriteStartElement("Clapperboard"); // + ClapperBoard
						ClapperBoard mClapper = o as ClapperBoard;
						BaseItem mBaseItem = o as BaseItem;
						PointF convertPoint = mClapper.Position;

						xWriter.WriteAttributeString("LocationX", convertPoint.X.ToString());
						xWriter.WriteAttributeString("LocationY", convertPoint.Y.ToString());
						if (mClapper.LimitAccessTo != null)
						{
							xWriter.WriteAttributeString("LimitAccessTo", mClapper.LimitAccessTo.Name.ToString());
						}
						else
						{
							xWriter.WriteAttributeString("LimitAccessTo", "");
						}
						xWriter.WriteAttributeString("Subject", mClapper.Subject.ToString());
						if (mClapper.KeyVariable != null)
						{
							xWriter.WriteAttributeString("KeyVariable", mClapper.KeyVariable.Name.ToString());
						}
						else
						{
							xWriter.WriteAttributeString("KeyVariable", "");
						}
						xWriter.WriteEndElement(); // - ClapperBoard
					}
				}

				#endregion ClapperBoard

				#region //States

				xWriter.WriteStartElement("States"); // + States
				foreach (GoObject o in PV.Document.WorkFlows[x])
				{
					if (o is BaseState)
					{
						xWriter.WriteStartElement("State"); // + State
						BaseState mBaseState = o as BaseState;
						PointF convertPoint = mBaseState.Position;

						xWriter.WriteAttributeString("LocationX", convertPoint.X.ToString());
						xWriter.WriteAttributeString("LocationY", convertPoint.Y.ToString());
						xWriter.WriteAttributeString("X2ID", mBaseState.X2ID.ToString());
						xWriter.WriteAttributeString("Type", o.GetType().ToString());
						xWriter.WriteAttributeString("StateName", mBaseState.Name.ToString());
						if (o is SystemState)
						{
							SystemState mSystemState = o as SystemState;
							xWriter.WriteAttributeString("UseAutoForward", mSystemState.UseAutoForward.ToString());
						}
						if (o is ArchiveState)
						{
							ArchiveState ass = o as ArchiveState;
							if (null != ass.ReturnActivity)
								xWriter.WriteAttributeString("ReturnActivity", ass.ReturnActivity.ToString());
							else
								xWriter.WriteAttributeString("ReturnActivity", "");
							if (null != ass.WorkflowToReturnTo)
								xWriter.WriteAttributeString("ReturnWorkflow", ass.WorkflowToReturnTo.WorkFlowName.ToString());
							else
								xWriter.WriteAttributeString("ReturnWorkflow", "");
						}
						// Code Sections
						xWriter.WriteStartElement("CodeSections"); // + CodeSections
						for (int y = 0; y < mBaseState.AvailableCodeSections.Length; y++)
						{
							xWriter.WriteStartElement("CodeSection"); // + CodeSection
							xWriter.WriteAttributeString("Name", mBaseState.AvailableCodeSections[y]);
							xWriter.WriteAttributeString("Code", mBaseState.GetCodeSectionData(mBaseState.AvailableCodeSections[y]));
							xWriter.WriteEndElement(); // -CodeSection
						}
						xWriter.WriteEndElement(); // -CodeSections
						BaseState mBaseStateContainer = o as BaseState;
						switch (mBaseStateContainer.WorkflowItemType)
						{
							case WorkflowItemType.CommonState:
								{
									CommonState mCommonState = o as CommonState;
									xWriter.WriteStartElement("AppliesTo"); // + Applies To
									for (int y = 0; y < mCommonState.AppliesTo.Count; y++)
									{
										xWriter.WriteStartElement("AppliesToCollection"); // + AppliesToCollection
										xWriter.WriteAttributeString("AppliesToCollectionItem", mCommonState.AppliesTo[y].Text.ToString());
										xWriter.WriteEndElement(); // - AppliesToListCollection
									}
									xWriter.WriteEndElement(); // - Applies To
									break;
								}
							default:
								{
									BaseStateWithLists mListState = o as BaseStateWithLists;
									if (mListState != null)
									{
										xWriter.WriteStartElement("WorkList"); // + WorkList
										for (int y = 0; y < mListState.WorkList.Count; y++)
										{
											if (mListState.WorkList[y].IsChecked)
											{
												xWriter.WriteStartElement("WorkListCollection"); // + WorkListCollection
												xWriter.WriteAttributeString("WorkListCollectionItem", mListState.WorkList[y].RoleItem.Name.ToString());
												xWriter.WriteEndElement(); // - WorkListCollection
											}
										}
										xWriter.WriteEndElement(); // - WorkList

										xWriter.WriteStartElement("TrackList"); // + TrackList
										for (int y = 0; y < mListState.TrackList.Count; y++)
										{
											if (mListState.TrackList[y].IsChecked)
											{
												xWriter.WriteStartElement("TrackListCollection"); // + TrackListCollection
												xWriter.WriteAttributeString("TrackListCollectionItem", mListState.TrackList[y].RoleItem.Name.ToString());
												xWriter.WriteEndElement(); // - TrackListCollection
											}
										}
										xWriter.WriteEndElement(); // - TrackList
									}
									break;
								}
						}
						if (o is UserState)
						{
							UserState mUserState = o as UserState;
							xWriter.WriteStartElement("CustomForms"); // + CustomForms
							for (int y = 0; y < mUserState.CustomForms.Count; y++)
							{
								xWriter.WriteStartElement("CustomFormCollection"); // + CustomFormCollection
								xWriter.WriteAttributeString("FormName", mUserState.CustomForms[y].Name.ToString());
								xWriter.WriteEndElement(); // - CustomFormCollection
							}
							xWriter.WriteEndElement(); // - CustomForms
						}

						if (o is HoldState)
						{
							HoldState mHoldState = o as HoldState;
							xWriter.WriteStartElement("CustomForms"); // + CustomForms
							for (int y = 0; y < mHoldState.CustomForms.Count; y++)
							{
								xWriter.WriteStartElement("CustomFormCollection"); // + CustomFormCollection
								xWriter.WriteAttributeString("FormName", mHoldState.CustomForms[y].Name.ToString());
								xWriter.WriteEndElement(); // - CustomFormCollection
							}
							xWriter.WriteEndElement(); // - CustomForms
						}

						xWriter.WriteEndElement(); // - State
					}
				}
				xWriter.WriteEndElement(); // - States

				#endregion //States

				#region //Activities

				xWriter.WriteStartElement("Activities"); // + Activities
				foreach (GoObject o in PV.Document.WorkFlows[x])
				{
					if (o is BaseActivity)
					{
						xWriter.WriteStartElement("Activity"); // + Activity
						BaseActivity mBaseActivity = o as BaseActivity;

						PointF convertPoint = mBaseActivity.Position;
						PointF mapPoint = MainForm.App.GetCurrentView().Document.WorkFlows[x].expandedLocation;
						//PointF nodePoint = new Point(Convert.ToInt32(mBaseState.Position.X), Convert.ToInt32(mBaseState.Position.Y));

						xWriter.WriteAttributeString("LocationX", convertPoint.X.ToString());
						xWriter.WriteAttributeString("LocationY", convertPoint.Y.ToString());

						xWriter.WriteAttributeString("Type", o.GetType().ToString());
						xWriter.WriteAttributeString("Id", mBaseActivity.Id);
						xWriter.WriteAttributeString("Name", mBaseActivity.Name);
						xWriter.WriteAttributeString("X2ID", mBaseActivity.X2ID.ToString());
						xWriter.WriteAttributeString("Message", mBaseActivity.Message);
						xWriter.WriteAttributeString("StageTransitionMessage", mBaseActivity.StageTransitionMessage);
						xWriter.WriteAttributeString("Description", mBaseActivity.Description);
						xWriter.WriteAttributeString("Priority", mBaseActivity.Priority.ToString());
						if (mBaseActivity.RaiseExternalActivity != null)
						{
							xWriter.WriteAttributeString("RaiseExternalActivity", mBaseActivity.RaiseExternalActivity.ExternalActivity);
						}
						else
						{
							xWriter.WriteAttributeString("RaiseExternalActivity", "");
						}

						xWriter.WriteAttributeString("SplitWorkFlow", mBaseActivity.SplitWorkFlow.ToString());

						if (o is UserActivity)
						{
							UserActivity mUserActivity = o as UserActivity;
							if (mUserActivity.CustomForm != null)
							{
								xWriter.WriteAttributeString("CustomForm", mUserActivity.CustomForm.Name.ToString());
							}
							else
							{
								xWriter.WriteAttributeString("CustomForm", "");
							}

							if (mUserActivity.LinkedActivity != null)
							{
								xWriter.WriteAttributeString("LinkedActivity", mUserActivity.LinkedActivity.Name.ToString());
							}
							else
							{
								xWriter.WriteAttributeString("LinkedActivity", "");
							}

							xWriter.WriteAttributeString("UseLinkedActivity", mUserActivity.UseLinkedActivity.ToString());
						}

						if (o is ExternalActivity)
						{
							ExternalActivity mExternalActivity = o as ExternalActivity;
							if (mExternalActivity.InvokeOnInstanceTarget != null)
							{
								xWriter.WriteAttributeString("ExternalActivityRaiseFolder", mExternalActivity.InvokeOnInstanceTarget.ToString());
							}
							else
							{
								xWriter.WriteAttributeString("ExternalActivityRaiseFolder", "");
							}

							if (mExternalActivity.InvokedBy != null)
							{
								xWriter.WriteAttributeString("InvokedBy", mExternalActivity.InvokedBy.ExternalActivity.ToString());
							}
							else
							{
								xWriter.WriteAttributeString("InvokedBy", "");
							}
						}

						if (o is CallWorkFlowActivity)
						{
							CallWorkFlowActivity mCallWorkFlowActivity = o as CallWorkFlowActivity;
							if (mCallWorkFlowActivity.WorkFlowToCall != null)
							{
								xWriter.WriteAttributeString("WorkFlowToCall", mCallWorkFlowActivity.WorkFlowToCall.WorkFlowName.ToString());
							}
							else
							{
								xWriter.WriteAttributeString("WorkFlowToCall", "");
							}

							if (mCallWorkFlowActivity.ActivityToCall != null)
							{
								xWriter.WriteAttributeString("ActivityToCall", mCallWorkFlowActivity.ActivityToCall.ToString());
							}
							else
							{
								xWriter.WriteAttributeString("ActivityToCall", "");
							}
							if (mCallWorkFlowActivity.ReturnActivity != null)
							{
								xWriter.WriteAttributeString("ReturnActivity", mCallWorkFlowActivity.ReturnActivity.Name);
							}
							else
							{
								xWriter.WriteAttributeString("ReturnActivity", "");
							}
						}

						GoNode fromNode = null;
						GoNode toNode = null;
						foreach (CustomLink l in mBaseActivity.Links)
						{
							if (l.FromNode != mBaseActivity as GoNode)
							{
								fromNode = l.FromNode as GoNode;
							}
							else
							{
								toNode = l.ToNode as GoNode;
							}
						}
						if (mBaseActivity.WorkflowItemType == WorkflowItemType.CallWorkFlowActivity
							|| mBaseActivity.WorkflowItemType == WorkflowItemType.ReturnWorkFlowActivity)
						{
							toNode = mBaseActivity as GoNode; ;
						}

						if (fromNode is ClapperBoard)
						{
							xWriter.WriteAttributeString("FromNode", "ClapperBoard");
						}
						else
						{
							xWriter.WriteAttributeString("FromNode", fromNode.Text.ToString());
						}
						if (toNode != null)
						{
							xWriter.WriteAttributeString("ToNode", toNode.Text.ToString());
						}
						else
						{
						}

						if (mBaseActivity.GetType() == typeof(UserActivity))
						{
							UserActivity mUserActivity = mBaseActivity as UserActivity;
							xWriter.WriteStartElement("Access"); // + Access
							for (int y = 0; y < mUserActivity.Access.Count; y++)
							{
								if (/*!RoleItemCollection.FixedRoles.Contains(mBaseAction.Access[x].RoleItem.Name) &&*/ mUserActivity.Access[y].IsChecked)
								{
									xWriter.WriteStartElement("AccessCollection"); // + AccessCollection
									xWriter.WriteAttributeString("AccessCollectionItem", mUserActivity.Access[y].RoleItem.Name.ToString());
									xWriter.WriteEndElement(); // - AccessCollection
								}
							}
							xWriter.WriteEndElement(); // - Access
						}

						// Code Sections
						xWriter.WriteStartElement("CodeSections"); // + CodeSections
						string[] CodeSections = mBaseActivity.GetAvailableCodeSections;
						for (int y = 0; y < CodeSections.Length; y++)
						{
							xWriter.WriteStartElement("CodeSection"); // + CodeSection
							xWriter.WriteAttributeString("Name", CodeSections[y]);
							xWriter.WriteAttributeString("Code", mBaseActivity.GetCodeSectionData(CodeSections[y]));
							xWriter.WriteEndElement(); // -CodeSection
						}

						xWriter.WriteEndElement(); // -CodeSections

						IBusinessStageTransitions busItem = o as IBusinessStageTransitions;
						if (busItem != null)
						{
							xWriter.WriteStartElement("BusinessStageTransitions");

							for (int t = 0; t < busItem.BusinessStageTransitions.Count; t++)
							{
								xWriter.WriteStartElement("BusinessStageTransition"); // + BusinessStageTransition

								xWriter.WriteAttributeString("StageDefinitionKey", busItem.BusinessStageTransitions[t].SDSDGKey.ToString());
								xWriter.WriteAttributeString("DefinitionGroupDescription", busItem.BusinessStageTransitions[t].DefinitionGroupDescription.ToString());
								xWriter.WriteAttributeString("DefinitionDescription", busItem.BusinessStageTransitions[t].DefinitionDescription.ToString());

								xWriter.WriteEndElement(); // -BusinessStageTransition
							}
							xWriter.WriteEndElement(); //BusinessStageTransitions
						}

						xWriter.WriteEndElement(); // - Activity
					}
				}
				xWriter.WriteEndElement(); // - Activities

				#endregion //Activities

				#region //Comments

				xWriter.WriteStartElement("Comments"); // + Comments
				foreach (GoObject o in PV.Document.WorkFlows[x])
				{
					if (o.GetType() == typeof(Comment))
					{
						xWriter.WriteStartElement("Comment"); // + Comment
						Comment mComment = o as Comment;
						xWriter.WriteAttributeString("LocationX", mComment.Position.X.ToString());
						xWriter.WriteAttributeString("LocationY", mComment.Position.Y.ToString());
						xWriter.WriteAttributeString("Name", mComment.Name);
						xWriter.WriteAttributeString("CommentText", mComment.CommentText);
						xWriter.WriteEndElement(); // - Comment
					}
				}
				xWriter.WriteEndElement(); // - Comments

				#endregion //Comments

				xWriter.WriteEndElement(); // - WorkFlow
			}
			xWriter.WriteEndElement(); // - WorkFlows

			#endregion WorkFlowS

			#region Roles

			xWriter.WriteStartElement("Roles"); // + Roles
			for (int r = 0; r < MainForm.App.GetCurrentView().Document.Roles.Count; r++)
			{
				if (!RoleItemCollection.FixedRoles.Contains(MainForm.App.GetCurrentView().Document.Roles[r].Name))
				{
					xWriter.WriteStartElement("Role"); // + Role
					xWriter.WriteAttributeString("Role", MainForm.App.GetCurrentView().Document.Roles[r].Name);
					xWriter.WriteAttributeString("Description", MainForm.App.GetCurrentView().Document.Roles[r].Description);
					xWriter.WriteAttributeString("IsDynamic", MainForm.App.GetCurrentView().Document.Roles[r].IsDynamic.ToString());
					xWriter.WriteAttributeString("RoleType", MainForm.App.GetCurrentView().Document.Roles[r].RoleType.ToString());
					if (MainForm.App.GetCurrentView().Document.Roles[r].WorkFlowItem != null)
					{
						xWriter.WriteAttributeString("WorkFlow", MainForm.App.GetCurrentView().Document.Roles[r].WorkFlowItem.WorkFlowName.ToString());
					}
					else
					{
					}
					if (MainForm.App.GetCurrentView().Document.Roles[r].IsDynamic == true)
					{
						// Code Sections
						xWriter.WriteStartElement("CodeSections"); // + CodeSections
						string[] CodeSections = MainForm.App.GetCurrentView().Document.Roles[r].AvailableCodeSections;
						for (int y = 0; y < CodeSections.Length; y++)
						{
							xWriter.WriteStartElement("CodeSection"); // + CodeSection
							xWriter.WriteAttributeString("Name", CodeSections[y]);
							xWriter.WriteAttributeString("Code", MainForm.App.GetCurrentView().Document.Roles[r].GetCodeSectionData(CodeSections[y]));
							xWriter.WriteEndElement(); // -CodeSection
						}
						xWriter.WriteEndElement(); // -CodeSections
					}

					xWriter.WriteEndElement(); // - Role
				}
			}

			xWriter.WriteEndElement(); // - Roles

			#endregion Roles

			#region Using Statements

			xWriter.WriteStartElement("UsingStatements");
			for (int r = 0; r < MainForm.App.GetCurrentView().Document.UsedUsingStatements.Count; r++)
			{
				xWriter.WriteStartElement("UsingStatement");
				xWriter.WriteAttributeString("Statement", MainForm.App.GetCurrentView().Document.UsedUsingStatements[r]);
				xWriter.WriteEndElement();
			}
			xWriter.WriteEndElement();

			#endregion Using Statements

			#region Nuget
			xWriter.WriteStartElement("NugetPackages");
			foreach (var nugetReference in MainForm.App.GetCurrentView().Document.NuGetPackages)
			{
				xWriter.WriteStartElement("NugetPackage");

				xWriter.WriteAttributeString("PackageName", nugetReference.PackageID);
				xWriter.WriteAttributeString("Version", nugetReference.PackageVersion);
				xWriter.WriteAttributeString("DependsOn", nugetReference.DependsOn);
				xWriter.WriteAttributeString("DependsOnVersion", nugetReference.DependsOnVersion);

				xWriter.WriteEndElement();
			}
			xWriter.WriteEndElement();
			#endregion

			#region References
			xWriter.WriteStartElement("References");

			for (int r = 0; r < MainForm.App.GetCurrentView().Document.References.Count; r++)
			{
				ReferenceItem ri = MainForm.App.GetCurrentView().Document.References[r];
				xWriter.WriteStartElement("Reference");

				xWriter.WriteAttributeString("FullName", ri.FullName);
				xWriter.WriteAttributeString("Path", ri.SavePath);
				xWriter.WriteAttributeString("Name", ri.Name + ".dll");
				xWriter.WriteAttributeString("Version", ri.Version.ToString());

				xWriter.WriteEndElement();
			}
			xWriter.WriteEndElement();

			#endregion References

			// Delete the backup file if it exists
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			xWriter.Flush();
			xWriter.Close();

			// Convert the XML file to X2P file
			if (File.Exists(xmlName))
			{
				File.Copy(xmlName, fileName);
				File.Delete(xmlName);
			}

			// Create the vanilla config file if it doesnt already exist
			if (!File.Exists(configFileName))
			{
				Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SAHL.X2Designer.Resources.vanilla.config");
				if (stream != null)
				{
					StreamReader sr = new StreamReader(stream);
					string contents = sr.ReadToEnd();

					using (var writer = new StreamWriter(configFileName))
					{
						writer.Write(contents);
					}
				}
			}
			Application.DoEvents();


			ProcessDocument PD = MainForm.App.GetCurrentView().Document;

			PV.Name = Path.GetFileNameWithoutExtension(PV.Document.Location);
			PV.Document.IsModified = false;
			PV.UpdateTitle();

            //MainForm.App.setStatusBar("Resolving NuGet Packages...");

            //var packageResolver = new PackageResolver(new List<IPackageRepository>{
            //    PackageRepositoryFactory.Default.CreateRepository(SAHL.X2Designer.Properties.Settings.Default.OfficialNuGetUrl),
            //    PackageRepositoryFactory.Default.CreateRepository(SAHL.X2Designer.Properties.Settings.Default.SAHLNuGetUrl),
            //});
            //packageResolver.ResolvePackages(
            //    coreVersionToUse: String.Empty,
            //    packageToUpdate: String.Empty,
            //    workflowMapLocation: fileName,
            //    binariesLocation: Path.Combine(Path.GetDirectoryName(fileName), "Binaries"));

			MainForm.App.Cursor = Cursors.Default;
		}
	}
}