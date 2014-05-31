PostCompile
===========

NuGet package to enable post compilation steps represented in code.

It is released on the NuGet gallery: https://www.nuget.org/packages/PostCompile/


What is it for?
---------------

Ever wanted to extend the build process without annoying configuratons or manipulating project files? Ever wanted to extend the build process with C# code? This NuGet package alows you to easily extend the build process of your projects by simply adding new classes to it.


Usage
-----

1. Add the NuGet package to your project.
2. Add a class that derives from `PostBuildTask` and implement the `RunAsync()`-method.
3. Add `Log.Error()` and `Log.Warning()` calls to your `RunAsync()`-method.
3. Build your project!


How it is working
-----------------

The package contains an executable called **PostCompile.exe**. Upon installing the package your project will be extended by a build target that is executed after each build. Within this build target the added executable will be executed, passing the path to your build assembly and the solution. The executable will then load this assembly and scan it for types implementing the `IPostCompileTask` interface, instanciate an instance of each type and call the `RunAsync`-method.

The type `PostCompileTask` is defined in an assembly named __PostCompile.Common__ that will be added to your project. This assembly is not required at runtime of your project, thus it can be removed from your finished product. The assembly is very lightweight, it only contains two interfaces and an abstract base type. The `PostCompileTask` has a `Log` property that offers `Error()`- and `Warning()`-methods.

The `Error(..)` and `Warning(..)` methods will output am error or warning message that is visible in Visual Studios **Error List** window. Calling `Error(..)` once will cause the build to cancel erroneous, just as you had a compilation error. There are overloads that take reflection objects such as `Type`, `MethodInfo` or `PropertyInfo`. Using these overloads will fill the *File*, *Line* and *Column* columns of the **Error List**, making it easy to navigate to the code by simply double clicking the entry. Microsoft Roslyn is used to parse the code and find the correct locations in your code.

There are also `UsageError(..)` and `UsageWarning(..)` methods, which will output an error or warning for every usage of a specified method, constructor or property - all together with file and line number of the usage.

You can override the `DependsOn` property to provide a list of dependencies, making it easy to run tasks in a specific order when required.

The assembly will modified after all tasks have run and the types of each task will be removed from the assembly. Additionally the added assembly *PostCompile.Common*  will be removed from the *bin**-directory. This way your resulting assembly is kept clean from your build-extensions!

Quick Demo
----------

```C#
public class MyTask : PostCompileTask
{
    public override async Task RunAsync()
    {
        Log.Warning("I appear as a warning.");
        Log.Error("I appear as an error and make the build fail.");
    }
}

public class OtherTask : PostCompileTask
{
	public override IEnumerable<Type> DependsOn
    {
    	get { return new[] { typeof(MyTask) }; }
    }
	
    public override async Task RunAsync()
    {
    	Log.Warning("I run after MyTask, because I depend on it.");
        
        var propertyInfo = typeof(Demo).GetProperty("Test");
        Log.Warning(propertyInfo, "I link directly to the property Test of the Demo class.");
        Log.UsageWarning(propertyInfo, "I will provide a warning for every usage of the property Test.");
    }
}

public class Demo
{
	public int Test { get; set; }
}
```

Examples
--------

I added an example solution named "TypeScriptGenerator". It demonstrates with a very primitive generator how PostCompile can be used to automatically generate TypeScript files from C# data structures during the build step. The *Data* project will create TypeScript code directy into a TypeScript file of the *Web* project. Since the *Web* project depends on the *Data* project, the *Data* project will always be compiled first. So if you remove a property of a C# class that is used in the corrosponding TypeScript structure in the *Web* project the whole compilation will fail.

Usage possibilities
-------------------

This package allows to easily add steps to your compilation process expressed in code.

I had two main usage ideas for why I created this package. The first one would be validation of the code. In an old project it was required to add an attribute above specific methods - something that was often forgotten by employees. Now the assembly could be scanned for those methods, checking if the attribute is present and returning an error message indicating what is wrong.

Another usage idea is code generation. A task would be created that scans the assembly (like a service project) and creates a service proxy directly into a code file of another project. This project would be compiled after the first one, leading to an always up-to-date service proxy.


Future development
------------------

This is an early version, but it works as intended. __Comments, bug reports, ideas or just any form of contribution is highly welcomed!__
