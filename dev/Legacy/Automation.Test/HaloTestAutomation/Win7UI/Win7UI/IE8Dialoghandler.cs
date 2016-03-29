using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using MS.Win32;
using System.Text;
using ATGTestInput;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Input;
using WatiN.Core;
using System.Windows;

namespace Win7BrowserLogin
{
    public class IE8Dialoghandler
    {
        public enum ButtonName
	    {
	         Open=4426,
             Close=4427,
             Cancel=2
	    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="dialogButton"></param>
        public static void ClickFileDownloadDialogButton(Browser browser, ButtonName dialogButton, string hyperlink)
        {            
            AutomationElement at = AutomationElement.FromHandle(browser.hWnd);
            //conditions for finding the window
            Condition findWindows= new AndCondition(new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            //getting the buttons
            Condition findButtons = new AndCondition(new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                                         new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));
            //find all the children
            AutomationElementCollection elements = at.FindAll(TreeScope.Children, findWindows);
            //go through them and find our File Download dialogue
            foreach (AutomationElement child in elements)
            {
                if (child.Current.LocalizedControlType == "Dialog" && child.Current.Name == "File Download")
                {
                    if (HyperlinkExists(child, hyperlink))
                    {
                        var buttons = child.FindAll(TreeScope.Children, findButtons);
                        //we should have the buttons now
                        foreach (AutomationElement button in buttons)
                        {
                            if (int.Parse(button.Current.AutomationId) == (int)dialogButton)
                                Input.MoveToAndClick(child);
                                Input.MoveToAndClick(button);                            
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Checks to see if one of the hyperlinkes with the provided name exists in a child collection of an automation element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="hyperlink"></param>
        /// <returns></returns>
        private static bool HyperlinkExists(AutomationElement element, string hyperlink)
        {
            bool linkExists = false;
            Condition findlinks = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Hyperlink),
                new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            //find the links
            AutomationElementCollection hyperlinks = element.FindAll(TreeScope.Children, findlinks);
            //loop through the links ensuring one matches our
            foreach (AutomationElement link in hyperlinks)
            {
                if (linkExists)
                    break;
                linkExists = link.Current.Name == hyperlink ? true : false;
            }
            return linkExists;
        }
    }
}
