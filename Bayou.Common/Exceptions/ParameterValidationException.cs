using System;

namespace Bayou.Common.Exceptions
{
    public class ParameterValidationException: ApplicationException
    {
        public ParameterValidationException(string msg): base(msg)
        {
        }
    }
}
