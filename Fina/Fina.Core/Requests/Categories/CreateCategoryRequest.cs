using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories
{
    public class CreateCategoryRequest : Request
    {
        [Required(ErrorMessage = "Título não informado")]
        [MaxLength(80, ErrorMessage = "O título deve conter até 80 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição não informada")]
        public string Description { get; set; } = string.Empty;
    }
}