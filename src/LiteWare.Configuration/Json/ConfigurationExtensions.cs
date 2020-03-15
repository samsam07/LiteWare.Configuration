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

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LiteWare.Configuration.Json
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets a value from a <see cref="IConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-value pairs.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A value of the type <typeparamref name="T"/>.</returns>
        public static T GetValue<T>(this IConfiguration configuration, string key, ISettingValueConverter<T> valueConverter = null)
            => ConfigurationUtility.GetValue(configuration, key, false, default, valueConverter);

        /// <summary>
        /// Gets a value from a <see cref="IConfiguration"/>. Returns <paramref name="defaultValue"/> if no key was found.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-value pairs.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">Default value to return if no key is found.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A value of the type <typeparamref name="T"/>.</returns>
        public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue, ISettingValueConverter<T> valueConverter = null)
            => ConfigurationUtility.GetValue(configuration, key, true, defaultValue, valueConverter);

        /// <summary>
        /// Gets a list of value from a <see cref="IConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-list of value.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<T> GetValueList<T>(this IConfiguration configuration, string key, ISettingValueConverter<T> valueConverter = null)
            => ConfigurationUtility.GetValueList(configuration, key, false, default, valueConverter);

        /// <summary>
        /// Gets a list of value from a <see cref="IConfiguration"/>. Returns <paramref name="defaultValueList"/> if no key was found.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-list of value.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValueList">Default value list to return if no key is found.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<T> GetValueList<T>(this IConfiguration configuration, string key, IEnumerable<T> defaultValueList, ISettingValueConverter<T> valueConverter = null)
            => ConfigurationUtility.GetValueList(configuration, key, true, defaultValueList, valueConverter);

        /// <summary>
        /// Gets a configuration value from a scope and name.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-value pairs.</param>
        /// <param name="scope">Scope of the configuration.</param>
        /// <param name="name">Name of the scope.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A configuration value of the type <typeparamref name="T"/></returns>
        public static T GetScopeValue<T>(this IConfiguration configuration, string scope, string name, ISettingValueConverter<T> valueConverter = null)
        {
            string key = ConfigurationUtility.MergeToKey(scope, name);
            T rtn = GetValue(configuration, key, valueConverter);

            return rtn;
        }

        /// <summary>
        /// Gets a configuration value from a scope and name. Returns <paramref name="defaultValue"/> if no key was found.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-value pairs.</param>
        /// <param name="scope">Scope of the configuration.</param>
        /// <param name="name">Name of the scope.</param>
        /// <param name="defaultValue">Default value to return if no key is found.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A configuration value of the type <typeparamref name="T"/></returns>
        public static T GetScopeValue<T>(this IConfiguration configuration, string scope, string name, T defaultValue, ISettingValueConverter<T> valueConverter = null)
        {
            string key = ConfigurationUtility.MergeToKey(scope, name);
            T rtn = GetValue(configuration, key, defaultValue, valueConverter);

            return rtn;
        }

        /// <summary>
        /// Gets a configuration value list from a scope and name.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-list of value.</param>
        /// <param name="scope">Scope of the configuration.</param>
        /// <param name="name">Name of the scope.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<T> GetScopeValueList<T>(this IConfiguration configuration, string scope, string name, ISettingValueConverter<T> valueConverter = null)
        {
            string key = ConfigurationUtility.MergeToKey(scope, name);
            IEnumerable<T> rtn = GetValueList(configuration, key, valueConverter);

            return rtn;
        }

        /// <summary>
        /// Gets a configuration value list from a scope and name. Returns <paramref name="defaultValueList"/> if no key was found.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="configuration">Configuration containing a list of key-list of value.</param>
        /// <param name="scope">Scope of the configuration.</param>
        /// <param name="name">Name of the scope.</param>
        /// <param name="defaultValueList">Default value list to return if no key is found.</param>
        /// <param name="valueConverter">A <see cref="ISettingValueConverter{T}"/> that converts configuration value literals to the type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<T> GetScopeValueList<T>(this IConfiguration configuration, string scope, string name, IEnumerable<T> defaultValueList, ISettingValueConverter<T> valueConverter = null)
        {
            string key = ConfigurationUtility.MergeToKey(scope, name);
            IEnumerable<T> rtn = GetValueList(configuration, key, defaultValueList, valueConverter);

            return rtn;
        }
    }
}