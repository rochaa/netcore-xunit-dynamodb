using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtension
    {
        public static bool IsPhoneValid(this string value)
        {
            Regex telefoneRegex = new Regex(@"^[0-9]*$");

            return !string.IsNullOrEmpty(value) &&
                telefoneRegex.Match(value).Success &&
                (value.Length == 10 || value.Length == 11);
        }

        public static bool IsValidZip(this string value)
        {
            Regex cepRegex = new Regex(@"[0-9]{5}-[0-9]{3}");

            return !string.IsNullOrEmpty(value) && cepRegex.Match(value).Success;
        }

        public static string RemoveDiacritics(this string value)
        {
            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool AllowedValue(this string value, Type type)
        {
            var fields = type.GetFields();
            return fields.Select(f => f.GetValue(null)).Contains(value);
        }

        public static bool IsEmailValid(this string value)
        {
            Regex email = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return !string.IsNullOrEmpty(value) && email.Match(value).Success;
        }
    }
}