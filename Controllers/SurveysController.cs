using Microsoft.AspNetCore.Mvc;
using SurveyApiSystem.Application.DTOs;
using SurveyApiSystem.Application.Services;

namespace SurveyApiSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveysController(SurveyService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateSurveyRequest request)
    {
        var id = await service.CreateSurveyAsync(request);
        return CreatedAtAction(nameof(GetReport), new { id }, new { id });
    }

    [HttpPost("{id}/vote")]
    public async Task<IActionResult> Vote(Guid id, [FromBody] VoteRequest request)
    { 
        var success = await service.VoteAsync(id, request.QuestionId, request.OptionId); 
        if (!success)
        {
            return NotFound(new { message = "Pesquisa não encontrada." });
        }

        return Ok(new { message = "Voto registrado com sucesso!" });
    }

    [HttpGet("{id}/report")]
    public async Task<IActionResult> GetReport(Guid id)
    {
        var report = await service.GetSurveyReportAsync(id); 
        if (report == null)
        {
            return NotFound(new { message = "Pesquisa não encontrada." });
        }

        return Ok(report);
    }
}