// MIT License
//
// Copyright (c) 2020 Hisham Maudarbocus
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using LiteWare.Configuration.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace LiteWare.Configuration
{
    internal static class ConfigurationUtility
    {
        public static string MergeToKey(string scope, string name)
            => scope + '.' + name;

        private static bool IsNullable(Type type, out Type underlyingType)
        {
            if (!type.IsValueType)
            {
                underlyingType = null;
                return true;
            }

            underlyingType = Nullable.GetUnderlyingType(type);
            bool isNullable = (underlyingType != null);

            return isNullable;
        }

        private static T Convert<T>(string value)
        {
            bool isNullable = IsNullable(typeof(T), out Type nullableUnderlyingType);
            if (value == null && isNullable)
            {
                return default;
            }

            Type conversionType = nullableUnderlyingType ?? typeof(T);
            T result = (T)System.Convert.ChangeType(value, conversionType);

            return result;
        }

        public static T GetValue<T>(NameValueCollection nameValueCollection, string key, bool optional, T defaultValue = default, ISettingValueConverter<T> valueConverter = null)
        {
            string[] values = nameValueCollection.GetValues(key);
            if (values != null)
            {
                string value = values[0];

                T rtn;
                if (valueConverter == null)
                {
                    rtn = Convert<T>(value);
                }
                else
                {
                    rtn = valueConverter.Convert(value);
                }

                return rtn;
            }

            if (optional)
            {
                return defaultValue;
            }

            throw new ConfigurationKeyNotFoundException(key);
        }

        public static IEnumerable<T> GetValueList<T>(NameValueCollection nameValueCollection, string key, bool optional, IEnumerable<T> defaultValueList = default, ISettingValueConverter<T> valueConverter = null)
        {
            string[] values = nameValueCollection.GetValues(key);
            if (values != null)
            {
                IEnumerable<T> rtn;
                if (valueConverter == null)
                {
                    rtn = values.Select(Convert<T>);
                }
                else
                {
                    rtn = values.Select(valueConverter.Convert);
                }

                return rtn;
            }

            if (optional)
            {
                return defaultValueList;
            }

            throw new ConfigurationKeyNotFoundException(key);
        }

        public static T GetValue<T>(IConfiguration configuration, string key, bool optional, T defaultValue = default, ISettingValueConverter<T> valueConverter = null)
        {
            bool hasKey = configuration
                .GetChildren()
                .Any(kv => string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase));
            if (hasKey)
            {
                string value = configuration[key];

                T rtn;
                if (valueConverter == null)
                {
                    rtn = Convert<T>(value);
                }
                else
                {
                    rtn = valueConverter.Convert(value);
                }

                return rtn;
            }

            if (optional)
            {
                return defaultValue;
            }

            throw new ConfigurationKeyNotFoundException(key);
        }

        public static IEnumerable<T> GetValueList<T>(IConfiguration configuration, string key, bool optional, IEnumerable<T> defaultValueList = default, ISettingValueConverter<T> valueConverter = null)
        {
            bool hasKey = configuration
                .GetChildren()
                .Any(kv => string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase));
            if (hasKey)
            {
                IEnumerable<string> values = configuration
                    .GetSection(key)
                    .GetChildren()
                    .Select(c => c.Value);

                IEnumerable<T> rtn;
                if (valueConverter == null)
                {
                    rtn = values.Select(Convert<T>);
                }
                else
                {
                    rtn = values.Select(valueConverter.Convert);
                }

                return rtn;
            }

            if (optional)
            {
                return defaultValueList;
            }

            throw new ConfigurationKeyNotFoundException(key);
        }
    }
}