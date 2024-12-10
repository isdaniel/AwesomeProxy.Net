using AwesomeProxy;
using AwesomeProxySample_net60;
using AwesomeProxySample_net60.Model;
using AwesomeProxySample_net60.Service;

Console.WriteLine("=================紀錄Log測試==================");

#region 紀錄Log

//1.使用 ProxyFactory.GetProxyInstance 取得代理物件
var t = ProxyFactory.GetProxyInstance<IService<int>>(typeof(IntService));

//2.執行方法
var result = t.add(1, 2);
t.SetPerson(new Person() { Age = 10 });
Console.WriteLine(result);

#endregion 紀錄Log

Console.WriteLine("=================權限驗證測試==================");

#region 權限攔截驗證

//權限驗證：撰寫權限驗證 攔截代替寫入核心程式碼
Console.WriteLine("權限驗證：只有RD可以查錢。");
var authService = ProxyFactory.GetProxyInstance<IAuthService, AuthService>();

LoginInfo RDuser = new LoginInfo()
{
    Name = "daniel",
    Type = AuthType.RD
};

LoginInfo PMuser = new LoginInfo()
{
    Name = "Amy",
    Type = AuthType.PM
};

Console.WriteLine(authService.GetMoney(RDuser));
Console.WriteLine(authService.GetMoney(PMuser));

#endregion 權限攔截驗證

Console.WriteLine("===================快取測試====================");

#region 快取測試

Console.WriteLine("快取測試：測試資料是否被快取。");

var cache = ProxyFactory.GetProxyInstance<ICacheService, CacheService>();
Console.WriteLine($"時間:{cache.GetCacheDate()}");

Console.WriteLine("休息5秒鐘!～～");
//睡眠5秒鐘
Thread.Sleep(5000);

Console.WriteLine($"時間:{cache.GetCacheDate()}");

#endregion 快取測試

Console.ReadKey();