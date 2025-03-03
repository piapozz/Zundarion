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
    public const float CHARACTER_ROTATE_SPEED = 1000;

    // プレイヤー関係
    public const float PARRY_SLOW_SPEED = 0.25f;
    public const float PARRY_SLOW_TIME = 0.5f;
    public const float PRE_INPUT_TIME = 0.5f;

    // シーン遷移
    public const float SCENE_FADE_TIME = 1.0f;

    // カメラ
    public const float CAMERA_SHAKE_TIME = 0.01f;
    public const float CAMERA_SHAKE_GAIN = 0.5f;
}
