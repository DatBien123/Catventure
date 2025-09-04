using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "FitbLevelData", menuName = "Scriptable Objects/FitbLevelData")]
public class FitbLevelData : ScriptableObject
{
    public string levelName; // tên của level này có thể là địa danh, nói chung là mô tả liên quan đến màn chơi
    public string levelDescription; // nội dung mô tả về level này có thể là nội dung chú mèo Zera làm gì tiếp theo chẳng hạn
    public List<FITBQuestionSO> questions; // một level chứa các câu hỏi
    public float timeRequired; // thời gian yêu cầu để hoàn thành level
    public float reward; // số lượng phần thưởng nhận được
    public VideoClip videoStart; // video chạy khi bắt đầu vào màn chơi
    public VideoClip videoEnd; // video chạy khi đã hoàn thành màn chơi

}
