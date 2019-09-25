using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<string, int> whiteBloodCells;

    // Start is called before the first frame update
    void Start()
    {
        whiteBloodCells = new Dictionary<string, int>();
        CreateCellType("Dual Shot", 5);
        CreateCellType("Spread Shot", 5);
        CreateCellType("Ricochet", 5);
        CreateCellType("Mega Shot", 5);
        CreateCellType("Stun", 5);
    }

    public void CreateCellType(string cellType, int initialAmount = 0) {
        if (initialAmount < 0) { initialAmount = 0; }
        whiteBloodCells.Add(cellType, initialAmount);
    }

    public void AddBloodCell(string cellType, int amount = 1) {
        if (!whiteBloodCells.ContainsKey(cellType)) { return; }
        whiteBloodCells[cellType] = whiteBloodCells[cellType] + amount;
    }

    public void RemoveBloodCell(string cellType, int amount = 1) {
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
