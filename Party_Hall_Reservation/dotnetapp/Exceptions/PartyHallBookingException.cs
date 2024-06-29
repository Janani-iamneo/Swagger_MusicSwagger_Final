using System;

namespace dotnetapp.Exceptions
{

public class PartyHallBookingException : Exception
{
    public PartyHallBookingException(string message) : base(message)
    {
    }
}
}

