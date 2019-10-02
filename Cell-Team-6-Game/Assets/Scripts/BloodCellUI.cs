using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodCellUI : MonoBehaviour
{
    public PlayerInventory.BulletType cellType;

    Text amountText;
    Text cellTypeText;
    PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        amountText = transform.Find("Amount").GetComponent<Text>();
        cellTypeText = transform.Find("Cell Type").GetComponent<Text>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    private void UpdateUI() {
        amountText.text = playerInventory.whiteBloodCells [cellType].ToString();
        cellTypeText.text = cellType.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
