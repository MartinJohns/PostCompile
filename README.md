PostCompile
===========

NuGet package to enable post compilation steps represented in code.


Usage
-----

1. Add NuGet package to your project (.NET, Silverlight, Store apps).
2. Adding classes to your project deriving from __PostCompileTask__.
3. Build your project!


How it is working
-----------------

The package contains an executable called **PostCompile.exe**. Upon installing the package your project will be extended by two build targets - one executed after building and one before cleaning. Within these build targets the added executable will be executed, passing the path to your build assembly as well as the step that is being executed (build or clean). The executable will then load this assembly and scan it for types deriving from __PostCompileTask__, instanciate an instance of each type and call the requested method.

The type __PostCompileTask__ is defined in an assembly named __PostCompile.Common__ that will be added to your project. This assembly is not required at runtime of your project, thus it can be removed from your finished product. The **Common**-assembly is a portable library, so it can be used on any kind of projects. The **Run**- and the **Clean**-methods get an instance implementing the __IUtils__ interface. This object can be used to output **warnings** or **error** messages, as well as access your local file system - something that would otherwise not be possible from a Silverlight or Store project.

The __Error(..)__ and __Warning(..)__ methods will output a error or warning message that is visible in Visual Studios **Error List** window. Calling __Error(..)__ once will cause the build or clean to cancel erroneous, just as you had a compilation error.


Usage possibilities
-------------------

This package allows to easily add steps to your compilation process expressed with code.

I had two main usage ideas for why I created this package. The first one would be validation of the code. In an old project it was required to add an attribute above specific methods - something that was often forgotten by employees. Now the assembly could be scanned for those methods, checking if the attribute is present and returning an error message indicating what is wrong.

Another usage idea is code generation. A task would be created that scans the assembly (like a service project) and creates a service proxy directly into a code file of another project. This project would be compiled after the first one, leading to an always up-to-date service proxy.


Future development
------------------

This is a very first version and it is still being developed. __Comments, bug reports, ideas or just any form of contribution is highly welcomed!__

I am not satisfied with the idea of the __IUtils__ interface, but I saw no other way to make the mentioned usage cases possible. Especially in .NET projects it's a bother that you can't access the entire .NET framework. Perhaps I will split up the **Common**-assembly into two - one for .NET and one as a portable version.

Another bother is the overloads for __Error(..)__ and __Warning(..)__ that accept a file, a line and a column number. I understand that this is mostly pointless in code that changes so often, but I added this options anyway - just because they're possible. One possibility to extend this functionality is to extend the **PostCompile**-program using **Mono.Cecil**. Using **Cecil** it would be possible to load the debug information (pdb files) of assemblies and provide overloads for __Error(..)__ and __Warning(..)__ accepting objects like __Type__, __MethodInfo__ or __PropertyInfo__ and figuring out the file and the line/column number automatically. This of course would only work when debug information is available.
