using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class LogManager
{
    static public string s_LogPath = "";

    public static void SetLogPath()
    {
        // �����̸�
        string t_Directory = "/Log";

        // �α� ���� �̸�
        string t_FileName = "Log.txt";

        // ���� ��θ� �����ϱ� ���� ���� (�ش� ������ ���� ��� �����ϱ� ����)
        string t_Path = "";

        // �������� ��� ���� ��� �� ���ϸ��� ���Ե� ��� ����
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // ios �׽�Ʈ ����
            t_Path = Path.Combine(Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 5), "Documents"), "");
            s_LogPath = Path.Combine(Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 5), "Documents/"), t_FileName);
        }
        // �ȵ���̵��� ��� ���� ��� �� ���ϸ��� ���Ե� ��� ����
        else if (Application.platform == RuntimePlatform.Android)
        {
            t_Path = Application.persistentDataPath + t_Directory;
            s_LogPath = Application.persistentDataPath + t_Directory + "/" + t_FileName;
        }
        // ��Ÿ � ü��(���⼭�� ����Ƽ)
        else
        {
            t_Path = (Application.dataPath + t_Directory);
            // �ش� ���α׷�(����Ƽ) ���� ������ ���� (���� ���� ����)
            s_LogPath = (Application.dataPath + t_Directory + "/" + t_FileName);
        }

        // ���� ��θ� Ȯ��(�����) �� ���ٸ� ������ ����
        if (!Directory.Exists(t_Path))
        {
            Directory.CreateDirectory(t_Path);
        }
    }

    public static void Log(int num)
    {
        Log(num.ToString());
    }

    public static void Log(float num)
    {
        Log(num.ToString());
    }

    public static void Log(double num)
    {
        Log(num.ToString());
    }

    public static void Log(bool isbool)
    {
        Log(isbool.ToString());
    }

    public static void Log(string _logmsg)
    {
        // �ý��� �α׵� ���
        Debug.Log(_logmsg);

        // ���� ��ΰ� ��������� ���� ���¶�� ���ϰ�� ���� �Լ�(�޼ҵ�)�� ����
        if (s_LogPath == "")
        {
            SetLogPath();
        }

        FileStream t_File = null;

        // ���� Ȯ���ϰ� ���ٸ� ������ ����
        if (!File.Exists(s_LogPath))
        {
            //File.Create(s_LogPath);
            t_File = new FileStream(s_LogPath, FileMode.Create, FileAccess.Write);
        }
        // ������ �ֵ�� ���� �߰� �������� ����
        else
        {
            t_File = new FileStream(s_LogPath, FileMode.Append);
        }

        // ���� ������ ũ���� ũ�ٸ� �ݰ� �� ���Ͻ�Ʈ������ ����
        if (t_File.Length > 1048000)
        {
            t_File.Close();
            t_File = new FileStream(s_LogPath, FileMode.Create, FileAccess.Write);
        }

        StreamWriter t_SW = new StreamWriter(t_File);

        // �α� ���� �տ� �ð� �߰� 
        string t_Logfrm = DateTime.Now.ToString("MM-dd hh:mm:ss") + " -- " + _logmsg;

        // �α� ���
        t_SW.WriteLine(t_Logfrm);

        // ����ߴ� ��Ʈ���� �ݱ�
        t_SW.Close();
        t_File.Close();
    }
}
