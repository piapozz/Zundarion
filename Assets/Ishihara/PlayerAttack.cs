using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _palyer = null;

    /// <summary>当たり判定生成コンポーネント</summary>
    //private TankMovement selfTankMovement = null;

    /// <summary>現在のコンボ数</summary>
    private int _comboCount = 0;

    /// <summary>プレイヤーのアニメータートリガー</summary>
    private List<string> _triggers;


    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();   // アニメーター取得
        _palyer = GetComponent<BasePlayer>();   // プレイヤー取得

        // トリガーパラメーター文字列を設定
        for(int i = 0;i< _palyer.selfComboCount; i++)
        {
            _triggers.Add("Attack" + (i + 1).ToString());
        }
    }

    // Update is called once per frame
    /// <summary>
    /// ゲームループで（1秒間に何回も）呼ばれる
    /// </summary>
    void Update()
    {
        // 定期的にコンボ回数を初期化する
        _comboCount = 0;
    }

    /// <summary>
    /// アニメーションを攻撃に更新して、当たり判定を生成する
    /// </summary>
    void Attack()
    {
        // コンボの派生がまだあるなら
        if(_comboCount >= _palyer.selfComboCount) return;

        // 攻撃できるアニメーション状況なら
        if(!CheckAssailable()) return;

        // カウンターを増やして。トリガーをセットする
        _comboCount++;
        _animator.SetTrigger(_triggers[_comboCount]);

        // 当たり判定を生成する

    }

    /// <summary>
    /// 現在のアニメーションが攻撃可能な状態なのか調べる
    /// </summary>
    bool CheckAssailable()
    {
        return true;
    }

}
