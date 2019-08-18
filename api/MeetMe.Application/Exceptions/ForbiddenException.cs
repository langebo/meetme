using System;

namespace MeetMe.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {    
        }
    }
}