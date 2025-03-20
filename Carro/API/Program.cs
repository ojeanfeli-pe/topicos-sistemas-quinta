using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Carro> carros = new List<Carro>(){
    new Carro {Id = 1, Name = "Fusca"},
    new Carro {Id = 2, Name = "Ferrari"}
};

//Exemplo de requisição
app.MapGet("/", () => "Hello World!");

//endpoint listar carros
app.MapGet("/api/carros", () => {
    return Results.Ok(carros);
});

app.Run();
