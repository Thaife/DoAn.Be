using MISA.WEB08.AMIS.Common.Enums;
using System;
using System.Collections;

namespace MISA.WEB08.AMIS.Common.Exceptions
{
    public class ValidateException : Exception
    {
        public IDictionary? DataError { get; set; }

        public MisaAmisErrorCode ErrorCode { get; set; }

        public ValidateException(string message, IDictionary dataError, MisaAmisErrorCode errorCode) : base(message)
        {
            DataError = dataError;
            ErrorCode = errorCode;
        }

        public ValidateException(string message, IDictionary dataError) : base(message)
        {
            DataError = dataError;
            
        }
    }
}
