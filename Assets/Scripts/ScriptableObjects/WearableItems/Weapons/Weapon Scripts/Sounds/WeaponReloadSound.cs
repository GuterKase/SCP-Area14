﻿using UnityEngine;
using Zenject;

public class WeaponReloadSound : WeaponSoundPlayer
{
    [Inject] private readonly WeaponReload _weaponReload;

    protected override AudioClip Sound => _weaponHandler.Weapon_SO.reloadSound;

    protected override void SubscribeToAction()
    {
        _weaponReload.OnReloadStarted += PlaySound;
    }

    protected override void UnscribeToAction()
    {
        _weaponReload.OnReloadStarted -= PlaySound;
    }
}
