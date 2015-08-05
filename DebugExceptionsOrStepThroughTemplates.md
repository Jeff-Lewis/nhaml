**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

**Note: All examples where code is used are in C# unless specified otherwise**

# Enable template debugging #

**Set the config setting**

```
  <nhaml outputDebugFiles="true">
...
  </nhaml>
```


**Temp Directory**

The next time a view is compiled the generated source file will be save to disk.

The location for these source files derived from the location of
```
Assembly.GetExecutingAssembly() 
```
```
Path.GetTempPath()
```
and **nhamltemp**

So, for example, if your app bin is
```
C:\Code\MVCSite\bin
```
And your temp path is
```
C:\Temp
```
Your nhaml temp directory will be in
```
C:\Temp\nhamlTemp\C_Code_MVCSite_bin
```

The path will also be written to Debug using
```
Debug.WriteLine()
```



# Diagnosing a Template Exception #

**Example template with exception**

```
- string x = null
= x.Substring(0,4)
```

**Enable "Break On Exceptions" in Visual Studio**

![http://nhaml.googlecode.com/svn/wiki/BreakOnException.gif](http://nhaml.googlecode.com/svn/wiki/BreakOnException.gif)

**Execute the template. Visual Studio will break into file when the exception is thrown.**

![http://nhaml.googlecode.com/svn/wiki/DebugException.gif](http://nhaml.googlecode.com/svn/wiki/DebugException.gif)


# Stepping Through Template Code #

**Execute the template at least once. This will cause a file for that template to be save to the nhamltemp directory.**

**Open the file in Visual Studio and add a breakpoint on the desired line**

![http://nhaml.googlecode.com/svn/wiki/BreakPoint.gif](http://nhaml.googlecode.com/svn/wiki/BreakPoint.gif)

**Execute the template in Debug Mode**

![http://nhaml.googlecode.com/svn/wiki/DebugBreakPoint.gif](http://nhaml.googlecode.com/svn/wiki/DebugBreakPoint.gif)


# Notes: #

  * Avoid running this on production code as it will cause a slight performance hit.
  * Nhaml does not clean up the nhamltemp directory.
  * The temp directory is currently not configurable. This will come in a future release.
  * This currently only works for VB and C#.