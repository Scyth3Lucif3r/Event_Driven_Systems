using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    public UIManager uiManager;
    public InventoryManager inventoryManager;

    [Header("Character Controller")]
    public Character character;

    [Header("Gameplay Objects")]
    public GameObject barriers1;
    public Door door;
    public GameObject inventoryItems;
    public PressurePlate pressurePlate;
    public doorPlate doorPlate1;
    public doorPlate doorPlate2;

    [Header("Prefabs")]
    public Inventory weightPrefab;

    void Start()
    {
        inventoryManager.OnInventoryChanged.AddListener(uiManager.UpdateInventoryUI);
        inventoryManager.OnInventorySpawned.AddListener(SpawnInventory);

        foreach (Transform child in inventoryItems.transform)
        {
            Inventory inventory = child.GetComponent<Inventory>();
            inventory.OnItemCollected.AddListener(inventoryManager.PickUpInventory);
        }

        // connect gameplay system events
        foreach (Transform child in barriers1.transform)
        {
            
            Barriers barrier = child.GetComponent<Barriers>();
            pressurePlate.OnToggle.AddListener(barrier.Move);
        }

        //listener for for the pressure plates for the door.
        doorPlate1.DoorToggle.AddListener(LockDoorPlates);
        

        character.OnInventoryShown.AddListener(uiManager.ShowInventory);
        character.OnItemDropped.AddListener(inventoryManager.DropInventory);
    }


    void LockDoorPlates(PlateState plateState)
    {
        //unlocks the door if the state of the plates is read as "Active."
        if (plateState == PlateState.Active)
        {
            door.SetLock(false);
        }
        //If the state is "Prepared", disables the first plates listener
        //This keeps players from being able to double triggers and unlock
        //the door using just the first plate.
        else if (plateState == PlateState.Prepared)
        {

            doorPlate2.DoorToggle.AddListener(LockDoorPlates);
            doorPlate1.DoorToggle.RemoveListener(LockDoorPlates);   
        }
        
    }

    void SpawnInventory(InventoryItem item)
    {
        switch (item)
        {
            case InventoryItem.Weight:
                PlaceInventory(weightPrefab);
                break;
        }
    }

    void PlaceInventory(Inventory inventoryPrefab)
    {
        Inventory inventory = Instantiate(inventoryPrefab);
        inventory.OnItemCollected.AddListener(inventoryManager.PickUpInventory);

        // drop the new inventory item at the player position and a little
        // bit in front of it the player
        inventory.transform.position = character.transform.position + character.transform.forward;
    }
}

