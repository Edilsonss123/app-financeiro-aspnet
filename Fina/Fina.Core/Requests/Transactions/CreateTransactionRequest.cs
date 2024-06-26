using System.ComponentModel.DataAnnotations;
using Fina.Core.Enums;

namespace Fina.Core.Requests.Transactions
{
    public class CreateTransactionRequest : Request
    {
        public long Id { get; set;}
        
        [Required(ErrorMessage = "Título não informado")]
        public string Title { get; set;} = string.Empty;

        [Required(ErrorMessage = "Tipo da transação inválido")]
        public ETransactionType Type { get; set; } = ETransactionType.WithDraw;

        [Required(ErrorMessage = "Valor não informado")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Categoria não informada")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Data operação não informada")]
        public DateTime? PaidOrReceivedAt { get; set; }
    }
}