namespace DevsFreeWebAPI.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class Cnpj : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string cnpj = value as string;
        
        if (string.IsNullOrWhiteSpace(cnpj))
            return new ValidationResult("CNPJ is required.");

        cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

        if (!IsValidCnpj(cnpj))
            return new ValidationResult("Invalid CNPJ.");

        return ValidationResult.Success;
    }

    private bool IsValidCnpj(string cnpj)
    {
        int[] firstMultiplier = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] secondMultiplier = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14)
            return false;

        string tempCnpj = cnpj.Substring(0, 12);
        int sum = tempCnpj.ToCharArray().Where(c => char.IsDigit(c)).Reverse().Select((c, index) => (c - '0') * firstMultiplier[index]).Sum();

        int remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        string digit = remainder.ToString();
        tempCnpj = tempCnpj + digit;
        sum = tempCnpj.ToCharArray().Where(c => char.IsDigit(c)).Reverse().Select((c, index) => (c - '0') * secondMultiplier[index]).Sum();

        remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        digit = digit + remainder.ToString();

        return cnpj.EndsWith(digit);
    }
}
