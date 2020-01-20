// MIT License
//
// Copyright (c) 2019 Hisham Maudarbocus
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
    /// <summary>
    /// Provides utility functions to retrieve configuration values from <see cref="NameValueCollection"/> and <see cref="IConfiguration"/>.
    /// </summary>
    internal static class ConfigurationUtility
    {
        /// <summary>
        /// Merges <paramref name="scope"/> and <paramref name="name"/> as a configuration key.
        /// </summary>
        /// <param name="scope">Scope of the configuration.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <returns>A merged configuration key.</returns>
        public static string MergeToKey(string scope, string name)
            => scope + '.' + name;

        /// <summary>
        /// Gets a value from a <see cref="NameValueCollection"/> configuration.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="nameValueCollection">Configuration containing a list of key-value pairs.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="optional">
        /// A value representing whether <paramref name="defaultValue"/> should be returned if no key was found.
        /// A <see cref="ConfigurationKeyNotFoundException"/> is thrown if this value is false and no key was found.
        /// </param>
        /// <param name="defaultValue">Default value to be returned when <paramref name="optional"/> is set to true.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A value of the type <typeparamref name="T"/>.</returns>
        public static T GetValue<T>(NameValueCollection nameValueCollection, string key, bool optional, T defaultValue = default, ISettingValueConverter<T> valueConverter = null)
        {
            string[] values = nameValueCollection.GetValues(key);
            if (values != null)
            {
                string value = values[0];

                T rtn;
                if (valueConverter == null)
                {
                    rtn = (T)Convert.ChangeType(value, typeof(T));
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

        /// <summary>
        /// Gets a list of value from a <see cref="NameValueCollection"/> configuration.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="nameValueCollection">Configuration containing a list of key-value pairs or list values.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="optional">
        /// A value representing whether <paramref name="defaultValueList"/> should be returned if no key was found.
        /// A <see cref="ConfigurationKeyNotFoundException"/> is thrown if this value is false and no key was found.
        /// </param>
        /// <param name="defaultValueList">Default value list to be returned when <paramref name="optional"/> is set to true.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A value of the type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> GetValueList<T>(NameValueCollection nameValueCollection, string key, bool optional, IEnumerable<T> defaultValueList = default, ISettingValueConverter<T> valueConverter = null)
        {
            string[] values = nameValueCollection.GetValues(key);
            if (values != null)
            {
                IEnumerable<T> rtn;
                if (valueConverter == null)
                {
                    rtn = values.Select(v => (T)Convert.ChangeType(v, typeof(T)));
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

        /// <summary>
        /// Gets a value from a <see cref="IConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-value pairs.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="optional">
        /// A value representing whether <paramref name="defaultValue"/> should be returned if no key was found.
        /// A <see cref="ConfigurationKeyNotFoundException"/> is thrown if this value is false and no key was found.
        /// </param>
        /// <param name="defaultValue">Default value to be returned when <paramref name="optional"/> is set to true.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A value of the type <typeparamref name="T"/>.</returns>
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
                    rtn = (T)Convert.ChangeType(value, typeof(T));
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

        /// <summary>
        /// Gets a list of value from a <see cref="IConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-value pairs or list values.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="optional">
        /// A value representing whether <paramref name="defaultValueList"/> should be returned if no key was found.
        /// A <see cref="ConfigurationKeyNotFoundException"/> is thrown if this value is false and no key was found.
        /// </param>
        /// <param name="defaultValueList">Default value list to be returned when <paramref name="optional"/> is set to true.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A value of the type <typeparamref name="T"/>.</returns>
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
                    rtn = values.Select(v => (T)Convert.ChangeType(v, typeof(T)));
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