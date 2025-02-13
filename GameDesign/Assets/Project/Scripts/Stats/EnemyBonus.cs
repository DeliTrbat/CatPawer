using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    public class EnemyBonus : ItemStats
    {
        private Structs.Body getDifficultyBody(int difficulty)
        {
            return new Structs.Body
            {
                MaxHP = difficulty * 20.0f,
                Armor = difficulty * 10,
                Speed = 0
            };
        }

        private Structs.Combat getDifficultyCombat(int difficulty)
        {
            return new Structs.Combat
            {
                DamageBase = difficulty * 10,
                DamageBonus = difficulty * 2
            };
        }

        private Structs.Gun getDifficultyGun(int difficulty)
        {
            return new Structs.Gun
            {
                FireRate = 0,
                MagazineCapacity = 0,
                ReloadSpeed = 0
            };
        }

        public void SetDifficulty(int difficulty)
        {
            RemoveMyself();
            Body = getDifficultyBody(difficulty);
            Combat = getDifficultyCombat(difficulty);
            Gun = getDifficultyGun(difficulty);
            AddMyself();
        }
    }
}
