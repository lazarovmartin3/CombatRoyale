using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private Image hp_image;

    private float currentHealth;
    private float maxHealth;

    private float updateTimeElapsed;
    private float updateTime = 1;

    private void Start()
    {
        DisableHpImage();
    }

    private void Update()
    {
        if (hp_image.enabled == true)
        {
            updateTimeElapsed += Time.deltaTime;
            if (updateTimeElapsed > updateTime)
            {
                updateTimeElapsed = 0;
                DisableHpImage();
            }
        }
    }

    public void SetHealth(float maxHP)
    {
        currentHealth = maxHealth = maxHP;
    }

    public void TakeDamage(float damage)
    {
        updateTimeElapsed = 0;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GetComponent<Unit>().Dead();
        }

        UpdateHpImage();
    }

    private void DisableHpImage()
    {
        hp_image.enabled = false;
    }

    private void UpdateHpImage()
    {
        hp_image.enabled = true;
        Vector3 scale = hp_image.transform.localScale;
        scale.x = (currentHealth / maxHealth);
        hp_image.transform.localScale = scale;
    }
}