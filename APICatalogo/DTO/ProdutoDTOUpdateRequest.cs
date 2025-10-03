using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO
{
    public class ProdutoDTOUpdateRequest : IValidatableObject
    {
        [Range(1,9999, ErrorMessage = "Estoque deve estar entre 1 e 9.999")]
        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DataCadastro.Date <= DateTime.Now)
            {
                yield return new ValidationResult(
                    "DataCadastro deve ser maior que a data atual",
                    new[] { nameof(this.DataCadastro) });
            }
        }
    }
}
