﻿using System.Collections.Generic;


public class EPlayerId
{
    private static List<EPlayerId> mList = new List<EPlayerId>();

    public static EPlayerId PLAYER_ONE = new EPlayerId("_1");
    public static EPlayerId PLAYER_TWO = new EPlayerId("_2");
    public static EPlayerId PLAYER_THREE = new EPlayerId("_3");
    public static EPlayerId PLAYER_FOUR = new EPlayerId("_4");

    private string mInputSuffix;
    private int mInputIndex;

    public string InputSuffix
    {
        get
        {
            return mInputSuffix;
        }
    }

    public int InputIndex
    {
        get
        {
            return mInputIndex;
        }
    }

    public EPlayerId(string aInputSuffix)
    {
        mInputIndex = mList.Count;
        mList.Add(this);

        mInputSuffix = aInputSuffix;
    }

    public static List<EPlayerId> GetList()
    {
        return mList;
    }
}
