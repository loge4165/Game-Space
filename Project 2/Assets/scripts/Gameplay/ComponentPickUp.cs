using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ComponentPickUp : MonoBehaviour
{
    public string title;
    public string description;
    void Start() {
        //TODO:make this planet based?
        GameConstantSingleton.GetInstance.planetItems.Add(this.gameObject);
    }
    public void PickUp(PlayerController picker) {    
        Component target = picker.gameObject.AddComponent(this.component.GetType());
        DuplicateComponent(this.component, target);
        GameConstantSingleton.GetInstance.planetItems.Remove(this.gameObject);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUDmanager>().PopUp(title,description);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUDmanager>().tutorialTextPopUp("You've picked up a useful supply pack that give you unique abilities, they stack so collect as many as you can!");
        Destroy(this.gameObject);

    }

    void DuplicateComponent(Component sourceComp, Component targetComp) {


        FieldInfo[] sourceFields = sourceComp.GetType().GetFields(BindingFlags.Public | 
                                                        BindingFlags.NonPublic | 
                                                        BindingFlags.Instance);
        int i = 0;
        for(i = 0; i < sourceFields.Length; i++) {
            var value = sourceFields[i].GetValue(sourceComp);
            sourceFields[i].SetValue(targetComp, value);
        }
    }

    public void OnDestroy() {
        GameConstantSingleton.GetInstance.planetItems.Remove(this.gameObject);
    }
    public Component component;
}
