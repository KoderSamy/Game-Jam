using UnityEngine;

public class SwordController : MonoBehaviour
{
    public PlayerController player; //  Можно убрать эту public переменную

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("PlayerController not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword OnTriggerEnter entered. Collided with: " + other.gameObject.name + ", tag: " + other.gameObject.tag);

        if (player == null) //  Эта проверка теперь избыточна, но можно оставить для безопасности.
        {
            Debug.LogError("PlayerController not found!");
            return;
        }

        if (other.CompareTag(player.breadTag) && player.breadCollected < player.breadNeeded)
        {
            Debug.Log("Collecting ingredient: " + other.tag); // other.tag
            player.CollectIngredient(other.tag);           // other.tag
            Destroy(other.gameObject);
        }
        else if (other.CompareTag(player.cheeseTag) && player.cheeseCollected < player.cheeseNeeded)
        {
            Debug.Log("Collecting ingredient: " + other.tag); // other.tag
            player.CollectIngredient(other.tag);           // other.tag
            Destroy(other.gameObject);
        }
        else if (player.extraIngredientsCollected < player.extraIngredientsAllowed)
        {
            Debug.Log("Collecting extra ingredient.");
            player.CollectIngredient("Extra"); 
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("Max ingredients or extra ingredients reached.");
        }
    }
}