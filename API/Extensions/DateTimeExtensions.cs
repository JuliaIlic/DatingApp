using System;

namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var age = today.Year - dob.Year;

        // age-- because they did not have bday this year
        if (dob > today.AddYears(-age)) age--;

        return age;
    }
}
