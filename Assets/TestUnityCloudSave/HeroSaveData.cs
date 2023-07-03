using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using System.Linq;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
    fileName = "HeroSaveData.asset",
    menuName = SOArchitecture_Utility.SAVE_DATA + "HeroSaveData",
    order = 120)]

    public class HeroSaveData : Collection<HeroSave>
    {
        public HeroSave FindHero(int _id)
        {
            return Value.Find(x => x.m_Id == _id);
        }

        public void SetValue(List<HeroSave> _value)
        {
            Value = _value;
        }
    }

    [System.Serializable]
    public class HeroSave
    {
        public int m_Id;
        public string m_Name;
        public List<Weapon> m_Weapons;
    }

    [Preserve]
    public enum WeaponType
    {
        [Preserve]
        Rifle,
        [Preserve]
        Bow,
        [Preserve]
        Pistol,
        [Preserve]
        Sniper,
    }

    [System.Serializable]
    public class Weapon
    {
        public int m_Id;
        public WeaponType m_WeaponType;
    }
}