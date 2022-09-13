using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Baskets.Commands;
using ShoppingCart.Application.Baskets.Queries;
using System;
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

        [HttpGet("{basketCode}")]
        public async Task<IActionResult> GetBasket([FromRoute] Guid basketCode)
        {
            return Ok(await _mediator.Send(new GetBasketByIdQuery { BasketCode = basketCode }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasket([FromBody] AddBasketCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{basketCode}")]
        public async Task<IActionResult> UpdateBasket([FromRoute] Guid basketCode, [FromBody] AddArticleToBasketCommand command)
        {
            command.AppendBasketCode(basketCode);

            return Ok(await _mediator.Send(command));
        }

        [HttpPatch("{basketCode}")]
        public async Task<IActionResult> PatchBasket([FromRoute] Guid basketCode, [FromBody] CloseBasketCommand command)
        {
            command.AppendBasketCode(basketCode);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
