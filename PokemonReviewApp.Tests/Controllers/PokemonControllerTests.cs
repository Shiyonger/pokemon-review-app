using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Tests.Controllers;

public class PokemonControllerTests
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public PokemonControllerTests()
    {
        _pokemonRepository = A.Fake<IPokemonRepository>();
        _reviewRepository = A.Fake<IReviewRepository>();
        _mapper = A.Fake<IMapper>();
    }

    [Fact]
    public void PokemonController_GetPokemons_ReturnOk()
    {
        var pokemons = A.Fake<ICollection<PokemonDto>>();
        var pokemonList = A.Fake<List<PokemonDto>>();
        A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonList);
        var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

        var result = controller.GetPokemons();

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    public void PokemonController_CreatePokemon_ReturnOk()
    {
        int ownerId = 1;
        int categoryId = 1;
        var pokemonCreate = A.Fake<PokemonDto>();
        var pokemon = A.Fake<Pokemon>();
        A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate)).Returns(null);
        A.CallTo(() => _mapper.Map<Pokemon>(pokemonCreate)).Returns(pokemon);
        A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon)).Returns(true);
        var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

        var result = controller.CreatePokemon(ownerId, categoryId, pokemonCreate);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
    }
}