namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiLoggerAttribute : Attribute
    {
        public bool Request { get; set; } = true;
        public bool Response { get; set; } = true;

    }
}
