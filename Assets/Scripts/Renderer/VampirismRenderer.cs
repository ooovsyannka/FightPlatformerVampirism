using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VampirismRenderer : MonoBehaviour
{
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private Slider _bar;
    [SerializeField] private SpriteRenderer _zone;

    private Coroutine _smoothlyChangeValueCoroutine;
    private Coroutine _smoothlyRendererZoneCoroutine;
    private Color _colorZone;
    private float _stepChangeValue;
    private float _stepRendererZone = 1f;
    private float _maxAlphaValue = 0.5f;

    private void Awake()
    {
        _colorZone = Color.red;
        _colorZone.a = 0;
        _zone.color = _colorZone;
        _bar.maxValue=  _vampirism.SkillDuration;
    }

    private void OnEnable()
    {
        _vampirism.SkillUsed += DecreaseValue;
        _vampirism.SkillRestored += IncreaseValue;
    }

    private void OnDisable()
    {
        _vampirism.SkillUsed -= DecreaseValue;
        _vampirism.SkillRestored -= IncreaseValue;
    }

    private void DecreaseValue()
    {
        if (_smoothlyChangeValueCoroutine != null)
            StopCoroutine(_smoothlyChangeValueCoroutine);

        _smoothlyChangeValueCoroutine = StartCoroutine(SmoothlyChangeValue(0, _vampirism.SkillDuration));

        if (_smoothlyRendererZoneCoroutine != null)
            StopCoroutine(_smoothlyRendererZoneCoroutine);

        _smoothlyRendererZoneCoroutine = StartCoroutine(SmoothlyRendererZone(_maxAlphaValue));
    }

    private void IncreaseValue()
    {
        if (_smoothlyChangeValueCoroutine != null)
            StopCoroutine(_smoothlyChangeValueCoroutine);

        _smoothlyChangeValueCoroutine = StartCoroutine(SmoothlyChangeValue(_bar.maxValue, _vampirism.CoolDownTimer));

        if (_smoothlyRendererZoneCoroutine != null)
            StopCoroutine(_smoothlyRendererZoneCoroutine);

        _smoothlyRendererZoneCoroutine = StartCoroutine(SmoothlyRendererZone(0));
    }

    private IEnumerator SmoothlyChangeValue(float targetValue, float time)
    {
        _stepChangeValue = _vampirism.SkillDuration / time;

        while (_bar.value != targetValue)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, targetValue, _stepChangeValue * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator SmoothlyRendererZone(float targetValue)
    {
        while (_zone.color.a != targetValue)
        {
            float newAlphaValue = Mathf.MoveTowards(_colorZone.a, targetValue, _stepRendererZone * Time.deltaTime);
            _colorZone.a = newAlphaValue;
            _zone.color = _colorZone;

            yield return null;
        }
    }
}