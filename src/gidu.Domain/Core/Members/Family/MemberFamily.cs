using System;
using System.Collections.Generic;

namespace gidu.Domain.Core.Members
{
    public class MemberFamily
    {
        public DateTime? DateMarried { get; private set; }
        public Family Spouse { get; private set; }
        public Family Father { get; private set; }
        public Family Mother { get; private set; }
        public List<Family> Children { get; private set; }

        public MemberFamily(DateTime? dateMarried, Family spouse, Family father, Family mother, List<Family> children)
        {
            DateMarried = dateMarried;
            Spouse = spouse;
            Father = father;
            Mother = mother;
            Children = children;
        }

        protected MemberFamily() { }
    }
}