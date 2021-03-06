﻿//#define SA_DEBUG_MODE
////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using System;
using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSSocialManager : ISN_Singleton<IOSSocialManager> {

	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _ISN_TwPost(string text, string url, string encodedMedia);

	[DllImport ("__Internal")]
	private static extern void _ISN_FbPost(string text, string url, string encodedMedia);

	[DllImport ("__Internal")]
	private static extern void _ISN_MediaShare(string text, string encodedMedia);

	[DllImport ("__Internal")]
	private static extern void _ISN_SendMail(string subject, string body,  string recipients, string encodedMedia);


	#endif
	


	//Actions
	public static event Action<ISN_Result> OnFacebookPostResult;
	public static event Action<ISN_Result> OnTwitterPostResult;
	public static event Action<ISN_Result> OnMailResult;

	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void ShareMedia(string text) {
		ShareMedia(text, null);
	}

	public void ShareMedia(string text, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			if(texture != null) {
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);
				_ISN_MediaShare(text, bytesString);
			} else {
				_ISN_MediaShare(text, "");
			}
		#endif
	}

	public void TwitterPost(string text, string url = null, Texture2D texture = null) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		if(text == null) {
			text = "";
		}

		if(url == null) {
			url = "";
		}

		string encodedMedia = "";

		if(texture != null) {
			byte[] val = texture.EncodeToPNG();
			encodedMedia = System.Convert.ToBase64String (val);
		}


		_ISN_TwPost(text, url, encodedMedia);
		#endif
	}



	public void FacebookPost(string text, string url = null, Texture2D texture = null) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		if(text == null) {
			text = "";
		}
		
		if(url == null) {
			url = "";
		}
		
		string encodedMedia = "";
		
		if(texture != null) {
			byte[] val = texture.EncodeToPNG();
			encodedMedia = System.Convert.ToBase64String (val);
		}
		
		
		_ISN_FbPost(text, url, encodedMedia);
		#endif
	}


	public void SendMail(string subject, string body, string recipients) {
		SendMail(subject, body, recipients, null);
	}
	
	public void SendMail(string subject, string body, string recipients, Texture2D texture) {
		if(texture == null) {
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISN_SendMail(subject, body, recipients, "");
			#endif
		} else {
			
			
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			byte[] val = texture.EncodeToPNG();
			string bytesString = System.Convert.ToBase64String (val);
			_ISN_SendMail(subject, body, recipients, bytesString);
			#endif
		}
	}

	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnTwitterPostFailed() {

		if(OnTwitterPostResult != null) {
			ISN_Result result = new ISN_Result(false);
			OnTwitterPostResult(result);
		}
	}

	private void OnTwitterPostSuccess() {

		if(OnTwitterPostResult != null) {
			ISN_Result result = new ISN_Result(true);
			OnTwitterPostResult(result);
		}

	}

	private void OnFacebookPostFailed() {

		if(OnFacebookPostResult != null) {
			ISN_Result result = new ISN_Result(false);
			OnFacebookPostResult(result);
		}
	}
	
	private void OnFacebookPostSuccess() {

		if(OnFacebookPostResult != null) {
			ISN_Result result = new ISN_Result(true);
			OnFacebookPostResult(result);
		}
	}

	private void OnMailFailed() {

		if(OnMailResult != null) {
			ISN_Result result = new ISN_Result(false);
			OnMailResult(result);
		}
	}

	private void OnMailSuccess() {

		if(OnMailResult != null) {
			ISN_Result result = new ISN_Result(true);
			OnMailResult(result);
		}
	}


	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------


	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
