using UnityEngine;

public class SwordController : MonoBehaviour
{
    public PlayerController player;  // Ссылка на PlayerController

    private void OnTriggerEnter(Collider other)
    {
         if (player == null)
        {
             player = FindObjectOfType<PlayerController>(); // Находим PlayerController в сцене
             if (player == null)
             {
                 Debug.LogError("PlayerController not found!");
                 return;
             }
         }


        if (other.CompareTag(player.breadTag) || other.CompareTag(player.cheeseTag))
        {
            player.CollectIngredient(other.tag);
            Destroy(other.gameObject);
        }
        else
        {
            player.CollectIngredient("Extra"); // Передаем "Extra" для лишних ингредиентов
            Destroy(other.gameObject);
        }
    }
}