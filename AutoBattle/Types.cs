using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {

        public struct CharacterClassSpecific
        {
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;

        }

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;

            public GridBox(int x, int y, bool ocupied, int index)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.Index = index;
            }

        }

        public struct CharacterSkills
        {
            public CharacterSkillName Name;
            public float damage;
            public float damageMultiplier;
        }

        public enum CharacterSkillName
        {
            KnockDown = 1,
            Heal = 2,
            Bleed = 3,
            Knockback = 4,
            StrongAttack = 5,
            Teleport = 6,
            Invisibility = 7,
            ThrowRock = 8,
            Poison = 9
        }

        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

    }
}
