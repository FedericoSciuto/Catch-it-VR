using UnityEngine;
using System.IO;
using System.Text;

public class HandsRecorder : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    private string filePath;
    private StringBuilder buffer;
    private bool isRecording = false;

    void Start()
    {
        buffer = new StringBuilder();
    }

    public void StartRecording(string filePath)
    {
        this.filePath = filePath;
        buffer.Clear();

        buffer.AppendLine("timestamp;leftPosX;leftPosY;leftPosZ;leftRotX;leftRotY;leftRotZ;leftRotW;rightPosX;rightPosY;rightPosZ;rightRotX;rightRotY;rightRotZ;rightRotW");

        isRecording = true;
    }

    public void StartRecordingSegment()
    {
        isRecording = true;
    }
    
    public void StopRecordingSegment()
    {
        isRecording = false;
    }

    public void SaveBufferToFile()
    {
        File.WriteAllText(filePath, buffer.ToString());
    }

    void Update()
    {
        if (!isRecording) return;

        string timestamp = Time.time.ToString("F3");

        Vector3 lp = leftHand.position;
        Quaternion lr = leftHand.rotation;

        Vector3 rp = rightHand.position;
        Quaternion rr = rightHand.rotation;

        buffer.AppendLine($"{timestamp};{lp.x};{lp.y};{lp.z};{lr.x};{lr.y};{lr.z};{lr.w};{rp.x};{rp.y};{rp.z};{rr.x};{rr.y};{rr.z};{rr.w}");
    }
}
