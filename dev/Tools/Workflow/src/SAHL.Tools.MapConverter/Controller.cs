using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SAHL.Tools.MapConverter.Commands;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.MapConverter
{
    class Controller
    {
        #region variables
        
        private CommandLineArguments commands;
        private DBController dbController;
        private ScriptController scriptController;
        #endregion

        #region constuctor
        public Controller(Commands.CommandLineArguments commands)
        {
            this.commands = commands;
            scriptController = new ScriptController();
            dbController = new DBController();

            DirectoryInfo info = new DirectoryInfo(commands.RootDirectory);
            FileInfo[] mapFiles = info.GetFiles("*.x2p",SearchOption.AllDirectories);
            foreach (FileInfo file in mapFiles.AsEnumerable().Where(x => x.Directory.Parent.FullName == commands.RootDirectory))
            {
                ProcessFiles(file);
            }

            scriptController.SaveScriptsToFile();
            Console.WriteLine("Completed!");
            Console.ReadLine();
        }
        #endregion

        #region methods
        public void ProcessFiles(FileInfo file)
        {
            scriptController.InsertHeader(file.Name);
            MapController map = new MapController(file,dbController,scriptController);
            map.Backup();
            map.ProcessStatesFromDB();
            map.ProcessStatesFromFile();

            map.ProcessActivitiesFromDB();
            map.ProcessActivitiesFromFile();

            map.Save();
        }
        #endregion
    }
}
