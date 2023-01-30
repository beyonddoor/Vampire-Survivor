using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUp : UI_Popup
{
    int MaxUpgradeNum = 3;
    
    public enum Panels
    {
        GridPanel,
        
    }

    public override Define.PopupUIGroup _popupID
    {
        get { return Define.PopupUIGroup.UI_LevelUp; }
    }
    public override void Init()
    {
        base.Init();
        Managers.Sound.Play("LevelUp", Define.Sound.Effect);
        Bind<GameObject>(typeof(Panels));

        GameObject gridPanel = Get<GameObject>((int)Panels.GridPanel);

        foreach(Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
        PlayerStat player = Managers.Game.getPlayer().GetOrAddComponent<PlayerStat>();
        List<string[]> itemList = Managers.Event.SetRandomItem(player, 3);
        for(int i = 0; i<itemList.Count; i++) 
        {
            Debug.Log($"Item{i + 1}  : {itemList[i][1]}");
        }

        //here we choose stat or weapon random number.
        string title = "패널 테스트";
        string desc = "패널 설명 테스트";
        for(int i = 0; i< MaxUpgradeNum; i++)
        {
            GameObject upgradePanel = Managers.UI.MakeSubItem<UpgdPanel>(parent:gridPanel.transform).gameObject;
            UpgdPanel upgradeDesc = upgradePanel.GetOrAddComponent<UpgdPanel>();
            upgradeDesc.SetData(itemList[i]);
            
            {
                //TODO 直接跳过ui，选择第一个
                Debug.LogWarning("TODO 直接跳过ui，选择第一个");
                var data = itemList[i];
                var itemType = int.Parse(data[0]);
                var itemName = data[1];
                Managers.Event.LevelUpOverEvent(itemType, itemName);
                // Managers.UI.ClosePopupUI(Define.PopupUIGroup.UI_LevelUp);
                break;
            }
            
            title = itemList[i][1];
            desc = itemList[i][1];
            upgradeDesc.SetInfo(title, desc);
        }
    }
}
