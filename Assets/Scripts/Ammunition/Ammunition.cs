using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ammunition : MonoBehaviour
{
    [SerializeField] private int _maxAllCountBullets;
    [SerializeField] private int _maxCountBulletsInClip;
    [SerializeField] private Bullet _bulletPrefab;

    private Queue<Bullet> _poolBullets;

    public int CurrentBulletCountInClip { get; private set; }
    public int CurrentAllBulletCount { get; private set; }

    public event Action ValueChanged;

    private void Awake()
    {
        _poolBullets = new Queue<Bullet>();
        CreatBullets();
    }

    private void OnEnable()
    {         
        CurrentBulletCountInClip = _maxCountBulletsInClip;
        CurrentAllBulletCount = _maxAllCountBullets;
    }

    public bool TryReload()
    {
        if (CurrentAllBulletCount == 0)
            return false;

        if (CurrentAllBulletCount >= _maxCountBulletsInClip)
        {
            int countBulletToReload = _maxCountBulletsInClip - CurrentBulletCountInClip;
            CurrentBulletCountInClip += countBulletToReload;
            CurrentAllBulletCount -= countBulletToReload;
        }
        else
        {
            CurrentBulletCountInClip = CurrentAllBulletCount;
            CurrentAllBulletCount = 0;
        }

        ValueChanged?.Invoke();

        return true;
    }

    public void ReplenishmentBulletsCount(int desiredCount = 0)
    {
        if (desiredCount == 0)
        {
            CurrentAllBulletCount = _maxAllCountBullets;
        }
        else
        {
            if (CurrentAllBulletCount + desiredCount < _maxCountBulletsInClip)
            {
                CurrentAllBulletCount += desiredCount;
            }
            else
            {
                CurrentAllBulletCount = _maxAllCountBullets;
            }
        }
    }

    public bool TryGetBullet(out Bullet bullet)
    {
        bullet = null;

        if (CurrentBulletCountInClip != 0)
        {
            bullet = _poolBullets.Dequeue();
            _poolBullets.Enqueue(bullet);
            CurrentBulletCountInClip--;
            bullet.transform.parent = null;
            ValueChanged?.Invoke();

            return true;
        }

        return false;
    }

    private void CreatBullets()
    {
        for (int i = _maxCountBulletsInClip; i > 0; i--)
        {
            Bullet bullet = Instantiate(_bulletPrefab);
            _poolBullets.Enqueue(bullet);
            bullet.GetParent(transform);
            bullet.gameObject.SetActive(false);
        }
    }
}
