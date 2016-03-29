namespace SAHL.Core.UI.Elements.Parts
{
    public class IconPart : Part
    {
        public IconPart(string iconName)
        {
            this.IconName = iconName;
        }

        public string IconName { get; protected set; }
    }
}