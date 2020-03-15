# Overview

**LiteWare.Configuration** is a simple library that allows the retrieval of string setting values as typed values form application configuration file. It does so by extending from `System.Configuration` (for XML application configuration) and `Microsoft.Extensions.Configuration` (for *custom* JSON application configuration).

# Install

The library is available as a [Nuget Package](https://www.nuget.org/packages/LiteWare.Configuration/).

# Usage

A typical code that uses application configuration might look like:

``` cs
string companyName = ConfigurationManager.AppSettings["companyName"];
decimal budget = Convert.ToDecimal(ConfigurationManager.AppSettings["budget"]);

IEnumerable<DateTime> holidays;
string[] holidayValues = ConfigurationManager.AppSettings.GetValues("holidays");
if (holidayValues != null)
{
    holidays = holidayValues.Select(Convert.ToDateTime);
}
```

With LiteWare.Configuration, you end up with the boilerplate code removed:

``` cs
string companyName = ConfigurationManager.AppSettings.GetValue<string>("companyName");
decimal budget = ConfigurationManager.AppSettings.GetValue<decimal>("budget");
IEnumerable<DateTime> holidays = ConfigurationManager.AppSettings.GetValueList<DateTime>("holidays");
```

There are different ways to consume application configuration files depending on the project's framework:

- .NET Framework projects will usually have an XML formatted configuration file (`app.config` or `web.config`) and use `System.Configuration` to access the settings.

- .NET Core or .NET Standard projects on the other hand use a custom JSON formatted configuration file along with `Microsoft.Extensions.Configuration` to access the settings.

LiteWare.Configuration caters for this by providing extension methods for both XML and JSON configuration files.

## XML Application Configuration File

You will need to import `LiteWare.Configuration.Xml` to be able to access the extension methods provided for `System.Configuration`.

``` cs
using LiteWare.Configuration.Xml;
using System.Configuration;

// ...

DateTime processStart = ConfigurationManager.AppSettings.GetValue("process", "start", DateTime.Now);
int processDelay = ConfigurationManager.AppSettings.GetValue("process", "delay", 1000);
```

## JSON Application Configuration File

You will need to import `LiteWare.Configuration.Json` to be able to access the extension methods provided for `Microsoft.Extensions.Configuration`.

``` cs
using LiteWare.Configuration.Json;

// ...

IEnumerable<string> blacklist = ConfigurationManager.AppSettings.GetValueList<string>("firewall.blacklist");
IEnumerable<string> whitelist = ConfigurationManager.AppSettings.GetValueList<string>("firewall", "whitelist");
```

Notice you don't need to import `Microsoft.Extensions.Configuration`. This is because `ConfigurationManager` is provided by `LiteWare.Configuration.Json` and already contain all the required imports. More on this is explained in [ConfigurationManager](#ConfigurationManager) section.

### ConfigurationManager

To keep a consistency with `System.Configuration`, `LiteWare.Configuration.Json` provides an almost similar `ConfigurationManager` class with an `AppSettings` property.

The library expects custom JSON configuration file to have the filename `app.config.json`, but this might not always be desirable. To go around this, you will need to manually call the proper `Initialize` method of the class before calling any `AppSettings` properties.

> Note that if you are using the default location for JSON configuration file, you will need to manually copy the file to your project output directory or set the file's `Copy to Output Directory` to `Copy always` or `Copy if newer`.

Application settings are stored in the `ApplicationSettings` section of the JSON file. Again, this section name can be changed by calling the proper `Initialize` method.

### JSON Configuration File Example

An example of a custom JSON configuration file might look like:

``` json
{
    "ApplicationSettings": {
        "companyName": "LiteWare",
        "budget": 5489562,
        "holidays": [ "20/01/2020", "22/01/2020" ],

        "process.start": "2020-01-19",
        "process.delay": 5000,

        "firewall.blacklist": [
            "10.130.*.*",
            "10.140.1.*",
            "10.140.63.85"
        ],
        "firewall.whitelist": [
            "10.130.45.*",
            "10.130.1.88"
        ]
    }
}
```

# Features

- ## Single-value and Multi-value settings

    A setting key can contain 1 or more values, for instance:
    
    ``` json
    {
        "ApplicationSettings": {
            "singleValue": "123",
            "listOfValues": [
                "value1",
                "value2",
                "value3"
            ]
        }
    }
    ```

    Extraction of these values are done with the methods `GetValue` and `GetValueList` respectively:

    ``` cs
    // Single value extraction
    int singleValue = ConfigurationManager.AppSettings.GetValue<int>("singleValue");

    // Multiple value extraction
    IEnumerable<string> listOfValues = ConfigurationManager.AppSettings.GetValueList<string>("listOfValues");
    ```

- ## Required and Optional settings

    By defalut, when calling the `GetValue` and `GetValueList` methods, the library assumes that the specified setting key is required and present in the application configuration file and will throw a `ConfigurationKeyNotFoundException` if the key is not found.
    
    To make the setting key optional and not throw an error, you need to pass a default value to the methods:

    ``` cs
    // Will throw an exception if setting key 'someString' is not present in the configuration file
    string someString = ConfigurationManager.AppSettings.GetValue<string>("someString");

    // Will return '123' if setting key 'someNumber' is not present in the configuration file
    int someNumber = ConfigurationManager.AppSettings.GetValue("someNumber", 123);
    ```

- ## Scoped settings

    Scoped settings was inspired from Visual Studio Code's setting file and are in the format `"{scope}.{name}"`. They are no different from the normal setting key and only serve as a way to organize (or scope) your settings.

    ``` cs
    // Both lines of code have same result

    // Setting retrieval using the normal key
    int windowSize1 = ConfigurationManager.AppSettings.GetValue<int>("window.size");

    // Setting retrieval using scoped setting convention
    int windowSize2 = ConfigurationManager.AppSettings.GetValue<int>("window", "size");
    ```
    
    > This feature is a work-in-progress and only provide the base functionality described above. Future release may determine the scope automatically based on the calling class.

- ## Custom setting value converter

    The library already provides setting value conversion to some of the most common types like `int`, `long`, `DateTime`, etc... In some cases, you might want to convert the string value to a more complex type or apply some transformation to the data. For that, we use the `ISettingValueConverter` interface to create a derived class and pass it as parameter to fetch a complex setting.

    An example of this is if you want to include database password in your configuration file but don't want to store it as plain text. You might have a JSON configuration like:

    ``` JSON
    {
        "ApplicationSettings": {
            "DataSource": "LiteWare/DB",
            "Username": "dev",
            "Password": "wD2+mxZmuS7jzu0BvbgxBnq7xPigMceTVN8Xw9Ejeik=",
        }
    }
    ```

    To retrieve the decrypted password, you need a derived class of `ISettingValueConverter`:

    ``` cs
    using LiteWare.Configuration;

    public class PasswordSettingValueConverter : ISettingValueConverter<string>
    {
        public string Convert(string valueLiteral)
        {
            string plainPassword = Decrypt(valueLiteral);
            return plainPassword;
        }
    }
    ```

    And finally, to retrieve the decrypted password:

    ``` cs
    PasswordSettingValueConverter passwordConverter = new PasswordSettingValueConverter();
    string password = ConfigurationManager.AppSettings.GetValue<string>("password", passwordConverter);
    ```
