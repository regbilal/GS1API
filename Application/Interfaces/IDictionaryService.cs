using Application.Features.Search.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDictionaryService
    {
        Task<KeywordDefinitionsDto> GetKeywordDefinitionAsync(string keyword, CancellationToken cancellationToken);
    }
}
