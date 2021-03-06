﻿// vim: tabstop=4 softtabstop=4 shiftwidth=4 expandtab
//
// Copyright (C) 2013-2016 iFunFactory Inc. All Rights Reserved.
//
// This work is confidential and proprietary to iFunFactory Inc. and
// must not be used, disclosed, copied, or distributed without the prior
// consent of iFunFactory Inc.

using Fun;
using System;
using System.Collections.Generic;
using UnityEngine;


public class AnnouncementTest : MonoBehaviour
{
    public void OnGUI()
    {
        GUI.enabled = true;
        GUI.Label(new Rect(30, 8, 300, 20), string.Format("Server - {0}:{1}", kAnnouncementIp, kAnnouncementPort));
        if (GUI.Button(new Rect(30, 35, 240, 40), "Request announcements"))
        {
            if (announcement_ == null)
            {
                announcement_ = new FunapiAnnouncement();
                announcement_.ResultCallback += new FunapiAnnouncement.EventHandler(OnAnnouncementResult);

                string url = string.Format("http://{0}:{1}", kAnnouncementIp, kAnnouncementPort);
                announcement_.Init(url);
            }

            announcement_.UpdateList(5);
        }
    }

    private void OnAnnouncementResult (AnnounceResult result)
    {
        DebugUtils.Log("OnAnnouncementResult - result: {0}", result);
        if (result != AnnounceResult.kSuccess)
            return;

        if (announcement_.ListCount > 0)
        {
            for (int i = 0; i < announcement_.ListCount; ++i)
            {
                Dictionary<string, object> list = announcement_.GetAnnouncement(i);
                string buffer = "";

                foreach (var item in list)
                {
                    buffer += string.Format("{0}: {1}\n", item.Key, item.Value);
                }

                DebugUtils.Log("announcement ({0}) >> {1}", i + 1, buffer);

                if (list.ContainsKey("image_url"))
                    DebugUtils.Log("image path > {0}", announcement_.GetImagePath(i));
            }
        }
    }


    // Please change this address for test.
    private const string kAnnouncementIp = "127.0.0.1";
    private const UInt16 kAnnouncementPort = 8080;

    private FunapiAnnouncement announcement_ = null;
}
