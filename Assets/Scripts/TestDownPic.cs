﻿using System;
using UnityEngine;
using System.Collections;
using NsHttpClient;



public class TestDownPic : MonoBehaviour {
	#if _USE_NGUI
    private Texture2D m_Tex = null;

    void OnDestroy() {
        ClearTex();
    }

    private void ClearTex() {
        UITexture t = GetComponent<UITexture>();
        if (t != null)
            t.mainTexture = null;
        if (m_Tex != null) {
            GameObject.Destroy(m_Tex);
            m_Tex = null;
        }
    }

    void OnHttpEnd(HttpClient client, HttpListenerStatus status) {
        if (status == HttpListenerStatus.hsDone) {
            // 已经读完
            ClearTex();
            HttpClientPicResponse pic = client.Listener as HttpClientPicResponse;
            if (pic != null) {
                m_Tex = pic.GeneratorTexture();
                UITexture t = GetComponent<UITexture>();
                if (t != null) {
                    t.mainTexture = m_Tex;
                    if (m_Tex != null) {
                        t.width = m_Tex.width;
                        t.height = m_Tex.height;
                    }
                }
            }
        }
    }

    private void OnHttpProcess(HttpClient client) {
        if (m_Label == null)
            m_Label = GetComponentInChildren<UILabel>();
        if (m_Label != null) {
            var rep = client.Listener as HttpClientResponse;
            float process = rep.DownProcess;
            m_Label.text = process.ToString("F2");
        }
    }

    private UILabel m_Label = null;

	public void StartHttp() {
        ClearTex();
        string url = "https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo/bd_logo1_31bdc765.png";
        //url = string.Format("{0}?time={1}", url, DateTime.Now.Ticks.ToString());
		var client = HttpHelper.OpenUrl<HttpClientPicResponse>(url, new HttpClientPicResponse(270, 129, false, 4 * 1024), OnHttpEnd, OnHttpProcess);
    }
#endif
}


