# Task Scheduler Managed Wrapper
This project provides a wrapper for the Windows Task Scheduler. It aggregates the multiple versions, provides an editor and allows for localization.

**Main Library**
Microsoft introduced version 2.0 (internally version 1.2) with a completely new object model with Windows Vista. The managed assembly closely resembles the new object model, but allows the 1.0 (internally version 1.1) COM objects to be manipulated. It will automatically choose the most recent version of the library found on the host system (up through 1.4). Core features include:

* Separate, functionally identical, libraries for .NET 2.0 and 4.0.
* Unlike the base library, this wrapper helps to create and view tasks up and down stream.
* Written in C#, but works with any .NET language including scripting languages.
* Maintain EmailAction and ShowMessageAction under Win8 where they have been deprecated.
* Supports serialization to XML for both 1.0 and 2.0 tasks (base library only supports 2.0)
* Supports task validation for targeted version.
* Supports secure task reading and maintenance.
* Fluent methods for task creation.
* Cron syntax for trigger creation.
* Supports "custom" triggers under Win8 and later.
* Numerous work-arounds and checks to compensate for base library shortcomings.

The project is based on work the originator started in January 2002 with the 1.0 library that is currently hosted on CodeProject.

**UI Library**
There is a second library that includes localized and localizable GUI editors and a wizard for tasks which mimic the ones in Vista and later and adds optional pages for new properties. Following is the list of available UI controls:

* A DropDownCheckList control that is very useful for selecting flag type enumerations.
* A FullDateTimePicker control which allows both date and time selection in a single control.
* A CredentialsDialog class for prompting for a password which wraps the Windows API.
* Simplified classes for pulling events from the system event log.
* Action editor dialog
* Trigger editor dialog
* Task editor dialog and tabbed control
* Event viewer dialog
* Task / task folder selection dialog
* Task history viewer
* Task run-times viewer
* Task creation wizard
* Task service connection dialog
* Sample Code

There is a help file included with the download that provides an overview of the various classes. Below is a brief example of how to use the library from C#.

```csharp
using System;
using Microsoft.Win32.TaskScheduler;

class Program
{
   static void Main(string[] args)
   {
      // Get the service on the local machine
      using (TaskService ts = new TaskService())
      {
         // Create a new task definition and assign properties
         TaskDefinition td = ts.NewTask();
         td.RegistrationInfo.Description = "Does something";

         // Create a trigger that will fire the task at this time every other day
         td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

         // Create an action that will launch Notepad whenever the trigger fires
         td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));

         // Register the task in the root folder
         ts.RootFolder.RegisterTaskDefinition(@"Test", td);

         // Remove the task we just created
         ts.RootFolder.DeleteTask("Test");
      }
   }
}
```

If you really want to squeeze things into a single line of code (without any error handling):

```csharp
new TaskService().AddTask("Test", new DailyTrigger(), new ExecAction("notepad.exe"));
```

For extended examples on how to the use the library, look in the source code area or look at the Examples Page. The library closely follows the Task Scheduler 2.0 Scripting classes. Microsoft has some examples on MSDN around it that may further help you understand how to use this library.
