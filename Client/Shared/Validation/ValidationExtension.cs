using System.ComponentModel.DataAnnotations;

namespace SharedApp.Validation;

public class NotZero : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is int intValue)
        {
            return intValue > 0;
        }

        return false;
    }
}
