﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class RoundStateWaitingForInput : RoundState {
    MoveRange range = new MoveRange();
    public RoundStateWaitingForInput(RoundManager roundManager) : base(roundManager)
    {

    }

    public void CreatePanel(Unit unit)
    {
        RoleInfoView.GetInstance().Open(unit.transform);
    }

    public override void OnUnitClicked(Unit unit)
    {
        RoleInfoView.TryClose();
        foreach (var f in BattleFieldManager.GetInstance().floors)
        {
            f.Value.SetActive(false);
        }
        range = new MoveRange();
        if (unit.playerNumber.Equals(roundManager.CurrentPlayerNumber) && !unit.UnitEnd)
        {
            roundManager.RoundState = new RoundStateUnitSelected(roundManager, unit);
        }
        else
        {
            var outline = Camera.main.GetComponent<RenderBlurOutline>();
            if (outline)
                outline.RenderOutLine(unit.transform);
            range.CreateMoveRange(unit.transform);
            CreatePanel(unit);
        }
#if (UNITY_STANDALONE)
        Camera.main.GetComponent<RTSCamera>().FollowTarget(unit.transform.position);
#endif
    }

    public override void OnStateEnter()
    {
        RoleInfoView.TryClose();
        if (BattleView.isInit)
        {
            BattleView.GetInstance().menuButton.gameObject.SetActive(true);
        }
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        if (BattleView.isInit)
        {
            BattleView.GetInstance().menuButton.gameObject.SetActive(false);
            BattleView.GetInstance().debugMenu.gameObject.SetActive(false);
        }
        RoleInfoView.TryClose();
        if (range != null)
            range.Delete();
        base.OnStateExit();
    }
}
