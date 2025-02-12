using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int breadNeeded = 2;
    public int cheeseNeeded = 1;
    public int extraIngredientsAllowed = 3;

    public string breadTag = "Bread";
    public string cheeseTag = "Cheese";

    public TextMeshProUGUI recipeText;
    public GameObject winPanel;
    public GameObject losePanel;

    private int breadCollected = 0;
    private int cheeseCollected = 0;
    private int extraIngredientsCollected = 0;


    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        UpdateRecipeUI();
    }

    public void CollectIngredient(string tag)
    {
        if (tag == breadTag && breadCollected < breadNeeded)
        {
            breadCollected++;
        }
        else if (tag == cheeseTag && cheeseCollected < cheeseNeeded)
        {
            cheeseCollected++;
        }
        else
        {
            extraIngredientsCollected++; //  Сюда попадут все лишние ингредиенты, включая избыточный хлеб/сыр
        }

        UpdateRecipeUI();
        CheckWin();
        CheckLose();
    }


    void UpdateRecipeUI()
    {
        recipeText.text = $"Хлеб: {breadCollected}/{breadNeeded}\n" +
                          $"Сыр: {cheeseCollected}/{cheeseNeeded}\n" +
                          $"Лишний: {extraIngredientsCollected}/{extraIngredientsAllowed}";
    }

    void CheckWin()
    {
        if (breadCollected == breadNeeded && cheeseCollected == cheeseNeeded)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0; // Пауза игры
        }
    }

    void CheckLose()
    {
        if (extraIngredientsCollected >= extraIngredientsAllowed)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0; // Пауза игры
        }
    }
}