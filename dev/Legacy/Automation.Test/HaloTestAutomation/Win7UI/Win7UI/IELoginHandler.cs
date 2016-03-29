namespace Win7BrowserLogin
{
    using ATGTestInput;
    using MS.Win32;
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Automation;
    using System.Windows.Input;

    public class IELoginHandler
    {
        // Time in milliseconds to wait for the application to start.
        private static int MAXTIME = 40000;

        // Time in milliseconds to wait before trying to find the application.
        private static int TIMEWAIT = 8000;

        /// -------------------------------------------------------------------
        /// <summary>
        /// Start IE, obtain an AutomationElement object, and run tests.
        /// </summary>
        /// -------------------------------------------------------------------
        [STAThread]
        public static void LoginToBrowser(IntPtr windowHandle, string UserName, string Password, string URL)
        {
            bool credentialsEntered = false;
            AutomationElement element = AutomationElement.FromHandle(windowHandle);
            Thread.Sleep(2000);
            string t = element.Current.ClassName.ToString();

            Condition windowConditions = new AndCondition(
                                        new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                                        new PropertyCondition(AutomationElement.ClassNameProperty, "#32770")
                                        );
            Condition userTileConditions = new AndCondition(
                                            new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                                            new PropertyCondition(AutomationElement.ClassNameProperty, "UserTile")
                                            );
            Condition Edit_condition = new AndCondition(
                                        new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)
                                        );
            Condition submitButtonConditions = new AndCondition(
                                            new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                                            new PropertyCondition(AutomationElement.AutomationIdProperty, "SubmitButton")
                                            );
            Condition userListConditions = new AndCondition(
                                            new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List),
                                            new PropertyCondition(AutomationElement.AutomationIdProperty, "UserList")
                                            );

            var windowsSecurityPrompt = element.FindFirst(TreeScope.Children, windowConditions);
            var submitButton = windowsSecurityPrompt.FindFirst(TreeScope.Children, submitButtonConditions);
            //userTile exists for Win7/XP
            var userTile = windowsSecurityPrompt.FindFirst(TreeScope.Children, userTileConditions);
            if (userTile != null)
            {
                SetUserNameAndPassword(UserName, Password, Edit_condition, userTile);
                credentialsEntered = true;
            }
            //userList exists for Win8
            var userList = windowsSecurityPrompt.FindFirst(TreeScope.Children, userListConditions);
            if (userList != null && !credentialsEntered)
            {
                //get the listitem
                var credentialProviderTile = userList.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "CredProvUserTile"));
                if (credentialProviderTile != null)
                    SetUserNameAndPassword(UserName, Password, Edit_condition, credentialProviderTile);
            }
            //click the submit button
            submitButton.SetFocus();
            Input.SendKeyboardInput(Key.Enter);
        }

        private static void SetUserNameAndPassword(string UserName, string Password, Condition Edit_condition, AutomationElement item)
        {
            AutomationElementCollection edits = item.FindAll(TreeScope.Children, Edit_condition);
            foreach (AutomationElement edit in edits)
            {
                if (edit.Current.Name.Contains("User name") || edit.Current.AutomationId == "CredProvClearTextEdit")
                {
                    edit.SetFocus();
                    SetValue(edit, UserName);
                }
                if (edit.Current.Name.Contains("Password") || edit.Current.AutomationId == "CredProvPasswordEdit")
                {
                    edit.SetFocus();
                    SetValue(edit, Password);
                }
            }
        }

        public static void SetValue(AutomationElement element, string value)
        {
            IntPtr _hwnd = IntPtr.Zero;

            // Validate arguments / initial setup
            if (value == null)
                throw new ArgumentNullException("value");

            if (element == null)
                throw new ArgumentNullException("element");

            // Get hwnd
            _hwnd = GetWindowHandleFromAutomationElement(element);

            // Sanity check #1: Is window enabled???
            if (!SafeNativeMethods.IsWindowEnabled(_hwnd))
            {
                throw new ElementNotEnabledException();
            }

            // Sanity check #2: Are there styles that prohibit us sending text to this control?
            NativeMethods.HWND hwnd = NativeMethods.HWND.Cast(_hwnd);
            int WindowStyle = SafeNativeMethods.GetWindowLong(hwnd, SafeNativeMethods.GWL_STYLE);

            if (IsBitSet(WindowStyle, NativeMethods.ES_READONLY))
            {
                throw new InvalidOperationException("Cannot set text to a read-only field");
            }

            // Sanity check #3: Is the size of the text we want to set greater than what the control accepts?
            IntPtr result;
            int resultInt;

            IntPtr resultSendMessage = UnsafeNativeMethods.SendMessageTimeout(hwnd, NativeMethods.EM_GETLIMITTEXT, IntPtr.Zero, IntPtr.Zero, 1, 10000, out result);
            int lastWin32Error = Marshal.GetLastWin32Error();

            if (resultSendMessage == IntPtr.Zero)
            {
                throw new InvalidOperationException("SendMessageTimeout() timed out");
            }
            resultInt = unchecked((int)(long)result);

            // A result of -1 means that no limit is set.
            if (resultInt != -1 && resultInt < value.Length)
            {
                throw new InvalidOperationException("Length of text (" + value.Length + ") is greater than upper limit of control (" + resultInt.ToString() + ")");
            }

            // Send the message...!
            result = UnsafeNativeMethods.SendMessageTimeout(hwnd, NativeMethods.WM_SETTEXT, IntPtr.Zero, new StringBuilder(value), 1, 10000, out result);
            resultInt = unchecked((int)(long)result);
            if (resultInt != 1)
            {
                throw new InvalidOperationException("Unable to set the text of the control, text = " + value);
            }
        }

        ///---------------------------------------------------------------------------
        /// <summary>
        /// Gets WindowHandle from an AutomationElement
        /// </summary>
        ///---------------------------------------------------------------------------
        internal static IntPtr GetWindowHandleFromAutomationElement(AutomationElement element)
        {
            if (element == null)
                throw new ArgumentException("Automation Element cannot be null");

            object objHwnd = element.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);
            IntPtr ptr = IntPtr.Zero;

            if (objHwnd is IntPtr)
                ptr = (IntPtr)objHwnd;
            else
                ptr = new IntPtr(Convert.ToInt64(objHwnd, CultureInfo.CurrentCulture));

            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException("Could not get the handle to the element(window)");

            return ptr;
        }

        internal static bool IsBitSet(int flags, int bit)
        {
            return (flags & bit) == bit;
        }
    }
}