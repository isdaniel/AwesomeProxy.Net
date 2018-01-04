## Aop是甚麼可以吃嗎? (Aspect-Oriented Programming)
#### Aop使用設計模式中Proxy pattern核心思想
#### 主要意圖將日誌記錄，性能統計，安全控制，事務處理，異常處理等代碼從業務邏輯代碼中劃分出來。
#### Asp.Net MVC中Filter就是使用AOP概念來實作

<br/>

![image error](/file/img/introductionImg.png)
### 如上圖 我們可以看到代理模式將 核心執行程式,寫日誌分離出來已便日後擴展
<br/>

### AOPLib 使用簡單，使用起來跟Asp.net MVC Filter類似
---

## 撰寫攔截器標籤
### 繼承`AopBaseAttribute` 重寫要執行的方法
1. `OnExcuted`   執行後動作
2. `OnExcutint`  執行前動作
3. `OnException` 錯誤時動作

```c#
public class ConsoleLogAttribute : AopBaseAttribute
{
    public override void OnExcuted(ExcutedContext result)
    {
        Console.WriteLine(JsonConvert.SerializeObject(result.Args));
    }

    public override void OnExcuting(ExcuteingContext args)
    {
        Console.WriteLine($"傳入參數:{JsonConvert.SerializeObject(args.Args)}");
    }
}
```

## 標註要攔截
### 1.標註在要攔截的類別或方法上
### 2.使用類別繼承`MarshalByRefObject`

```c#
[ConsoleLog]
public abstract class ServiceBase : MarshalByRefObject, IService<int>
{
    public int add(int t1, int t2)
    {
        return t1 + t2;
    }

    public Person SetPerson(Person p)
    {
        p.Age = 100;
        p.Name = "test";
        return p;
    }
}
```

## 使用
### 1.使用ProxyFactory.GetProxyInstance 動態產生代理類別(此類別需繼承 `MarshalByRefObject`)
### 2.呼叫使用方法
```c#
private static void Main(string[] args)
{
    //1.使用 ProxyFactory.GetProxyInstance 取得代理物件
    var t = ProxyFactory.GetProxyInstance<ServiceBase>(typeof(IntService));
    //2.執行方法
    var result = t.add(1, 2);
    t.SetPerson(new Model.Person() { Age = 10 });

    Console.WriteLine(result);
    Console.ReadKey();
}
```
<br/>

## 執行結果如下
![image error](/file/img/result.png "Optional title")