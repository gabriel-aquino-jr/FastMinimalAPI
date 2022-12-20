namespace FastMinimalAPI;

public static class HeroEndpoints
{
    public static void MapHeroEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Hero").WithTags(nameof(Hero));

        group.MapGet("/", async (FastMinimalAPIContext db) =>
        {
            return await db.Hero.ToListAsync();
        })
        .WithName("GetAllHeros")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Hero>, NotFound>> (int id, FastMinimalAPIContext db) =>
        {
            return await db.Hero.FindAsync(id)
                is Hero model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetHeroById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, Hero hero, FastMinimalAPIContext db) =>
        {
            var foundModel = await db.Hero.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }
            
            db.Update(hero);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateHero")
        .WithOpenApi();

        group.MapPost("/", async (Hero hero, FastMinimalAPIContext db) =>
        {
            db.Hero.Add(hero);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Hero/{hero.Id}",hero);
        })
        .WithName("CreateHero")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok<Hero>, NotFound>> (int id, FastMinimalAPIContext db) =>
        {
            if (await db.Hero.FindAsync(id) is Hero hero)
            {
                db.Hero.Remove(hero);
                await db.SaveChangesAsync();
                return TypedResults.Ok(hero);
            }

            return TypedResults.NotFound();
        })
        .WithName("DeleteHero")
        .WithOpenApi();
    }
}
