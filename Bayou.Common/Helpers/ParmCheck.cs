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
                ParameterValidationException.ThrowNullParameterValidationException(parmName);
            }
            return parmValue;
        }

        public static TType NotNullOrEmpty<TType>(string parmName, TType parmValue)
                                       where TType : class
        {
            if ((parmValue == null) || (parmValue == default(TType)))
            {
                ParameterValidationException.ThrowNullOrEmptyParameterValidationException(parmName);
            }
            return parmValue;
        }
    }
}
