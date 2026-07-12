using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bookify.Web.Helpers
{
    [HtmlTargetElement("a", Attributes = "active-when")]
    public class ActiveTag : TagHelper
    {
        public string? ActiveWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }


        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ActiveWhen) || ViewContextData is null)
            {
                return Task.CompletedTask;
            }

            var currentController = ViewContextData.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            if(currentController!.Equals(ActiveWhen)) {
                if (output.Attributes.ContainsName("class"))
                    {
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }


            }
            return Task.CompletedTask;



        }
    }
}