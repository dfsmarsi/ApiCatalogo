using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "Nome de usuário obrigatório!")]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória!")]
        public string? Password { get; set; }
    }
}
