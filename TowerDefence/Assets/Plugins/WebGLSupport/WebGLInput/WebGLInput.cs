﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace WebGLSupport
{
    class WebGLInputPlugin
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern int WebGLInputCreate(int x, int y, int width, int height, int fontsize, string text, bool isMultiLine);

        [DllImport("__Internal")]
        public static extern void WebGLInputEnterSubmit(int id, bool flag);

        [DllImport("__Internal")]
        public static extern void WebGLInputFocus(int id);

        [DllImport("__Internal")]
        public static extern void WebGLInputOnFocus(int id, Action<int> cb);

        [DllImport("__Internal")]
        public static extern void WebGLInputOnBlur(int id, Action<int> cb);

        [DllImport("__Internal")]
        public static extern void WebGLInputOnValueChange(int id, Action<int, string> cb);
        
        [DllImport("__Internal")]
        public static extern void WebGLInputOnEditEnd(int id, Action<int, string> cb);

        [DllImport("__Internal")]
        public static extern int WebGLInputSelectionStart(int id);

        [DllImport("__Internal")]
        public static extern int WebGLInputSelectionEnd(int id);

        [DllImport("__Internal")]
        public static extern int WebGLInputSelectionDirection(int id);

        [DllImport("__Internal")]
        public static extern void WebGLInputSetSelectionRange(int id, int start, int end);

        [DllImport("__Internal")]
        public static extern void WebGLInputMaxLength(int id, int maxlength);

        [DllImport("__Internal")]
        public static extern void WebGLInputText(int id, string text);

        [DllImport("__Internal")]
        public static extern void WebGLInputDelete(int id);
#else
        public static int WebGLInputCreate(int x, int y, int width, int height, int fontsize, string text, bool isMultiLine) { return 0; }
        public static void WebGLInputEnterSubmit(int id, bool flag) { }
        public static void WebGLInputFocus(int id) { }
        public static void WebGLInputOnFocus(int id, Action<int> cb) { }
        public static void WebGLInputOnBlur(int id, Action<int> cb) { }
        public static void WebGLInputOnValueChange(int id, Action<int, string> cb) { }
        public static void WebGLInputOnEditEnd(int id, Action<int, string> cb) { }
        public static int WebGLInputSelectionStart(int id) { return 0; }
        public static int WebGLInputSelectionEnd(int id) { return 0; }
        public static int WebGLInputSelectionDirection(int id) { return 0; }
        public static void WebGLInputSetSelectionRange(int id, int start, int end) { }
        public static void WebGLInputMaxLength(int id, int maxlength) { }
        public static void WebGLInputText(int id, string text) { }
        public static void WebGLInputDelete(int id) { }
#endif
    }

    [RequireComponent(typeof(InputField))]
    public class WebGLInput : MonoBehaviour
    {
        static Dictionary<int, InputField> instances = new Dictionary<int, InputField>();

        int id = -1;
        InputField input;

        private void Start()
        {
            input = GetComponent<InputField>();
#if !(UNITY_WEBGL && !UNITY_EDITOR)
            // WebGL 以外、更新メソッドは動作しないようにします
            enabled = false;
#endif
        }
        /// <summary>
        /// 対象が選択されたとき
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect(/*BaseEventData eventData*/)
        {
            var rect = GetScreenCoordinates(input.textComponent.GetComponent<RectTransform>());

            var x = (int)(rect.x);
            //var y = (int)(Screen.height - (rect.y + rect.height));
            //id = WebGLInputPlugin.WebGLInputCreate(x, y, (int)rect.width, (int)rect.height, input.textComponent.fontSize, input.text);
            var y = (int)(Screen.height - (rect.y));
            id = WebGLInputPlugin.WebGLInputCreate(x, y, (int)rect.width, (int)1, input.textComponent.fontSize, input.text, input.lineType != InputField.LineType.SingleLine);

            instances[id] = input;
            WebGLInputPlugin.WebGLInputEnterSubmit(id, input.lineType != InputField.LineType.MultiLineNewline);
            WebGLInputPlugin.WebGLInputOnFocus(id, OnFocus);
            WebGLInputPlugin.WebGLInputOnBlur(id, OnBlur);
            WebGLInputPlugin.WebGLInputOnValueChange(id, OnValueChange);
            WebGLInputPlugin.WebGLInputOnEditEnd(id, OnEditEnd);
            // default value : https://www.w3schools.com/tags/att_input_maxlength.asp
            WebGLInputPlugin.WebGLInputMaxLength(id, (input.characterLimit > 0) ? input.characterLimit : 524288);
            WebGLInputPlugin.WebGLInputFocus(id);
        }

        /// <summary>
        /// 画面内の描画範囲を取得する
        /// </summary>
        /// <param name="uiElement"></param>
        /// <returns></returns>
        Rect GetScreenCoordinates(RectTransform uiElement)
        {
            var worldCorners = new Vector3[4];
            uiElement.GetWorldCorners(worldCorners);
            return new Rect(worldCorners[0].x, worldCorners[0].y, worldCorners[2].x - worldCorners[0].x, worldCorners[2].y - worldCorners[0].y);
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        static void OnFocus(int id)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
#endif
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        static void OnBlur(int id)
        {
            WebGLInputPlugin.WebGLInputDelete(id);
            instances[id].DeactivateInputField();
            instances.Remove(id);
#if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.WebGLInput.captureAllKeyboardInput = true;
#endif
        }

        [MonoPInvokeCallback(typeof(Action<int, string>))]
        static void OnValueChange(int id, string value)
        {
            var input = instances[id];
            var index = input.caretPosition;
            input.text = value;

            // InputField.ContentType.Name が Name の場合、先頭文字が強制的大文字になるため小文字にして比べる
            if (input.contentType == InputField.ContentType.Name)
            {
                if (string.Compare(input.text, value, true) == 0)
                {
                    value = input.text;
                }
            }

            // InputField の ContentType による整形したテキストを HTML の input に再設定します
            if (value != input.text)
            {
                WebGLInputPlugin.WebGLInputText(id, input.text);
                WebGLInputPlugin.WebGLInputSetSelectionRange(id, index, index);
            }
        }
        [MonoPInvokeCallback(typeof(Action<int, string>))]
        static void OnEditEnd(int id, string value)
        {
            instances[id].text = value;
        }

        void Update()
        {
            if (!input.isFocused) return;
            // 未登録の場合、選択する
            if (!instances.ContainsKey(id))
            {
                OnSelect();
            }

            var start = WebGLInputPlugin.WebGLInputSelectionStart(id);
            var end = WebGLInputPlugin.WebGLInputSelectionEnd(id);
            // 選択方向によって設定します
            if (WebGLInputPlugin.WebGLInputSelectionDirection(id) == -1)
            {
                input.selectionFocusPosition = start;
                input.selectionAnchorPosition = end;
            }
            else
            {
                input.selectionFocusPosition = end;
                input.selectionAnchorPosition = start;
            }

            input.Rebuild(CanvasUpdate.LatePreRender);
            input.textComponent.SetAllDirty();
        }
    }
}
