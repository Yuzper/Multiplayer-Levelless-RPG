using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField]
    bool canCutGrass = false;

    GrassComputeScript grassComputeScript;

    [ConditionalHide("canCutGrass")]
    [SerializeField]
    float radius = 1f;

    [ConditionalHide("canCutGrass")]
    [SerializeField]
    Transform centerPointForRadius = null;

    private bool updateCuts;

    Vector3 cachedPos;


    [Header("Collider")]
    [SerializeField] protected Collider damageCollider;

    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage; // (When calculating damage this is used to check for attackers damage modifiers, effetcs etc)

    [Header("Damage")]
    public float physicalDamage = 0; // Could be subdivided into standard, strike, slash and pierce
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();


    protected virtual void Awake()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // Check if we can damage this target based on friendly fire
            if (WorldUtilityManager.instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup))
            {
                // Check if target is blocking

                DamageTarget(damageTarget);
            }
        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        // We don't want to damage the same target more than once in a single attack
        // So we add them to a list that checks before applying damage
        if (charactersDamaged.Contains(damageTarget)) return;

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
        grassComputeScript = GameObject.FindGameObjectWithTag("GrassComputeHolder")?.GetComponent<GrassComputeScript>();
        if (grassComputeScript != null)
        {
            updateCuts = true;
        }
        
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        updateCuts = false;
        charactersDamaged.Clear(); // WE RESET THE CHARACTERS THAT HAVE BEEN HIT WHEN WE RESET THE COLLIDER, SO THEY MAY BE HIT AGAIN.
    }


    void Update()
    {
        if (canCutGrass && updateCuts && transform.position != cachedPos)
        {
            grassComputeScript.UpdateCutBuffer(transform.position, radius);
            cachedPos = transform.position;
        }
    }


}
