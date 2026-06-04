using System;
using Enums;
using UnityEngine;

namespace Configs.Data
{
    [Serializable]
    internal sealed class WeaponSound
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private AudioClip _audioClip;

        public WeaponType WeaponType => _weaponType;
        public AudioClip AudioClip => _audioClip;
    }
}