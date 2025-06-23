using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Training
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Behavior Data", menuName = "Combat System/Behavior/Melee Attack Action")]
    public class MeleeAttack : ComboAttackAction
    {
        public override void Execute(CharacterAI agent)
        {
            if(agent.currentWeapon.currentWeaponState != EWeaponState.Equip)agent.characterRigComponent.EquipWeapon();
            agent.comboComponent.SaveComboEnemy(combo);
        }
        public override void Finished(CharacterAI agent)
        {
            base.Finished(agent);
        }
    }
}