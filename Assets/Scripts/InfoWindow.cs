using UnityEngine;

public class InfoWindow : MonoBehaviour
{
    private const string githubRepoUrl = "https://www.github.com/Devanshu19/PokeApiApp";
    private const string pokeApiUrl = "https://www.pokeapi.co/";

    //Open url based on the webpage name
    public void OpenWebpade(string webpageName)
    {
        if (webpageName.ToLower() == "github")
        {
            Application.OpenURL(githubRepoUrl);
        }
        else if (webpageName.ToLower() == "pokeapi")
        {
            Application.OpenURL(pokeApiUrl);
        }
    }
}
