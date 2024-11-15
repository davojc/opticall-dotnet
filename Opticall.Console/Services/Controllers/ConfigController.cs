using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opticall.Console.Config;

namespace Opticall.Console.Services.Controllers;

[ApiController]
[Route("api/config")]
public class ConfigController : ControllerBase
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly ILogger<ConfigController> _logger;

    public ConfigController(ISettingsProvider settingsProvider, ILogger<ConfigController> logger)
    {
        _settingsProvider = settingsProvider;
        _logger = logger;
        _logger.LogWarning("Added ConfigController.");
    }

    [HttpGet("target")]
    public IActionResult GetTarget()
    {
        return Ok(_settingsProvider.Target);
    }

    [HttpGet("group")]
    public IActionResult GetGroup()
    {
        return Ok(_settingsProvider.Group);
    }

    [HttpPut("target")]
    public IActionResult UpdateTarget([FromBody] string target)
    {
        _settingsProvider.SetTarget(target);
        return Ok(_settingsProvider.Target);
    }

    [HttpPut("group")]
    public IActionResult UpdateGroup([FromBody] string group)
    {
        _settingsProvider.SetGroup(group);
        return Ok(_settingsProvider.Group);
    }
}