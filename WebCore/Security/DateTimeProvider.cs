namespace WebCore.Security
{
    using System;
    using Interfaces;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
