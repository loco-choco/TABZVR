using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace TABZVR
{
	public static class WeaponHandlerPatchs
	{

		public static void ApplyPatchs(Harmony harmonyInstance)
		{
			MethodBase MoveArmsIdleMethod = AccessTools.Method(typeof(WeaponHandler), "MoveArmsIdle");
			MethodBase MoveWeaponMethod = AccessTools.Method(typeof(WeaponHandler), "MoveWeapon");

			harmonyInstance.Patch(MoveArmsIdleMethod , prefix: new HarmonyMethod(typeof(WeaponHandlerPatchs).GetMethod(nameof(MoveArmsIdlePrefix))));
			harmonyInstance.Patch(MoveWeaponMethod, prefix: new HarmonyMethod(typeof(WeaponHandlerPatchs).GetMethod(nameof(MoveWeaponPrefix))));

		}

		static FieldInfo WeaponHandlerPunchTime = AccessTools.Field(typeof(WeaponHandler), "punchTime");
		static FieldInfo WeaponHandlerHitCdRight = AccessTools.Field(typeof(WeaponHandler), "hitCdRight");
		static FieldInfo WeaponHandlerHitCdLeft = AccessTools.Field(typeof(WeaponHandler), "hitCdLeft");


		static FieldInfo WeaponHandlerRightArm = AccessTools.Field(typeof(WeaponHandler), "rightArm");
		static FieldInfo WeaponHandlerLeftArm = AccessTools.Field(typeof(WeaponHandler), "leftArm");


		static FieldInfo WeaponHandlerWeapon = AccessTools.Field(typeof(WeaponHandler), "weapon");
		static FieldInfo WeaponHandlerWeaponRotationForce = AccessTools.Field(typeof(WeaponHandler), "weaponRotationForce");
		static FieldInfo WeaponHandlerSinceShotMultiplier = AccessTools.Field(typeof(WeaponHandler), "sinceShotMultiplier");
		static FieldInfo WeaponHandlerConfigJoint2 = AccessTools.Field(typeof(WeaponHandler), "configJoint2");

		static MethodInfo WeaponHandlerMoveRig = AccessTools.Method(typeof(WeaponHandler), "MoveRig");
		
		public static bool MoveArmsIdlePrefix(WeaponHandler __instance)
		{
			float punchTime = (float)WeaponHandlerPunchTime.GetValue(__instance);
			float hitCdRight = (float)WeaponHandlerHitCdRight.GetValue(__instance);
			float hitCdLeft = (float)WeaponHandlerHitCdLeft.GetValue(__instance);

			Rigidbody rightArm = (Rigidbody)WeaponHandlerRightArm.GetValue(__instance);
			Rigidbody leftArm = (Rigidbody)WeaponHandlerLeftArm.GetValue(__instance);

			if (hitCdRight > punchTime && ModInnit.rightHand != null)
			{
				WeaponHandlerMoveRig.Invoke(__instance, new object[] { rightArm, ModInnit.rightHand, 5f, 200f, true, false });
			}
			if (hitCdLeft > punchTime && ModInnit.leftHand != null)
			{
				WeaponHandlerMoveRig.Invoke(__instance, new object[] { leftArm, ModInnit.leftHand, 5f, 200f, true, false });
			}

			return true;
		}

        public static bool MoveWeaponPrefix(WeaponHandler __instance)
        {
            Rigidbody weapon = (Rigidbody)WeaponHandlerWeapon.GetValue(__instance);
            float weaponRotationForce = (float)WeaponHandlerWeaponRotationForce.GetValue(__instance);
            float sinceShotMultiplier = (float)WeaponHandlerSinceShotMultiplier.GetValue(__instance);
			Rigidbody rightArm = (Rigidbody)WeaponHandlerRightArm.GetValue(__instance);
			Rigidbody leftArm = (Rigidbody)WeaponHandlerLeftArm.GetValue(__instance);


			ConfigurableJoint configJoint2 = (ConfigurableJoint)WeaponHandlerConfigJoint2.GetValue(__instance);

			if (ModInnit.rightHand != null)
            {
				float d = Vector3.Angle(weapon.transform.up, ModInnit.rightHand.up);
                Vector3 a = Vector3.Cross(weapon.transform.up, ModInnit.rightHand.up);
                weapon.AddTorque(a * d * weaponRotationForce * 0.0166f * sinceShotMultiplier, ForceMode.VelocityChange);

				Vector3 forward = weapon.transform.forward;
				float d2 = Vector3.Angle(forward, ModInnit.rightHand.forward);
				Vector3 a3 = Vector3.Cross(forward, ModInnit.rightHand.forward);
				weapon.AddTorque(a3 * d2 * 200f * Time.deltaTime * sinceShotMultiplier, ForceMode.VelocityChange);

				Vector3 a2 = ModInnit.rightHand.position - rightArm.position;
				weapon.AddForceAtPosition(a2 * 10f, rightArm.position, ForceMode.VelocityChange);
				//WeaponHandlerMoveRig.Invoke(__instance, new object[] { weapon, ModInnit.rightHand, 5f, 200f, false, false });
            }
			if (ModInnit.leftHand != null && configJoint2 != null)
			{
				float d = Vector3.Angle(weapon.transform.up, ModInnit.leftHand.up);
				Vector3 a = Vector3.Cross(weapon.transform.up, ModInnit.leftHand.up);
				weapon.AddTorque(a * d * weaponRotationForce * 0.0166f * sinceShotMultiplier, ForceMode.VelocityChange);


				Vector3 forward = weapon.transform.forward;
				float d2 = Vector3.Angle(forward, ModInnit.leftHand.forward);
				Vector3 a3 = Vector3.Cross(forward, ModInnit.leftHand.forward);
				weapon.AddTorque(a3 * d2 * 200f * Time.deltaTime * sinceShotMultiplier, ForceMode.VelocityChange);

				Vector3 a2 = ModInnit.leftHand.position - leftArm.position;
				weapon.AddForceAtPosition(a2 * 10f, leftArm.position, ForceMode.VelocityChange);
			}
			return true;
        }
    }
}