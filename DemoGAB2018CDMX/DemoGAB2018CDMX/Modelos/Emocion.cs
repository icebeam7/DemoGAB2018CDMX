using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoGAB2018CDMX.Modelos
{
    public class Emocion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public float Score { get; set; }
        public string Foto { get; set; }

        public string Resultado
        {
            get
            {
                return $"Se detectó {Nombre} ({Score * 100} %)";
            }
        }

    }
}
