using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVS.API.Entities
{
    [Table("Log")]
    public partial class Log
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Log()
        {
        }
        
        public long Id { get; set; }

        [StringLength(450)]
        public string AspnetId { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Long { get; set; }

        [StringLength(50)]
        public string IpAddress { get; set; }

        [StringLength(100)]
        public string Method { get; set; }
    }
}