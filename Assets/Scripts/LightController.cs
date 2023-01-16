using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public Vector2 BrightnessRange;
        public Vector2 ContrastRange;
    }


    public PostProcessProfile PostProcess;
    public Slider Slider;
    public Image ValueIcon;
    public ColorPickerUnityUI ColorPicker;

    public Sprite BrightnessSprite;
    public Sprite ContrastSprite;

    public Settings settings;

    private Action<float> _sliderChange;
    private ColorGrading _colorGrading;
    


    private void OnEnable()
    {
        Slider.onValueChanged.AddListener(OnSliderChange);
    }

    private void Start()
    {
        _colorGrading = PostProcess.GetSetting<ColorGrading>();
        _colorGrading.postExposure.overrideState = true;
        _colorGrading.colorFilter.overrideState = true;
        _colorGrading.contrast.overrideState = true;
        _colorGrading.contrast.value = 0;
        _colorGrading.postExposure.value = 0;
        _colorGrading.colorFilter.value = Color.white;
        ColorPicker.value = Color.white;
        EnableBrightnessMode();
    }

    private void Update() => _colorGrading.colorFilter.value = ColorPicker.value;

    private void OnSliderChange(float value) => _sliderChange.Invoke(value);

    private void OnDisable() => Slider.onValueChanged.RemoveListener(OnSliderChange);

    public void EnableBrightnessMode()
    {
        Slider.gameObject.SetActive(true);
        ColorPicker.gameObject.SetActive(false);
        ValueIcon.sprite = BrightnessSprite;
        _sliderChange = ChangeBrightness;
        Slider.value = Mathf.InverseLerp(settings.BrightnessRange.x, settings.BrightnessRange.y,
            _colorGrading.postExposure.value);
        print("EnableBrightMode");
    }

    public void EnableContrastMode()
    {
        Slider.gameObject.SetActive(true);
        ColorPicker.gameObject.SetActive(false);
        ValueIcon.sprite = ContrastSprite;
        _sliderChange = ChangeContrast;
        Slider.value = Mathf.InverseLerp(settings.ContrastRange.x, settings.ContrastRange.y,
            _colorGrading.contrast.value);
        print("EnableContrastMode");
    }

    public void EnableColorModer()
    {
        Slider.gameObject.SetActive(false);
        ColorPicker.gameObject.SetActive(true);
        print("EnableColorModer");
    }


    private void ChangeBrightness(float value) => _colorGrading.postExposure.value =
        Mathf.Lerp(settings.BrightnessRange.x, settings.BrightnessRange.y, value);

    private void ChangeContrast(float value) => _colorGrading.contrast.value =
        Mathf.Lerp(settings.ContrastRange.x, settings.ContrastRange.y, value);
}