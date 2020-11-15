using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Models
{
    public class QueryMiddlewareModel
    {
        [Required]
        public string Name { get; set; }
    }
}
