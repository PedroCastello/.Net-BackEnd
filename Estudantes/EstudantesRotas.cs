using ApiCrud.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes;

public static class EstudantesRotas
{
    public static void addRotasEstudantes(this WebApplication app)
    {
        var rotasEstudantes = app.MapGroup("estudantes");
       
        //Adicionar Estudante
        rotasEstudantes.MapPost("",
            async (AddEstudanteRequest request, AppDbContext context) =>
            {
                var jaExiste = await context.Estudantes
                    .AnyAsync(estudante => estudante.Nome == request.Nome);

                if (jaExiste)
                    return Results.Conflict("Ja existe!");
                
                var novoEstudante = new Estudante2(request.Nome); 
                await context.Estudantes.AddAsync(novoEstudante);
                await context.SaveChangesAsync();

                var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);

                return Results.Ok(estudanteRetorno);

            });
        //Retornar todos Estudantes
        rotasEstudantes.MapGet("", async (AppDbContext context) =>
        {
            var estudantes = await context
                .Estudantes
                .Where(estudante => estudante.Ativo)
                .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
                .ToListAsync();
            return estudantes;
        });
        // Atualizar Estudante
        rotasEstudantes.MapPut("{id}", async (Guid id,UpdateEstudanteRequest request, AppDbContext context) =>
        {
            var estudante = await context.Estudantes
                .SingleOrDefaultAsync(estudante => estudante.Id == id);

            if (estudante == null)
                return Results.NotFound();
            
            estudante.AtualizarNome(request.Nome);

            await context.SaveChangesAsync();

            return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
        });
        
        // Deletar Estudante
        rotasEstudantes.MapDelete("{id}",
            async (Guid id, AppDbContext context) =>
        {
            var estudante = await context.Estudantes
                .SingleOrDefaultAsync(estudante => estudante.Id == id);

            if (estudante == null)
                return Results.NotFound();

            estudante.Desativar();

            await context.SaveChangesAsync();

            return Results.Ok();
        });
    }
}