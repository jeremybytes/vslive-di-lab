Lab 02 - TDDing into New Functionality with DI
===============================================

Objectives
-----------
Dependency injection can make unit testing easier by isolating functionality. In this lab, you will use Test-Driven Development (TDD) to implement functionality. Along the way, property injection will help with make both the tests and the code easy to read and write.

Application Overview
---------------------

The "Starter" folder contains the code files for this lab. Open the "SunsetTDD" solution.

The "Sunset.Interface" project contains an interface that we want to implement:

```c#
    public interface ISunsetCalculator
    {
        DateTime GetSunset(DateTime date);
        DateTime GetSunrise(DateTime date);
    }
```

Data for this method can be obtained from a .NET Core WebAPI service in the "SolarCalculator.Service" project. The service can be started from Visual Studio by selecting "Debug | Start Without Debugging" from the menu (Ctrl+F5). *Note: If you are prompted to trust an SSL certificate, just answer "No". The service is configured to run without SSL.*

Alternately, the service can be started from the command line by navigating to the "SolarCalculator.Service" folder and typing "dotnet run".

The service can be reached with the following URI:

http://localhost:8973/api/SolarCalculator/47.6429/-122.1277/2019-08-13

Parameters include latitude, longitude, and date. The service produces a JSON result with the following format:

```json
{"results":{"sunrise":"6:01:04 AM","sunset":"8:25:51 PM","solar_noon":"1:13:28 PM","day_length":"14:24:46.7200000"},"status":"OK"}
```

*Note: The times provided are local time.*

If the parameters are out-of-range, the service produces the following result:

```json
    {"results":null,"status":"ERROR"}
```

Lab Goals
----------

Using TDD (Red-Green-Refactor), create an implementation of the "ISunsetCalculator" interface that uses the SolarCalculator service for data. Along the way, we'll use Dependency Injection to add a seam so that we can swap out the real service call for a fake service call in the tests.

*Note: Since we're focusing on DI, we'll be creating tests for the "happy path". Additional tests should be added to cover expected failure paths as well.*


Project Setup
--------------

In addition to the service, the following stubs have already been provided in the solution.

* Sunset.Library project / SunsetCalculator class
This class is a stub that will be used to implement the desired behavior.

```c#
    public class SunsetCalculator
    {
        public DateTime GetSunrise(DateTime date)
        {
            throw new NotImplementedException();
        }

        public DateTime GetSunset(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
```

* Sunset.Library project / SolarService class  
This class wraps the call to the service. This will be used by the "SunsetCalculator" class.

* Sunset.Library.Tests project  
This project contains the tests that will be used to build the code. The tests are commented out so that they can be added one test/step at a time. This project uses NUnit for unit testing. In addition, the NUnit test adapter has also be installed. The unit tests will show in the "Test Explorer" window in Visual Studio. If this window is not already showing, it is accessible from "Test | Windows | Test Explorer" on the menu.

* Sunset.Library.Tests project / SunsetCalculatorTests class  
This is the test class. It contains a "goodResults" string that can be used for tests as well as an initial failing test.

The initial test is to ensure that the class-under-test implements the required interface: "ISunsetCalculator".

* Sunset.Library.Tests project / GoodService class
This is a fake service caller that corresponds to the "SolarService" class mentioned above. This can be used for testing the "happy path".


Hints
------

* The task can be broken down into the following parts: (1) calling the service, (2) parsing the appropriate string from the JSON result, (3) converting the string to a DateTime.

* Start with the easy bits (converting a string to a DateTime), and work your way outward. (These steps are numbered in the "GetSunset" method of the starter project).

* Parsing the JSON results can be accomplished with the Newtonsoft.Json package: 

```c#
    dynamic data = JsonConvert.DeserializeObject(serviceData);
```

* A variety of techniques can be used for isolation and testing. This about appropriate interfaces and dependency injection patterns that can help keep code loosely coupled to make testing easier.

Step-by-step instructions are included below. The step-by-step instructions start with the implementation of "GetSunset" (and its constituent parts). If you need more assistance, keep reading. 
Otherwise, **STOP READING NOW**


Test #1: Implementing the ISunsetCalculator Interface
------------------------------------------------------

1. The first test is already created.

```c#
    [Test]
    public void SunsetCalculator_ImplementsISunsetCalculator()
    {
        var calculator = new SunsetCalculator();
        Assert.IsInstanceOf(typeof(ISunsetCalculator), calculator);
    }
```

2. Run the test and verify the failure in the Test explorer

3. Update the "SunsetCalculator" class so that it implements the "ISunsetCalculator" interface.

```c#
    public class SunsetCalculator : ISunsetCalculator
```

*Note: This is the only update that needs to be made since the members of the interface (GetSunrise and GetSunset) are already included in the class.*

4. Run the test and verify that the test now passes.


Test #2: Convert Time String to DateTime value
-----------------------------------------------

1. The next test is for  a new method to parse a local time string (such as "8:25:51 PM") into a DateTime (such as 2019-08-13T20:25:51"). We can create this as a static method on the SunsetCalculator class.

```c#
    [Test]
    public void ConvertToLocalTime_OnValidTimeString_ReturnsDateTime()
    {
        string inputTime = "8:25:51 PM";
        DateTime date = new DateTime(2019, 08, 13);
        DateTime expected = new DateTime(2019, 08, 13, 20, 25, 51);

        DateTime result = SunsetCalculator.ToLocalTime(inputTime, date);

        Assert.AreEqual(expected, result);
    }
```

*Note: Compiling now will result in a build failure. The "ToLocalTime" method does not yet exist.*

2. Implement the "ToLocalTime" method. Visual Studio will help out. Click on "ToLocalTime" and use the "lightbulb" or "Ctrl+." then select "Generate method..." 

```c#
    public static DateTime ToLocalTime(string inputTime, DateTime date)
    {
        throw new NotImplementedException();
    }
```

The code will now compile, but the test will fail.

3. Fill in the code for "ToLocalTime" to do the conversion. Here is a possible implementation:

```c#
    public static DateTime ToLocalTime(string inputTime, DateTime date)
    {
        DateTime time = DateTime.Parse(inputTime);
        DateTime result = date.Date + time.TimeOfDay;
        return result;
    }
```

4. Run the test and verify that the test now passes.

As noted above, we're concerned with the "happy path" for this lab. Additionl tests can/should be added to cover bad inputs and appropriate error handling.


Test #3: Parse the Time String from the JSON Data
--------------------------------------------------

1. The next step will be to parse the "sunset" value from the JSON data that returns from the service. The test class already contains a sample string that we can use for parsing.

```json
    string goodResult = "{\"results\":{\"sunrise\":\"6:01:04 AM\",\"sunset\":\"8:25:51 PM\",\"solar_noon\":\"1:13:28 PM\",\"day_length\":\"14:24:46.7200000\"},\"status\":\"OK\"}";
```

*Note: If you need to add escape characters to a string (such as all the escaped double-quote characters here), check out FreeFormatter.com.*

https://www.freeformatter.com/java-dotnet-escape.html

2. In the test, we can use a "ParseSunsetTime" method on the SunsetCalculator class. This will take the JSON string and return the "sunset" value.

```c#
    [Test]
    public void ParseSunsetTime_OnValidData_ReturnsExpectedString()
    {
        string expected = "8:25:51 PM";

        string result = SunsetCalculator.ParseSunsetTime(goodResult);

        Assert.AreEqual(expected, result);
    }
```

3. The result is a build failure since "ParseSunsetTime" does not yet exist. As above, click on "ParseSunsetTime" and use "Ctrl+." to have Visual Studio generate the method.

4. The code now compiles, but the test fails with a "NotImplementedException".

5. Implement code to parse the JSON value. For simplicity, the sample code uses "dynamic". This removes compile-time checking (and code-completion in the IDE), but it allows us to get the value without creating a complete class.

```c#
    public static string ParseSunsetTime(string goodResult)
    {
        dynamic data = JsonConvert.DeserializeObject(goodResult);
        string sunsetTimeString = data.results.sunset;
        return sunsetTimeString;
    }
```

6. Run the test and verify that the test now passes.

7. The parameter name generated by Visual Studio ("goodResult") could be better. We'll refactor and rename the parameter to "jsonData".

```c#
    public static string ParseSunsetTime(string jsonData)
    {
        dynamic data = JsonConvert.DeserializeObject(jsonData);
        string sunsetTimeString = data.results.sunset;
        return sunsetTimeString;
    }
```

8. Run the test and verify that the test still passes.


Test #4: Running "GetSunset"
-----------------------------

The next test will run the "GetSunset" method. Before adding the actual service call, we'll take the approach of hard-coding our sample data to make sure that our tests and code paths are working. The we can add the actual service call, and then work on keeping loose coupling with Dependency Injection. Here's the path we'll take:

* Part 1 - Use hard-coded data to get the tests and general code path working.
* Part 2 - Add the actual service call. This will break the test.
* Part 3 - Use DI to add a seam to code. This allows us to swap out the actual service call.
* Part 4 - Use a fake object in the tests to get things working again.

This will be a bit disconcerting since the test will fail during Part 2 & Part 3. But we can use these failing tests to make sure that we're still on the right track.


Test #4 - Part 1: Running "GetSunset" with Hard-Coded Data
-----------------------------------------------------------

1. The next test calls "GetSunset" on the SunsetCalculator class. We'll use an input date of 2019-08-13 (the date of our sample data), and an expected output DateTime of 2019-08-13T20:25:51 (also from the sample data).

```c#
    [Test]
    public void GetSunset_WithValidDate_ReturnsExpectedDateTime()
    {
        DateTime inputDate = new DateTime(2018, 09, 18);
        DateTime expected = new DateTime(2018, 09, 18, 18, 51, 57);

        var calculator = new SunsetCalculator();

        DateTime result = calculator.GetSunset(inputDate);

        Assert.AreEqual(expected, result);
    }
```

2. This code will build (since GetSunset already exists), but the test will fail with a "NotImplementedException".

3. Implement the GetSunset method by calling the "ParseSunsetTime" and "ToLocalTime" methods created earlier. Rather than calling the service, we'll use our sample data in the code.

```c#
    public DateTime GetSunset(DateTime date)
    {
        // Step 3a: hardcoded-data (temporary)
        string goodResult = "{\"results\":{\"sunrise\":\"6:01:04 AM\",\"sunset\":\"8:25:51 PM\",\"solar_noon\":\"1:13:28 PM\",\"day_length\":\"14:24:46.7200000\"},\"status\":\"OK\"}";

        // Step 2: Parse time string from the JSON data
        string sunsetTimeString = ParseSunsetTime(goodResult);

        // Step 1: Convert time string to datetime value
        DateTime sunsetTime = ToLocalTime(sunsetTimeString, date);

        return sunsetTime;
    }
```

4. Build and run the tests. The test should now pass.

Even though we have a passing test, it's not very useful. We wrote "just enough code" to get the test to pass, but now we need to make the code useful.

During this process, we'll have a failing test for a while. Don't let that discourage you. As we'll see, the failing test still lets us know that we're on the right track.


Test #4: Part 2: Running "GetSunset" with the Actual Service
-------------------------------------------------------------

1. Now we'll change our code to use the actual service.

```c#
    public DateTime GetSunset(DateTime date)
    {
        // Step 3b: Get data from services
        var Service = new SolarService();
        string serviceData = Service.GetServiceData(date);

        // Step 2: Parse time string from the JSON data
        string sunsetTimeString = ParseSunsetTime(serviceData);

        // Step 1: Convert time string to datetime value
        DateTime sunsetTime = ToLocalTime(sunsetTimeString, date);

        return sunsetTime;
    }
```

2. Build and run the tests. There should be a failing test. Unfortunately, it's failing for the wrong reasons.

The error message is an HTTP exception that the service is not available. If the service is started with Visual Studio ("Start without Debugging"), then the service will be shut down on a build of the solution. Instead, we should start the service in a separate command window.

3. Start the service by opening a command prompt/PowerShell window. Navigate to the "SolarCalculator.Service" folder. Then type "dotnet run". This should start the service. You'll know the service is running if the output says "Now listening on: http://localhost:8973".

You can test the service by using the sample urls in a browser window.

4. With the service running, re-run the tests. The test will still fail, but with a different result.

```
    Message: Expected: 2019-08-13 20:25:51
             But was:  2019-08-13 20:28:42
```

This tells us that our code is now hitting the real service. The reason for the time discrepancy is that the sample URL location (latitude / longitude) is different from the location in the "SolarService" class.

The location in the sample URL is Microsoft Building 33 (Redmond, WA).
The location in the service caller class is my hometown - Sedro-Woolley, WA.

The difference in location accounts for the different sunset times.

5. This is the hard part. We leave move on to Part 3 with our test failing. But because it is a known failure mode (a discrepancy in the times), we can make sure that the test continues to fail in this same way until we get the seam added to our code.

Test #4 - Part 3: Calling "GetSunset" Adding a Seam to the Code with DI
------------------------------------------------------------------------

We want to add a seam to our code so that we can swap out the real service call with a fake service call for our tests. To do this, we'll use Dependency Injection, and specifically Property Injection. Property Injection allows us to have a good default value (the production service caller) that is used if we do nothing special. But it gives us a seam where we can swap this out for a fake object for tests.

1. Add an abstraction for the "SolarService" class. An interface that has the behavior we need will let us create another implementation for testing. For simplicity, we can add this interface to the same file as the "SolarService" class.

```c#
    public interface ISolarService
    {
        string GetServiceData(DateTime date);
    }

    public class SolarService : ISolarService {...}
```

2. In the "SunsetCalculator" class, refactor the "Service" variable in "GetSunset" to a class-level property. (Be sure to remove the "var" from inside the GetSunset method so that it uses the class-level property);

```c#
    public class SunsetCalculator : ISunsetCalculator
    {
        private ISolarService service;
        public ISolarService Service
        {
            get { return service; }
            set { service = value; }
        }

        public DateTime GetSunset(DateTime date)
        {
            // Step 3b: Get data from services
            Service = new SolarService();
            string serviceData = Service.GetServiceData(date);
            ...
        }
        ...
    }
```

3. Build and re-run the tests. The same test will fail with the same error.

```
    Message: Expected: 2019-08-13 20:25:51
             But was:  2019-08-13 20:28:42
```

This means that our refactoring did not impact the functionality.

4. Update the getter of the property to automatically create an instance of "SolarService" if the backing field is null. In addition, we'll need to remove the creating of the "SolarService" class out of the "GetSunset" method.

```c#
    public class SunsetCalculator : ISunsetCalculator
    {
        private ISolarService service;
        public ISolarService Service
        {
            get
            {
                if (service == null)
                    service = new SolarService();
                return service;
            }
            set { service = value; }
        }

        public DateTime GetSunset(DateTime date)
        {
            // Step 3b: Get data from services
            string serviceData = Service.GetServiceData(date);
            ...
        }
        ...
    }
```

With this code in place, if we do nothing then an instance of "SolarService" (our production service caller) is created. However, we can also use the setter to provide our own implementation.

5. Build and re-run the tests. The same test will fail with the same error.

```
    Message: Expected: 2019-08-13 20:25:51
             But was:  2019-08-13 20:28:42
```

This means that our refactoring did not impact the functionality.

We now have a seam in our code that we can use in our tests. That's what we'll do in the last part.


Test #4 - Part 4: Calling "GetSunset" Using a Fake Service Implementation
--------------------------------------------------------------------------

1. Our test project already has a "GoodService" class. Take a look at it.

```c#
    public class GoodService // : ISolarService
    {
        public string GetServiceData(DateTime date)
        {
            return "{\"results\":{\"sunrise\":\"6:01:04 AM\",\"sunset\":\"8:25:51 PM\",\"solar_noon\":\"1:13:28 PM\",\"day_length\":\"14:24:46.7200000\"},\"status\":\"OK\"}";
        }
    }
```

This class has a "GetServiceData" method that returns our sample data. Notice that it is all set up to implement the "ISolarService" interface.

2. Remove the comment characters from the class definition.

```c#
    public class GoodService : ISolarService
```

Now it implements that interface, and we can use it in our tests.

3. In the test, set the "Service" property on the "SunsetCalculator" class to our fake service.

```c#
    [Test]
    public void GetSunset_WithValidDate_ReturnsExpectedDateTime()
    {
        DateTime inputDate = new DateTime(2019, 08, 13);
        DateTime expected = new DateTime(2019, 08, 13, 20, 25, 51);

        var calculator = new SunsetCalculator();
        calculator.Service = new GoodService(); // swap in our fake service

        DateTime result = calculator.GetSunset(inputDate);

        Assert.AreEqual(expected, result);
    }
```

4. Build and run the tests. The tests should all now pass, and we're back to our good state.

The final 2 tests round out the "happy path" by adding the "GetSunrise" functionality. This shows how easy it is to get additional data out of our service with what we already have in place.

Test #5: Parsing the Sunrise Time
----------------------------------

1. We have a way to get the sunset time string out of the JSON data. We'll add another test to get the sunrise time string.

```c#
    [Test]
    public void ParseSunriseTime_OnValidData_ReturnsExpectedString()
    {
        string expected = "6:01:04 AM";

        string result = SunsetCalculator.ParseSunriseTime(goodResult);

        Assert.AreEqual(expected, result);
    }
```

This test looks similar to the "ParseSunsetTime" above, but with a few different values.

2. This code does not compile (since "ParseSunriseTime" does not exist). Use "Ctrl-." to have Visual Studio stub out the method.

```c#
    public static string ParseSunriseTime(string jsonData)
    {
        throw new NotImplementedException();
    }
```

3. Build and run the tests. The test is currently in the "red" state.

4. Implement the method using similar code to the "ParseSunsetTime" method.

```c#
    public static string ParseSunriseTime(string jsonData)
    {
        dynamic data = JsonConvert.DeserializeObject(jsonData);
        return data.results.sunrise;
    }
```

5. Build and run the tests. The test should now pass.


Test #6: "GetSunrise"
----------------------

1. Create a test for the "GetSunrise" method on the "SunsetCalculator" class. This is nearly identical to the "GetSunset" test.

```c#
    [Test]
    public void GetSunrise_WithValidDate_ReturnsExpectedDateTime()
    {
        DateTime inputDate = new DateTime(2019, 08, 13);
        DateTime expected = new DateTime(2019, 08, 13, 06, 01, 04);

        var calculator = new SunsetCalculator();
        calculator.Service = new GoodService();

        DateTime result = calculator.GetSunrise(inputDate);

        Assert.AreEqual(expected, result);
    }
```

2. This code builds (since the "GetSunrise" method exists). But the test will fail with a NotImplementedException.

3. Implement the "GetSunrise" method. This uses the already-existing methods in the class.

```c#
    public DateTime GetSunrise(DateTime date)
    {
        string serviceData = Service.GetServiceData(date);
        string sunriseTimeString = ParseSunriseTime(serviceData);
        DateTime sunriseTime = ToLocalTime(sunriseTimeString, date);
        return sunriseTime;
    }
```

4. Build and re-run the tests. All tests should now pass.


Additional Tests
-----------------

These tests complete the "happy path". This let us look at how we can use interfaces and Dependency Injection to add seams and isolate our code. This makes it much easier to swap out dependencies for testing purposes.

To round out this code, we need to add error checking and guard clauses to ensure that the parameter values are appropriate. Also, the service may return an "Error" result if incorrect parameters are passed in.

A few things to check:
1. "ToLocalTime" parses a time string. What happens (or should happen) when the string is not parsable?
2. "ParseSunsetTime" expects certain values to be in the JSON structure. What happens if the structure is different (such as when the "error" result is passed back)?
3. "ParseSunriseTime" has the same concerns regarding the JSON structure.
4. "GetSunset" expects that "GetServiceData" returns successfully. What happens if the service is not available or another exception is thrown?
5. "GetSunset" has the same concerns regarding the "GetServiceData" call.

As we can see, there are a number of additional tests required to fully round out this test suite. For additional information, you can view the materials for "Test-Driven Development in the Real World": http://www.jeremybytes.com/Demos.aspx#RealWorldTDD

 
***
*End of Lab 02 - TDDing into New Functionality with DI*
***