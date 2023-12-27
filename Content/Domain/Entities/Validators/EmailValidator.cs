using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.Entities.Validators;

public static class EmailValidator
{
    public static Task Validate(string email)
    {
        const string patternEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        bool isEmail = Regex.IsMatch(email, patternEmail);
        if (!isEmail)
        {
            throw new InvalidEmailException(Messages.InvalidEmailException);
        }

        return Task.CompletedTask;
    }
}