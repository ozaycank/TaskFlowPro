namespace Velyo.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("You are not authorized to access this resource.") { }

    public ForbiddenAccessException(string message) : base(message) { }
}