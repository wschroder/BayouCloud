using System;

namespace Bayou.Common.Exceptions
{
    public class ParameterValidationException: ApplicationException
    {
        public ParameterValidationException(string msg): base(msg)
        {
        }

        public static void ThrowNullOrEmptyParameterValidationException(string parmName)
        {
            string msg = $"Required parameter [{parmName}] has a NULL or empty value.";
            throw new ParameterValidationException(msg);
        }

        public static void ThrowNullParameterValidationException(string parmName)
        {
            string msg = $"Required parameter [{parmName}] has a NULL value.";
            throw new ParameterValidationException(msg);
        }
    }
}
