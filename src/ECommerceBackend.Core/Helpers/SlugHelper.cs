using System;

namespace ECommerceBackend.Core.Helpers
{
    public static class SlugHelper
    {
        public static string Slugify(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new NullReferenceException("The string to be slugified should not be null or empty");

            Slugify.SlugHelper slugHelper = new Slugify.SlugHelper();

            var slugifiedStr = slugHelper.GenerateSlug(str);

            if (string.IsNullOrEmpty(slugifiedStr))
                throw new Exception($"{str} cannot be slugified");

            if (slugifiedStr.StartsWith('-'))
                slugifiedStr = slugifiedStr.Remove(0, 1);

            if (slugifiedStr.EndsWith('-'))
                slugifiedStr = slugifiedStr.Remove(slugifiedStr.Length - 1);

            return slugifiedStr;
        }
    }
}
