using System;
using System.Numerics;
using UniRx;
using UnityEngine;

[Serializable]
public class Model 
{
    [SerializeField] private ReactiveProperty<bool>  dead = new ReactiveProperty<bool>();
    [SerializeField] private ReactiveProperty<bool> damagable = new ReactiveProperty<bool>();
    [SerializeField] private ReactiveProperty<bool> canAttack = new ReactiveProperty<bool>();
    [SerializeField] private ReactiveProperty<BigInteger> atk = new ReactiveProperty<BigInteger>();
    [SerializeField] private ReactiveProperty<BigInteger> currentHp= new ReactiveProperty<BigInteger>();
    [SerializeField] private ReactiveProperty<BigInteger> maxHp= new ReactiveProperty<BigInteger>();
    [SerializeField] private ReactiveProperty<double> attackSpeed= new ReactiveProperty<double>();
    [SerializeField] private ReactiveProperty<int> currentMp= new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<int> maxMp= new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<int> armor= new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<int> magicResist= new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<int> cube= new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<int> crystal= new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<BigInteger> gold= new ReactiveProperty<BigInteger>();
    [SerializeField] private ReactiveProperty<float> acc= new ReactiveProperty<float>();
    [SerializeField] private ReactiveProperty<float> dodge= new ReactiveProperty<float>();
    [SerializeField] private ReactiveProperty<double> criticalChance= new ReactiveProperty<double>();
    [SerializeField] private ReactiveProperty<BigInteger> criticalDamage= new ReactiveProperty<BigInteger>();
    [SerializeField] private ReactiveProperty<float> attackRange= new ReactiveProperty<float>();
    [SerializeField] private ReactiveProperty<float> moveSpeed= new ReactiveProperty<float>();
    [SerializeField] private ReactiveProperty<BigInteger> exp= new ReactiveProperty<BigInteger>();
    
    

    #region StatProperty
    public ReactiveProperty<bool> Dead
    {
        get => dead;
        set => dead = value;
    }

    public ReactiveProperty<bool> Damagable
    {
        get => damagable;
        set => damagable = value;
    }

    public ReactiveProperty<bool> CanAttack
    {
        get => canAttack;
        set => canAttack = value;
    }

    public ReactiveProperty<BigInteger> Atk
    {
        get => atk;
        set => atk = value;
    }

    public ReactiveProperty<BigInteger> CurrentHp
    {
        get => currentHp;
        set => currentHp = value;
    }

    public ReactiveProperty<BigInteger> MAXHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public ReactiveProperty<int> CurrentMp
    {
        get => currentMp;
        set => currentMp = value;
    }

    public ReactiveProperty<int> MAXMp
    {
        get => maxMp;
        set => maxMp = value;
    }

    public ReactiveProperty<double> AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }

    public ReactiveProperty<int> Armor
    {
        get => armor;
        set => armor = value;
    }

    public ReactiveProperty<int> MagicResist
    {
        get => magicResist;
        set => magicResist = value;
    }

    public ReactiveProperty<int> Cube
    {
        get => cube;
        set => cube = value;
    }

    public ReactiveProperty<int> Crystal
    {
        get => crystal;
        set => crystal = value;
    }

    public ReactiveProperty<BigInteger> Gold
    {
        get => gold;
        set => gold = value;
    }

    public ReactiveProperty<float> Acc
    {
        get => acc;
        set => acc = value;
    }

    public ReactiveProperty<float> Dodge
    {
        get => dodge;
        set => dodge = value;
    }

    public ReactiveProperty<double> CriticalChance
    {
        get => criticalChance;
        set => criticalChance = value;
    }

    public ReactiveProperty<BigInteger> CriticalDamage
    {
        get => criticalDamage;
        set => criticalDamage = value;
    }

    public ReactiveProperty<float> AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }


    public ReactiveProperty<float> MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public ReactiveProperty<BigInteger> Exp
    {
        get => exp;
        set => exp = value;
    }

    #endregion


}

public interface IModelBuilder
{
    
    public IModelBuilder SetDead(bool dead);
    public IModelBuilder SetDamagable(bool damagable);
    public IModelBuilder SetCanAttack(bool canAttack);
    public IModelBuilder SetAtk(BigInteger atk);
    public IModelBuilder SetCurrentHp(BigInteger currentHp);
    public IModelBuilder SetMaxHp(BigInteger maxHp);
    public IModelBuilder SetAttackSpeed(double attackSpeed);
    public IModelBuilder SetCurrentMp(int currentMp);
    public IModelBuilder SetMaxMp(int maxMp);
    public IModelBuilder SetArmor(int armor);
    public IModelBuilder SetMagicResist(int magicResist);
    public IModelBuilder SetCube(int cube);
    public IModelBuilder SetCrystal(int crystal);
    public IModelBuilder SetGold(BigInteger gold);
    public IModelBuilder SetAcc(float acc);
    public IModelBuilder SetDodge(float dodge);
    public IModelBuilder SetCriticalChance(double criticalChance);
    public IModelBuilder SetCriticalDamage(BigInteger criticalDamage);
    public IModelBuilder SetAttackRange(float attackRange);
    public IModelBuilder SetMoveSpeed(float moveSpeed);
    public IModelBuilder SetExp(BigInteger exp);
    public Model Build();
}

public class ModelBuilder : IModelBuilder
{
    private Model _model = new Model();



    public IModelBuilder SetDead(bool dead)
    {
        _model.Dead.Value = dead;
        return this;
    }


    public IModelBuilder SetDamagable(bool damagable)
    {
        _model.Damagable.Value = damagable;
        return this;
    }

    public IModelBuilder SetCanAttack(bool canAttack)
    {
        _model.CanAttack.Value = canAttack;
        return this;
    }

    public IModelBuilder SetAtk(BigInteger atk)
    {
        _model.Atk.Value = atk;
        return this;
    }

    public IModelBuilder SetCurrentHp(BigInteger currentHp)
    {
        _model.CurrentHp.Value = currentHp;
        return this;
    }

    public IModelBuilder SetMaxHp(BigInteger maxHp)
    {
        _model.MAXHp.Value = maxHp;
        return this;
    }

    public IModelBuilder SetAttackSpeed(double attackSpeed)
    {
        _model.AttackSpeed.Value = attackSpeed;
        return this;
    }

    public IModelBuilder SetCurrentMp(int currentMp)
    {
        _model.CurrentMp.Value = currentMp;
        return this;
    }

    public IModelBuilder SetMaxMp(int maxMp)
    {
        _model.MAXMp.Value = maxMp;
        return this;
    }

    public IModelBuilder SetArmor(int armor)
    {
        _model.Armor.Value = armor;
        return this;
    }

    public IModelBuilder SetMagicResist(int magicResist)
    {
        _model.MagicResist.Value = magicResist;
        return this;
    }

    public IModelBuilder SetCube(int cube)
    {
        _model.Cube.Value = cube;
        return this;
    }

    public IModelBuilder SetCrystal(int crystal)
    {
        _model.Crystal.Value = crystal;
        return this;
    }

    public IModelBuilder SetGold(BigInteger gold)
    {
        _model.Gold.Value = gold;
        return this;
    }

    public IModelBuilder SetAcc(float acc)
    {
        _model.Acc.Value = acc;
        return this;
    }

    public IModelBuilder SetDodge(float dodge)
    {
        _model.Dodge.Value = dodge;
        return this;
    }

    public IModelBuilder SetCriticalChance(double critical)
    {
        _model.CriticalChance.Value = critical;
        return this;
    }

    public IModelBuilder SetCriticalDamage(BigInteger criticalDamage)
    {
        _model.CriticalDamage.Value = criticalDamage;
        return this;
    }

    public IModelBuilder SetAttackRange(float attackRange)
    {
        _model.AttackRange.Value = attackRange;
        return this;
    }


    public IModelBuilder SetMoveSpeed(float moveSpeed)
    {
        _model.MoveSpeed.Value = moveSpeed;
        return this;
    }

    public IModelBuilder SetExp(BigInteger exp)
    {
        _model.Exp.Value = exp;
        return this;
    }

    public Model Build() => _model;
}