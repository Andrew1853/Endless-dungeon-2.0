using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage; // Sprite для прицела
    public Sprite aimHitSprite; // GameObject для индикации попадания
    Sprite defaultSprite;
    public Color targetTypeAColor = Color.red; // Цвет для первого типа объектов
    public Color targetTypeBColor = Color.green; // Цвет для второго типа объектов
    public float fadeDuration = 0.5f; // Время затухания спрайта

    [SerializeField] string _enemyTag;
    [SerializeField] string _wallTag;
    private void Start()
    {
        defaultSprite = crosshairImage.sprite;
    }
    private void Update()
    {
        if (crosshairImage.sprite == aimHitSprite)
        {
            return;
        }
        // Проверяем, на что смотрит камера
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider != null)
            {
                // Проверяем тип объекта
                if (hit.collider.CompareTag(_enemyTag))
                {
                    crosshairImage.color = targetTypeAColor;
                }
                else if (hit.collider.CompareTag(_wallTag))
                {
                    crosshairImage.color = targetTypeBColor;
                }
                else
                {
                    crosshairImage.color = Color.white; // Цвет по умолчанию
                }
            }
        }
        else
        {
            crosshairImage.color = Color.white; // Цвет по умолчанию
        }
    }
    [ContextMenu(nameof(ShowHitIndicator))]
    public void ShowHitIndicator()
    {
        if (aimHitSprite != null)
        {
            StopAllCoroutines();
            crosshairImage.sprite = aimHitSprite;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        Color color = crosshairImage.color;
        color.a = 1; // Устанавливаем альфа-канал на 1
        crosshairImage.color = color;

        // Ждем полсекунды
        yield return new WaitForSeconds(fadeDuration);

        // Плавное затухание
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            crosshairImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Устанавливаем альфа-канал на 0 и деактивируем объект
        color.a = 0;
        crosshairImage.color = color;
        crosshairImage.sprite = defaultSprite;
    }
}
