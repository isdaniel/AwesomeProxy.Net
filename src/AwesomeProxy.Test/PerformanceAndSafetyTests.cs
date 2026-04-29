using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AwesomeProxy.Test.IntegrationObject;

namespace AwesomeProxy.Test
{
    public interface ISimpleService
    {
        string GetValue();
        int Add(int a, int b);
        void DoNothing();
    }

    public class SimpleService : ISimpleService
    {
        public string GetValue() => "hello";
        public int Add(int a, int b) => a + b;
        public void DoNothing() { }
    }

    public interface IThrowingService
    {
        void ThrowException();
    }

    public class ThrowingService : IThrowingService
    {
        public void ThrowException()
        {
            throw new InvalidOperationException("original error");
        }
    }

    public interface IConcurrentService
    {
        string Echo(string input);
    }

    public class ConcurrentService : IConcurrentService
    {
        public string Echo(string input) => input;
    }

    [TestFixture]
    public class FilterCacheTests
    {
        [SetUp]
        public void Setup()
        {
            FilterCache.Clear();
        }

        [Test]
        public void GetOrAdd_ReturnsSameInstance_OnRepeatedCalls()
        {
            var targetType = typeof(ExceptionClass);
            var method = typeof(IExceptionClass).GetMethod(nameof(IExceptionClass.GetException));

            var first = FilterCache.GetOrAdd(targetType, method);
            var second = FilterCache.GetOrAdd(targetType, method);

            Assert.AreSame(first, second);
        }

        [Test]
        public void GetOrAdd_ReturnsDifferentInstances_ForDifferentMethods()
        {
            var targetType = typeof(ExceptionClass);
            var method1 = typeof(IExceptionClass).GetMethod(nameof(IExceptionClass.GetException));
            var method2 = typeof(IExceptionClass).GetMethod(nameof(IExceptionClass.ThrowDefaultException));

            var first = FilterCache.GetOrAdd(targetType, method1);
            var second = FilterCache.GetOrAdd(targetType, method2);

            Assert.AreNotSame(first, second);
        }

        [Test]
        public void GetOrAdd_DiscoversExecuteFilters()
        {
            var targetType = typeof(RefArgClass);
            var method = typeof(IRefArgClass).GetMethod(nameof(IRefArgClass.ExecuteRef));

            var cached = FilterCache.GetOrAdd(targetType, method);

            Assert.IsNotEmpty(cached.ExecuteFilters);
        }

        [Test]
        public void GetOrAdd_DiscoversExceptionFilters()
        {
            var targetType = typeof(CError);
            var method = typeof(ICError).GetMethod(nameof(ICError.GetError));

            var cached = FilterCache.GetOrAdd(targetType, method);

            Assert.IsNotEmpty(cached.ExceptionFilters);
        }

        [Test]
        public void GetOrAdd_ThreadSafe_ConcurrentAccess()
        {
            var targetType = typeof(ExceptionClass);
            var method = typeof(IExceptionClass).GetMethod(nameof(IExceptionClass.GetException));

            var results = new ConcurrentBag<CachedFilterInfo>();
            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    results.Add(FilterCache.GetOrAdd(targetType, method));
                }));
            }

            Task.WaitAll(tasks.ToArray());

            var first = results.ToArray()[0];
            foreach (var r in results)
            {
                Assert.AreSame(first, r);
            }
        }
    }

    [TestFixture]
    public class ThreadSafetyTests
    {
        [Test]
        public void ConcurrentProxyCalls_NoCorruption()
        {
            var proxy = ProxyFactory.GetProxyInstance<IConcurrentService>(() => new ConcurrentService());

            var errors = new ConcurrentBag<Exception>();
            var results = new ConcurrentDictionary<int, string>();
            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                int index = i;
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        var input = $"thread-{index}";
                        var result = proxy.Echo(input);
                        results[index] = result;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.IsEmpty(errors);
            Assert.AreEqual(100, results.Count);

            foreach (var kvp in results)
            {
                Assert.AreEqual($"thread-{kvp.Key}", kvp.Value);
            }
        }
    }

    [TestFixture]
    public class StackTracePreservationTests
    {
        [Test]
        public void ThrownException_PreservesOriginalStackTrace()
        {
            var proxy = ProxyFactory.GetProxyInstance<IThrowingService>(() => new ThrowingService());

            try
            {
                proxy.ThrowException();
                Assert.Fail("Expected exception was not thrown");
            }
            catch (InvalidOperationException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("original error"));
                Assert.That(ex.StackTrace, Does.Contain(nameof(ThrowingService.ThrowException)));
            }
        }
    }

    [TestFixture]
    public class CompiledDelegateTests
    {
        [Test]
        public void Proxy_StringReturn_ReturnsCorrectValue()
        {
            var proxy = ProxyFactory.GetProxyInstance<ISimpleService>(() => new SimpleService());
            Assert.AreEqual("hello", proxy.GetValue());
        }

        [Test]
        public void Proxy_ValueTypeReturn_ReturnsCorrectValue()
        {
            var proxy = ProxyFactory.GetProxyInstance<ISimpleService>(() => new SimpleService());
            Assert.AreEqual(7, proxy.Add(3, 4));
        }

        [Test]
        public void Proxy_VoidMethod_DoesNotThrow()
        {
            var proxy = ProxyFactory.GetProxyInstance<ISimpleService>(() => new SimpleService());
            Assert.DoesNotThrow(() => proxy.DoNothing());
        }

        [Test]
        public void Proxy_RefParameter_StillWorks()
        {
            string arg = "daniel";
            string expectedRef = "Hello danielHello";

            ProxyFactory.GetProxyInstance<IRefArgClass>(() => new RefArgClass()).ExecuteRef(ref arg);
            Assert.AreEqual(expectedRef, arg);
        }

        [Test]
        public void Proxy_OutParameter_StillWorks()
        {
            string expectedOut = "Hello Hello";

            string outString;
            ProxyFactory.GetProxyInstance<IRefArgClass>(() => new RefArgClass()).ExecuteOut(out outString);
            Assert.AreEqual(expectedOut, outString);
        }

        [Test]
        public void Proxy_MultipleCallsSameMethod_CacheHit()
        {
            var proxy = ProxyFactory.GetProxyInstance<ISimpleService>(() => new SimpleService());

            for (int i = 0; i < 1000; i++)
            {
                Assert.AreEqual("hello", proxy.GetValue());
            }
        }
    }
}
