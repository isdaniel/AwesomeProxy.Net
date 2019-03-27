using System.Linq;

namespace AwesomeProxy
{
    public abstract class ContextBase
    {        
        /// <summary>
        /// 返回結果
        /// </summary>
        public object Result { get; set; }
        public object[] Args { get; set; }

        /// <summary>
        /// 取得第一個參數
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T GetFirstArg<T>()
            where T : class
        {
            T result = default(T);

            if (IsExistArgs)
                result = Args.OfType<T>().FirstOrDefault();

            return result;
        }

        public virtual bool TryGetFirstArg<T>(out T arg)
            where T : class
        {
            arg = Args.OfType<T>().FirstOrDefault();

            return arg != default(T);
        }

        private bool IsExistArgs => Args.Length > 0;

        public string MethodName { get; set; }
    }
}