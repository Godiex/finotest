using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.Entities.Validators;

public static class HostValidator
{
    public static Task Validate(string host)
    {
        const string patternHost = @"^[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        bool isHostValid = Regex.IsMatch(host, patternHost);

        if (!isHostValid)
        {
            throw new InvalidHostNameException(Messages.InvalidHostException);
        }

        return Task.CompletedTask;
    }
}