namespace Entity.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaskedAttribute : Attribute
    {
        public int Index { get; set; }
        public int Length { get; set; }

        public MaskedAttribute(int Index = 0, int Length = 0)
        {
            this.Index = Index;
            this.Length = Length;
        }
    }
}
