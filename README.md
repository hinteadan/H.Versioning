# H's Versioning Library for .NET apps

## Why?
Because I'm tired of version conflicts and hard-to-maintain versioning mechanisms that depend on external systems like build servers or CI systems. We already have one source of truth regarding the code base, which is the GIT repository, so let's use it.

## What?
One very easy solution for tracking the version of your application. **Zero human input** as the library processes the current version by analyzing the app's GIT repository.


## How?
Install the NuGet:  ```Install-Package H.Versioning```. _(NuGet URL: [https://www.nuget.org/packages/H.Versioning](https://www.nuget.org/packages/H.Versioning))_

In your code, wherever you need the current running version, get it via:

```csharp
var version = H.Versioning.Version.Self.GetCurrent();
```

The library will calculate the current version by analyzing the GIT repository of the running app.

If you have a ```version.txt``` file in the app's root folder, containing the result of:

```csharp
H.Versioning.Version.Self.GetCurrent().ToString()
```

then the library will parse the version from there.

Alternatively, you can specify the path of the ```version file``` in a config entry with the key ```H.Versioning.VersionFile``` or in code via ```FileVersionProviderSettings.Default.VersionFilePath```. Examples below:


1. Application configuration file (_web.config_ or _app.config_):
```xml
<appSettings>
   <add key="H.Versioning.VersionFile" value="C:\Path\to\version.txt" />
</appSettings>
```

2. Programmatically:
```csharp
FileVersionProviderSettings.Default.VersionFilePath = Server.MapPath("~/version.txt");
```


This is usefull when you deploy the app and therefore don't have access to the GIT repo.

You can easily generate this file upon deployment by running the following Windows Batch command:

```powershell
.\H.Versioning.Cli.exe >> version.txt
```

The **H.Versioning.Cli.exe** utility is included in the library's nuget and you'll see it in your build output folder.

So basically... that's it! Easy, consistent, versioning!


### Semantic tags

The version number is built by parsing the [semantic version tags](http://semver.org/) in your git repository.
If you are not tagging your releases I strongly suggest you start doing it because it's kind of the norm.

So this is the source for the **Major, Minor and Patch** numbers.

In addition to these, it also calculates the **Build number** by counting the number of commits between the **latest tag** _(considered to be the latest released version)_ and the current **HEAD**. This way you get a specific, unique version per feature or bugfix, which is pretty awesome. 

If you don't do semantic tagging you still get a consistent version but it will look like **0.0.0.42**, which is not so cool.

Alongside the version number, I also expose some additional useful info like the exact **commit hash**, the **branch** and the GIT **timestamp** of the current HEAD so you can easily track back to code the running version of your app.


### Custom version number parsers

Starting with version **v1.1.0** I added the ability to define custom version number parsers. This is useful if you have a custom versioning format.


To use this feature do one of the following:


1. Approach **A**:

   a. Specifiy a parsing function, easiest way via a lambda: 
   
   ```csharp
   Version.UseParser(v => new VersionNumber(int.Parse(v.Substring(0, 1)), int.Parse(v.Substring(1))));
   ```
   
2. Approach **B**:

   a. Write your own custom class that implements the interface: 
   
   ```csharp
   H.Versioning.VersionNumberParsers.ICanParseVersionNumber
   ```

   b. Tell H.Versioning to use it: 
   
   ```csharp
   Version.UseParser(MyCustomVersionNumberParser);
   ```


The parsers are internally stored in a stack, therefore their priority is **Last In, First Out**.

The library will use the first parsing result that succeeds. If all of the registered parsers fail, it will throw an ```AggregateException``` containing the exceptions thrown by each parser.


### Ignore some tags

With version **1.2.0** you can ignore specific annotated tags by providing a predicate.

This is for scenarios where you create tags for quick reference but they are not actual Release tags.


Example:

```csharp
H.Versioning.Version.IgnoreTag(t => t.StartsWith("x")); //Ignore tags that start with "x"
```

[![Analytics](https://ga-beacon.appspot.com/UA-91841249-2/home)](https://github.com/igrigorik/ga-beacon)
