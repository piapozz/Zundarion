/**
 * @file GameConst.cs
 * @brief �萔��`
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

    // �L�����N�^�[�֌W
    public const float HIT_STOP_SPEED = 0.05f;
    public const int HIT_STOP_FRAME = 5;

    // �v���C���[�֌W
    public const float PARRY_SLOW_SPEED = 0.25f;
    public const float PARRY_SLOW_TIME = 0.5f;
    public const float AVOID_SLOW_SPEED = 0.25f;
    public const float AVOID_SLOW_TIME = 0.25f;
    public const float PRE_INPUT_TIME = 0.5f;

    // �V�[���J��
    public const float SCENE_FADE_TIME = 1.0f;

    // �J����
    public const float CAMERA_SHAKE_TIME = 0.01f;
    public const float CAMERA_SHAKE_GAIN = 0.5f;
}
