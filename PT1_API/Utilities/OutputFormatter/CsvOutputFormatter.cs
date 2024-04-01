    using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace PT1_API.Utilities.OutputFormatter
{
    public class CsvOutputFormatters : TextOutputFormatter
    {
        public CsvOutputFormatters()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;

            using (var writer = new StreamWriter(response.Body, selectedEncoding))
            {
                // Assuming you have a collection of objects to serialize to CSV
                var data = context.Object as IEnumerable<object>;

                if (data != null)
                {
                    // Write CSV header
                    var header = string.Join(",", GetPropertyNames(data.First().GetType()));
                    writer.WriteLine(header);

                    // Write CSV rows
                    foreach (var item in data)
                    {
                        var values = GetPropertyValues(item);
                        var line = string.Join(",", values);
                        writer.WriteLine(line);
                    }
                }
            }

            return Task.CompletedTask;
        }

        protected override bool CanWriteType(Type? type)
        {
            // Customize this method based on your specific conditions.
            // For example, check if the type is the one you want to handle.
            return true;
        }

        private IEnumerable<string> GetPropertyNames(Type type)
        {
            return type.GetProperties().Select(property => property.Name);
        }

        private IEnumerable<string> GetPropertyValues(object obj)
        {
            var properties = obj.GetType().GetProperties();

            return properties.Select(property =>
            {
                var value = property.GetValue(obj);
                return value != null ? value.ToString() : string.Empty;
            });
        }
    }
}
