using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour, IPointerEnter, IPointerExit
{
    [Header("Reference")]
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image armorIcon;
    [SerializeField] private TextMeshProUGUI armorText;
    [Space]
    [SerializeField] private Color red;
    [SerializeField] private Color blue;
    [Space]
    [SerializeField] private BuffUI buffUIPrefab;
    [SerializeField] private RectTransform buffBar;
    [Space]
    [SerializeField] private PreviewUI previewUI;

    public Entity Entity { get; private set; }

    public readonly Dictionary<BuffType, BuffUI> buffUIs = new();

    public int attackAddtion;
    public float attackMultiplier;

    public int armorAddtion;
    public float armorMultiplier;

    public float damageMultiplier;

    public void Setup(Entity entity)
    {
        Entity = entity;
        image.sprite = entity.Image;

        OnHealthChanged(Entity.CurrentHealth, Entity.MaxHealth);

        Entity.HealthChanged += OnHealthChanged;
        Entity.Dead += OnDead;
        Entity.BuffChanged += OnBuffChanged;
        Entity.PreviewChanged += OnPreviewChanged;
    }
    private void OnDisable()
    {
        if (Entity == null) return;

        Entity.HealthChanged -= OnHealthChanged; 
        Entity.Dead -= OnDead;
        Entity.BuffChanged -= OnBuffChanged;
        Entity.PreviewChanged -= OnPreviewChanged;
    }

    public void OnHealthChanged(float current, float max)
    {
        healthBar.value = current / max;
        healthText.text = $"{current}/{max}";
    }
    public void OnDead()
    {

    }
    public void OnBuffChanged(Buff buff)
    {
        if (buff == null) return;

        BuffType type = buff.BuffType;

        if (type == BuffType.Block)
        {
            if (buff.Stack == 0 && armorIcon.gameObject.activeSelf)
            {
                armorIcon.gameObject.SetActive(false);
                healthBarFill.color = Color.red;
            }
            else if (buff.Stack > 0 && !armorIcon.gameObject.activeSelf)
            {
                armorIcon.gameObject.SetActive(true);
                healthBarFill.color = Color.blue;
            }
            armorText.text = buff.Stack.ToString();
            return;
        }

        if(buff.Image == null) return;

        bool hasBuff = Entity.buffs.ContainsKey(type);
        bool hasUI = buffUIs.TryGetValue(type, out var ui);

        if (hasBuff)
        {
            if (!hasUI)
            {
                ui = Instantiate(buffUIPrefab, buffBar, false);
                buffUIs.Add(type, ui);
            }

            ui.Setup(buff);
            ui.UpdateStack(buff.Stack);
            if (!buff.Stackable) ui.HideStack();
        }
        else
        {
            if (hasUI)
            {
                Destroy(ui.gameObject);
                buffUIs.Remove(type);
            }
        }
    }
    public void OnPreviewChanged(ExposeType type, int attackTimes, int attack) => previewUI.UpdateUI(type, attackTimes, attack);

    public void OnPointerEnter()
    {
        if(SelectSystem.Instance.selectedHand != null)
        {
            if (Entity.isDead) return;
            SelectSystem.Instance.SetTarget(this);
            return;
        }
    }
    public void OnPointerExit()
    {
        if (SelectSystem.Instance.selectedHand != null)
        {
            if (Entity.isDead) return;
            SelectSystem.Instance.SetTarget(null);
            return;
        }
    }
}
