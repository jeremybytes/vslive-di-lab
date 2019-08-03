Leveling Up: Dependency Injection in .NET
==========================================
Visual Studio LIVE! Hand-on Lab  
12 Aug 2019 - Redmond, WA

Description
-----------
Loosely coupled code is easier to maintain, extend, and test. Dependency Injection (DI) helps us get there. In this workshop, we'll see how interfaces can add "seams" to our code that makes it more flexible and maintainable. From there, we'll dig into loose coupling with Dependency Injection. DI doesn't have to be complicated. With just a few simple changes to our constructors or properties, we can have code that is easy to extend and test.

After laying a good foundation, we'll take a closer look by diving into various DI patterns (such as constructor injection and property injection) as well as other patterns that help us handle interception and optional dependencies. Along the way, we'll see how DI helps us adhere to the SOLID principles in our code. We'll also we'll look at common stumbling blocks like dealing with constructor over-injection, managing static dependencies, and handling disposable dependencies.

Throughout the day, we'll go hands-on with labs to give you a chance to put the concepts into action.

If you're a C# developer who wants to get better with concepts like abstraction, loose coupling, extensibility, and unit testing, then this is the workshop for you.

You will learn:

* How and why of dependency injection (DI)
* Software patterns (such as the Decorator) that make features easy to compose
* Getting over common stumbling blocks like over injection and handling disposable dependencies

Prerequisites
-------------
To get the most out of this hand-on lab, you should have an understanding of the basics C# and object oriented programming (classes, inheritance, methods and properties). No prior experience with dependency injection is necessary.

**Hardware & Software**  
To participate in the hands-on portion, be sure to bring the following:
* **Laptop and power adapter** (it will be a long day) 
* **Visual Studio 2019** is recommended.  
You can download the free Community Edition here: https://visualstudio.microsoft.com/downloads/ (Visual Studio 2017 and other editions of Visual Studio 2019 will also work if you already have one installed.)

The following Workloads should be installed with Visual Studio:
* **ASP<i>.</i>NET and web development**
* **.NET desktop development**

In addition, we will use the following:
* **.NET Framework 4.7.2**  
Available here: https://dotnet.microsoft.com/download/dotnet-framework/net472 
 * **.NET Core 2.2 SDK**  
Available here: https://dotnet.microsoft.com/download -- be sure to get the SDK for your version of Visual Studio.

Be sure to double-check versions of .NET that you have installed prior to hands-on lab.

Materials
---------
**Download Note**  
If you download the .zip file from GitHub, you should "unblock" it before unzipping the content.
* Right-click on the ".zip" file
* Choose "Properties"
* Click "Unblock" in the Security section
* Click "OK"

The code contains some compiled code (.dlls and .exes). If you do not unblock the files before using them, Visual Studio gives unhelpful errors.

**Slides**  
Slides are available in the "Dependency Injection Workshop.pdf" file. This is primarily for reference (lots of lists and advice).

**Code Samples**  
The code samples used in the various sections are in the "00" through "05" folders. The samples correpond to the sections of the workshop. The "00-Main Demo" is used throughout the day and has sub-folders showing completed code for various topics and techniques.

**Labs**  
The labs are in the "Lab01" through "Lab04" folders. Each lab contains "Starter" code with the initial solution and "Completed" code with the finished product. The "Lab-Instructions<i>.</i>md" markdown files contain the lab setup, tasks, and step-by-step walkthrough (if needed). These can be viewed easily on GitHub if you do not have a markdown viewer on your machine.

More
----
Get more articles, videos, and code about Dependency Injection at [jeremybytes.com](http://www.jeremybytes.com).