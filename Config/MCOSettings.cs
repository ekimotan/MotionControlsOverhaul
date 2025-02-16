using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base;
using System.Xml.Serialization;
using MCM.Common;
using System.Runtime.CompilerServices;
using MotionControlsOverhaul.Behaviours;

namespace MotionControlsOverhaul.Config
{
    public class MCOSettings : AttributeGlobalSettings<MCOSettings>
    {


        public override string Id => "MCOSettings_v1";
        public override string DisplayName => "Motion Controls Overhaul Settings";
        public override string FolderName => "MotionControlsOverhaul";
        public override string FormatType => "json2";



        [SettingPropertyBool("Disable Gyro on Touch", Order = 0, RequireRestart = false, HintText = "Use this feature for gyro ratcheting.\nWhen enabled touching the touchpad will disable gyro")]
        [SettingPropertyGroup("General Settings")]
        public bool TouchDisable { get; set; } = true;

        [SettingPropertyFloatingInteger("Gyro Sensitivity", 0.1f, 10.0f, Order = 1, RequireRestart = false, HintText = "Adjust the sensitivity.\nYour mouse sensitivity also effects this setting")]
        [SettingPropertyGroup("General Settings")]
        public float GyroSensitivity { get; set; } = 3.5f;


        [SettingPropertyBool("Enable Deadzone", Order = 3, RequireRestart = false, HintText = "Enable or disable the feature.")]
        [SettingPropertyGroup("General Settings/Deadzone")]
        public bool EnableDeadzone { get; set; } = true;

        [SettingPropertyFloatingInteger("Gyro Deadzone", 0.1f, 5f, Order = 4, RequireRestart = false, HintText = "If gyro movement is lower than this value it will be disregarded.\nFor shaky hands.")]
        [SettingPropertyGroup("General Settings/Deadzone")]
        public float GyroDeadzone { get; set; } = 0.7f;

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            GyroPatch.gyroSensitivity = GyroSensitivity;
            GyroPatch.touchDisable = TouchDisable;
            GyroPatch.enableDeadzone = EnableDeadzone;
            GyroPatch.gyroDeadzone = GyroDeadzone;

        }

        //[settingpropertydropdown("select option")]
        //[settingpropertygroup("general settings")]
        //public dropdown<string> options { get; set; } = new dropdown<string>(new[] { "option 1", "option 2", "option 3" }, 0);

        //[settingpropertyinteger("some value", 1, 100, requirerestart = false, hinttext = "set a value between 1 and 100.")]
        //[settingpropertygroup("general settings")]
        //public int somevalue { get; set; } = 50;
    }
}
