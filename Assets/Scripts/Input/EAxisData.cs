using System;
using System.Collections.Generic;
using UnityEngine;

public class EAxisData
{
    private static List<EAxisData> mList = new List<EAxisData>();

    /***********************************************************/
    /***********************************************************/
    /***********************************************************/

    private int mId;
    private float mMinimalValue;
    private string mAxisName;
    private string mAxisInputString;
    private int mAxisDirection;


    /***********************************************************/
    /***********************************************************/
    /***********************************************************/

    private const float BUTTON_MINIMAL_INPUT = 0.01f;
    private const float AXIS_MINIMAL_INPUT = 0.5f;


    private const string A_NAME = "A";
    private const string B_NAME = "B";
    private const string X_NAME = "X";
    private const string Y_NAME = "Y";

    private const string X_AXIS_NAME = "L_XAxis";
    private const string Y_AXIS_NAME = "L_YAxis";

    public const int DIRECTION_UP = 0;
    public const int DIRECTION_RIGHT = 1;
    public const int DIRECTION_DOWN = 2;
    public const int DIRECTION_LEFT = 3;

    public static EAxisData A_BUTTON = new EAxisData("A", A_NAME, BUTTON_MINIMAL_INPUT, DIRECTION_DOWN);
    public static EAxisData B_BUTTON = new EAxisData("B", B_NAME, BUTTON_MINIMAL_INPUT, DIRECTION_RIGHT);
    public static EAxisData X_BUTTON = new EAxisData("X", X_NAME, BUTTON_MINIMAL_INPUT, DIRECTION_LEFT);
    public static EAxisData Y_BUTTON = new EAxisData("Y", Y_NAME, BUTTON_MINIMAL_INPUT, DIRECTION_UP);

    public static EAxisData UP = new EAxisData("U", Y_AXIS_NAME, -AXIS_MINIMAL_INPUT, DIRECTION_UP);
    public static EAxisData DOWN = new EAxisData("D", Y_AXIS_NAME, AXIS_MINIMAL_INPUT, DIRECTION_DOWN);
    public static EAxisData LEFT = new EAxisData("L", X_AXIS_NAME, -AXIS_MINIMAL_INPUT, DIRECTION_LEFT);
    public static EAxisData RIGHT = new EAxisData("R", X_AXIS_NAME, AXIS_MINIMAL_INPUT, DIRECTION_RIGHT);


    public EAxisData(string aAxisName, string aAxisInputString, float aMinimalValue, int aAxisDirection)
    {
        mAxisName = aAxisName;
        mAxisInputString = aAxisInputString;
        mMinimalValue = aMinimalValue;
        mAxisDirection = aAxisDirection;

        mId = mList.Count;

        mList.Add(this);
    }

    public int Id
    {
        get
        {
            return mId;
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

    public int AxisDirection
    {
        get
        {
            return mAxisDirection;
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


    public static EAxisData GetAxisByName(string aName)
    {
        foreach (EAxisData axisData in mList)
        {
            if (axisData.AxisName == aName)
            {
                return axisData;
            }
        }

        return null;
    }

    public static List<EAxisData> GetList()
    {
        return mList;
    }
}