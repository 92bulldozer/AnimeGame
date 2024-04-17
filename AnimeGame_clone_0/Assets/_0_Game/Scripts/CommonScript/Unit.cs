using System;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using PathologicalGames;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    /// <summary>
    /// 변수
    /// </summary>

    #region Field

    [Header("TargetObject")] [Space(10)] [SerializeField]
    private GameObject returnedObject;

   

    [Header("Model")] 
    public Model model;

    [Space(10)] 
    [Header("SpawnPoolOption")]
    [SerializeField]private bool useSpawnPool;

    [Space(10)] [Header("DamageOption")] 
    public bool isMeshRenderer;
    public Color hitColor;
    private Material _material;
    private Color _originColor;
    
    [Space(10)]
    
    [Header("Component")]
    [SerializeField]private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField]private MeshRenderer meshRenderer;



    public GameObject ReturnedObject
    {
        get => returnedObject;
        set => returnedObject = value;
    }

    public bool UseSpawnPool1
    {
        get => useSpawnPool;
        set => useSpawnPool = value;
    }

    /// <summary>
    /// Shader
    /// </summary>
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    /// <summary>
    /// Property
    /// </summary>
    public bool UseSpawnPool
    {
        get => useSpawnPool;
        set => useSpawnPool = value;
    }
    
  
    /// <summary>
    /// Disposable
    /// </summary>
    protected IDisposable deadDisposable;
    protected IDisposable hpDisposable;

    #endregion
    
    protected virtual void Awake()
    {
        if (UseSpawnPool)
            return;
        InitComponent();
        InitView();
        InitProperty();
        InitModel();
        RegisterStream();
    }

    protected virtual void Start()
    {
        
    }

   
    
    /// <summary>
    /// 초기화 함수
    /// InitProperty
    /// InitView
    /// InitModel
    /// InitComponent
    /// ResetModel
    /// </summary>
    #region Init
    protected virtual void InitProperty()
    {
        if (isMeshRenderer)
            _material = meshRenderer.material;
        else
            _material = _skinnedMeshRenderer.material;
        
        
        _originColor = _material.GetColor(BaseColor);
    }
    protected virtual void InitView()
    {
        
    }
    protected virtual void InitModel()
    {
        try
        {
            "Unit InitModel Function".Log();
          
        }
        catch (Exception e)
        {
            "InitModel Error".Log();
        }
       
    }
   
    protected virtual void InitComponent()
    {
        
    }
    
    protected virtual void ResetModel()
    {
        
    }
    

    #endregion
    
    /// <summary>
    /// 공격타입
    /// 1 . 평타
    /// 2 . 패시브
    /// 3 . 스킬
    /// </summary>
    #region AttackType

    public virtual void NormalAttack()
    {
        //오브젝트가 존재할경우
       
    }
    
    public virtual void Passive()
    {
        
    }

    public virtual void SkillAttack()
    {
        
    }
    
    #endregion

    /// <summary>
    /// 공격 , 피격 , 사망
    /// DamagedColor
    /// Damaged
    /// Hit
    /// Dead
    /// </summary>
    #region Hit or Attack 
    /// <summary>
    /// 맞았을때 색상 변경
    /// </summary>
    protected virtual void DamagedColor()
    {
        _material.DOPause();
        _material.SetColor("_BaseColor", hitColor);
        _material.DOColor(_originColor, "_BaseColor", 0.5f);
    }

    /// <summary>
    /// 피격
    /// 피격 함수는 Hit 함수에의해 인터페이스로 구성된 IDamage를 상속하는 Damage클래스로부터 콜되고있음
    /// Hit -> Damage.Damaged -> 자식 unit.Damaged -> 부모 unit.Damaged
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damaged(int damage)
    {
        DamagedColor();
        $"unitDamaged {damage}".Log();
        if(model.Damagable.Value)
            model.CurrentHp.Value -= damage;
        if (model.CurrentHp.Value <= 0)
            model.Dead .Value= true;
        
    }
    
    
    /// <summary>
    /// 공격
    /// </summary>
    protected virtual void Attack()
    {
        $"Unit Attack".Log();
    }
    
    /// <summary>
    /// 사망
    /// </summary>
    protected virtual void Dead()
    {
        "UnitDead".Log();
        model.Dead.Value = true;

        PoolManager.Pools["Unit"].Despawn(transform);

    }
    

    #endregion
    
    /// <summary>
    /// Stream 처리 함수
    /// </summary>
    #region UnirxStream
    protected virtual void DisposeStream()
    {
        "Unit DisposeStream".Log();
        deadDisposable.Dispose();
    }
    
    protected virtual void RegisterStream()
    {
        deadDisposable = Observable.EveryUpdate().Select(_ => model.Dead).DistinctUntilChanged()
            .Where(modelDead => modelDead.Value)
            .Subscribe(x =>
            {
                "Dead".Log();
                Dead();
                hpDisposable.Dispose();
            });
    }
    

    #endregion
   
    
    /// <summary>
    /// 유닛 스폰 ( 풀매니저 사용함 )
    /// OnSpawned
    /// Despawned
    /// ResetModel
    /// </summary>
    /// <param name="pool"></param>
    #region PoolOption

    protected virtual void OnSpawned(SpawnPool pool)
    {
        "unit OnSpawned".Log();
        InitComponent();
        InitView();
        InitProperty();
        InitModel();
        RegisterStream();
    }
	
    protected virtual void OnDespawned(SpawnPool pool)
    {
        "Unit OnDespawned".Log();
       ResetModel();
       DisposeStream();
    }
    
    #endregion
    
   
}
