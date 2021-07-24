public class PokemonMove
{
    public string moveName;
    public int levelLearnedAt;
    public string learnMethod;

    public PokemonMove(string name = "", int level = 0, string method = "")
    {
        this.moveName = name;
        this.levelLearnedAt = level;
        this.learnMethod = method;
    }
}