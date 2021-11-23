using System.Threading;
using System.Threading.Tasks;
using Application.Features.DictionaryKeywords.Comands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles ="Basic")]
    public class DictionaryKeywordController : BaseApiController
    {
        [HttpPost()]
        public async Task<IActionResult> AddKeyword([FromBody] CreateDictionaryKeyword.AddDictionaryKeyword query, CancellationToken cancellationToken)
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
