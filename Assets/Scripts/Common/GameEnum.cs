using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 入力の種類
public enum InputType
{
    None,
    Move,
    Run,
    Attack,
    Parry,
    Menu,
    Cancel,
    Decision,
    Skip,

    Max
};

// プレイヤーのアニメーション
public enum PlayerAnimation
{
    INVALID = -1,
    IDLE,
    WALK,
    RUN,
    ATTACK,
    IMPACT,
    PARRY,
    PARRY_MISS,
    AVOID,
    JUST_AVOID,
    VICTORY,
    DIE,

    MAX
}

public enum EnemyAnimation
{
    INVALID = -1,
    IDLE,
    WANDERING,
    FOUND,
    CHASING,
    RESTAINT,
    ATTACK,
    STRONG_ATTACK,
    JUMP_ATTACK,
    HIT_BACK_HIGH,
    HIT_BACK_LOW,
    DYING,
    UPPER,
    HAMMER,
    DECIDE,
    VIGILANCE,
    JUMP,
    WAIT,

    MAX
}

public enum BGM
{
    TITLE = 0,
    MAIN,
    OTHER,

    MAX
}

public enum SE
{
    HIT = 0,
    ENEMY_OMEN,
    PARRY,
    SLOW,
    BARRIER_MISS,
    BARRIER_BREAK,
    MAX
}