using UnityEngine;
using TMPro;
using System.Collections;

public class AttemptManager : MonoBehaviour
{
    private TMP_Text attemptText;
    private const string PREFS_KEY = "LevelAttempts";

    [Header("Settings")]
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float fadeSpeed = 2f;

    private void Awake()
    {
        attemptText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        int currentAttempt = PlayerPrefs.GetInt(PREFS_KEY, 1);

        attemptText.text = $"Попытка {currentAttempt}";

        PlayerPrefs.SetInt(PREFS_KEY, currentAttempt + 1);
        PlayerPrefs.Save();

        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        yield return new WaitForSeconds(displayTime);

        Color originalColor = attemptText.color;

        while (attemptText.color.a > 0)
        {
            float newAlpha = attemptText.color.a - (Time.deltaTime * fadeSpeed);
            attemptText.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public static void ResetAttempts()
    {
        PlayerPrefs.DeleteKey(PREFS_KEY);
    }
}