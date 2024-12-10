using AwesomeProxy.FilterAttribute;
using AwesomeProxy;
using System.Text.Json;
using AwesomeProxySample_net60.Model;

namespace AwesomeProxySample_net60.Service
{
    [ConsoleLog]
    public interface IService<T>
    {
        T add(T t1, T t2);
        Person SetPerson(Person p);
    }

    /// <summary>
    /// 紀錄Log標籤
    /// </summary>
    public class ConsoleLogAttribute : AopBaseAttribute
    {
        public override void OnExecuted(ExecutedContext context)
        {
            Console.WriteLine(JsonSerializer.Serialize(context.Args));
        }
        public override void OnExecuting(ExecutingContext context)
        {
            Console.WriteLine($"傳入參數:{JsonSerializer.Serialize(context.Args)}"); 
        } 
    }
}