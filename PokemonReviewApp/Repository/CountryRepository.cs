﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Country> GetCountries()
    {
        return _context.Countries.ToList();
    }

    public Country GetCountry(int id)
    {
        return _context.Countries.FirstOrDefault(c => c.Id == id);
    }

    public Country GetCountryByOwner(int ownerId)
    {
        return _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();
    }

    public bool CountryExists(int id)
    {
        return _context.Countries.Any(c => c.Id == id);
    }

    public bool CreateCountry(Country country)
    {
        _context.Add(country);
        return Save();
    }

    public bool UpdateCountry(Country country)
    {
        _context.Update(country);
        return Save();
    }

    public bool DeleteCountry(Country country)
    {
        _context.Remove(country);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}