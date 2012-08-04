namespace WebCore.Security.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class PasswordResetException : ApplicationException
    {
        private const string StatusCodeFieldName = "StatusCode";

        public PasswordResetException(PasswordResetStatus status)
        {
            StatusCode = status;
        }

        public PasswordResetException(PasswordResetStatus status, string message)
            : base(message)
        {
            StatusCode = status;
        }

        public PasswordResetException(PasswordResetStatus status, string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = status;
        }

        public PasswordResetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = (PasswordResetStatus)info.GetValue(StatusCodeFieldName, typeof(PasswordResetStatus));
        }

        public PasswordResetStatus StatusCode { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(StatusCodeFieldName, StatusCode);
        }
    }
}
