using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RizTechBookStore.Helpers
{
    public class CustomEmailTagHelper: TagHelper
    {
        public string MyEmail { get; set; } 
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a"; // Replaces <custom-email> with <a> tag
            output.Attributes.SetAttribute("href", $"mailto:{MyEmail}");
            output.Content.SetContent("my-email");
        }
    }
}
