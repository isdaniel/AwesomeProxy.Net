[![AwesomeProxy.Net](https://img.shields.io/nuget/v/AwesomeProxy.Net.svg?style=plastic)](https://www.nuget.org/packages/AwesomeProxy.Net/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AwesomeProxy.Net.svg)](https://www.nuget.org/packages/AwesomeProxy.Net/)
[![Build status](https://ci.appveyor.com/api/projects/status/kgvtee5tgnxbaa4j/branch/master?svg=true)](https://ci.appveyor.com/project/isdaniel/awesomeproxy-net/branch/master)
[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/isdaniel/AwesomeProxy.Net)

-----

## Introduction to AOP (Aspect-Oriented Programming)

AOP is a programming paradigm that extends OOP (Object-Oriented Programming) (it does not replace OOP but extends it).

Introducing AOP helps to:

Separate core logic from non-core logic code, which reduces module coupling and facilitates future extensions.

Non-core logic code includes aspects like: (logging, performance statistics, security control, transaction processing, exception handling, etc.), separating them from business logic code.

For example:

Instead of embedding logging-related code within business logic methods, which violates the single responsibility principle, we can refactor and extract logging methods (as shown in the right image).

like below image.

![https://ithelp.ithome.com.tw/upload/images/20180209/20096630UyP6I4l2MB.png](https://ithelp.ithome.com.tw/upload/images/20180209/20096630UyP6I4l2MB.png)

Classic Example: In Asp.Net MVC, Controller, Action filters (FilterAttribute)

-----

### Introduction to AwesomeProxy.Net:

AwesomeProxy.Net mainly intercepts method processing:

* Before method execution
* After method execution
* On method exception

How to Use: Usage is similar to Controller, Action filters in Asp.Net MV

### Before 1.5 version

1.	Write an attribute to mark the interception action

```c#
public class CacheAttribute : AopBaseAttribute
{
    public string CacheName { get; set; }

    public override void OnExecting(ExecuteingContext context)
    {
        object cacheObj = CallContext.GetData(CacheName);
        if (cacheObj != null)
        {
            context.Result = cacheObj;
        }
    }

    public override void OnExecuted(ExecutedContext context)
    {
        CallContext.SetData(CacheName, context.Result);
    }
}
```


2. Inherit the class to be intercepted from `MarshalByRefObject`

```C#
public class CacheService : MarshalByRefObject
{
        [Cache]
	public string GetCacheDate()
	{
		return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
	}
}
```

3. Dynamically generate the proxy class using `ProxyFactory.GetProxyInstance`
``` c#
CacheService cache = ProxyFactory.GetProxyInstance<CacheService>();
```


4. Directly call the method to execute the interception action on the attribute

```C#
CacheService cache = ProxyFactory.GetProxyInstance<CacheService>();
Console.WriteLine(cache.GetCacheDate());
```

### After 1.5 version

1.	Write an attribute to mark the interception action

```c#
public class CacheAttribute : AopBaseAttribute
{
	public string CacheName { get; set; }

	public override void OnExecuted(ExecutedContext context)
	{
	    CallContext.SetData(CacheName, context.Result);
	}

	public override void OnExecuting(ExecutingContext context)
	{
	    object cacheObj = CallContext.GetData(CacheName);
	    if (cacheObj != null)
	    {
		context.Result = cacheObj;
	    }
	}
}
```

2. Create an interface and class

```c#
public class CacheService : ICacheService
{
	public string GetCacheDate()
	{
	    return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
	}
}

public interface ICacheService {
	[Cache(CacheName = "GetCacheDate")]
	public string GetCacheDate();
}
```

3. Dynamically generate the proxy class using `ProxyFactory.GetProxyInstance`

``` c#
var cache = ProxyFactory.GetProxyInstance<CacheService>();
```


4. Directly call the method to execute the interception action on the attribute

```C#
var cache = ProxyFactory.GetProxyInstance<CacheService>();
Console.WriteLine(cache.GetCacheDate());
```

### Simple Code:

* Write Log
* Permission Verification
* Caching

![https://ithelp.ithome.com.tw/upload/images/20180209/20096630BB4lN2NYOW.png](https://ithelp.ithome.com.tw/upload/images/20180209/20096630BB4lN2NYOW.png)

Unit Test Results

![https://ithelp.ithome.com.tw/upload/images/20180209/20096630tbgj7MbcAL.png](https://ithelp.ithome.com.tw/upload/images/20180209/20096630tbgj7MbcAL.png)

