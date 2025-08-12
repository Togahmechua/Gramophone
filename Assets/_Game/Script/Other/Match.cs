using UnityEngine;

public class Match : MonoBehaviour
{
    public EMatch eMatch;
    public bool isMatch = true; // dùng để đánh dấu điểm có thể nối

    private void OnMouseDown()
    {
        if (isMatch) // chỉ gọi khi match hợp lệ
            LineManager.Instance.OnMatchClicked(this);
    }
}

public enum EMatch
{
    Gramophone,
    Disk
}
