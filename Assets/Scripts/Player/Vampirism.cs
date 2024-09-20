using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class Vampirism : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _stealHealthCount;

    private List<EnemyHealth> _enemyHealths = new List<EnemyHealth>();
    private Coroutine _stealHealthCoroutine;
    private Coroutine _coolDownSkillCoroutine;
    private bool _canUse;

    [field: SerializeField] public float SkillDuration { get; private set; }
    [field: SerializeField] public float CoolDownTimer { get; private set; }

    public event Action SkillUsed;
    public event Action SkillRestored;

    private void Start()
    {
        _canUse = true;
    }

    public void AddEnemyInList(EnemyHealth enemyHealth)
    {
        _enemyHealths.Add(enemyHealth);
    }

    public void RemoveEnemyFromList(EnemyHealth enemyHealth)
    {
        _enemyHealths.Remove(enemyHealth);
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

    private IEnumerator StealHealth()
    {
        _canUse = false;

        SkillUsed?.Invoke();

        float adaptiveStealHealthCount = _stealHealthCount * Time.deltaTime;
        float timer = SkillDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (_enemyHealths.Count > 0)
            {
                for (int i = 0; i < _enemyHealths.Count; i++)
                {
                    _enemyHealths[i].TakeDamage(adaptiveStealHealthCount);
                    _health.Regenerate(adaptiveStealHealthCount);
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
        SkillRestored?.Invoke();

        yield return new WaitForSeconds(CoolDownTimer);

        _canUse = true;
    }
}
