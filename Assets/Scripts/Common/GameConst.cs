/**
 * @file GameConst.cs
 * @brief 定数定義
 * @author sein
 * @date 2025/1/20
 */

public class GameConst
{
    public enum EnemyState
    {
        INVALID = -1,
        IDLE = 0,
        MOVE,
        FOUND,
        RUN,
        ATTACK,
        STRONG_ATTACK,
        UNIQUE_ATTACK,

        HITBACK_LOW,
        HITBACK_HIGH,
        DYING,
    }

    // キャラクター関係
    public const float CHARACTER_ROTATE_SPEED = 500.0f;
}
