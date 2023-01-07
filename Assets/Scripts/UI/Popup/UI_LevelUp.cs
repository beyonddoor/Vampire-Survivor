using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUp : UI_Popup
{
    int MaxUpgradeNum = 3;
    
    public enum Panels
    {
        GridPanel
    }

    public override void Init()
    {
        base.Init();
        _popupID = Define.PopupUIGroup.UI_LevelUp;
        Bind<Image>(typeof(Panels));

        GameObject gridPanel = Get<GameObject>((int)Panels.GridPanel);

        foreach(Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }


        //here we choose stat or weapon random number.
        string title = "�г� �׽�Ʈ";
        string desc = "�г� ���� �׽�Ʈ";
        for(int i = 0; i< MaxUpgradeNum; i++)
        {
            GameObject upgradePanel = Managers.UI.MakeSubItem<UpgdPanel>(parent:gridPanel.transform).gameObject;
            UpgdPanel upgradeDesc = upgradePanel.GetOrAddComponent<UpgdPanel>();
            upgradeDesc.SetInfo(title+i.ToString(),desc+i.ToString());
        }
    }
}