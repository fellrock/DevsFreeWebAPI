using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace DevsFreeWebAPI.Models;

public class Collaborator
{
    [Key]
    [DisplayName("Id")]
    public int Id{get; set;}

    [Required(ErrorMessage = "Informe o nome")]
    [StringLength(80, ErrorMessage ="O nome deve conter até 80 caracteres")]
    [MinLength(5, ErrorMessage = "O nome deve conter pelo menos 5 caracteres")]
    [DisplayName("Nome Completo")]
    public string Name {get; set;} = string.Empty;

    [Required(ErrorMessage = "Informe o E-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    [DisplayName("E-mail")]
    public string Email {get; set;} = string.Empty;

    private string? _telefone;
    
    // USE THIS TO FRONT END PHONE DISPLAY:
    // var formattedNumber = phoneNumberUtil.Format(number, PhoneNumberFormat.INTERNATIONAL);
    
    // Adjusting Frontend phone input:
    // If you have a frontend form where users enter their phone numbers,
    // consider providing a dropdown list of countries.
    // When a user selects their country, you can pass the country's code
    // as a second argument to the phoneNumberUtil.Parse(value, countryCode)
    // method to provide a hint to the parser.
    // This way, the library can more accurately parse the number.
    
    [Required(ErrorMessage = "Informe o número de contato")]
    [DisplayName("Contato (Whatsapp)")]
    public string? Telefone
    {
        get => _telefone;
        set
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var number = phoneNumberUtil.Parse(value, null); // 'null' means it'll try to determine the region automatically
                if (phoneNumberUtil.IsValidNumber(number))
                {
                    _telefone = phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
                }
                else
                {
                    throw new Exception("Telefone inválido"); // Handle this exception as per your application's needs
                }
            }
            catch (NumberParseException)
            {
                throw new Exception("Número de telefone não é válido ou não está em um formato aceitável");
            }
        }
    }
    
    [Required(ErrorMessage = "Deixe uma mensagem")]
    [DisplayName("Mensagem")]
    public string Mensagem {get; set;} = string.Empty;

    [DisplayName("Date Added")]
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}