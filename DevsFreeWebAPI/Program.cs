using Microsoft.EntityFrameworkCore;
using DevsFreeWebAPI.Data;  // Adjust this if your DatabaseContext is in a different namespace.
using DevsFreeWebAPI.Models;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Your API Name", Version = "v1" });
});


//builder.Services.AddDbContext<DatabaseContext>(options =>
 //   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
    });
}

app.UseHttpsRedirection();

// COLLABORATORS CRUD

app.MapGet("/collaborators", async (DatabaseContext context) =>
{
    return await context.Collaborators.ToListAsync();
}).WithOpenApi();

app.MapPost("/collaborators", async (DatabaseContext context, Collaborator collaborator) =>
{
    context.Collaborators.Add(collaborator);
    await context.SaveChangesAsync();
    return Results.Created($"/collaborators/{collaborator.Id}", collaborator);
}).WithOpenApi();

app.MapPut("/collaborators/{id}", async (DatabaseContext context, int id, Collaborator updatedCollaborator) =>
{
    var collaborator = await context.Collaborators.FindAsync(id);
    if (collaborator == null)
    {
        return Results.NotFound();
    }

    collaborator.Name = updatedCollaborator.Name;
    collaborator.Email = updatedCollaborator.Email;
    collaborator.Telefone = updatedCollaborator.Telefone;
    collaborator.Mensagem = updatedCollaborator.Mensagem;

    await context.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();

app.MapDelete("/collaborators/{id}", async (DatabaseContext context, int id) =>
{
    var collaborator = await context.Collaborators.FindAsync(id);
    if (collaborator == null)
    {
        return Results.NotFound();
    }

    context.Collaborators.Remove(collaborator);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();

// PARTNERS CRUD

app.MapGet("/partners", async (DatabaseContext context) =>
{
    return await context.Partners.ToListAsync();
}).WithOpenApi();

app.MapPost("/partners", async (DatabaseContext context, Partner partner) =>
{
    context.Partners.Add(partner);
    await context.SaveChangesAsync();
    return Results.Created($"/partners/{partner.Id}", partner);
}).WithOpenApi();

app.MapPut("/partners/{id}", async (DatabaseContext context, int id, Partner updatedPartner) =>
{
    var partner = await context.Partners.FindAsync(id);
    if (partner == null)
    {
        return Results.NotFound();
    }

    partner.Name = updatedPartner.Name;
    partner.Email = updatedPartner.Email;
    partner.Telefone = updatedPartner.Telefone;
    partner.Mensagem = updatedPartner.Mensagem;
    partner.CNPJ = updatedPartner.CNPJ;

    await context.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();

app.MapDelete("/partners/{id}", async (DatabaseContext context, int id) =>
{
    var partner = await context.Partners.FindAsync(id);
    if (partner == null)
    {
        return Results.NotFound();
    }

    context.Partners.Remove(partner);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();

app.Run();