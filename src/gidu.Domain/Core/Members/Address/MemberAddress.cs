namespace gidu.Domain.Core.Members
{
    public class MemberAddress
    {
        public string Address { get; private set; }
        public string Neighborhood { get; private set; }
        public string Zip { get; private set; }

        public MemberAddress(string address, string neighborhood, string zip)
        {
            FillMemberAddress(address, neighborhood, zip);

            (new MemberAddressValidation()).ValidateRules(this);
        }

        public MemberAddress() { }

        private void FillMemberAddress(string address, string neighborhood, string zip)
        {
            Address = address;
            Neighborhood = neighborhood;
            Zip = zip;
        }
    }
}