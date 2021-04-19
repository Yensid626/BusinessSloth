using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOffice : MonoBehaviour
{
    public float mySpawnChance = 1f; //changes how likely this object is to spawn, regardless of chance of Script calling it
    public bool overrideChildrenSpawnChances = false;
    public bool chooseSingleItem = false; //if the spawnChance is met, only one item from all children will spawn in
    public float childrenSpawnChance = 0.0f; //how likely each child in this object is to spawn
    public int singleItemIndex = -1; //if set to -1, will choose a random item, otherwise will attempt to choose item at index
        //This value only affects chances if the GameObject is a part of Decorations.
    private List<GameObject> decorations = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.name != "Decorations")
        {
            GetDecorations();
            DecorateOffice(childrenSpawnChance);
        }
        else
        {
            if (chooseSingleItem)
            {
                GetChildren();
                ChooseItem();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetChildren()
    {
        decorations.Clear();
        decorations.TrimExcess();
        foreach (Transform child in transform)
        {
            //print("For each child: " + child.name);
            //print("    and parent: " + child.parent.gameObject.GetComponents<ShipPhysics>());
            decorations.Add(child.gameObject);
            //entitiesPhysics.Add(child.gameObject.GetComponent<PhysicsAndInput>());
            //entitiesPhysics.Add(child.GetComponent<PhysicsAndInput>());
        }
    }

    void GetDecorations()
    {
        decorations.Clear();
        decorations.TrimExcess();
        foreach (Transform child in transform.Find("Decorations").transform)
        {
            //print("For each child: " + child.name);
            //print("    and parent: " + child.parent.gameObject.GetComponents<ShipPhysics>());
            decorations.Add(child.gameObject);
            //entitiesPhysics.Add(child.gameObject.GetComponent<PhysicsAndInput>());
            //entitiesPhysics.Add(child.GetComponent<PhysicsAndInput>());
        }
    }

    void DecorateOffice(float chance)
    {
        float comparison = chance;
        foreach (GameObject child in decorations)
        {
            if (child.GetComponent<ChangeOffice>() != null && !overrideChildrenSpawnChances) { chance = child.GetComponent<ChangeOffice>().mySpawnChance; }
            else { chance = comparison; }
            if (Random.Range(0.0f, 1.0f) <= chance)
                { child.SetActive(true); }
            else
                { child.SetActive(false); }
        }
    }

    void ChooseItem()
    {
        int selected = Random.Range(0, decorations.Count - 1);
        if (singleItemIndex >= 0 && singleItemIndex < decorations.Count) { selected = singleItemIndex; }
        foreach (GameObject child in decorations)
        {
            child.SetActive(false);
        }
        decorations[selected].SetActive(true);
    }
}
