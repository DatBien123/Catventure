using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "FillInBlankQuestionSO", menuName = "Scriptable Objects/FillInBlankQuestionSO")]
public class FITBQuestionSO : ScriptableObject
{
    public string sentenceWithBlank; // câu hỏi ví dụ như là The cat is ____ on the the mat.
    public Sprite illustration; // hình ảnh con mèo nằm trên thảm
    public List<string> answers; // ["Sleeping", "Eating", "Jumping"]
    public string correctAnswer; // đáp án đúng
    public AudioClip pronunciation;// âm phát từ đúng
    public int catFootCoinNumber; // số lượng xu chân mèo nhận được khi trả lời đúng câu này
    public int languagueEnergyNumber; // số năng lượng ngôn ngữ nhận được khi trả lời đúng câu này
    public VideoClip correctAnswerVideo; // đoạn phim hoạt hình sau khi trả lời đúng câu hỏi này
    public VideoClip wrongAnswerVideo; // đoạn phim hoạt hình sau khi trả lời sai câu hỏi này


}
