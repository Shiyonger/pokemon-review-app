using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Pokemon> GetPokemons()
    {
        return _context.Pokemon.OrderBy(p => p.Id).ToList();
    }

    public Pokemon GetPokemon(int id)
    {
        return _context.Pokemon.FirstOrDefault(p => p.Id == id);
    }

    public Pokemon GetPokemon(string name)
    {
        return _context.Pokemon.FirstOrDefault(p => p.Name == name);
    }

    public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonDto)
    {
        return GetPokemons().FirstOrDefault(c => 
            c.Name.Trim().ToUpper() == pokemonDto.Name.Trim().ToUpper());
    }

    public decimal GetPokemonRating(int pokeId)
    {
        var reviews = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

        if (!reviews.Any())
            return 0;

        return (decimal)reviews.Average(r => r.Rating);
    }

    public bool PokemonExists(int pokeId)
    {
        return _context.Pokemon.Any(p => p.Id == pokeId);
    }

    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
        var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon
        };
        var pokemonCategory = new PokemonCategory()
        {
            Category = category,
            Pokemon = pokemon
        };

        _context.Add(pokemonOwner);
        _context.Add(pokemonCategory);
        _context.Add(pokemon);

        return Save();
    }

    public bool UpdatePokemon(Pokemon pokemon)
    {
        _context.Update(pokemon);
        return Save();
    }

    public bool DeletePokemon(Pokemon pokemon)
    {
        _context.Remove(pokemon);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}