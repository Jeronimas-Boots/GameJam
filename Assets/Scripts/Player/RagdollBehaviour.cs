using UnityEngine;
using System.Collections.Generic;

/// <summary>
///     JointModeSetting 
/// </summary>
[System.Serializable]
public class JointModeSetting
{
    public float angularXDriveDamping;
    public float angularXDriveSpring;
    public float angularYZDriveDamping;
    public float angularYZDriveSpring;
}

[System.Serializable]
public class JointMode
{
    public string modeName;
    public JointModeSetting setting;
}

/// <summary>
/// Ragdoll
/// </summary>
[System.Serializable]
public class JointSettings
{
    public ConfigurableJoint jointReference;
    public string modeName;
}
[System.Serializable]
public class RagdollSetting
{
    public List<JointSettings> jointSettings;
}




public class RagdollBehaviour : MonoBehaviour
{
    public Dictionary<string, JointModeSetting> jointModes2;
    public List<JointMode> jointModes;
    public List<RagdollSetting> ragdollModes;
    

    public int GetRagdollModes()
    {
        return ragdollModes.Count;
    }
    public void ChangeRagdollMode(int index)
    {
        foreach (JointSettings jointSetting in ragdollModes[index].jointSettings)
        {
            JointDrive driveX = jointSetting.jointReference.angularXDrive;

            var JointMode = jointModes.Find(mode => mode.modeName == jointSetting.modeName);

            driveX.positionDamper = JointMode.setting.angularXDriveDamping;
            driveX.positionSpring = JointMode.setting.angularXDriveSpring;
            jointSetting.jointReference.angularXDrive = driveX;

            JointDrive driveYZ = jointSetting.jointReference.angularYZDrive;

            driveYZ.positionDamper = JointMode.setting.angularYZDriveDamping;
            driveYZ.positionSpring = JointMode.setting.angularYZDriveSpring;
            jointSetting.jointReference.angularYZDrive = driveYZ;
        }
        
    }
}
