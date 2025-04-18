using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgBase
{
    public string protoName = "";

    public static byte[] Encode(MsgBase msgBase)
    {
        string s = JsonUtility.ToJson(msgBase);
        return System.Text.Encoding.UTF8.GetBytes(s);
    }
    public static MsgBase Decode(string protoName, byte[] bytes,int offest,int count)
    {
        Debug.Log("StartDecode");
        string s = System.Text.Encoding.UTF8.GetString(bytes,offest,count);
        Debug.Log(s);
        MsgBase msgBase = (MsgBase)JsonUtility.FromJson(s, Type.GetType(protoName));
        return msgBase;                                 
    }
    public static byte[] EncodeName(MsgBase msgBase)
    {
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        Int16 len = (Int16)nameBytes.Length;
        byte[] bytes = new byte[len + 2];
        bytes[0] = (byte)(len % 256);
        bytes[1] = (byte)(len / 256);
        Array.Copy(nameBytes, 0, bytes, 2, len);
        return bytes;
    }
    public static string DecodeName(byte[] bytes,int offset,out int count)
    {
        count = 0;
        if(offset+2>bytes.Length)
        {
            return "";
        }
        Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
        if(len<=0)
        {
            return "";
        }
        if(offset + 2 + len>bytes.Length)
        {
            return "";
        }
        count = 2 + len;
        string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        return name;

    }
}
