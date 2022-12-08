using UnityEngine;

// 게임 점수를 증가시키는 아이템
public class Coin : ItemObject
{
    public override void Use()
    {
        // 게임 매니저로 접근해 점수 추가
        GameManager.instance.AddScore(score);
        // 사용되었으므로, 자신을 파괴
        ObjectPool.instance.ReturnObject(this);
    }

    public int score = 50; // 증가할 점수
}