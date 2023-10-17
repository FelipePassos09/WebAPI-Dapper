using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Bem-Vindo as API Tarefas - {DateTime.Now}");

        app.MapGet("/tarefas", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var tarefas = con.GetAll<Tarefa>().ToList();
            return Results.Ok(tarefas);
        }
        );

        app.MapGet("tarefas/{id}", async (GetConnection conneectionGetter, int id) =>
        {
            using var connection = await conneectionGetter();
            var tarefa = connection.Get<Tarefa>(id);

            if (tarefa is null) { return Results.NotFound("Tarefa não encontrada"); }

            return Results.Ok(tarefa);
        }
        );

        app.MapPost("tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
        {
            using var connection = await connectionGetter();
            var id = connection.Insert(tarefa);
            return Results.Created($"/tarefas/{id}", tarefa);
        }
        );

        app.MapPut("tarefas/{id}", async (GetConnection connectionGetter, int id, Tarefa tarefa) =>
        {
            using var connection = await connectionGetter();
            var search = connection.Get<Tarefa>(id);
            if (search is null) { return Results.NotFound(); }

            if (connection.Update<Tarefa>(tarefa)) { return Results.Ok(tarefa); } else { return Results.BadRequest(); }

        });

        app.MapDelete("tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var connection = await connectionGetter();
            var tarefa = connection.Get<Tarefa>(id);

            if (tarefa is null) { return Results.NotFound("Id não encontrado."); }

            else
            {
                try
                {
                    if (connection.Delete(tarefa)) { return Results.Ok(tarefa); }
                    else { return Results.Problem(); }
                }
                catch
                {
                    return Results.BadRequest();
                }
            }

        });


    }
}
