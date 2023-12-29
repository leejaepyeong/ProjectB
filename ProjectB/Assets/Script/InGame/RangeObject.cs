using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RangeObject : MonoBase
{
    public enum eRangeType
    {
        Circle,
        Rect,
        FanShape,
    }

    [SerializeField] private Tweener tween;
    [SerializeField] private eRangeType rangeType;
    [ShowIf("@rangeType == eRangeType.FanShape"), SerializeField] private GameObject[] fanShapeObjs;

    private SkillInfo skillInfo;
    private UnitBehavior caster;
    private HitEvent hitEvent;
    private UnityEngine.Events.UnityAction clickAction;

    private int angle;
    public virtual void Open(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction clickAction = null)
    {
        base.Open();
        this.skillInfo = skillInfo;
        this.caster = caster;
        this.hitEvent = hitEvent;
        this.clickAction = clickAction;

        switch (hitEvent.GetHitData())
        {
            case HitEvenet.Rect rect:
                break;
            case HitEvenet.Circle circle:
                break;
            case HitEvenet.FanShape fanShape:
                break;
            default:
                break;
        }

        switch (angle)
        {
            case 30:
                fanShapeObjs[0].SetActive(true);
                break;
            case 45:
                fanShapeObjs[1].SetActive(true);
                break;
            case 90:
                fanShapeObjs[2].SetActive(true);
                break;
            case 180:
                fanShapeObjs[3].SetActive(true);
                break;
        }
    }

    public override void UpdateFrame(float deltaTime)
    {
        SearchTargetColor();
    }

    private void SearchTargetColor()
    {

    }
    
    public void OnClickAction()
    {
        if (clickAction != null)
            clickAction.Invoke();

        skillInfo.UseSkill();
    }
}
