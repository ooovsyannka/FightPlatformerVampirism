using System;
using System.Collections;
using UnityEngine;

public class Vampirism : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _stealHealthCount;
    [SerializeField] private VampirismDetector _detector;

    private EnemyHealth _enemyHealth;
    private Coroutine _stealHealthCoroutine;
    private Coroutine _coolDownSkillCoroutine;
    private float _skillDelay = 0.5f;
    private bool _canUse;
    private WaitForSeconds _waitForSeconds;

    [field: SerializeField] public float SkillDuration { get; private set; }
    [field: SerializeField] public float CoolDownTimer { get; private set; }

    public event Action SkillUsed;
    public event Action SkillRestored;

    private void Awake()
    {
        _canUse = true;
        _detector.gameObject.SetActive(false);
    }

    public void UseSkill()
    {
        if (_canUse == false)
            return;

        if (_stealHealthCoroutine != null)
            StopCoroutine(_stealHealthCoroutine);

        _stealHealthCoroutine = StartCoroutine(StealHealth());
    }

    public void RecoverySkill()
    {
        if (_stealHealthCoroutine != null)
            StopCoroutine(_stealHealthCoroutine);

        if (_coolDownSkillCoroutine != null)
            StopCoroutine(_coolDownSkillCoroutine);

        _coolDownSkillCoroutine = StartCoroutine(CooldownSkill());
    }

    public void GetEnemyHealth(EnemyHealth enemyHealth)
    {
        _enemyHealth = enemyHealth;
    }

    private IEnumerator StealHealth()
    {
        _waitForSeconds = new WaitForSeconds(_skillDelay);
        _detector.gameObject.SetActive(true);
        _canUse = false;

        SkillUsed?.Invoke();

        float timer = SkillDuration;

        float currenttealHealthCount;

        while (timer > 0)
        {
            timer -= _skillDelay;

            _enemyHealth = _detector.GetNearestEnemy();

            if (_enemyHealth != null)
            {
                if (_enemyHealth.CurrentValue > _stealHealthCount)
                {
                    currenttealHealthCount = _stealHealthCount;
                }
                else
                {
                    currenttealHealthCount = _enemyHealth.CurrentValue;
                }

                _enemyHealth.TakeDamage(currenttealHealthCount);
                _health.Regenerate(currenttealHealthCount);
            }

            yield return _waitForSeconds;
        }

        if (_coolDownSkillCoroutine != null)
            StopCoroutine(_coolDownSkillCoroutine);

        _coolDownSkillCoroutine = StartCoroutine(CooldownSkill());
    }

    private IEnumerator CooldownSkill()
    {
        _waitForSeconds = new WaitForSeconds(CoolDownTimer);
        _detector.gameObject.SetActive(false);
        SkillRestored?.Invoke();

        yield return _waitForSeconds;

        _canUse = true;
    }
}