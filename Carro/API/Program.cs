using API.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    /*options.EnableAnnotations();*/
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        
        Title = "Swagger Documentação Web API Carros",
        Description = "EndPoints para gerenciar cadastro de carros",
        Contact = new OpenApiContact(){
            Name = "Jean Moreira",
            Email = "jean.moreira@cs.up.edu.br"
    },
        License = new OpenApiLicense(){
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    }
);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//  Endpoints relacionados ao recurso de Carros
//GET: Lista todos os carros cadastrados
app.MapGet("/api/carros", ([FromServices] AppDataContext ctx) =>{
    if(ctx.Carros.Any()){
        return Results.Ok(ctx.Carros.ToList());
    }
    return Results.NotFound();
});

//POST: CADASTRA NOVOS CARROS
app.MapPost("/api/carros", ([FromBody]Carro carro,[FromServices] AppDataContext ctx) =>{

    carro.Modelo = ctx.Modelos.Find(carro.Modelo.Id);

    if(carro.Modelo == null){
        return Results.BadRequest("Modelo do carro não existe!");
    }
    if (carro.Name == null || carro.Name.Length < 3){
        return Results.BadRequest("Nome do carro deve conter mais de 3 caracteres.");
    }

        ctx.Carros.Add(carro);
        ctx.SaveChanges();

        return Results.Created("",carro);

});

//GET: LISTA OS CARROS PELO (ID)
app.MapGet("api/carros/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>{

    Carro? carro = ctx.Carros.Find(id);
    if(carro != null){
        return Results.Ok(carro);
    }
    return Results.NotFound();
    
});

app.MapPut("api/carros/{id}", ([FromRoute] int id, [FromBody] Carro carro, [FromServices] AppDataContext ctx) =>{

    Carro? carroUpdate = ctx.Carros.Find(id);
    carroUpdate.Modelo = ctx.Modelos.Find(id);

     if(carroUpdate.Modelo == null){
        return Results.BadRequest("Modelo do carro não existe!");
    }
    if (carro.Name == null || carro.Name.Length < 3){
        return Results.BadRequest("Nome do carro deve conter mais de 3 caracteres.");
    }


   if   (carroUpdate != null){
        carroUpdate.Name = carro.Name;
        ctx.Carros.Update(carroUpdate);
        ctx.SaveChanges();
        return Results.Ok(carroUpdate);
   }
   return Results.NotFound();

});

app.MapDelete("api/carros/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>{
    
    Carro? carro = ctx.Carros.Find(id);
    
    if(carro == null){
        return Results.NotFound();
    }
    ctx.Carros.Remove(carro);
    ctx.SaveChanges();
    return Results.NoContent();
});

//GET: Lista todos os modelos cadastrados
app.MapGet("api/modelos", ( [FromQuery] string? name, [FromServices] AppDataContext ctx) =>{

    var query = ctx.Modelos.AsQueryable();

    if(!string.IsNullOrWhiteSpace(name)){
        query.Where(m => EF.Functions.Like(m.Nome, $"%{name}%"));
    }

    var modelos = query.ToList();
    if(modelos == null || modelos.Count == 0){
        return Results.NotFound();
    }
    return Results.Ok(modelos);
});


//GET: Busca carro pelo ID
app.MapGet("api/modelos/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>{

    var modelos = ctx.Modelos.Find(id);
    if(modelos == null){
        return Results.NotFound();
    }
    return Results.Ok(modelos);
});

app.Run();
