using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winPanel;

    [Header("Settings")]
    [SerializeField] private string menuSceneName = "Menu";

    private bool isFinished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFinished) return;

        if (collision.TryGetComponent(out PlayerController player))
        {
            FinishLevel(player);
        }
    }

    private void FinishLevel(PlayerController player)
    {
        isFinished = true;
        player.enabled = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = Vector2.zero;
#else
        rb.velocity = Vector2.zero;
#endif
        rb.bodyType = RigidbodyType2D.Kinematic;

        winPanel.SetActive(true);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}