using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainAPI.Domain.Constants
{
    public class JwtClaims
    {
        public const string EMAIL = nameof(EMAIL);
        public const string USER_ID = nameof(USER_ID);
        public const string ROLE = nameof(ROLE);
    }
}