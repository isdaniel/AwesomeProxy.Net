namespace AwesomeProxy
{
    /// <summary>
    /// 執行過程過濾器
    /// </summary>
    public interface IExcuteFilter
    {
        void OnExcuted(ExcutedContext excuteContext);

        void OnExcuting(ExcuteingContext excutingContext);
    }

    /// <summary>
    /// 錯誤過濾器
    /// </summary>
    public interface IExceptionFilter
    {
        void OnException(ExceptionContext exceptionContext);
    }
}