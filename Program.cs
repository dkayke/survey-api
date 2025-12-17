using Microsoft.EntityFrameworkCore;
using SurveyApiSystem.Application.Services;
using SurveyApiSystem.Domain.Interfaces;
using SurveyApiSystem.Infrastructure.Persistence;
using SurveyApiSystem.Infrastructure.Repositories; 

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SurveyDb"));

// Injeção de Dependência
builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<SurveyService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();