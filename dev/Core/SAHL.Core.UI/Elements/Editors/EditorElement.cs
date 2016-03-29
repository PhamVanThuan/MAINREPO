using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Enums;
using System;

namespace SAHL.Core.UI.Elements.Editors
{
    public class EditorElement : VisualElement
    {
        public EditorElement(BusinessContext businessContext, Type editorModelType, Type editorModelConfigurationType, string title, EditorAction editorAction)
            : base(ElementNames.EditorPrefix, businessContext)
        {
            this.EditorModelType = editorModelType;
            this.EditorModelConfigurationType = editorModelConfigurationType;
            this.EditorTitle = title;
            this.EditorAction = editorAction;
        }

        public Type EditorModelType { get; protected set; }

        public Type EditorModelConfigurationType { get; protected set; }

        public string EditorTitle { get; protected set; }

        public EditorAction EditorAction { get; protected set; }
    }
}