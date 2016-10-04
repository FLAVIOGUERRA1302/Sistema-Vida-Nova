using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class Attachment
    {
        public Attachment() { }
        

        [Key]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Type { get; set; }
        
        [Required]
        public int IdInformativo { get; set; }

        [ForeignKey("IdInformativo")]
        public Informativo Informativo { get; set; }


        public MimePart GetMimePart(string webPath)
        {
            var attachment = new MimePart(Type)
            {
                ContentObject = new ContentObject(File.OpenRead(Path.Combine(webPath, "attachment", Informativo.Id.ToString(), Id.ToString())), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = FileName
            };
            return attachment;
        }
    }
}
