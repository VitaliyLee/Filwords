using GamePush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdapter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color barsColor = Color.black;
    [SerializeField] private Sprite _backgroundCameraImage;

    private Camera _mainCamera;
    private Camera _backgroundCamera;
    private const float TargetAspect = 9f / 16f;

    private int _lastWidth;
    private int _lastHeight;

    private void OnEnable()
    {
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
    }

    private void Start()
    {
        CreateBackgroundCamera();
        InitializeCameras();
        UpdateCameraView();
    }

    private void CreateBackgroundCamera()
    {
        // Создаем объект для фоновой камеры
        GameObject bgCameraObject = new GameObject("BackgroundCamera");
        _backgroundCamera = bgCameraObject.AddComponent<Camera>();

        // Настраиваем параметры фоновой камеры
        _backgroundCamera.depth = -2;
        _backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        _backgroundCamera.cullingMask = 0;
    }

    private void InitializeCameras()
    {
        _mainCamera = GetComponent<Camera>();
        _mainCamera.depth = -1;
        _mainCamera.clearFlags = CameraClearFlags.Depth;

        UpdateBackgroundColor();
    }

    private void UpdateCameraView()
    {
        if (Screen.height > Screen.width) return;

        float currentAspect = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(currentAspect, TargetAspect))
        {
            _mainCamera.rect = new Rect(0, 0, 1, 1);
            return;
        }

        if (currentAspect > TargetAspect)
        {
            float width = TargetAspect / currentAspect;
            _mainCamera.rect = new Rect((1f - width) * 0.5f, 0f, width, 1f);
        }
        else
        {
            float height = currentAspect / TargetAspect;
            _mainCamera.rect = new Rect(0f, (1f - height) * 0.5f, 1f, height);
        }
    }

    // Метод для динамического изменения цвета
    public void SetBarsColor(Color newColor)
    {
        barsColor = newColor;
        UpdateBackgroundColor();
    }

    private void UpdateBackgroundColor()
    {
        if (_backgroundCamera != null)
        {
            _backgroundCamera.backgroundColor = barsColor;
        }
    }

    // Для обработки изменения разрешения
    //private void Update()
    //{
    //    if (Screen.width != _lastWidth || Screen.height != _lastHeight)
    //    {
    //        UpdateCameraView();
    //    }
    //}
}
