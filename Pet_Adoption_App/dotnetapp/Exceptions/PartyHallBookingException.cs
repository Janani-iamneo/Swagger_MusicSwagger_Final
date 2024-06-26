using System;

namespace dotnetapp.Exceptions
{

public class PetAdoptionException : Exception
{
    public PetAdoptionException(string message) : base(message)
    {
    }
}
}

