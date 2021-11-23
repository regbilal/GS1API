using Application.Common;

namespace Application.Features.DictionaryKeywords.Models
{
    public partial class DictionaryKeyword : AuditableBaseEntity
    {
        public string Keyword { get; set; }
    }
}
