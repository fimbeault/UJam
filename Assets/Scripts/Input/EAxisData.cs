using System;
using System.Collections.Generic;
using UnityEngine;

public class EAxisData
{
    private static List<EAxisData> mList = new List<EAxisData>();

    /***********************************************************/
    /***********************************************************/
    /***********************************************************/


    private int mSpriteIndex;
    private float mMinimalValue;
    private string mAxisName;
    private string mAxisInputString;


    /***********************************************************/
    /***********************************************************/
    /***********************************************************/

    private static float BUTTON_MINIMAL_INPUT = 0.01f;
    private static float AXIS_MINIMAL_INPUT = 0.5f;


    private static string A_NAME = "A";
    private static string B_NAME = "B";
    private static string X_NAME = "X";
    private static string Y_NAME = "Y";

    private static string X_AXIS_NAME = "L_XAxis";
    private static string Y_AXIS_NAME = "L_YAxis";

    public static EAxisData A_BUTTON = new EAxisData("A", A_NAME, BUTTON_MINIMAL_INPUT, 2);
    public static EAxisData B_BUTTON = new EAxisData("B", B_NAME, BUTTON_MINIMAL_INPUT, 1);
    public static EAxisData X_BUTTON = new EAxisData("X", X_NAME, BUTTON_MINIMAL_INPUT, 0);
    public static EAxisData Y_BUTTON = new EAxisData("Y", Y_NAME, BUTTON_MINIMAL_INPUT, 3);

    public static EAxisData UP = new EAxisData("U", Y_AXIS_NAME, -AXIS_MINIMAL_INPUT, 5);
    public static EAxisData DOWN = new EAxisData("D", Y_AXIS_NAME, AXIS_MINIMAL_INPUT, 6);
    public static EAxisData LEFT = new EAxisData("L", X_AXIS_NAME, -AXIS_MINIMAL_INPUT, 7);
    public static EAxisData RIGHT = new EAxisData("R", X_AXIS_NAME, AXIS_MINIMAL_INPUT, 4);


    public EAxisData(string aAxisName, string aAxisInputString, float aMinimalValue, int aSpriteIndex)
    {
        mAxisName = aAxisName;
        mAxisInputString = aAxisInputString;
        mMinimalValue = aMinimalValue;

        mSpriteIndex = aSpriteIndex;

        mList.Add(this);
    }

    public int SpriteIndex
    {
        get
        {
            return mSpriteIndex;
        }
    }

    public string AxisName
    {
        get
        {
            return mAxisName;
        }
    }

    public float MinimalValue
    {
        get
        {
            return mMinimalValue;
        }
    }

    public string GetFullAxisName(EPlayerId aPlayerId)
    {
        return mAxisName + aPlayerId.InputSuffix;
    }

    public string GetFullInputName(EPlayerId aPlayerId)
    {
        return mAxisInputString + aPlayerId.InputSuffix;
    }

    override public string ToString()
    {
        return mAxisInputString + "," + mMinimalValue;
    }

    public static List<EAxisData> GetList()
    {
        return mList;
    }
}