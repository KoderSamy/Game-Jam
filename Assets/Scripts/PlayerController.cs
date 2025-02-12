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

    public int breadCollected = 0; //  Убираем static!
    public int cheeseCollected = 0; //  Убираем static!
    public int extraIngredientsCollected = 0; //  Убираем static!

    void Start()
    {
        ResetIngredients(); //  Вызываем как обычный метод
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        UpdateRecipeUI();
    }


    public void ResetIngredients()  //  Убираем static!
    {
        breadCollected = 0;
        cheeseCollected = 0;
        extraIngredientsCollected = 0;
        UpdateRecipeUI(); // Обновляем UI сразу после сброса
    }

    public void CollectIngredient(string tag)
    {
        Debug.Log("PlayerController CollectIngredient: " + tag + 
              ", breadCollected: " + breadCollected + 
              ", cheeseCollected: " + cheeseCollected +
              ", extraIngredientsCollected: " + extraIngredientsCollected);

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

        Debug.Log("After collection: breadCollected: " + breadCollected + 
              ", cheeseCollected: " + cheeseCollected +
              ", extraIngredientsCollected: " + extraIngredientsCollected);
    }


    void UpdateRecipeUI()
    {
        recipeText.text = $"Хлеб: {breadCollected}/{breadNeeded}\n" +
                          $"Сыр: {cheeseCollected}/{cheeseNeeded}\n" +
                          $"Лишний: {extraIngredientsCollected}/{extraIngredientsAllowed}";
    }

    void CheckWin()
    {
        Debug.Log("Checking for win. Bread: " + breadCollected + ", Cheese: " + cheeseCollected);
        if (breadCollected == breadNeeded && cheeseCollected == cheeseNeeded)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0; // Пауза игры
        }
    }

    void CheckLose()
    {
        Debug.Log("Checking for lose. Extra ingredients: " + extraIngredientsCollected);
        if (extraIngredientsCollected >= extraIngredientsAllowed)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0; // Пауза игры
        }
    }
}