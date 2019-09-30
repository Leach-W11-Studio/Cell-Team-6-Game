using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<BulletType, int> whiteBloodCells;

    public enum BulletType
    {
        DualShot,
        SpreadShot,
        RicochetShot,
        MegaShot,
        StunShot,
    }

    // Start is called before the first frame update
    void Start()
    {
        whiteBloodCells = new Dictionary<BulletType, int>();
        CreateCellType(BulletType.DualShot, 5);
        CreateCellType(BulletType.SpreadShot, 5);
        CreateCellType(BulletType.RicochetShot, 5);
        CreateCellType(BulletType.MegaShot, 5);
        CreateCellType(BulletType.StunShot, 5);
    }

    public void CreateCellType(BulletType cellType, int initialAmount = 0) {
        if (initialAmount < 0) { initialAmount = 0; }
        whiteBloodCells.Add(cellType, initialAmount);
    }

    public void AddBloodCell(BulletType cellType, int amount = 1) {
        if (!whiteBloodCells.ContainsKey(cellType)) { return; }
        whiteBloodCells[cellType] = whiteBloodCells[cellType] + amount;
    }

    public void RemoveBloodCell(BulletType cellType, int amount = 1) {
        if (!whiteBloodCells.ContainsKey(cellType)) { return; }

        if (whiteBloodCells[cellType] - amount < 0)
        {
            whiteBloodCells[cellType] = 0;
        }
        else {
            whiteBloodCells[cellType] = whiteBloodCells[cellType] - amount;
        }
    }
}
