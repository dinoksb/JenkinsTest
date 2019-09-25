﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using System.IO;
using System;

namespace JENKINS
{
    public class AutoBuilder : ScriptableObject
    {
        static string[] SCENES = FindEnabledEditorScenes();

        // Use real app name here
        /* Anyway the App will have the name as configured within the Unity-Editor
           This Appname is just for the Folder in which to Build */
        static string APP_NAME;
        static string TARGET_DIR;

        [MenuItem("Custom/CI/Windows Mixed Reality Build (UWP)")]
        public static void JenkinsAutoBuildTest()
        {
            APP_NAME = GetArg("-appName");
            TARGET_DIR = GetArg("-buildFolder");//////"D:\\UnityProject\\TestProject\\UnityJenkinsTest\\Build";
            Debug.Log("Jenkins-Build: APP_NAME: " + APP_NAME + " TARGET_DIR: " + TARGET_DIR);

            GenericBuild(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.None);
        }

        private static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }

            return EditorScenes.ToArray();
        }

        private static void GenericBuild(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
        {
            //EditorUserBuildSettings.SwitchActiveBuildTarget(build_target_group, BuildTarget.Android);
            PlayerSettings.keyaliasPass = GetArg("-keyaliasPass");
            PlayerSettings.keystorePass = GetArg("-keystorePass");

            Debug.LogFormat("**** keyaliasPass : {0}", PlayerSettings.keyaliasPass);
            Debug.LogFormat("**** keystorePass : {0}", PlayerSettings.keystorePass);

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;
            buildPlayerOptions.locationPathName = app_target;
            buildPlayerOptions.target = (BuildTarget)Enum.Parse(typeof(BuildTarget), GetArg("-buildTarget"));//BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;

            Debug.LogFormat("**** app_target : {0}", app_target);

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            Debug.LogFormat("**** player : {0}", report);

            var summary = report.summary;

            Debug.LogFormat("**** summary.result : {0}", summary.result);

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("**** Succeeded!");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log("**** Failed!");
                foreach (var step in report.steps)
                {
                    foreach (var message in step.messages)
                    {
                        Debug.Log("****" + message);
                    }
                }
            }
            else if (summary.result == BuildResult.Cancelled)
            {
                Debug.Log("**** Cancelled!");
            }
            else
            { // Unknown
                Debug.Log("**** Unknown!");
            }
        }

        /**
         * Get Arguments from the command line by name
         */
        private static string GetArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }

            return null;
        }
    }
}

