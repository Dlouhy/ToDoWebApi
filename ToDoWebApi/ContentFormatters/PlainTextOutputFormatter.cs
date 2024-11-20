using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace ToDoWebApi.ContentFormatters;

public class PlainTextOutputFormatter : TextOutputFormatter
{
    public PlainTextOutputFormatter()
    {
        SupportedMediaTypes.Add("text/plain");
        SupportedEncodings.Add(Encoding.UTF8);
    }

    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        // Check if the object is a collection
        if (context.Object is IEnumerable<object> collection)
        {
            // Convert each item in the collection to a string (you can customize the ToString behavior)
            var text = string.Join(Environment.NewLine, collection.Select(item => item.ToString()));

            // Write the result to the response
            return context.HttpContext.Response.WriteAsync(text, selectedEncoding);
        }

        // If it's not a collection, just call ToString on the object
        var singleItemText = context.Object.ToString();
        return context.HttpContext.Response.WriteAsync(singleItemText, selectedEncoding);
    }
}