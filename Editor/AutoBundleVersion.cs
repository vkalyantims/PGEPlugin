using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Editor
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;

    // Any class in an Editor folder that implements IPreprocessBuildWithReport
    // will have its OnPreprocessBuild called before a build begins.
    public class AutoBundleVersion : IPreprocessBuildWithReport
    {
        // Call this preprocessor first.
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // 1) Figure out the Pacific Time zone object (Windows vs. macOS/Linux)
            TimeZoneInfo pacificZone;
            try
            {
                // Windows ID
                pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback for macOS/Linux
                pacificZone = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
            }

            // 2) Take UTC now and convert to Pacific
            DateTime utcNow = DateTime.UtcNow;
            DateTime pacificNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, pacificZone);

            // 3) Format as "yyMMdd.HHmm" (e.g. "250606.1432")
            string newVersion = pacificNow.ToString("yyMMdd.HHmm");

            // 4) Assign to bundleVersion
            PlayerSettings.bundleVersion = newVersion;

            Debug.Log($"[AutoBundleVersion] bundleVersion set to {newVersion} (Pacific Time: {pacificNow:yyyy-MM-dd HH:mm})");
        }
    }
}
