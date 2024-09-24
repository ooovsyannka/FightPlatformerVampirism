using System;
using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(VampirismDetector))]

public class Vampirism : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _stealHealthCount;

    [SerializeField] private VampirismDetector _detector;
    private EnemyHealth _enemyHealth;
    private Coroutine _stealHealthCoroutine;
    private Coroutine _coolDownSkillCoroutine;
    private bool _canUse;

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
        _detector.gameObject.SetActive(true);
        _canUse = false;

        SkillUsed?.Invoke();

        float adaptiveStealHealthCount = _stealHealthCount * Time.deltaTime;
        float timer = SkillDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (_enemyHealth != null)
            {
                _enemyHealth.TakeDamage(adaptiveStealHealthCount);

                if (_enemyHealth.CurrentValue > adaptiveStealHealthCount)
                {
                    _health.Regenerate(adaptiveStealHealthCount);
                }
                else
                {
                    _health.Regenerate(_enemyHealth.CurrentValue);
                }
            }

            yield return null;
        }

        if (_coolDownSkillCoroutine != null)
            StopCoroutine(_coolDownSkillCoroutine);

        _coolDownSkillCoroutine = StartCoroutine(CooldownSkill());
    }

    private IEnumerator CooldownSkill()
    {
        _detector.gameObject.SetActive(false);
        SkillRestored?.Invoke();

        yield return new WaitForSeconds(CoolDownTimer);

        _canUse = true;
    }
}