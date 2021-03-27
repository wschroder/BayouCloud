using System;

namespace Bayou.Common.Exceptions
{
    public class TestException : ApplicationException
    {
        public TestException(string msg): base(msg)
        {
        }
    }
}
