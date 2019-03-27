namespace AwesomeProxy
{
    /// <summary>
    /// 執行過程過濾器
    /// </summary>
    public interface IExcuteFilter
    {
        void OnExecuted(ExecutedContext executeContext);

        void OnExecuting(ExecutingContext executing);
    }

    /// <summary>
    /// 錯誤過濾器
    /// </summary>
    public interface IExceptionFilter
    {
        void OnException(ExceptionContext exceptionContext);
    }
}