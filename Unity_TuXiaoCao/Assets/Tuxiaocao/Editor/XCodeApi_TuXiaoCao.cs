using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;
using UnityEngine;

namespace yoyohan.tuxiaocao
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2020-03-26 11:36:39
    /// </summary>
    public class XCodeApi_TuXiaoCao
    {

        [PostProcessBuild(2)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            // 只处理IOS工程， pathToBuildProject会传入导出的ios工程的根目录
            if (buildTarget != BuildTarget.iOS)
                return;

            // 创建工程设置对象
            var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject project = new PBXProject();
            project.ReadFromFile(projectPath);
            string targetGuid = project.TargetGuidByName("Unity-iPhone");


            #region 添加lib
            AddLibToProject(project, targetGuid, "WebKit.framework");
            #endregion

            File.WriteAllText(projectPath, project.WriteToString());


            #region 删掉plist的UIApplicationExitsOnSuspend
            //删掉info.plist的UIApplicationExitsOnSuspend 因为该属性苹果不再使用 在上传appstore报错
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;
            rootDict.values.Remove("UIApplicationExitsOnSuspend");
            #endregion


            plist.WriteToFile(plistPath);
        }


        static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }


    }
}
