namespace WebCore.Security.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class MembershipException : ApplicationException
    {
        private const string StatusCodeFieldName = "StatusCode";

        public MembershipException(MembershipStatus status)
        {
            StatusCode = status;
        }

        public MembershipException(MembershipStatus status, string message)
            : base(message)
        {
            StatusCode = status;
        }

        public MembershipException(MembershipStatus status, string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = status;
        }

        public MembershipException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = (MembershipStatus)info.GetValue(StatusCodeFieldName, typeof(MembershipStatus));
        }

        public MembershipStatus StatusCode { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(StatusCodeFieldName, StatusCode);
        }
    }
}
