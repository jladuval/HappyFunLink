namespace WebCore.Security.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class ActivationException : ApplicationException
    {
        private const string StatusCodeFieldName = "StatusCode";

        public ActivationException(ActivationStatus status)
        {
            StatusCode = status;
        }

        public ActivationException(ActivationStatus status, string message)
            : base(message)
        {
            StatusCode = status;
        }

        public ActivationException(ActivationStatus status, string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = status;
        }

        public ActivationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = (ActivationStatus)info.GetValue(StatusCodeFieldName, typeof(ActivationStatus));
        }

        public ActivationStatus StatusCode { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(StatusCodeFieldName, StatusCode);
        }
    }
}
