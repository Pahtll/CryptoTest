using CryptoTest.API.Traits;
using CryptoTest.Application.Interfaces;
using CryptoTest.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTest.API.Endpoints;

public static class MessageEndpoint
{
    public static IEndpointRouteBuilder MapMessageEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/messages/all", GetAllMessages);
        app.MapGet("/messages/{id:int}", GetMessageById);
        app.MapGet("/messages/since", GetAllMessagesSince);
        app.MapPost("/messages/create", CreateMessage);
        
        return app;
    }
    
    private static async Task<IResult> GetAllMessages(IMessageService messageService)
    {
        try
        {
            var messages = await messageService.GetAll();
            return Results.Ok(messages);
        }
        catch (ArgumentException argumentException)
        {
            return Results.BadRequest(argumentException.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetMessageById(IMessageService messageService, int id)
    {
        try
        {
            var message = await messageService.GetById(id);
            return Results.Ok(message);
        }
        catch (ArgumentException argumentException)
        {
            return Results.BadRequest(argumentException.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    private static async Task<IResult> GetAllMessagesSince(
        [FromServices] IMessageService messageService, 
        [FromBody] DateTimeRange timeRange)
    {
        try
        {
            var messages = await messageService.GetAllMessagesSince(timeRange.Since, timeRange.Until);
            return Results.Ok(messages);
        }
        catch (ArgumentException argumentException)
        {
            return Results.BadRequest(argumentException.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    private static async Task<IResult> CreateMessage(
        IMessageService messageService, 
        [FromBody] string text)
    {
        try
        {
            var message = new Message
            {
                Text = text,
                SentAt = DateTime.Now
            };
            message.Id = await messageService.Create(message);
            return Results.Created($"/messages/{message.Id}", message);
        }
        catch (ArgumentException argumentException)
        {
            return Results.BadRequest(argumentException.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}