namespace PosApi.Exceptions
{
    using System;
    using System.Collections.Generic;
    using PosApi.Shared;

    // Custom validation / business exception for application layer
    public class AppValidationException : Exception
    {
        public AppErrorCode? Code { get; }
        public IReadOnlyList<string> Errors { get; }
        public int Status { get; set; } = 400;

        public AppValidationException(string message, int status = 400)
            : base(message)
        {
            Errors = Array.Empty<string>();
            Status = status;
        }

        public AppValidationException(string message, AppErrorCode code, int status = 400)
             : base(message)
        {
            Code = code;
            Errors = new string[] {message};
            Status = status;
        }

        public AppValidationException(string message, IEnumerable<string> errors, AppErrorCode? code = null, int status = 400)
       : base(message)
        {
            Code = code;
            Errors = new List<string>(errors);
            Status = status;
        }
    }
}
