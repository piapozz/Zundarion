using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̏����ݒ�</summary>
    public virtual void Setup()
    {
       
    }

    /// <summary>���g�̌��݂̗̑�</summary>
    //public float selfCurrentHealth { get => selfTankHealth?.CurrentHealth ?? 0.0f; }

    /// <summary>�U���R���{�̍ő吔</summary>
    public float selfComboCount { get; protected set; }

    /// <summary>�v���C���[�̈ړ����x</summary>
    public float selfMoveSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̍Đ����x</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    public float selfFrontAngle { get; set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>���t���[���Ă΂��AI�ɂ�鑀��</summary>
    protected abstract void UpdateAI();

    /// <summary>��Q���̃��C���[�}�X�N</summary>
    protected LayerMask obstacleLayerMask { get; private set; }

    // private //////////////////////////////////////////////////////////////////

    /// <summary>�A�j���[�^�[�R���|�[�l���g(��ԈȊO��null)</summary>
    private Animator _selfAnimator = null;

    /// <summary>���g����Ԃ������ꍇ��TankHealth�R���|�[�l���g(��ԈȊO��null)</summary>
    //private TankHealth selfTankHealth = null;

    /// <summary>��Ԃ̈ړ��R���|�[�l���g</summary>
    //private TankMovement selfTankMovement = null;

    /// <summary>��Ԃ̖C�e���˃R���|�[�l���g</summary>
    //private TankShooting selfTankShooting = null;

    /// <summary>�^�[�Q�b�g�̑O�t���[���ł̍��W</summary>
    private Vector3 _oldPosition = Vector3.zero;

    /// <summary>�^�[�Q�b�g�̑O�t���[���ł̔��ˏ������</summary>
    //private bool oldLaunchPrepare = false;

    /// <summary>
    /// OnEnable()���O��1�x�����Ă΂��
    /// </summary>
    private void Awake()
    {

#if GUI_OUTPUT
        gui_instanceNum = BasePlayer.gui_instanceTotalNum++;
#endif
    }

    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        obstacleLayerMask = LayerMask.GetMask("FieldObject");
        _selfAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    /// <summary>
    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
    /// </summary>
    private void Update()
    {
        // �^�[�Q�b�g�̏����W

        // AI�̍X�V
        UpdateAI();

        // �o�͂̒���
        Quaternion q = Quaternion.AngleAxis(selfFrontAngle, Vector3.up);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, this.transform.rotation * q, 1);

        // ���t���[���̂��߂ɏ����c��
    }

#if GUI_OUTPUT

    /// <summary>GUI�o�͗p �C���X�^���X�J�E���^</summary>
    static private int gui_instanceTotalNum = 0;

    /// <summary>GUI�o�͗p �C���X�^���X�ԍ�</summary>
    private int gui_instanceNum;

    /// <summary>�C���X�y�N�^�pGUI�\���^��\���t���O</summary>
    [SerializeField]
    private bool enableGUIOutput = true;

    /// <summary>
    /// ���t���[���Ă΂��GUI�o�͗p���\�b�h
    /// <para>
    /// ��ʂɃf�o�b�O�p�̏����o��
    /// �i�d���̂Ŏ��@�ɂ͏��Ȃ��悤�ɂ���j
    /// </para>
    /// </summary>
    private void OnGUI()
    {
        if (!enableGUIOutput)
        {
            return;
        }
        Color oldColor = GUI.color;
        GUI.color = Color.yellow;
        using (new GUILayout.AreaScope(new Rect(0, 0, Screen.width, Screen.height)))
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (gui_instanceNum == 0)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            GUIOutputSelfInfo();
                            GUILayout.Space(20);
                        }
                    }
                    GUILayout.FlexibleSpace();
                    if (gui_instanceNum == 1)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            GUIOutputSelfInfo();
                            GUILayout.Space(20);
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                if (gui_instanceNum == 0)
                {

                }
            }
        }
        GUI.color = oldColor;
    }
#endif  // GUI_OUTPUT

    /// <summary>GUI�Ɏ��g�̏����o��</summary>
    virtual protected void GUIOutputSelfInfo()
    {
#if GUI_OUTPUT
        GUILayout.Label("SelfPosition: " + transform.position);
        GUILayout.Label("SelfYRotation: " + transform.rotation.eulerAngles.y);
#endif  // GUI_OUTPUT
    }
}
