using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class LoteDto
    {
       public int Id { get; set; }

        [Required(ErrorMessage="Campo Obrigatorio")]
        public string Nome { get; set; }
        [Required]
        public decimal Preco { get; set; }

        public string DataInicio { get; set; }

        public string DataFim { get; set; }
        [Range(2,120000,ErrorMessage="Quantidade de Pessoas é entre 2 e 120000")]
        public int Quantidade { get; set; }

        public EventoDto Evento { get;}
    }
}