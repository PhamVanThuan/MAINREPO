namespace SAHL.Core.UI.Elements.Parts
{
    public class StaticTextPart : Part
    {
        public StaticTextPart(string text)
        {
            this.Text = text;
        }

        public string Text { get; protected set; }
    }
}