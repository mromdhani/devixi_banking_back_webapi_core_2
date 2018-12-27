using System.ComponentModel.DataAnnotations;

namespace MonProjetBanking_Back.Models
{
    public class Compte
    {
     [Key]
      public string Numero { get; set; }
      public string Proprietaire { get; set; }
      public decimal Solde { get; set; }
    }
}