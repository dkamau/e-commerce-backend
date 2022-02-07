using System;
using System.Globalization;
using System.Text;
using System.Data.Entity.Design.PluralizationServices;

namespace ECommerceBackend.Core.Helpers
{
    public static class StringHelper
    {
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return "";

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Pluralize(this string word, int count)
        {
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
            if(count > 1)
                return ps.Pluralize(word);

            return word;
        }
    }
}
