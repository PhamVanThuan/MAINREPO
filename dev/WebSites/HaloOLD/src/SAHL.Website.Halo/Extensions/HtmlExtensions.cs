using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using SAHL.Website.Halo;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString DisplayForModel<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var fullTypeName = modelMetaData.Model != null ? modelMetaData.Model.GetType().FullName : typeof(TValue).FullName;
            return System.Web.Mvc.Html.DisplayExtensions.DisplayFor<TModel, TValue>(html, expression, fullTypeName);
        }

        public static MvcHtmlString EditorForModel<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string inputLength = "input-medium")
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var fullTypeName = modelMetaData.Model != null ? modelMetaData.Model.GetType().FullName : typeof(TValue).FullName;
            if (modelMetaData.IsComplexType || modelMetaData.ModelType == typeof(DateTime))
            {
                return System.Web.Mvc.Html.EditorExtensions.EditorFor<TModel, TValue>(html, expression, fullTypeName);
            }
            return html.TextBoxFor(expression, new { @class = inputLength });
        }

        public static MvcHtmlString EditorLabelModelPairFor<TModel, TValue>(this HtmlHelper<TModel> html, string label, Expression<Func<TModel, TValue>> modelExpression, bool hideIfModelIsNull = false, string inputLength = "input-medium", object htmlAttributes = null)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(modelExpression, html.ViewData);
            if (hideIfModelIsNull && modelMetaData.Model == null)
            {
                return new MvcHtmlString(String.Empty);
            }

            var controlGroupTag = new TagBuilder("div");
            controlGroupTag.AddCssClass("label-model-pair");
            controlGroupTag.AddCssClass("control-group");

            var labelTag = new TagBuilder("div");
            labelTag.AddCssClass("model-label control-label");
            labelTag.SetInnerText(label);

            controlGroupTag.InnerHtml = labelTag.ToString();

            var controlsTag = new TagBuilder("div");
            controlsTag.AddCssClass("controls");

            MvcHtmlString modelTag;
            if (modelMetaData.IsComplexType || modelMetaData.ModelType == typeof(DateTime))
            {
                modelTag = EditorExtensions.EditorFor(html, modelExpression, htmlAttributes);
            }
            else
            {
                modelTag = html.TextBoxFor(modelExpression, new { @class = inputLength });
            }
            controlsTag.InnerHtml = modelTag.ToString();
            controlGroupTag.InnerHtml += controlsTag;

            return new MvcHtmlString(controlGroupTag.ToString());
        }

        public static MvcHtmlString EditorLabelDropdownListPairFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return EditorLabelDropdownListPairFor(html,
                                                  ExpressionHelper.GetExpressionText(expression).PutSpacesInBetweenCapitals(),
                                                  expression,
                                                  selectList);
        }

        public static MvcHtmlString EditorLabelDropdownListPairFor<TModel, TProperty>(this HtmlHelper<TModel> html, string label, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var controlGroupTag = new TagBuilder("div");
            controlGroupTag.AddCssClass("label-model-pair");
            controlGroupTag.AddCssClass("control-group");

            var labelTag = new TagBuilder("div");
            labelTag.AddCssClass("model-label control-label");
            labelTag.SetInnerText(label);

            controlGroupTag.InnerHtml = labelTag.ToString();

            var controlsTag = new TagBuilder("div");
            controlsTag.AddCssClass("controls");
            var modelInputElement = SelectExtensions.DropDownListFor(html, expression, selectList, html.ViewData);

            controlsTag.InnerHtml = modelInputElement.ToString();
            controlGroupTag.InnerHtml += controlsTag;

            return new MvcHtmlString(controlGroupTag.ToString());
        }

        public static MvcHtmlString DisplayLabelModelPairFor<TModel, TValue>(this HtmlHelper<TModel> html, string label, Expression<Func<TModel, TValue>> modelExpression, bool hideIfModelIsNull = false)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(modelExpression, html.ViewData);
            return DisplayLabelModelPair(html, label, modelMetaData.Model == null ? String.Empty : modelMetaData.Model.ToString(), hideIfModelIsNull);
        }

        public static MvcHtmlString DisplayLabelModelPairFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> labelExpression, Expression<Func<TModel, TValue>> modelExpression, bool hideIfModelIsNull = false)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(modelExpression, html.ViewData);
            var labelMetaData = ModelMetadata.FromLambdaExpression(labelExpression, html.ViewData);
            return DisplayLabelModelPair(html, labelMetaData.Model == null ? String.Empty : labelMetaData.Model.ToString().PutSpacesInBetweenCapitals(),
                                                   modelMetaData.Model == null ? String.Empty : modelMetaData.Model.ToString(), hideIfModelIsNull);
        }

        public static MvcHtmlString DisplayLabelModelPair(this HtmlHelper html, string label, string model, bool hideIfModelIsNull = false)
        {
            if (hideIfModelIsNull && String.IsNullOrEmpty(model))
            {
                return new MvcHtmlString(String.Empty);
            }
            var labelModelTag = new TagBuilder("div");
            labelModelTag.AddCssClass("label-model-pair");

            var labelTag = new TagBuilder("div");
            labelTag.AddCssClass("model-label");
            labelTag.SetInnerText(String.Format("{0}:", label));

            labelModelTag.InnerHtml = labelTag.ToString();

            var modelTag = new TagBuilder("div");
            modelTag.AddCssClass("model");
            modelTag.SetInnerText(String.IsNullOrEmpty(model) ? "-" : model);

            labelModelTag.InnerHtml += modelTag.ToString();

            return new MvcHtmlString(labelModelTag.ToString());
        }

        public static MvcHtmlString TitleFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) where TModel : class
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return Title(html, modelMetaData.Model == null ? String.Empty : modelMetaData.Model.ToString());
        }

        public static MvcHtmlString Title(this HtmlHelper html, string title)
        {
            var titleTag = new TagBuilder("h1");
            titleTag.SetInnerText(title);
            return new MvcHtmlString(titleTag.ToString());
        }

        public static MvcHtmlString TitleWithDescriptionFor<TModel, TDescription, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> titleExpression, Expression<Func<TModel, TDescription>> descriptionExpression) where TModel : class
        {
            var titleMetaData = ModelMetadata.FromLambdaExpression(titleExpression, html.ViewData);
            var descriptionMetaData = ModelMetadata.FromLambdaExpression(descriptionExpression, html.ViewData);

            return TitleWithDescription(html, titleMetaData.Model == null ? String.Empty : titleMetaData.Model.ToString(),
                                                  descriptionMetaData.Model == null ? String.Empty : descriptionMetaData.Model.ToString());
        }

        public static MvcHtmlString TitleWithDescription(this HtmlHelper html, string title, string description)
        {
            var titleTag = new TagBuilder("h1");
            titleTag.SetInnerText(title);

            var descriptionTag = new TagBuilder("h4");
            descriptionTag.SetInnerText(description);

            return new MvcHtmlString(titleTag.ToString() + descriptionTag.ToString());
        }

        public static MvcHtmlString DisplayNotificationIconFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string notificationType = "warning", string icon = "warning-sign") where TModel : class
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            if (modelMetaData.Model == null)
                return new MvcHtmlString("");
            return DisplayNotificationIcon(html, modelMetaData.Model.ToString(), notificationType, icon);
        }

        public static MvcHtmlString DisplayNotificationIcon(this HtmlHelper html, string warning, string notificationType = "warning", string icon = "warning-sign")
        {
            var badgeTag = new TagBuilder("span");
            badgeTag.AddCssClass("badge");
            badgeTag.AddCssClass("badge-" + notificationType);

            var iconTag = new TagBuilder("i");
            iconTag.AddCssClass("icon");
            iconTag.AddCssClass("icon-" + icon);
            iconTag.Attributes.Add("title", warning);

            StringBuilder sb = new StringBuilder();
            sb.Append(badgeTag.ToString(TagRenderMode.StartTag));
            sb.Append(iconTag.ToString());
            sb.Append(badgeTag.ToString(TagRenderMode.EndTag));

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcControlGroup BeginControlGroup(this HtmlHelper html, string title)
        {
            TagBuilder controlGroupTagBuilder = new TagBuilder("div");
            controlGroupTagBuilder.AddCssClass("control-group");

            var labelTag = new TagBuilder("div");
            labelTag.AddCssClass("control-label");
            labelTag.InnerHtml = title;

            var controlsTagBuilder = new TagBuilder("div");
            controlsTagBuilder.AddCssClass("controls");

            html.ViewContext.Writer.Write(controlGroupTagBuilder.ToString(TagRenderMode.StartTag));
            html.ViewContext.Writer.Write(labelTag.ToString());
            html.ViewContext.Writer.Write(controlsTagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcControlGroup(html.ViewContext);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<KeyValuePair<int, string>> items, IEnumerable<int> checkedItems = null)
        {
            IEnumerable<SelectListItem> selectListItems = items.ToCheckBoxListSource(checkedItems);

            var output = new StringBuilder();
            output.Append(@"<div class=""checkboxList"">");

            foreach (SelectListItem selectListItem in selectListItems)
            {
                output.Append(name.CreateHtmlCheckBoxItemFromSelectListItem(selectListItem));
            }
            output.Append("</div>");
            return new MvcHtmlString(output.ToString());
        }

        public static MvcHtmlString CheckBoxItem(this HtmlHelper helper, string name, string item, string label = null, bool selected = false)
        {
            if (string.IsNullOrEmpty(label))
                return new MvcHtmlString(name.CreateHtmlCheckBoxItemFromSelectListItem(new SelectListItem() { Text = item, Selected = selected }));
            else
            {
                var labelTag = new TagBuilder("div");
                labelTag.AddCssClass("model-label");
                labelTag.SetInnerText(String.Format("{0}: ", label));

                return new MvcHtmlString(labelTag.ToString() + name.CreateHtmlCheckBoxItemFromSelectListItem(new SelectListItem() { Text = item, Selected = selected }));
            }
        }
    }

    public static class ControlGroupExtensions
    {
        internal static void EndControlGroup(ViewContext viewContext)
        {
            viewContext.Writer.Write("</div></div>");//end the control-group and controls tag
        }
    }

    public class MvcControlGroup : IDisposable
    {
        private readonly ViewContext viewContext;
        private bool disposed;

        public MvcControlGroup(ViewContext viewContext)
        {
            this.viewContext = viewContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                ControlGroupExtensions.EndControlGroup(viewContext);
            }
        }

        public void EndControlGroup()
        {
            Dispose(true);
        }
    }
}