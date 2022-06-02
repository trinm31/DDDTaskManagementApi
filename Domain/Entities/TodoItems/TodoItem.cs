using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Entities.Tasks;
using Domain.Entities.Todos;

namespace Domain.Entities.TodoItems
{
    public partial class TodoItem : AuditEntity<int>
    {
        [Required]
        public string Content { get; set; }
        public bool IsDone { get; set; } = false;
        [Required]
        public int TodoId { get; set; }
        [ForeignKey("TodoId")]
        public virtual Todo Todo { get; set; }
    }
}
