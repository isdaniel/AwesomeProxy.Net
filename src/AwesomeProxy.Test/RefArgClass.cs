namespace AwesomeProxy.Test
{
    public class RefArgClass : IRefArgClass
    {
        public void ExecuteRef(ref string name)
        {
            name = $"Hello {name}";
        }
        public void ExecuteOut(out string name)
        {
            name = $"Hello ";
        }
    }

    public interface IRefArgClass
    {
        [Ref]
        public void ExecuteRef(ref string name);
        [Out]
        public void ExecuteOut(out string name);
    }
}