using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : BaseController
{
    public GameObject hudDamageText;
    
    protected EnemyStat _stat;
    public RuntimeAnimatorController[] animeCon;
    public Rigidbody2D _target;
    bool _isLive = true;


    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anime = GetComponent<Animator>();
        _type = Define.WorldObject.Enemy;
    }

    private void FixedUpdate()
    {
        if (!_isLive)
            return;

        Vector2 dirVec = _target.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * (_stat.MoveSpeed * Time.fixedDeltaTime);

        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        _sprite.flipX = (_target.position.x - _rigid.position.x < 0) ? true : false;
    }

    private void OnEnable()
    {
        _target = Managers.Instance._player.GetComponent<Rigidbody2D>();
        _isLive = true;
        _stat.HP = _stat.MaxHP;
    }

    public void Init(SpawnData data)
    {
        _anime.runtimeAnimatorController = animeCon[data.spriteType];
        _stat.MoveSpeed = data.speed;
        _stat.MaxHP = SetRandomStat(data.maxHp);
        _stat.HP = _stat.MaxHP;
        _stat.Attack = SetRandomStat(data.attack);
        _stat.Defense = SetRandomStat(data.defense);
        _stat.ExpPoint = SetRandomStat(data.exp);
    }
    
    int SetRandomStat(int value)
    {
        value = (int)(value * Random.Range(0.8f, 1.2f));
        return value;
    }
    
    
    public override void OnDamaged(int damage)
    {
        _stat.HP -= Mathf.Max(damage - _stat.Defense, 1);
        FloatDamageText(damage);
        OnDead();
    }

    void FloatDamageText(int damage)
    {
        GameObject hudText = Instantiate(hudDamageText); // 생성할 텍스트 오브젝트
        hudText.transform.position = transform.position + Vector3.up*1.5f; // 표시될 위치
        hudText.GetComponent<UI_DamageText>().damage = damage; // 데미지 전달
    }
    
    public override void OnDead()
    {
        if(_stat.HP <= 0)
        {
            _isLive = false;
            _stat.HP = 0;

            SpawnExp();
            Managers.Game.Despawn(gameObject);
        }
    }

    void SpawnExp()
    {
        GameObject expGo = Managers.Game.Spawn(Define.WorldObject.Unknown, "Content/Exp");
        Exp_Item expPoint = expGo.GetComponent<Exp_Item>();
        expPoint._exp = _stat.ExpPoint;
        expGo.transform.position = transform.position;
        if (expPoint._exp < 5)
            expGo.GetComponent<SpriteRenderer>().sprite = expPoint._sprite[0];
        else if(expPoint._exp<10)
            expGo.GetComponent<SpriteRenderer>().sprite = expPoint._sprite[1];
        else
            expGo.GetComponent<SpriteRenderer>().sprite = expPoint._sprite[2];

    }
}