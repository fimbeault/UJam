using System;
using System.Collections.Generic;
using UnityEngine;

public class ETimingFeedbackType
{
    private static List<ETimingFeedbackType> mList = new List<ETimingFeedbackType>();

    /***********************************************************/
    /***********************************************************/
    /***********************************************************/

    private int mId;
    private string mFeedbackName;


    /***********************************************************/
    /***********************************************************/
    /***********************************************************/

    public static ETimingFeedbackType OK = new ETimingFeedbackType("OK");
    public static ETimingFeedbackType GOOD = new ETimingFeedbackType("GOOD");
    public static ETimingFeedbackType PERFECT = new ETimingFeedbackType("PERFECT");
    public static ETimingFeedbackType MISS = new ETimingFeedbackType("MISS");

    public ETimingFeedbackType(string aFeedbackName)
    {
        mFeedbackName = aFeedbackName;

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

    public string FeedbackName
    {
        get
        {
            return mFeedbackName;
        }
    }

    override public string ToString()
    {
        return mFeedbackName;
    }

    public static List<ETimingFeedbackType> GetList()
    {
        return mList;
    }
}