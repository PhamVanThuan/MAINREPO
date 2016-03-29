using Common.Enums;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Origination
{
    public class AssignInitialConsultant : AssignInitialConsultantControls
    {
        /// <summary>
        /// Assigns the consultant to the case.
        /// </summary>
        /// <param name="User"></param>
        /// <param name="buttonToClick"></param>
        public void AssignConsultant(string User, ButtonTypeEnum buttonToClick)
        {
            string[] UserPieces = User.Split('\\');
            if (UserPieces.Length > 0)
            {
                User = UserPieces[0];
                for (int i = 1; i < UserPieces.Length; i++)
                {
                    User = User + "\\\\" + UserPieces[i];
                }
            }
            Console.WriteLine("{0}", User);
            base.selectUser.Option(new System.Text.RegularExpressions.Regex(@"^[\x20-\x7E]*" + User + "[\x20-\x7E]*$")).Select();

            switch (buttonToClick)
            {
                case ButtonTypeEnum.Submit:
                    base.btnSubmit.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;
            }
        }
    }
}