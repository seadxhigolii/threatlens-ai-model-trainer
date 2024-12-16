using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Threatlens.Application.Common;

namespace Threatlens.Application.Features.Model;

public class TrainModelCommandController : ApiControllerBase
{
    [HttpPost("/api/train-model")]
    public async Task<ActionResult<int>> Create(TrainModelCommand command)
    {
        return await Mediator.Send(command);
    }
}

public record TrainModelCommand(string? Title) : IRequest<int>;


