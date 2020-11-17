using CsvHelper.Configuration.Attributes;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace FullTextSearch.Models
{
    public class Image
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [Name("title")]
        public string Title { get; set; }
        [Name("text")]
        public string Description { get; set; }
        [Name("tags")]
        public string KeyWords { get; set; }
        public NpgsqlTsVector ImageVector { get; set; }

        public Image()
        {

        }
    }
}
