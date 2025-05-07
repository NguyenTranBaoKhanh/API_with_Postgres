using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using log4net;

namespace Common.Library.Collection.Extension
{
    public static partial class StringExtension
    {
        #region Regex Gen

        [GeneratedRegex("[\\!-/:-?{-~!\" ^ _`\\[\\]「」]")]
        private static partial Regex InvalidCharRegex();
        [GeneratedRegex("[Ç-■]")]
        private static partial Regex NonStandardCharRegex();
        [GeneratedRegex("\\s+")]
        private static partial Regex SpaceCharRegex();
        [GeneratedRegex("\\s")]
        private static partial Regex HyphenRegex();

        #endregion


        #region Private Members

        private static ILog CurrentLog => LogManager.GetLogger(typeof(StringExtension));

        #endregion


        public static string Base64Encode(this string val)
        {
            return val.Base64Encode(Encoding.UTF8);
        }

        public static string Base64Encode(this string val, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(val))
                return val;

            var plainTextBytes = encoding.GetBytes(val);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string val)
        {
            return val.Base64Decode(Encoding.UTF8);
        }

        public static string Base64Decode(this string val, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(val))
                return val;

            var base64EncodedBytes = Convert.FromBase64String(val);
            return encoding.GetString(base64EncodedBytes);
        }

        public static string MaskEmail(this string email)
        {
            try
            {
                if (!email.Contains('@'))
                    return email;

                var emailParts = email.Split('@', StringSplitOptions.RemoveEmptyEntries);

                if (emailParts.Length < 2)
                    return email;

                var sb = new StringBuilder();

                if (emailParts[0].Length > 3)
                    sb.Append(emailParts[0].AsSpan(0, 2));
                else if (emailParts[0].Length > 1)
                    sb.Append(emailParts[0].AsSpan(0, 1));
                else
                    sb.Append(emailParts[0]);

                sb.Append("*****@*****");

                if (emailParts[1].IndexOf('.') > 0)
                {
                    sb.Append(emailParts[1].AsSpan(emailParts[1].IndexOf('.')));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                CurrentLog.Error("Mask email failed", ex);
                return email;
            }
        }

        public static string MaskMobile(this string mobile)
        {
            return mobile.MaskText();
        }

        public static string MaskText(this string textValue)
        {
            try
            {
                if (textValue.Length < 2)
                    return textValue + "********";

                var chars = textValue.ToCharArray();

                return string.Format("{0}********{1}", chars[0], chars[chars.Length - 1]);
            }
            catch (Exception ex)
            {
                CurrentLog.Error("Mask Text failed", ex);
                return textValue;
            }
        }

        public static string FileExtension(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            try
            {
                return Path.GetExtension(fileName).Substring(1);
            }
            catch (Exception ex)
            {
                CurrentLog.Error("Check extension failed", ex);
                var index = fileName.LastIndexOf('.');
                return index < 0 ? string.Empty : fileName.Substring(index);
            }
        }

        public static string UrlEncode(this string val)
        {
            return WebUtility.UrlEncode(val) ?? string.Empty;
        }

        public static bool AsBool(this string val)
        {
            if (bool.TryParse(val, out var boolVal))
                return boolVal;

            return false;
        }

        public static int AsInt(this string val)
        {
            if (int.TryParse(val, out var intVal))
                return intVal;

            return 0;
        }

        public static long AsLong(this string val)
        {
            if (long.TryParse(val, out var longVal))
                return longVal;

            return 0;
        }

        public static bool Same(this string val, string compareVal)
        {
            return val.Equals(compareVal, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string PrettyXml(this string val)
        {
            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(val);

                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    NewLineOnAttributes = true
                };

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                CurrentLog.Error("Format XML failed", ex);
            }

            return val;
        }

        public static string BuildFullName(string firstName, string lastName = "", string middleName = "")
        {
            var name = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(firstName))
                name.AppendFormat("{0} ", firstName);

            if (!string.IsNullOrWhiteSpace(middleName))
                name.AppendFormat("{0} ", middleName);

            if (!string.IsNullOrWhiteSpace(lastName))
                name.Append(lastName);

            if (name.Length == 0)
                name.Append("NA");

            return name.ToString().TrimEnd(' ');
        }

        public static string GenerateSlug(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return val;

            var str = val.RemoveDiacritics().ToLower();

            str = InvalidCharRegex().Replace(str, " "); // invalid chars
            str = NonStandardCharRegex().Replace(str, " "); // invalid chars

            str = SpaceCharRegex().Replace(str, " ").Trim(); // convert multiple spaces into one space   
            str = HyphenRegex().Replace(str, "-"); // hyphens   

            return str;
        }

        public static string RemoveDiacritics(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return val;

            var stFormD = val.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var t in stFormD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(t);

                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
