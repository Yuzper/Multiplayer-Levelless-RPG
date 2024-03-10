using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{

    [Header("Status")]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
    [HideInInspector] public CharacterStatsManager characterStatsManager;


    [Header("Character group")]
    public CharacterGroup characterGroup;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isDancing = false;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    protected virtual void Start()
    {
        IgnoreMyOwnColliders();
    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", characterLocomotionManager.isGrounded);
        // IF THIS CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION OF OUR TRANSFORM
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
        else
        {
            // Position
            transform.position = Vector3.SmoothDamp
                (transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);
            // Rotation
            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        animator.SetBool("isMoving", characterNetworkManager.isMoving.Value);
        characterNetworkManager.OnIsActiveChanged(false, characterNetworkManager.isActive.Value);

        characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
        characterNetworkManager.isActive.OnValueChanged += characterNetworkManager.OnIsActiveChanged;

        // STATS
        characterNetworkManager.currentHealth.OnValueChanged += characterNetworkManager.CheckHP;
        // Set Health based on Constitution
        characterNetworkManager.maxHealth.Value = characterStatsManager.CalculateHealthBasedOnConstitution(characterNetworkManager.constitution.Value);
        characterNetworkManager.currentHealth.Value = characterNetworkManager.maxHealth.Value;
        // Set Mana based on Intelligence
        characterNetworkManager.maxMana.Value = characterStatsManager.CalculateManaBasedOnIntelligence(characterNetworkManager.intelligence.Value);
        characterNetworkManager.currentMana.Value = characterNetworkManager.maxMana.Value;
        // Set Stamina based on Endurance
        characterNetworkManager.maxMana.Value = characterStatsManager.CalculateStaminaBasedOnEndurance(characterNetworkManager.endurance.Value);
        characterNetworkManager.currentStamina.Value = characterNetworkManager.maxStamina.Value;

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
        characterNetworkManager.isActive.OnValueChanged -= characterNetworkManager.OnIsActiveChanged;
        // STATS
        characterNetworkManager.currentHealth.OnValueChanged -= characterNetworkManager.CheckHP;
    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            characterLocomotionManager.canMove = false;
            characterLocomotionManager.canRotate = false;

            // Reset any flags here that need to be reset

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
            }
        }
        // Play death SFX

        yield return new WaitForSeconds(4);

        // Award players with loot
        // Disable Character model

        // Disable control over player movement
        

    }

    public virtual void ReviveCharacter()
    {
        isDead.Value = false;

        // Reenable control over player movement
        characterLocomotionManager.canRotate = true;
        characterLocomotionManager.canMove = true;
    }

    protected virtual void IgnoreMyOwnColliders()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
        List<Collider> ignoreColliders = new List<Collider>();

        // ADDS ALL OF OUR DAMAGEABLE CHARACTER COLLIDERS, TO THE LIST THAT WILL BE USED TO IGNORE COLLISIONS
        foreach (var collider in damageableCharacterColliders)
        {
            ignoreColliders.Add(collider);
        }

        // ADDS OUR CHARACTER CONTROLLER COLLIDER TO THE LIST THAT WILL BE USED TO IGNORE COLLISIONS
        ignoreColliders.Add(characterControllerCollider);

        // GOES THROUGH EVERY COLLIDER ON THE LIST, AND IGNORES COLLISION WITH EACH OTHER
        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }


    }



}
