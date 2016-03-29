using Microsoft.VisualStudio.Shell;
using System;

namespace SAHomeloans.SAHL_VSExtensions.Themes
{
    public static class ThemeHelper
    {
        private static Guid EnvironmentGuid = new Guid("624ed9c3-bdfd-41fa-96c3-7c824ea32e3d");
        private static Guid NewProjectDialogGuid = new Guid("c36c426e-31c9-4048-84cf-31c111d65ec0");

        public static ThemeResourceKey EnvironmentProjectDesignerBackgroundGradientBegin
        {
            get
            {
                return new ThemeResourceKey(EnvironmentGuid, "ProjectDesignerBackgroundGradientBegin", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey EnvironmentBackground
        {
            get
            {
                return new ThemeResourceKey(EnvironmentGuid, "EnvironmentBackground", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey NewProjectDialogBackground
        {
            get
            {
                return new ThemeResourceKey(NewProjectDialogGuid, "Background", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey NewProjectDialogWonderbarTreeInactiveSelected
        {
            get
            {
                return new ThemeResourceKey(NewProjectDialogGuid, "WonderbarTreeInactiveSelected", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey NewProjectDialogContentSelected
        {
            get
            {
                return new ThemeResourceKey(NewProjectDialogGuid, "ContentSelected", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey NewProjectDialogWonderbar
        {
            get
            {
                return new ThemeResourceKey(NewProjectDialogGuid, "Wonderbar", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey NewProjectDialogContentBackground
        {
            get
            {
                return new ThemeResourceKey(NewProjectDialogGuid, "Content", ThemeResourceKeyType.BackgroundBrush);
            }
        }

        public static ThemeResourceKey NewProjectDialogContentForeground
        {
            get
            {
                return new ThemeResourceKey(NewProjectDialogGuid, "Content", ThemeResourceKeyType.ForegroundBrush);
            }
        }

        //Microsoft.VisualStudio.Shell.ThemeResourceKey
    }
}