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
    [SerializeField] private Collider2D collider;
    [ShowIf("@rangeType == eRangeType.FanShape"), SerializeField] private GameObject[] fanShapeObjs;

    private SkillInfo skillInfo;
    private UnitBehavior caster;
    private HitEvent hitEvent;
    private UnityEngine.Events.UnityAction clickAction;

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
                transform.localScale = rect.Range;
                break;
            case HitEvenet.Circle circle:
                transform.localScale = Vector3.one * hitEvent.radius;
                break;
            case HitEvenet.FanShape fanShape:
                transform.localScale = Vector3.one * hitEvent.radius;
                for (int i = 0; i < fanShapeObjs.Length; i++)
                {
                    fanShapeObjs[i].SetActive(false);
                }
                switch (fanShape.Angle)
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
                break;
            default:
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

    public void OnClickAction(Vector3 clickPosition)
    {
        if (clickAction != null)
            clickAction.Invoke();

        skillInfo.targetPos = clickPosition;
        skillInfo.UseSkill();
    }
}
