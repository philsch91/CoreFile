# CoreFile

Handle and write text files easier and thread-safe.

#### Classes
- CFTextFile
- CFRotatableTextFile
- CFLogFile

#### Enums
- CFLogEntryType

```csharp
//create and write a logfile 
//that gets automatically roatated 
//when it reaches the specified maximum size in bytes
CFLogFile logfile = new CFLogFile("File.log", 10240);
logfile.MaxCount = 5;
logfile.Write("this is a new enty",CFLogEntryType.Info);
```




