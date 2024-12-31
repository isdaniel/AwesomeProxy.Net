[![AwesomeProxy.Net](https://img.shields.io/nuget/v/AwesomeProxy.Net.svg?style=plastic)](https://www.nuget.org/packages/AwesomeProxy.Net/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AwesomeProxy.Net.svg)](https://www.nuget.org/packages/AwesomeProxy.Net/)
[![Build status](https://ci.appveyor.com/api/projects/status/kgvtee5tgnxbaa4j/branch/master?svg=true)](https://ci.appveyor.com/project/isdaniel/awesomeproxy-net/branch/master)
-----

## 簡單介紹 **AOP** (Aspect-Oriented Programming)

AOP 是 **OOP(物件導向)一個變化程式撰寫思想。**（非取代OOP而是擴充）

導入AOP幫助：
    可幫我們分離**核心邏輯**跟**非核心邏輯**代碼，很好降低模組間耦合性，已便日後擴充。

　　非核心邏輯代碼像：(日誌記錄，性能統計，安全控制，事務處理，異常處理等代碼從業務邏輯代碼中劃分出來)

例如下圖：

![https://ithelp.ithome.com.tw/upload/images/20180209/20096630UyP6I4l2MB.png](https://ithelp.ithome.com.tw/upload/images/20180209/20096630UyP6I4l2MB.png)

　　原本寫法把寫日誌相關程式寫入，業務邏輯方法中。導致此方法非單一職則。我們可以把程式重構改寫成(右圖)，將寫日誌方法抽離出來更有效達成模組化。

**經典例子:**

Asp.Net MVC中Contoller，Action過濾器(FilterAttribute)


-----


## AwesomeProxy.Net介紹：

AwesomeProxy.Net 主要是攔截方法處理
1.	方法執行前
2.	方法執行後
3.	方法異常


### How to Use:

   使用方法類似於Asp.Net MVC中Contoller，Action過濾器

#### Before 1.5 version

1.	撰寫一個標籤(Attribute) 標記攔截動作
```c#
public class CacheAttribute : AopBaseAttribute
{
    public string CacheName { get; set; }

    public override void OnExecuting(ExecuteingContext context)
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


2. 將要被攔截類別繼承於**MarshalByRefObject**類別

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

3. 由ProxyFactory.GetProxyInstance 動態產生被代理類別
``` c#
CacheService cache = ProxyFactory.GetProxyInstance<CacheService>();
```


4.直接呼叫方法就可執行標籤上的攔截動作
```C#
CacheService cache = ProxyFactory.GetProxyInstance<CacheService>();
Console.WriteLine(cache.GetCacheDate());
```

#### after 1.5 version

1.	撰寫一個標籤(Attribute) 標記攔截動作

```
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

2. 建立 interface and class

```
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

3. 由ProxyFactory.GetProxyInstance 動態產生被代理類別
``` c#
var cache = ProxyFactory.GetProxyInstance<CacheService>();
```


4.直接呼叫方法就可執行標籤上的攔截動作

```C#
var cache = ProxyFactory.GetProxyInstance<CacheService>();
Console.WriteLine(cache.GetCacheDate());
```

Simple Code：

  **撰寫Log**
  **權限驗證**
  **快取**


![https://ithelp.ithome.com.tw/upload/images/20180209/20096630BB4lN2NYOW.png](https://ithelp.ithome.com.tw/upload/images/20180209/20096630BB4lN2NYOW.png)

**Unit Test 結果**

![https://ithelp.ithome.com.tw/upload/images/20180209/20096630tbgj7MbcAL.png](https://ithelp.ithome.com.tw/upload/images/20180209/20096630tbgj7MbcAL.png)


**2018-11-26 發布至Nuget上**

Nuget：[AwesomeProxy.Net](https://www.nuget.org/packages/AwesomeProxy.Net/)
