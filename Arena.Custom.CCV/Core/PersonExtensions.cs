using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arena.Core;

namespace Arena.Custom.CCV.Core
{
    public static class PersonExtensions
    {
        public static string PhoneNumber(this Person person, Guid phoneType)
        {
            foreach (PersonPhone phone in person.Phones)
                if (phone.PhoneType.Guid == phoneType)
                    return phone.ToString();
            return string.Empty;
        }
    }
}
