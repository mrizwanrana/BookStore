using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RizTechBookStore.Helpers
{
    [HtmlTargetElement("big")]
    [HtmlTargetElement(Attributes = "big")]
    public class BigTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h1"; // Replaces <big> with <strong> tag
            output.Attributes.RemoveAll("big"); // Removes the 'big' attribute if it exists
        }
    }
}
