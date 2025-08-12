using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineManager : MonoBehaviour
{
    public static LineManager Instance;

    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float epsilon = 0.0001f; // so sánh gần đúng

    [Header("All matches in the level (optional, auto-filled if empty)")]
    [SerializeField] private List<Match> allMatches = new List<Match>();

    private LineRenderer lineRenderer;
    private List<Match> connectedMatches = new List<Match>();
    private List<Vector2> points = new List<Vector2>();

    private bool isWin = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // nếu bạn quên kéo list vào inspector, auto find tất cả Match trong scene
        if (allMatches == null || allMatches.Count == 0)
        {
            Match[] found = FindObjectsOfType<Match>();
            allMatches = new List<Match>(found);
            Debug.Log($"Auto-filled allMatches: {allMatches.Count}");
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    /// <summary>
    /// Gọi khi người chơi click vào 1 match (ví dụ từ Match.OnMouseDown hoặc IPointerClickHandler)
    /// </summary>
    public void OnMatchClicked(Match clicked)
    {
        if (UIManager.Ins.pauseCanvas != null && UIManager.Ins.pauseCanvas.gameObject.activeSelf)
            return;

        if (isWin) return;
        if (clicked == null) return;
        if (!clicked.isMatch) // nếu match bị disable thì bỏ qua
        {
            Debug.Log("Clicked match is disabled (isMatch=false) -> ignored");
            return;
        }

        // nếu chưa có điểm nào -> thêm làm điểm đầu
        if (connectedMatches.Count == 0)
        {
            AddPoint(clicked);
            Debug.Log($"Start from {clicked.name}");
            return;
        }

        Match last = connectedMatches[connectedMatches.Count - 1];

        // nếu click lại chính điểm cuối -> ignore
        if (clicked == last)
        {
            Debug.Log("Clicked same as last -> ignore");
            return;
        }

        // nếu click vào điểm đầu (cố gắng đóng vòng)
        if (clicked == connectedMatches[0])
        {
            // chỉ cho đóng vòng nếu đã có >= 3 điểm
            if (connectedMatches.Count < 3)
            {
                Debug.Log("Need at least 3 points to close loop");
                return;
            }

            // kiểm tra rule enum: last và first phải khác nhau
            if (last.eMatch == clicked.eMatch)
            {
                Debug.Log("Cannot close loop: last and first have same type");
                return;
            }

            // kiểm tra đoạn last->first có cắt đoạn nào trước đó không
            Vector2 lastPos = last.transform.position;
            Vector2 firstPos = connectedMatches[0].transform.position;
            if (WouldSegmentIntersectExisting(lastPos, firstPos))
            {
                Debug.Log("Closing segment intersects existing segments -> cannot close");
                return;
            }

            // --- MỚI: kiểm tra phải nối đủ tất cả các match hợp lệ trước khi win ---
            int totalActiveMatches = CountActiveMatches();
            Debug.Log($"Attempt to close loop: connected {connectedMatches.Count} / needed {totalActiveMatches}");
            if (connectedMatches.Count != totalActiveMatches)
            {
                Debug.Log($"Not all matches connected yet. Need {totalActiveMatches}, current {connectedMatches.Count} -> cannot win");
                return;
            }

            // thành công: đóng vòng → add final point (first) để hiển thị kín
            points.Add(firstPos);
            UpdateLineRenderer();
            isWin = true;
            Debug.Log("WIN! Closed loop and covered all matches.");

            AudioManager.Ins.PlaySFX(AudioManager.Ins.complete);
            OnWin();
            return;
        }

        // nếu click vào điểm đã chọn trước đó (không phải first) -> không cho
        if (connectedMatches.Contains(clicked))
        {
            Debug.Log("This point already selected");
            return;
        }

        // kiểm tra enum phải khác với last
        if (clicked.eMatch == last.eMatch)
        {
            Debug.Log("Cannot connect same type");
            return;
        }

        // kiểm tra đoạn last->clicked có cắt đoạn nào không
        Vector2 lastP = last.transform.position;
        Vector2 newP = clicked.transform.position;
        if (WouldSegmentIntersectExisting(lastP, newP))
        {
            Debug.Log("New segment would intersect existing segments -> cannot add");
            return;
        }

        // hợp lệ -> thêm point
        AddPoint(clicked);
    }

    private void AddPoint(Match match)
    {
        connectedMatches.Add(match);
        points.Add(match.transform.position);
        UpdateLineRenderer();

        Debug.Log($"Added point {match.name}. Connected: {connectedMatches.Count}/{CountActiveMatches()}");

        AudioManager.Ins.PlaySFX(AudioManager.Ins.match);
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            lineRenderer.SetPosition(i, points[i]);
    }

    private void OnWin()
    {
        // TODO: xử lý khi win: hiệu ứng, âm thanh, mở UI...
        Debug.Log("Handle win screen / effect here");

        LevelManager.Ins.isWin = true;

        UIManager.Ins.CloseUI<MainCanvas>();
        UIManager.Ins.OpenUI<GramophoneCanvas>();


        // Ví dụ: disable all matches (chỉ minh họa)
        foreach (var m in allMatches)
            if (m != null) m.isMatch = false;
    }

    /// <summary>Đếm số match "hợp lệ" cần nối (isMatch == true)</summary>
    private int CountActiveMatches()
    {
        int cnt = 0;
        if (allMatches != null)
        {
            foreach (var m in allMatches)
                if (m != null && m.isMatch) cnt++;
        }
        return cnt;
    }

    /// <summary>
    /// Kiểm tra đoạn newSeg (a->b) có giao cắt với bất kỳ đoạn hiện có nào không.
    /// Bỏ qua các đoạn chia sẻ đỉnh (có cùng điểm đầu/điểm cuối).
    /// </summary>
    private bool WouldSegmentIntersectExisting(Vector2 a, Vector2 b)
    {
        if (points.Count < 2) return false;

        // existing segments: points[i] -> points[i+1] for i in [0 .. points.Count-2]
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 s1 = points[i];
            Vector2 s2 = points[i + 1];

            // Nếu đoạn hiện có chia sẻ đầu hoặc cuối với đoạn mới => bỏ qua
            if (ApproximatelyEqual(s1, a) || ApproximatelyEqual(s2, a) ||
                ApproximatelyEqual(s1, b) || ApproximatelyEqual(s2, b))
            {
                continue;
            }

            if (LinesIntersect(a, b, s1, s2))
                return true;
        }

        return false;
    }

    // Hàm kiểm tra 2 đoạn có giao nhau (không tính chạm tại đỉnh)
    private bool LinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        // Sử dụng phương pháp hướng (orientation) / CCW
        return (CCW(a1, b1, b2) != CCW(a2, b1, b2)) &&
               (CCW(a1, a2, b1) != CCW(a1, a2, b2));
    }

    private bool CCW(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p3.y - p1.y) * (p2.x - p1.x) > (p2.y - p1.y) * (p3.x - p1.x);
    }

    private bool ApproximatelyEqual(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) <= epsilon * epsilon;
    }

    public void SetMatches(List<Match> matches)
    {
        // Reset line & dữ liệu
        lineRenderer.positionCount = 0;
        points.Clear();
        connectedMatches.Clear();
        isWin = false;

        // Lấy danh sách match từ level
        allMatches = matches != null ? new List<Match>(matches) : new List<Match>();

        Debug.Log($"[LineManager] Loaded {allMatches.Count} matches from Level");
    }

}
