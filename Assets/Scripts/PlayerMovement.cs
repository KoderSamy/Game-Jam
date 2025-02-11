using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float laneDistance = 1f;
    public float laneChangeDuration = 0.2f;
    private Rigidbody rb;
    private int currentLane = 1;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector3 targetPosition;
    private bool isMovingToLane = false;
    private float laneChangeStartTime;

    public float jumpForce = 6f;
    private bool jumpEnabled = true;
    public LayerMask groundLayer; // Слой земли для проверки приземления
    private InputAction jumpAction;
    private bool isGrounded;

    public GameObject shieldPrefab; // Префаб щита
    private GameObject shieldInstance; // Экземпляр щита
    private InputAction shieldAction; // Действие для щита (правая кнопка мыши)
    public float maxShieldTime = 2f;
    private float shieldTimer = 0f;
    private float shieldDistance = 0.5f; // Расстояние щита по умолчанию

    public GameObject swordPrefab; // Префаб меча
    private GameObject swordInstance; // Экземпляр меча
    private Level1 levelSettings;
    private InputAction sliceAction; // Новое действие для меча
    public float swordActiveTime = 2f;  // Время жизни меча
    private float swordTimer = 0f;  // Таймер для меча

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        moveAction = playerInput.Player.Move;
        jumpAction = playerInput.Player.Jump;
        shieldAction = playerInput.Player.Fire;
        sliceAction = playerInput.Player.Slice;
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += Jump;
        shieldAction.Enable();
        shieldAction.performed += Shield;
        shieldAction.canceled += Shield;
        sliceAction.Enable();
        sliceAction.performed += OnFire;
        sliceAction.canceled += OnFire;
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        jumpAction.performed -= Jump;
        shieldAction.Disable();
        shieldAction.performed -= Shield;
        shieldAction.canceled -= Shield;
        sliceAction.Disable();
        sliceAction.performed -= OnFire;
        sliceAction.canceled -= OnFire;
        DestroyShield();
        DestroySword();
    }

    void Update()
    {
        if (shieldInstance != null)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= maxShieldTime)
            {
                DestroyShield();
            }
        }

        if (swordInstance != null)
        {
            swordTimer += Time.deltaTime;
            if (swordTimer >= swordActiveTime)
            {
                DestroySword();
            }
        }
    }

    //============================================================================
    //================================== SHIELD ==================================
    //============================================================================

    private void Shield(InputAction.CallbackContext context)
    {
        if (context.performed && swordInstance == null) //И чтобы не было меча
        {
            CreateShield();
            shieldTimer = 0f;
        }
         else if (context.canceled)
        {
            DestroyShield();
        }
    }

    void CreateShield()
    {
        if (shieldInstance == null && levelSettings != null && this.levelSettings.enabled)
        {
            swordTimer = 0;
            DestroySword();
            shieldInstance = Instantiate(shieldPrefab, transform.position + transform.forward * shieldDistance, transform.rotation);
            shieldInstance.transform.parent = transform;
        }
    }

    void DestroyShield()
    {
        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
            shieldInstance = null; //  Добавляем это для немедленного обнуления
        }
    }

    //============================================================================
    //================================== SWORD ===================================
    //============================================================================

    void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && levelSettings != null && levelSettings.canSlice && shieldInstance == null)
        {
            CreateSword();
            swordTimer = 0f; //Обнуляем
        }
        else if (context.canceled)
        {
            DestroySword();
        }
    }

    private void CreateSword()
    {
        if (swordPrefab != null && swordInstance == null)
        {
            shieldTimer = 0;
            DestroyShield();
            swordInstance = Instantiate(swordPrefab, transform.position + transform.forward * 0.5f, transform.rotation);
            swordInstance.SetActive(true);  //  Добавляем эту строку, чтобы активировать меч!
            swordTimer = 0f;
        }
    }

    void DestroySword()
    {
        if (swordInstance != null)
        {
            Destroy(swordInstance);
            swordInstance = null; //  Добавляем это для немедленного обнуления
        }
    }

    //============================================================================
    //================================== OTHER ===================================
    //============================================================================
    
    void Start()
    {
        targetPosition = transform.position;
        rb.constraints = RigidbodyConstraints.FreezeRotation; //  Заморозить вращение, если нужно

        // Проверяем наличие LevelSlothSettings в сцене
        levelSettings = FindObjectOfType<Level1>();
        if (levelSettings != null)
        {
            shieldDistance = levelSettings.shieldDistance; // Получаем расстояние щита из LevelSlothSettings
            jumpEnabled = !levelSettings.disableJump; // Инвертируем значение, тк по умолчанию прыжок включен.
        }
    }

    void FixedUpdate()
    {
        // Движение вперед
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

        Vector2 input = moveAction.ReadValue<Vector2>();

        // Перемещение по дорожкам (теперь все в FixedUpdate)
        if (input.x != 0)
        {
            int targetLane = currentLane + (int)Mathf.Sign(input.x);
            targetLane = Mathf.Clamp(targetLane, 0, 2);

            if (targetLane != currentLane && !isMovingToLane)
            {
                currentLane = targetLane;
                targetPosition.x = (currentLane - 1) * laneDistance; //  Используем  laneDistance
                isMovingToLane = true;
                laneChangeStartTime = Time.time; //  Инициализируем время здесь
            }
        }

        if (isMovingToLane)
        {
            float elapsedTime = Time.time - laneChangeStartTime;
            float t = Mathf.Clamp01(elapsedTime / laneChangeDuration);
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, transform.position.z), t);

            if (t >= 1f)
            {
                isMovingToLane = false;
                transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z); //  Устанавливаем точную позицию
            }
        }

        // Проверка приземления
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.1f, 0.45f, groundLayer);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && jumpEnabled)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (shieldInstance == null) // Умираем от пули, только если щита нет
            {
                Die();
            }
        }

         if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die()  //  Новый метод для перезапуска сцены
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other) // Используем OnTriggerEnter
    {
        if (other.CompareTag("Finish"))
        {
            LoadNextLevel(); // Вызываем метод загрузки следующего уровня
        }

         if (other.gameObject.CompareTag("Object"))
        {
             Destroy(other.gameObject);
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Вы прошли все уровни!"); // Обработка окончания игры
        }
    }
}