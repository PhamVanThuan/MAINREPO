using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SAHL.Common.Configuration
{
    /// <summary>
    /// This class allows editing and saving of Settings in an app's exe.config file
    /// Do not use unless authorized
    /// </summary>
    public class ConfigHelper
    {
        private string _configPath;
        private XmlDocument _config;
        private XmlNode _settingsNode;

        private Dictionary<string, string> _settings;

        public Dictionary<string, string> Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFilePath">the full path to the exe.config file that will be used by this class</param>
        public ConfigHelper(string configFilePath)
        {
            if (!File.Exists(configFilePath))
                throw new Exception("The specified file does not exist");

            _config = new XmlDocument();
            _configPath = configFilePath;
            _config.Load(_configPath);

            XmlNode appSettings = _config.SelectSingleNode("//applicationSettings");

            if (appSettings == null)
                throw new Exception("The config file does not have an 'applicationSettings' section");

            foreach (XmlNode node in appSettings.ChildNodes)
            {
                if (node.Name.Contains(".Properties.Settings"))
                {
                    _settingsNode = node;
                    break;
                }
            }

            //_appName = _configPath.Remove(0, _configPath.LastIndexOf('\\') + 1);
            //_appName = _appName.Remove(_appName.IndexOf(".exe.config"));

            if (_settingsNode == null)
                throw new Exception("Settings node not found");

            _settings = new Dictionary<string, string>();
            //XmlNodeList nodes = _config.SelectNodes("//setting");

            foreach (XmlNode node in _settingsNode.ChildNodes)
                if (node.InnerText != null)
                    _settings.Add(node.Attributes["name"].InnerText, node.ChildNodes[0].InnerText);
        }

        /// <summary>
        /// Retrieve the current value of a setting
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public string GetSetting(string settingName)
        {
            if (_settings.ContainsKey(settingName))
                return _settings[settingName];

            throw new Exception("Specified setting was not found");
        }

        /// <summary>
        /// Update a setting node. Will add a new node if a match is not found.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        public void SaveSetting(string settingName, string value)
        {
            if (_settings.ContainsKey(settingName))
            {
                _settings[settingName] = value;
                SaveSettings();
            }
            else
                AddSetting(settingName, value);
        }

        /// <summary>
        /// Save _settings to app.exe.config
        /// </summary>
        public void SaveSettings()
        {
            if (_config == null || _settings == null)
                return;

            //XmlNodeList nodes = _config.SelectNodes("//setting");
            bool changed = false;

            foreach (XmlNode node in _settingsNode.ChildNodes)
            {
                string name = node.Attributes["name"].InnerText;

                if (_settings.ContainsKey(name))
                {
                    if (node.ChildNodes[0].InnerText != _settings[name])
                    {
                        node.ChildNodes[0].InnerText = _settings[name];
                        changed = true;
                    }
                }
            }

            if (changed)
                _config.Save(_configPath);
        }

        /// <summary>
        /// Adds a new setting node to the exe.config file
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        public void AddSetting(string settingName, string value)
        {
            if (_config == null || _settings == null)
                return;

            XmlNode node = _config.CreateNode(XmlNodeType.Element, "Setting", null);
            XmlAttribute nameAttr = _config.CreateNode(XmlNodeType.Attribute, "name", null) as XmlAttribute;
            XmlAttribute serializeAsAttr = _config.CreateNode(XmlNodeType.Attribute, "serializeAs", null) as XmlAttribute;
            nameAttr.Value = settingName;
            serializeAsAttr.Value = "String";
            node.Attributes.Append(nameAttr);
            node.Attributes.Append(serializeAsAttr);
            XmlNode val = _config.CreateNode(XmlNodeType.Element, "value", null);
            val.InnerText = value;
            node.AppendChild(val);

            _settingsNode.AppendChild(node);
            _config.Save(_configPath);
            _settings.Add(settingName, value);
        }
    }
}