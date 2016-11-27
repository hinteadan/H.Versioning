# H's Versioning Library

## Why?
Because I'm sick of version conflicts and complicated versioning mechanisms that depend on external systems like build servers or CI systems. We already have one source of truth regarding the code base, which is the GIT repository, so let's use it.

## What?
One very easy solution for tracking the version of your application. **Zero human input** as the library processes the current version by analyzing the app's GIT repository.


## How?
Install the NuGet:  ```Install-Package H.Versioning```. _(NuGet URL: https://www.nuget.org/packages/H.Versioning)_

In your code, wherever you need the current running version, get it via:
```csharp
var version = H.Versioning.Version.Self.GetCurrent();
```

The library will calculate the current version by analyzing the GIT repository of the running app.

If you have a ```version.txt``` file in the app's root folder, containing the result of ```H.Versioning.Version.Self.GetCurrent().ToString()```, the library will parse the version from there.

This is usefull when you deploy the app and therefore don't have access to the GIT repo.

You can easily generate this file upon deployment by running the following Windows Batch command:
```
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