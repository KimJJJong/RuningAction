using System.Collections;
using TMPro;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    GameManager gameManager;
    HpController hpController;
    new Camera camera;
    //cam movement
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.5f;

    public bool isDmg;
    public bool isDmgIteam;
    public float chance;
    private Animator animator;
    public bool canInteract { get; set; }

    public TextMeshProUGUI cooldownText;

    void Start()
    {

        camera = FindAnyObjectByType<Camera>();
        hpController = FindAnyObjectByType<HpController>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();

        canInteract = true;
        cooldownText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isDmgIteam = UnityEngine.Random.value <= chance;
            other.gameObject.GetComponent<Collider>().enabled = false;
            if (!isDmg && !isDmgIteam)
            {
                Damage();
                cooldownText.enabled = true;
            }
            else
            {
                GameManager.Instance.score.IncreasObsScore();
                Debug.Log("뿌순건가?"); // Rush 파괴시 점수 추가는 여기에!!!
            }

        }
    }

    void Damage()
    {
        CamShake();
        if (canInteract)
        {
            animator.Play("Stumble");
            hpController.collsionObstacle();

            if (hpController.getValue() <= 0)
            {
                Die();
            }
            StartCoroutine(Cooldown());
        }
    }
    IEnumerator Cooldown()
    {
        StartCoroutine(DieAnimation());
        canInteract = false;
        StartCoroutine(BlinkCooldownText());


        yield return new WaitForSeconds(2f);
        cooldownText.enabled = false;

        canInteract = true;
    }

    void Die()
    {
        animator.Play("Stumble");
        GameManager.Instance.GameOver();
    }

    public void CamShake()
    {
        StartCoroutine(Shake());
    }
    IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(3);
    }

    IEnumerator BlinkCooldownText()
    {
        while (!canInteract)
        {
            cooldownText.enabled = !cooldownText.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            camera.transform.position += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }



}
