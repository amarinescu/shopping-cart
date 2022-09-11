using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Baskets.Commands;
using ShoppingCart.Application.Baskets.Queries;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //get
        [HttpGet("{basketId}")]
        public async Task<IActionResult> GetBasket([FromRoute] int basketId)
        {
            return Ok(await _mediator.Send(new GetBasketByIdQuery { BasketId = basketId }));
        }

        //post
        [HttpPost]
        public async Task<IActionResult> CreateBasket([FromBody] AddBasketCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        //put
        [HttpPut("{basketId}")]
        public async Task<IActionResult> UpdateBasket([FromRoute] int basketId, [FromBody] AddArticleToBasketCommand command)
        {
            command.AppendBasketId(basketId);

            return Ok(await _mediator.Send(command));
        }

        //patch
        [HttpPatch("{basketId}")]
        public async Task<IActionResult> PatchBasket([FromRoute] int basketId, [FromBody] CloseBasketCommand command)
        {
            command.AppendBasketId(basketId);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
