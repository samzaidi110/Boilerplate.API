using Boilerplate.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities.Logging;
public class AuditTrail:Entity
{
    public DomainId EntityId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public Guid? UserId { get; set; }
    [Column(TypeName = "jsonb")]
    public string Changes { get; set; }=string.Empty;

    public string CorrelationId { get; set; } = string.Empty;

}