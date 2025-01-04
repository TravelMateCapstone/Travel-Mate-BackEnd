using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class BlockchainKey
    {
        [Key]
        public int UserId { get; set; }
        public string PublicKey { get; set; } // Lưu public key
        public string PrivateKey { get; set; } // Mã hóa private key, không lưu dưới dạng plain text
    }

}
