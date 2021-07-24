using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PokemonData
{
    public string id { get; set; }
    public string name { get; set; }
    public string[] types { get; set; }
    public List<PokemonAbility> abilities;
    public Texture2D frontSprite { get; set; }
    public Texture2D backSprite { get; set; }
    public string height { get; set; }
    public string weight { get; set; }
    public List<PokemonMove> moves;
    public string pokedexInfo { get; set; }

    public PokemonData(string id = "0", string name = "", string[] types = null, List<PokemonAbility> ability = null, Texture2D f_sprite = null, Texture2D b_sprite = null, string height = "", string weight = "", List<PokemonMove> moves = null, string info = null)
    {
        this.id = id;
        this.name = name;
        this.types = types == null ? new string[0] : types;
        this.abilities = ability == null ? new List<PokemonAbility>() : ability;
        this.frontSprite = f_sprite;
        this.backSprite = b_sprite;
        this.height = height;
        this.weight = weight;
        this.moves = moves == null ? new List<PokemonMove>() : moves;
        this.pokedexInfo = info;
    }
}