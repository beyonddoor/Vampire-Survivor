using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_ItemBoxOpen : UI_Popup
{
    [SerializeField]
    List<Transform> weaponUILocation;
    List<Define.Weapons> weaponList;
    PlayerStat player;

    public override Define.PopupUIGroup _popupID { get { return Define.PopupUIGroup.UI_ItemBoxOpen; } }

    enum Buttons
    {
        ItemBoxButton
    }
    enum Texts
    {
        ItemBoxText
    }
    enum Images
    {
        ItemBoxPanel,
        ItemBoxImage,
        BackgroundImg
    }

    public override void Init()
    {
        base.Init();
        player = Managers.Game.getPlayer().GetOrAddComponent<PlayerStat>();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetImage((int)Images.BackgroundImg).gameObject.AddUIEvent(OnOpenChest);
        {
            //自动打开
            OnOpenChest(null);
        }
    }

    void OnOpenChest(PointerEventData data)
    {
        Managers.Sound.Play("BoxOpen", Define.Sound.Effect, 0.8f);
        weaponList = Managers.Event.SetRandomWeaponfromItemBox(player);
        if (weaponList == null)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubItem/WeaponInven", parent: weaponUILocation[0].transform);
            Util.FindChild<Image>(go, "WeaponImg", true).sprite = Managers.Resource.LoadSprite("Health");
        }
        else
            for (int i = 0; i < weaponList.Count; i++)
            {
                GameObject go = Managers.Resource.Instantiate("UI/SubItem/WeaponInven", parent: weaponUILocation[i].transform);
                Util.FindChild<Image>(go, "WeaponImg", true).sprite = Managers.Resource.LoadSprite(weaponList[i].ToString());
            }
        Get<Image>((int)Images.ItemBoxImage).GetComponent<Animator>().Play("Open");

        Button btn = GetButton((int)Buttons.ItemBoxButton);
        btn.gameObject.SetActive(true);
        btn.gameObject.AddUIEvent(Close);
        // {
        //     //TODO 自动获取
        //     Close(null);
        // }
    }

    void Close(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.Event.SetLevelUpWeaponfromItemBox(weaponList, player);
        Managers.UI.CloseAllGroupPopupUI(_popupID);
    }
}
