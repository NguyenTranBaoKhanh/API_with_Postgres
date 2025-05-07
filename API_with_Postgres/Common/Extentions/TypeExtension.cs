using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using Newtonsoft.Json;

namespace API_with_Postgres.Common.Extentions
{
    public static class TypeExtension
    {
        public static Dictionary<string, object> ToDict(dynamic val)
        {
            var dictionary = new Dictionary<string, object>();

            if (val == null)
                return dictionary;

            if (val is ExpandoObject)
                dictionary = (Dictionary<string, object>)val;
            else
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(val))
                {
                    var obj = propertyDescriptor.GetValue(val);
                    dictionary.TryAdd(propertyDescriptor.Name, obj);
                }
            }

            return dictionary;
        }

        public static Dictionary<string, string> ToStringDict(dynamic val)
        {
            Dictionary<string, object> dict = ToDict(val);

            if (dict.Count == 0)
                return new Dictionary<string, string>();

            return dict.ToList().ToDictionary(x => x.Key, x => x.Value.ToString());
        }

        public static string ToJson(dynamic val)
        {
            if (val == null)
                return string.Empty;

            return JsonConvert.SerializeObject(ToDict(val));
        }

#pragma warning disable 8632
        public static async Task<T> RunPrivateMethodAsync<T>(this object obj, string methodName, object?[]? pm)
#pragma warning restore 8632
            where T : class
        {
            var method = await Task.Run(() => obj.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance));

            try
            {
                if (method == null)
                    return null;

                return await (Task<T>) method.Invoke(obj, pm);
            }
            catch
            {
                // Error ignore, private method are for exception clean up calls
            }

            return null;
        }

#pragma warning disable 8632
        public static T RunPrivateMethod<T>(this object obj, string methodName, object?[]? pm)
#pragma warning restore 8632
            where T : class
        {
            var method = obj.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                if (method == null)
                    return null;

                return (T) method.Invoke(obj, pm);
            }
            catch
            {
                // Error ignore, private method are for exception clean up calls
            }

            return null;
        }
    }
}
