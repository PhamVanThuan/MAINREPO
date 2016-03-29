using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.MapConverter
{
    class MapController
    {
        #region variables
        XDocument xmlFile;
        FileInfo file;
        private DBController dbController;
        private ScriptController scriptController;
        #endregion

        #region constructor
        public MapController(FileInfo file, DBController dbController, ScriptController scriptController)
        {
            this.file = file;
            this.dbController = dbController;
            this.scriptController = scriptController;
            xmlFile = XDocument.Load(file.FullName);
        }
        #endregion

        #region methods
        internal void Backup()
        {
            DirectoryInfo info = new DirectoryInfo(file.Directory.FullName + @"\Backup");
            if (!info.Exists)
                info.Create();
            xmlFile.Save(info.FullName + "\\" + file.Name);
        }
        internal void ProcessStatesFromDB()
        {
            Dictionary<string, int> workflowNameIDs = dbController.GetMapWorkFlowIDs(xmlFile.Descendants("ProcessName").SingleOrDefault().Attribute("Name").Value);
            Console.WriteLine("Processing File - " + file.Name);

            foreach (KeyValuePair<string, int> kvp in workflowNameIDs)
            {
                IList<State> statesInDB = dbController.GetStatesByWorkflowID(kvp.Value);
                foreach (State state in statesInDB)
                {
                    XElement element = GetElementByWorkflowAndStateName(kvp.Key, state.Name);
                    if (element != null)
                    {
                        bool updateFromDB = element.Attribute("X2ID") == null ? true : (element.Attribute("X2ID").Value == Guid.Empty.ToString() ? true : (element.Attribute("X2ID").Value != state.X2ID.ToString() ? true : false));
                        if (updateFromDB)
                        {
                            UpdateState(element, state);
                        }
                    }
                    else
                    {
                        int a = 0;
                    }
                }
            }
        }
        internal void ProcessStatesFromFile()
        {
            var elements = xmlFile.Descendants("State")
                           .AsParallel()
                           .Where(x => x.Attribute("Type").Value != "SAHL.X2Designer.Items.CommonState" && (x.Attribute("X2ID") == null ? true : (x.Attribute("X2ID").Value == Guid.Empty.ToString() ? true : false)));
            foreach (XElement element in elements.AsParallel())
            {
                UpdateState(element, Guid.NewGuid());
            }
        }

        internal void ProcessActivitiesFromDB()
        {
            Dictionary<string, int> workflowNameIDs = dbController.GetMapWorkFlowIDs(xmlFile.Descendants("ProcessName").SingleOrDefault().Attribute("Name").Value);
            foreach (KeyValuePair<string, int> kvp in workflowNameIDs)
            {
                IList<Activity> activitiesInDb = dbController.GetActivitiesByWorkflowID(kvp.Value);
                foreach (Activity activity in activitiesInDb)
                {
                    XElement element = GetElementByWorkflowAndActivitySettings(kvp.Key, activity.Name, GetName(activity.FromState), GetName(activity.ToState));
                    if (element != null)
                    {
                        bool updateFromDB = element.Attribute("X2ID") == null ? true : (element.Attribute("X2ID").Value == Guid.Empty.ToString() ? true : (element.Attribute("X2ID").Value != activity.X2ID.ToString() ? true : false));
                        if (updateFromDB)
                        {
                            UpdateActivity(element, activity);
                        }
                    }
                }
            }
        }

        internal void ProcessActivitiesFromFile()
        {
            var elements = xmlFile.Descendants("Activity")
                           .AsParallel()
                           .Where(x => x.Attribute("X2ID") == null ? true : (x.Attribute("X2ID").Value == Guid.Empty.ToString() ? true : false));
            foreach (XElement element in elements.AsParallel())
            {
                UpdateActivity(element, Guid.NewGuid());
            }
        }

        internal void Save()
        {
            file.IsReadOnly = false;
            xmlFile.Save(file.FullName);
            file.IsReadOnly = true;
        }
        #endregion

        #region private methods
        private void UpdateState(XElement element, State state)
        {
            Console.WriteLine(string.Format("State Name = {0}", state.Name));
            element.SetAttributeValue("X2ID", state.X2ID);
            scriptController.AddStateToScript(state);
        }

        private void UpdateState(XElement element,Guid newGuid)
        {
            Console.WriteLine(string.Format("State Name = {0}", element.Attribute("StateName").Value));
            element.SetAttributeValue("X2ID", newGuid);
        }

        private void UpdateActivity(XElement element, Activity activity)
        {
            Console.WriteLine(string.Format("Activity Name = {0}", activity.Name));
            element.SetAttributeValue("X2ID", activity.X2ID);
            scriptController.AddActivityToScript(activity);
        }
        private void UpdateActivity(XElement element, Guid newGuid)
        {
            Console.WriteLine(string.Format("Activity Name = {0}", element.Attribute("Name").Value));
            element.SetAttributeValue("X2ID", newGuid);
        }

        private XElement GetElementByWorkflowAndStateName(string workflowName,string stateName)
        {
            return xmlFile.Descendants("WorkFlow")
                        .Where(x => x.Attribute("WorkFlowName").Value == workflowName)
                        .Descendants("State").
                        Where(x => x.Attribute("StateName").Value == stateName)
                        .SingleOrDefault();
        }

        private XElement GetElementByWorkflowAndActivitySettings(string workflowName, string activityName,string fromStateName,string toStateName)
        {
            return xmlFile.Descendants("WorkFlow")
                        .Where(x => x.Attribute("WorkFlowName").Value == workflowName)
                        .Descendants("Activity").
                        Where(x => x.Attribute("Name").Value == activityName && (x.Attribute("FromNode").Value == fromStateName ? true : (x.Attribute("FromNode").Value == "ClapperBoard" ? true : false)) && (x.Attribute("ToNode").Value == toStateName))
                        .SingleOrDefault();
        }

        private string GetName(State state)
        {
            if (state != null)
            {
                return state.Name;
            }
            return "";
        }
        #endregion
    }
}
