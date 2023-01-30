using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class ShotgunController : WeaponController
{
    [SerializeField] private GameObject gunhole;

    private bool _isCool = false;
    private float _bulletTargetRange = 60f;

    public override int _weaponType { get { return (int)Define.Weapons.Shotgun; } }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.AngleAxis(SetAngleFromHandToCursor(), Vector3.forward), 5f * Time.deltaTime);
        if (!_isCool)
        {
            StartCoroutine(ShotCoolTime());
        }
    }

    void SetBulletAngle()
    {
        float bulletAngle = _bulletTargetRange / (_countPerCreate+1);
        float angle = SetAngleFromHandToCursor();
        float startBulletAngle = angle - (_bulletTargetRange/2);
        for (int i = 1; i <= _countPerCreate; i++)
        {
            CreateBullet(i, bulletAngle, startBulletAngle);
        }
        
    }

    void CreateBullet(int num, float bulletAngle, float startBulletAngle)
    {
        GameObject bullet = Managers.Game.Spawn(Define.WorldObject.Unknown, "Weapon/Bullet", gunhole.transform.position);
        //set damage, dir 
        Bullet bulletStat = bullet.GetOrAddComponent<Bullet>();
        float _ang = startBulletAngle + bulletAngle * num + Random.Range(-5f, 5f);
        Vector3 bulletDir = new Vector3(Mathf.Cos(_ang * Mathf.Deg2Rad), Mathf.Sin(_ang * Mathf.Deg2Rad), 0);
        bulletStat.SetBulletDir(bulletDir);
        bulletStat._damage = _damage;
        bulletStat._movSpeed = _movSpeed;
        bulletStat._penetrate = _penetrate;
        bulletStat._force = _force;
    }

    float SetAngleFromHandToCursor()
    {
        Vector3 dirVec = (Managers.Game.WorldMousePos - transform.position).normalized;
        return Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
        
    }


    IEnumerator ShotCoolTime()
    {
        _isCool = true;
        Managers.Sound.Play("Shoot_02");
        SetBulletAngle();
        yield return new WaitForSeconds(_cooldown);

        _isCool = false;
    }
}
