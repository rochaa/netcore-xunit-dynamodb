namespace gidu.Domain.Core.Members
{
    public class Family
    {
        public string Name { get; private set; }
        public string MemberId { get; private set; }

        public Family(string name, string memberId)
        {
            Name = name;
            MemberId = memberId;
        }

        protected Family() { }
    }
}