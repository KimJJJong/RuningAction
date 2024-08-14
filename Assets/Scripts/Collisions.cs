using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collisions : MonoBehaviour
{
    GameManager gameManager;
    HpController hpController;
    new Camera camera;
    //cam movement
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.5f;

    public int currentHealth;
    private Animator animator;
    public bool canInteract{get; set;}

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
        if (other.CompareTag("Obstacle") )
        {
            Damage();
            cooldownText.enabled = true;

        }
    }

    void Damage()
    {
        CamShake();
        if (canInteract)
        {
            animator.Play("Stumble");
            hpController.collsionObstacle();

            if (hpController.getValue()<= 0)
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
            camera.transform.position +=  (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }



}
