using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTag : MonoBehaviour
{

    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private Image _ship;
    [SerializeField] private RectTransform _tagTransform;

    private SpaceshipConfig _config = null;
    private Quaternion _lastParentRotation;
    private Quaternion _lastTagRotation;

    private void Update()
    {
        // Inverse Tag rotation to keep name tag and other stuff in the correct orientation
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) *
                                  _lastParentRotation * transform.localRotation;
        _lastParentRotation = transform.parent.localRotation;

        // Inverse the inverse to keep the ship rotation correct.
        _ship.transform.localRotation = Quaternion.Inverse(transform.localRotation) *
                                  _lastTagRotation * _ship.transform.localRotation;
        _lastTagRotation = _ship.transform.parent.localRotation;

        // Great Success!!
    }

    public void InitTag(Player player, float angle, float radius, float rotationSpeed)
    {
        _nickname.text = player.NickName;
        _config = MainMenuManager.Instance.ShipConfigList.GetPlayerConfig(player);
        _nickname.color = _config.Color;
        _ship.material = _config.Material;

        SetTransform(angle, radius);

        _lastParentRotation = transform.parent.localRotation;
        _lastTagRotation = transform.localRotation;

        gameObject.SetActive(true);
    }

    public void HideTag()
    {
        gameObject.SetActive(false);
    }

    private void SetTransform(float angle, float radius)
    {
        Vector2 position = new Vector2();

        
        position.x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        position.y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        _tagTransform.anchoredPosition = position;
        _tagTransform.rotation = Quaternion.identity;
        _ship.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    
}
