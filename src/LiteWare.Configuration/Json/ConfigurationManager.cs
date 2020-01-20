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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;

namespace LiteWare.Configuration.Json
{
    /// <summary>
    /// Provide initialization methods and access to the application configuration.
    /// </summary>
    public static class ConfigurationManager
    {
        /// <summary>
        /// The default name of the application configuration file.
        /// </summary>
        public const string DefaultApplicationConfigurationFilename = "app.config.json";

        /// <summary>
        /// The default name of the configuration section in the application configuration file.
        /// </summary>
        public const string DefaultApplicationSettingsSectionName = "ApplicationSettings";

        private static IConfiguration _configuration;

        private static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    Initialize();
                }

                return _configuration;
            }
        }

        /// <summary>
        /// Gets the application configuration file path. Is 'null' if not initialized.
        /// </summary>
        public static string ApplicationConfigurationPath { get; private set; }

        /// <summary>
        /// Gets the application settings section name in configuration file.
        /// </summary>
        public static string ApplicationSettingsSectionName { get; private set; }

        /// <summary>
        /// Gets the application settings section from the application configuration file.
        /// </summary>
        /// <returns><see cref="IConfigurationSection"/>.</returns>
        public static IConfigurationSection AppSettings
            => Configuration.GetSection(ApplicationSettingsSectionName);

        /// <summary>
        /// Gets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public static string GetConfigurationValue(string key)
            => Configuration[key];

        /// <summary>
        /// Sets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <param name="value">The configuration value to set.</param>
        public static void SetConfigurationValue(string key, string value)
            => Configuration[key] = value;

        /// <summary>
        /// Initialize <see cref="AppSettings"/> with the default application configuration filename located in the same directory of the executable.
        /// </summary>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        public static void Initialize(bool optional = false, bool reloadOnChange = true)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Initialize(DefaultApplicationConfigurationFilename, currentDirectory, DefaultApplicationSettingsSectionName, optional, reloadOnChange);
        }

        /// <summary>
        /// Initialize <see cref="AppSettings"/> with the specified application configuration filename located in the same directory of the executable.
        /// </summary>
        /// <param name="applicationConfigurationFilename">Name of the application configuration file.</param>
        /// <param name="applicationSettingsSectionName">The application settings section name</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        public static void Initialize(string applicationConfigurationFilename, string applicationSettingsSectionName = DefaultApplicationSettingsSectionName, bool optional = false, bool reloadOnChange = true)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Initialize(applicationConfigurationFilename, currentDirectory, applicationSettingsSectionName, optional, reloadOnChange);
        }

        /// <summary>
        /// Initialize <see cref="AppSettings"/> with the specified application configuration filename located in the specified directory.
        /// </summary>
        /// <param name="applicationConfigurationFilename">Name of the application configuration file.</param>
        /// <param name="baseDirectory">The absolute path of application configuration file.</param>
        /// <param name="applicationSettingsSectionName">The application settings section name</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        public static void Initialize(string applicationConfigurationFilename, string baseDirectory, string applicationSettingsSectionName, bool optional = false, bool reloadOnChange = true)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile(applicationConfigurationFilename, optional, reloadOnChange);

            ApplicationConfigurationPath = Path.Combine(baseDirectory, applicationConfigurationFilename);
            ApplicationSettingsSectionName = applicationSettingsSectionName;
            _configuration = configurationBuilder.Build();
        }

        /// <summary>
        /// Gets a configuration sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</returns>
        /// <remarks>
        ///     This method will never return <c>null</c>. If no matching sub-section is found with the specified key,
        ///     an empty <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" /> will be returned.
        /// </remarks>
        public static IConfigurationSection GetSection(string key)
            => Configuration.GetSection(key);

        /// <summary>
        /// Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public static IEnumerable<IConfigurationSection> GetChildren()
            => Configuration.GetChildren();

        /// <summary>
        /// Returns a <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> that can be used to observe when this configuration is reloaded.
        /// </summary>
        /// <returns>A <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" />.</returns>
        public static IChangeToken GetReloadToken()
            => Configuration.GetReloadToken();
    }
}