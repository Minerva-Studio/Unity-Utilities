﻿using System;
using UnityEngine;

namespace Minerva.Module.Editor
{
    public readonly struct GUIScrollView : IDisposable
    {
        public GUIScrollView(ref Vector2 position, params GUILayoutOption[] options)
        {
            position = GUILayout.BeginScrollView(position, options);
        }

        public GUIScrollView(ref Vector2 position, GUIStyle style, params GUILayoutOption[] options)
        {
            position = GUILayout.BeginScrollView(position, style, options);
        }

        public readonly void Dispose()
        {
            GUILayout.EndScrollView();
        }
    }
}