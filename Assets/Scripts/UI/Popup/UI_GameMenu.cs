using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameMenu : UI_Popup
{
    enum Buttons
    {
        BackToGameButton,
        BackToMainButton
    }

    enum Images
    {
        BackgroundImage,
    }

    enum Texts
    {
        MenuText,
        SoundText,
        SoundSelectText
    }

    enum Sliders
    {
        VolumeSlider
    }

    enum Dropdowns
    {
        BGMSelectorDD
    }

    public override Define.PopupUIGroup _popupID
    {
        get { return Define.PopupUIGroup.UI_GameMenu; }
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<TMP_Dropdown>(typeof(Dropdowns));

        GetImage((int)Images.BackgroundImage, typeof(Images)).gameObject.AddUIEvent(OnBackToGame);
        GetButton((int)Buttons.BackToGameButton, typeof(Buttons)).gameObject.AddUIEvent(OnBackToGame);
        GetButton((int)Buttons.BackToMainButton, typeof(Buttons)).gameObject.AddUIEvent(OnBackToMain);
    }


    public void OnBackToGame(PointerEventData data)
    {
        Managers.UI.CloseAllGroupPopupUI(Define.PopupUIGroup.UI_GameMenu);
        Managers.GamePlay();
    }

    public void OnBackToMain(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.SceneType.MainMenuScene);
        Managers.GamePlay();
    }
}