using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.Text;

namespace ToDoWebApi.ContentFormatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var streamWriter = new StreamWriter(context.HttpContext.Response.Body, selectedEncoding, leaveOpen: true);

        await using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));
        await csvWriter.WriteRecordsAsync((IEnumerable)context.Object);
    }
}