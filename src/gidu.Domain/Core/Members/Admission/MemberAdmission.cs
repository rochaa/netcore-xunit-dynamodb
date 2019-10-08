using System;

namespace gidu.Domain.Core.Members
{
    public class MemberAdmission
    {
        public DateTime? Date { get; private set; }
        public int? Minutes { get; private set; }
        public string Reception { get; private set; }
        public int OrderNumber { get; private set; }
        public string Pastor { get; private set; }

        public MemberAdmission(DateTime? date, int? minutes, string reception, int orderNumber, string pastor)
        {
            Date = date;
            Minutes = minutes;
            Reception = reception;
            OrderNumber = orderNumber;
            Pastor = pastor;

            (new MemberAdmissionValidation()).ValidateRules(this);
        }

        public MemberAdmission() { }
    }
}