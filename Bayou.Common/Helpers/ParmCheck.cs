using Bayou.Common.Exceptions;

namespace Bayou.Common.Helpers
{
    public static class ParmCheck
    {
        public static TType NotNull<TType>(string parmName, TType parmValue)
                                       where TType: class
        {
            if (parmValue == null)
            {
                string msg = $"Required parameter [{parmName}] has a NULL value.";
                throw new ParameterValidationException(msg);
            }
            return parmValue;
        }

        public static TType NotNullOrEmpty<TType>(string parmName, TType parmValue)
                                       where TType : class
        {
            if ((parmValue == null) || (parmValue == default(TType)))
            {
                string msg = $"Required parameter [{parmName}] has a NULL or empty value.";
                throw new ParameterValidationException(msg);
            }
            return parmValue;
        }

        public static int Positive(string parmName, int parmValue)
        {
            if (parmValue <= 0)
            {
                string msg = $"Parameter [{parmName}] should be a positive integer but is {parmValue}.";
                throw new ParameterValidationException(msg);
            }
            return parmValue;
        }
    }
}
