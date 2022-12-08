using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health;
        public float BaseDamage;
        public float DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; }
        private bool isPlayer;
        public CharacterClass characterClass;
        public CharacterSkills skill = new CharacterSkills();
        public CharacterSkills specialSkill = new CharacterSkills();
        public CharacterSkillName status;

        //Adding the initial position to the contructor of the character class
        public Character(CharacterClass characterClass, GridBox initialPosition, bool isPlayer)
        {
            this.characterClass = characterClass;
            currentBox = initialPosition;
            this.isPlayer = isPlayer;
            CharacterClassesAtributes();
        }


        public bool TakeDamage(float amount)
        {
            if ((Health -= BaseDamage) <= 0)
            {
                Die();
                return true;
            }
            //Healing if its a cleric and also if the player is below 40% health, also note that this verification comes after the verification if the player dies
            if (characterClass == CharacterClass.Cleric && Health <= Health * .4)
            {
                Health += Health * .2f;
                Console.WriteLine($"Player {PlayerIndex} healed for {Health * .2f} points of health");
            }
            return false;
        }

        public void Die()
        {
            Console.WriteLine($"{Target.PlayerIndex} died from taking to much damage.\n{PlayerIndex} was victourious!");
        }

        public void WalkTO(bool CanWalk)
        {

        }

        public void StartTurn(Grid battlefield)
        {

            if (CheckCloseTargets(battlefield) && status != CharacterSkillName.KnockDown)
            {
                Attack(Target);


                return;
            }
            else if (status != CharacterSkillName.KnockDown)
            {   // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if (this.currentBox.xIndex > Target.currentBox.xIndex)
                {
                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                    {
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                        currentBox.ocupied = true;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength);
                        //Updating a grid everytime a player moves
                        UpdateGrid(battlefield);
                        return;
                    }
                }
                else if (currentBox.xIndex < Target.currentBox.xIndex)
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
                    currentBox.ocupied = true;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength);
                    UpdateGrid(battlefield);
                    return;
                }

                if (this.currentBox.yIndex > Target.currentBox.yIndex)
                {
                    battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength);
                    this.currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    this.currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght));
                    this.currentBox.ocupied = true;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    UpdateGrid(battlefield);
                    return;
                }
                else if (this.currentBox.yIndex < Target.currentBox.yIndex)
                {
                    this.currentBox.ocupied = true;
                    battlefield.grids[currentBox.Index] = this.currentBox;
                    this.currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght));
                    this.currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength);
                    UpdateGrid(battlefield);
                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght).ocupied);

            //There was no way that the other player could be in all positions at the same time. Original code left & right & up & down
            if (left || right || up || down)
            {
                return true;
            }
            return false;
        }

        public void Attack(Character target)
        {
            var rand = new Random();
            //Implementing the damage multiplier
            target.TakeDamage(rand.Next(0, (int)(BaseDamage * DamageMultiplier)));
            if (skill.Name == CharacterSkillName.Knockback)
            {
                ////Using damage multplier as a RNG for the effect
                if (rand.Next(0, (int)DamageMultiplier * 3) < DamageMultiplier)
                {
                    Target.status = CharacterSkillName.KnockDown;
                    Console.WriteLine($"Player {Target.PlayerIndex} was knock down, it will not be able to have an action this turn");
                }
            }
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {Target.PlayerIndex} and did {BaseDamage} damage\n");
        }

        //Updating the grid so all characters are ocupied in the screen
        void UpdateGrid(Grid battlefield)
        {
            if (isPlayer)
            {
                battlefield.playerPosition = currentBox;
            }
            else
            {
                battlefield.enemyPosition = currentBox;
            }
        }

        void CharacterClassesAtributes()
        {
            switch (characterClass)
            {
                case CharacterClass.Paladin:
                    Health *= 2;
                    DamageMultiplier = 1.3f;
                    skill = new CharacterSkills { Name = CharacterSkillName.KnockDown, damage = 20, damageMultiplier = 0 };
                    specialSkill = new CharacterSkills { Name = CharacterSkillName.StrongAttack, damage = BaseDamage * 2, damageMultiplier = 0 };
                    break;

                case CharacterClass.Cleric:
                    skill = new CharacterSkills { Name = CharacterSkillName.Heal, damage = 0, damageMultiplier = 0 };
                    specialSkill = new CharacterSkills { Name = CharacterSkillName.Poison, damage = Target.Health * .1f, damageMultiplier = 0 };
                    break;

                case CharacterClass.Archer:
                    skill = new CharacterSkills { Name = CharacterSkillName.Bleed, damage = Target.Health * .2f, damageMultiplier = 1.2f };
                    break;

                case CharacterClass.Warrior:
                    Health *= 2;
                    DamageMultiplier = 1.5f;
                    skill = new CharacterSkills { Name = CharacterSkillName.Bleed, damage = Target.Health * .2f, damageMultiplier = 1.2f };
                    break;
            }
        }
    }
}
