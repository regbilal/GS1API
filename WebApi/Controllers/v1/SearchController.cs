using System.Threading;
using System.Threading.Tasks;
using Application.Features.DictionaryKeywords.Comands;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SearchController : BaseApiController
    {
        [HttpPost()]
        public async Task<IActionResult> Search([FromBody] Search.SearchKeyword query, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
