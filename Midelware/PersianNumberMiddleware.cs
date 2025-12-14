using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace taavoni3.Midelware
{

public class PersianNumberMiddleware
{
    private readonly RequestDelegate _next;

    public PersianNumberMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

            responseBody = ConvertNumbersToPersian(responseBody);

            var responseBytes = System.Text.Encoding.UTF8.GetBytes(responseBody);
            await originalBodyStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }

    private string ConvertNumbersToPersian(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        string[] englishDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] persianDigits = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

        for (int i = 0; i < 10; i++)
        {
            input = Regex.Replace(input, englishDigits[i], persianDigits[i]);
        }

        return input;
    }
}

}