using System;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

[RequireComponent(typeof(CharacterBleeding))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Sprite m_imageForEmptyCell;
    [Inject] readonly SceneTransition m_sceneTransition;

    public int CurrentHealthCellIndex { get; set; }
    public int LastHealthCellIndex { get; set; } = 0;
    public List<HealthCell> HealthCells { get; set; } = new List<HealthCell>();
    public Action OnPlayerDie { get; set; }
    public Action OnFirstInjuary { get; set; }
    public Action OnPlayerGetsDamage { get; set; }

    void Start()
    {
        CurrentHealthCellIndex = HealthCells.Count - 1;
    }

    public void Damage()
    {
        GetCurrentHealthCell().MakeCellEmpty();
        CurrentHealthCellIndex--;

        OnPlayerGetsDamage?.Invoke();
        if (CurrentHealthCellIndex < LastHealthCellIndex)
        {
            Die();
        }
    }

    public HealthCell GetCurrentHealthCell()
    {
        return HealthCells[CurrentHealthCellIndex];
    }

    public HealthCell GetFirstHealthCell()
    {
        return HealthCells[HealthCells.Count - 1];
    }

    public int GetCurrentHealthPercent() => (CurrentHealthCellIndex * 100 / HealthCells.Count) + 25;

    void Die()
    {
        OnPlayerDie?.Invoke();
        m_sceneTransition.LoadScene((int)SceneTransition.Scenes.RespawnScene);
    }

}
