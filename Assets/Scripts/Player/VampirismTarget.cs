using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VampirismTarget : MonoBehaviour
{
    [SerializeField] private Vampirism _vampirism;

    private List<EnemyHealth> _enemyHealths = new List<EnemyHealth>();
    private Coroutine _setNearesrEnemyСoroutine;
    private float _coroutineDelay = 0.1f;
    private bool _isOn = false;

    private void OnEnable()
    {
        _vampirism.SkillUsed += SwitchOn;
        _vampirism.SkillRestored += SwitchOff;
    }

    private void OnDisable()
    {
        _vampirism.SkillUsed -= SwitchOn;
        _vampirism.SkillRestored -= SwitchOff;
    }

    public void AddEnemyHealthInList(EnemyHealth enemyHealth)
    {
        print("1");
        _enemyHealths.Add(enemyHealth);
    }

    public void RemoveEnemyHealthInList(EnemyHealth enemyHealth)
    {
        print("2");
        _enemyHealths.Remove(enemyHealth);
    }

    private void SwitchOn()
    {
        _isOn = true;

        if (_setNearesrEnemyСoroutine != null)
            StopCoroutine(_setNearesrEnemyСoroutine);

        _setNearesrEnemyСoroutine = StartCoroutine(SetNearesrEnemy());
    }

    private void SwitchOff()
    {
        _isOn = false;
        _enemyHealths.Clear();
    }

    private IEnumerator SetNearesrEnemy()
    {
        while (_isOn)
        {
            _vampirism.GetEnemyHealth(GetNearestEnemy());

            yield return new WaitForSeconds(_coroutineDelay);
        }
    }

    private EnemyHealth GetNearestEnemy()
    {
       EnemyHealth enemyHealth = null;

        if (_enemyHealths.Count > 0)
        {
            enemyHealth = _enemyHealths.OrderBy(enemyHealth => 
            transform.position.SqrDistance(enemyHealth.transform.position)).FirstOrDefault();
        }

        return enemyHealth;
    }
}