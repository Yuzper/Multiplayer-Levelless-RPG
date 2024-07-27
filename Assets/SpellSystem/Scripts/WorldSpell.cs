using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldSpell : MonoBehaviour
{
    protected CharacterSpellManager characterCausingDamage;
    public virtual void StartSpell(CharacterSpellManager characterCausingDamage, BaseSpell spell, Vector3 direction)
    {
        this.characterCausingDamage = characterCausingDamage;
    }
}
