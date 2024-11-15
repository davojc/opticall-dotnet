using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opticall.Console.Command;
using Opticall.Console.Command.Commands;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Services.Controllers;

[ApiController]
[Route("api/led")]
public class LedController : ControllerBase
{
    private readonly ILogger<ConfigController> _logger;
    private readonly ILuxaforDeviceManager _deviceManager;

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public LedController(ILuxaforDeviceManager deviceManager, ILogger<ConfigController> logger)
    {
        _logger = logger;
        _deviceManager = deviceManager;
        _logger.LogWarning("Added ConfigController.");
    }

    [HttpPost("{command}")]
    public async Task<IActionResult> Execute(string command)
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        if (body == null)
        {
            throw new ArgumentException("Body appears to be empty.");
        }

        ICommand ledCommand;

        switch (command)
        {
            case "wave":
                ledCommand = JsonSerializer.Deserialize<WaveCommand>(body, _options);
                break;

            case "pattern":
                ledCommand = JsonSerializer.Deserialize<PatternCommand>(body, _options);
                break;

            default:
                throw new ArgumentException(nameof(command), command);
        }

        _deviceManager.Run(ledCommand);
        return Ok();
    }
    

    [HttpPost("{target}/{command}")]
    public async Task<IActionResult> RunAction(string target, string command)
    {
        if (!Enum.TryParse<Led>(target, true, out var led))
        {
            throw new ArgumentException("Do not recognise target Led '{0}'", target);
        }
        
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        if (body == null)
        {
            throw new ArgumentException("Body appears to be empty.");
        }

        ICommand ledCommand;

        switch (command)
        {
            case "on":
                var on = JsonSerializer.Deserialize<OnCommand>(body);
                on.Led = led;
                ledCommand = on;
                break;

            case "off":
                var off = new OffCommand
                {
                    Led = led
                };
                ledCommand = off;
                break;

            case "fade":
                var fade = JsonSerializer.Deserialize<FadeCommand>(body);
                fade.Led = led;
                ledCommand = fade;
                break;

            case "strobe":
                var strobe = JsonSerializer.Deserialize<StrobeCommand>(body);
                strobe.Led = led;
                ledCommand = strobe;
                break;

            default:
                throw new ArgumentException(nameof(command), command);
        }
        
        _deviceManager.Run(ledCommand);
        return Ok();
    }
}