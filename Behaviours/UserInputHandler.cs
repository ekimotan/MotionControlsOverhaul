/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000032 RID: 50
	[GameStateScreen(typeof(MissionState))]
	public partial class MissionScreen : ScreenBase, IMissionSystemHandler, IGameStateListener, IMissionScreen, IMissionListener
	{
		// Token: 0x06000232 RID: 562 RVA: 0x00013DD4 File Offset: 0x00011FD4
		private void HandleUserInput(float dt)
		{
			bool flag = false;
			bool flag2 = this._isGamepadActive && this.PhotoModeRequiresMouse;
			if (this.Mission == null || this.Mission.CurrentState == Mission.State.EndingNextFrame)
			{
				return;
			}
			if (!flag && Game.Current.CheatMode)
			{
				flag = this.HandleUserInputCheatMode(dt);
			}
			if (flag)
			{
				return;
			}
			float num = this.SceneLayer.Input.GetMouseSensitivity();
			if (!this.MouseVisible && this.Mission.MainAgent != null && this.Mission.MainAgent.State == AgentState.Active && this.Mission.MainAgent.IsLookRotationInSlowMotion)
			{
				num *= ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ReducedMouseSensitivityMultiplier);
			}
			float num2 = dt / 0.0009f;
			float num3 = dt / 0.0009f;
			float num4 = 0f;
			float num5 = 0f;
			if ((!MBCommon.IsPaused || this.IsPhotoModeEnabled) && !this.IsRadialMenuActive && this._cameraSpecialTargetFOV > 9f && this.Mission.Mode != MissionMode.Barter)
			{
				if (this.MouseVisible && !this.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton))
				{
					if (this.Mission.Mode != MissionMode.Conversation)
					{
						if (this.Mission.Mode == MissionMode.Deployment)
						{
							num4 = num2 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX");
							num5 = -num3 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY");
						}
						else
						{
							if (this.SceneLayer.Input.GetMousePositionRanged().x <= 0.01f)
							{
								num4 = -400f * dt;
							}
							else if (this.SceneLayer.Input.GetMousePositionRanged().x >= 0.99f)
							{
								num4 = 400f * dt;
							}
							if (this.SceneLayer.Input.GetMousePositionRanged().y <= 0.01f)
							{
								num5 = -400f * dt;
							}
							else if (this.SceneLayer.Input.GetMousePositionRanged().y >= 0.99f)
							{
								num5 = 400f * dt;
							}
						}
					}
				}
				else if (!this.SceneLayer.Input.GetIsMouseActive())
				{
					float gameKeyAxis = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX");
					float gameKeyAxis2 = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY");
					if (gameKeyAxis > 0.9f || gameKeyAxis < -0.9f)
					{
						num2 = dt / 0.00045f;
					}
					if (gameKeyAxis2 > 0.9f || gameKeyAxis2 < -0.9f)
					{
						num3 = dt / 0.00045f;
					}
					if (this._zoomToggled)
					{
						num2 *= BannerlordConfig.ZoomSensitivityModifier;
						num3 *= BannerlordConfig.ZoomSensitivityModifier;
					}
					num4 = num2 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX") + this.SceneLayer.Input.GetMouseMoveX();
					num5 = -num3 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY") + this.SceneLayer.Input.GetMouseMoveY();
					if (this._missionMainAgentController.IsPlayerAiming && NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableGyroAssistedAim) == 1f)
					{
						float config = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.GyroAimSensitivity);
						float gyroX = Input.GetGyroX();
						Input.GetGyroY();
						float gyroZ = Input.GetGyroZ();
						num4 += config * gyroZ * 12f * -1f;
						num5 += config * gyroX * 12f * -1f;
					}
				}
				else
				{
					num4 = this.SceneLayer.Input.GetMouseMoveX();
					num5 = this.SceneLayer.Input.GetMouseMoveY();
					if (this._zoomAmount > 0.66f)
					{
						num4 *= BannerlordConfig.ZoomSensitivityModifier * this._zoomAmount;
						num5 *= BannerlordConfig.ZoomSensitivityModifier * this._zoomAmount;
					}
				}
			}
			if (NativeConfig.EnableEditMode && base.DebugInput.IsHotKeyPressed("MissionScreenHotkeySwitchCameraSmooth"))
			{
				this._cameraSmoothMode = !this._cameraSmoothMode;
				if (this._cameraSmoothMode)
				{
					MessageManager.DisplayMessage("Camera smooth mode Enabled.", uint.MaxValue);
				}
				else
				{
					MessageManager.DisplayMessage("Camera smooth mode Disabled.", uint.MaxValue);
				}
			}
			float num6 = 0.0035f;
			float num8;
			if (this._cameraSmoothMode)
			{
				num6 *= 0.02f;
				float num7 = 0.02f + dt - 8f * (dt * dt);
				num8 = MathF.Max(0f, 1f - 2f * num7);
			}
			else
			{
				num8 = 0f;
			}
			this._cameraBearingDelta *= num8;
			this._cameraElevationDelta *= num8;
			bool isSessionActive = GameNetwork.IsSessionActive;
			float num9 = num6 * num;
			float num10 = -num4 * num9;
			float num11 = (NativeConfig.InvertMouse ? num5 : (-num5)) * num9;
			if (isSessionActive)
			{
				float num12 = 0.3f + 10f * dt;
				num10 = MBMath.ClampFloat(num10, -num12, num12);
				num11 = MBMath.ClampFloat(num11, -num12, num12);
			}
			this._cameraBearingDelta += num10;
			this._cameraElevationDelta += num11;
			if (isSessionActive)
			{
				float num13 = 0.3f + 10f * dt;
				this._cameraBearingDelta = MBMath.ClampFloat(this._cameraBearingDelta, -num13, num13);
				this._cameraElevationDelta = MBMath.ClampFloat(this._cameraElevationDelta, -num13, num13);
			}
			Agent agentToFollow = this.GetSpectatingData(this.CombatCamera.Frame.origin).AgentToFollow;
			if (this.Mission.CameraIsFirstPerson && agentToFollow != null && agentToFollow.Controller == Agent.ControllerType.Player && agentToFollow.HasMount && ((ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson) == 1f && !agentToFollow.WieldedWeapon.IsEmpty && agentToFollow.WieldedWeapon.CurrentUsageItem.IsRangedWeapon) || (ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson) == 2f && (agentToFollow.WieldedWeapon.IsEmpty || agentToFollow.WieldedWeapon.CurrentUsageItem.IsMeleeWeapon)) || ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson) == 3f))
			{
				this._cameraBearingDelta += agentToFollow.MountAgent.GetTurnSpeed() * dt;
			}
			if (this.InputManager.IsGameKeyDown(28))
			{
				Mission.CameraAddedDistance -= 2.1f * dt;
			}
			if (this.InputManager.IsGameKeyDown(29))
			{
				Mission.CameraAddedDistance += 2.1f * dt;
			}
			Mission.CameraAddedDistance = MBMath.ClampFloat(Mission.CameraAddedDistance, 0.7f, 2.4f);
			this._isGamepadActive = (!Input.IsMouseActive && Input.IsControllerConnected);
			if (this._isGamepadActive)
			{
				Agent mainAgent = this.Mission.MainAgent;
				bool flag3;
				if (mainAgent == null)
				{
					flag3 = false;
				}
				else
				{
					WeaponComponentData currentUsageItem = mainAgent.WieldedWeapon.CurrentUsageItem;
					bool? flag4 = (currentUsageItem != null) ? new bool?(currentUsageItem.IsRangedWeapon) : null;
					bool flag5 = true;
					flag3 = (flag4.GetValueOrDefault() == flag5 & flag4 != null);
				}
				if (!flag3)
				{
					goto IL_6F1;
				}
			}
			bool flag6;
			if (this.CustomCamera == null)
			{
				flag6 = !this.IsRadialMenuActive;
				goto IL_6F2;
			}
			IL_6F1:
			flag6 = false;
			IL_6F2:
			bool flag7 = flag6 || this._forceCanZoom;
			if (flag7)
			{
				float applicationTime = Time.ApplicationTime;
				if (this.SceneLayer.Input.IsHotKeyPressed("ToggleZoom"))
				{
					this._zoomToggleTime = applicationTime;
				}
				if (applicationTime - this._zoomToggleTime > 0.01f && this.SceneLayer.Input.IsHotKeyDown("ToggleZoom"))
				{
					this._zoomToggleTime = float.MaxValue;
					this._zoomToggled = !this._zoomToggled;
				}
			}
			else
			{
				this._zoomToggled = false;
			}
			bool photoModeOrbit = this.Mission.Scene.GetPhotoModeOrbit();
			if (this.IsPhotoModeEnabled)
			{
				if (photoModeOrbit && !flag2)
				{
					this._zoomAmount -= this.SceneLayer.Input.GetDeltaMouseScroll() * 0.002f;
					this._zoomAmount = MBMath.ClampFloat(this._zoomAmount, 0f, 50f);
				}
			}
			else
			{
				if (agentToFollow != null && agentToFollow.IsMine && (this._zoomToggled || (flag7 && this.SceneLayer.Input.IsGameKeyDown(24))))
				{
					this._zoomAmount += 5f * dt;
				}
				else
				{
					this._zoomAmount -= 5f * dt;
				}
				this._zoomAmount = MBMath.ClampFloat(this._zoomAmount, 0f, 1f);
			}
			if (!this.IsPhotoModeEnabled)
			{
				if (this._zoomAmount.ApproximatelyEqualsTo(1f, 1E-05f))
				{
					this.Mission.Scene.SetDepthOfFieldParameters(this._zoomAmount * 160f * 110f, this._zoomAmount * 1500f * 0.3f, false);
				}
				else
				{
					this.Mission.Scene.SetDepthOfFieldParameters(0f, 0f, false);
				}
			}
			float depthOfFieldFocus;
			this.Mission.Scene.RayCastForClosestEntityOrTerrain(this.CombatCamera.Position + this.CombatCamera.Direction * this._cameraRayCastOffset, this.CombatCamera.Position + this.CombatCamera.Direction * 3000f, out depthOfFieldFocus, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags);
			this.Mission.Scene.SetDepthOfFieldFocus(depthOfFieldFocus);
			Agent mainAgent2 = this.Mission.MainAgent;
			if (mainAgent2 != null && !this.IsPhotoModeEnabled)
			{
				if (this._isPlayerAgentAdded)
				{
					this._isPlayerAgentAdded = false;
					if (this.Mission.Mode != MissionMode.Deployment)
					{
						this.CameraBearing = (this.Mission.CameraIsFirstPerson ? mainAgent2.LookDirection.RotationZ : mainAgent2.MovementDirectionAsAngle);
						this.CameraElevation = (this.Mission.CameraIsFirstPerson ? mainAgent2.LookDirection.RotationX : 0f);
						this._cameraSpecialTargetAddedBearing = 0f;
						this._cameraSpecialTargetAddedElevation = 0f;
						this._cameraSpecialCurrentAddedBearing = 0f;
						this._cameraSpecialCurrentAddedElevation = 0f;
					}
				}
				if (this.Mission.ClearSceneTimerElapsedTime >= 0f)
				{
					bool flag8;
					if (!this.IsViewingCharacter() && this.Mission.Mode != MissionMode.Conversation && this.Mission.Mode != MissionMode.Barter && !mainAgent2.IsLookDirectionLocked)
					{
						MissionMainAgentController missionMainAgentController = this._missionMainAgentController;
						flag8 = (((missionMainAgentController != null) ? missionMainAgentController.LockedAgent : null) == null);
					}
					else
					{
						flag8 = false;
					}
					if (!flag8)
					{
						if (this.Mission.Mode != MissionMode.Barter)
						{
							if (this._missionMainAgentController.LockedAgent != null)
							{
								this.CameraBearing = mainAgent2.LookDirection.RotationZ;
								this.CameraElevation = mainAgent2.LookDirection.RotationX;
							}
							else
							{
								this._cameraSpecialTargetAddedBearing = MBMath.WrapAngle(this._cameraSpecialTargetAddedBearing + this._cameraBearingDelta);
								this._cameraSpecialTargetAddedElevation = MBMath.WrapAngle(this._cameraSpecialTargetAddedElevation + this._cameraElevationDelta);
								this._cameraSpecialCurrentAddedBearing = MBMath.WrapAngle(this._cameraSpecialCurrentAddedBearing + this._cameraBearingDelta);
								this._cameraSpecialCurrentAddedElevation = MBMath.WrapAngle(this._cameraSpecialCurrentAddedElevation + this._cameraElevationDelta);
							}
						}
						float num14 = this.CameraElevation + this._cameraSpecialTargetAddedElevation;
						num14 = MBMath.ClampFloat(num14, -1.3659099f, 1.1219975f);
						this._cameraSpecialTargetAddedElevation = num14 - this.CameraElevation;
						num14 = this.CameraElevation + this._cameraSpecialCurrentAddedElevation;
						num14 = MBMath.ClampFloat(num14, -1.3659099f, 1.1219975f);
						this._cameraSpecialCurrentAddedElevation = num14 - this.CameraElevation;
					}
					else
					{
						this._cameraSpecialTargetAddedBearing = 0f;
						this._cameraSpecialTargetAddedElevation = 0f;
						if (this.Mission.CameraIsFirstPerson && agentToFollow != null && agentToFollow == this.Mission.MainAgent && !this.IsPhotoModeEnabled && !agentToFollow.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_lock_camera) && !agentToFollow.GetCurrentAnimationFlag(1).HasAnyFlag(AnimFlags.anf_lock_camera))
						{
							if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
							{
								MissionMainAgentController missionMainAgentController2 = this._missionMainAgentController;
								if (((missionMainAgentController2 != null) ? missionMainAgentController2.InteractionComponent.CurrentFocusedObject : null) != null && this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent)
								{
									goto IL_D60;
								}
							}
							if (this._missionMainAgentController == null || !this._missionMainAgentController.CustomLookDir.IsNonZero)
							{
								float num15 = MBMath.WrapAngle(this.CameraBearing + this._cameraBearingDelta);
								float num16 = MBMath.WrapAngle(this.CameraElevation + this._cameraElevationDelta);
								float num17;
								float num18;
								this.CalculateNewBearingAndElevationForFirstPerson(agentToFollow, num15, num16, out num17, out num18);
								if (num17 != num15)
								{
									this._cameraBearingDelta = (MBMath.IsBetween(MBMath.WrapAngle(this._cameraBearingDelta), 0f, 3.1415927f) ? MBMath.ClampFloat(MBMath.WrapAngle(num17 - this.CameraBearing), 0f, this._cameraBearingDelta) : MBMath.ClampFloat(MBMath.WrapAngle(num17 - this.CameraBearing), this._cameraBearingDelta, 0f));
								}
								if (num18 != num16)
								{
									this._cameraElevationDelta = (MBMath.IsBetween(MBMath.WrapAngle(this._cameraElevationDelta), 0f, 3.1415927f) ? MBMath.ClampFloat(MBMath.WrapAngle(num18 - this.CameraElevation), 0f, this._cameraElevationDelta) : MBMath.ClampFloat(MBMath.WrapAngle(num18 - this.CameraElevation), this._cameraElevationDelta, 0f));
								}
							}
						}
						IL_D60:
						this.CameraBearing += this._cameraBearingDelta;
						this.CameraElevation += this._cameraElevationDelta;
						this.CameraElevation = MBMath.ClampFloat(this.CameraElevation, -1.3659099f, 1.1219975f);
					}
					if (this.LockCameraMovement)
					{
						this._cameraToggleStartTime = float.MaxValue;
						return;
					}
					if (!Input.IsMouseActive)
					{
						float applicationTime2 = Time.ApplicationTime;
						if (this.SceneLayer.Input.IsGameKeyPressed(27))
						{
							if (this.SceneLayer.Input.GetGameKeyAxis("MovementAxisX") <= 0.1f && this.SceneLayer.Input.GetGameKeyAxis("MovementAxisY") <= 0.1f)
							{
								this._cameraToggleStartTime = applicationTime2;
							}
						}
						else if (!this.SceneLayer.Input.IsGameKeyDown(27))
						{
							this._cameraToggleStartTime = float.MaxValue;
						}
						if (this.GetCameraToggleProgress() >= 1f)
						{
							this._cameraToggleStartTime = float.MaxValue;
							this.Mission.CameraIsFirstPerson = !this.Mission.CameraIsFirstPerson;
							this._cameraApplySpecialMovementsInstantly = true;
							return;
						}
					}
					else if (this.SceneLayer.Input.IsGameKeyPressed(27))
					{
						this.Mission.CameraIsFirstPerson = !this.Mission.CameraIsFirstPerson;
						this._cameraApplySpecialMovementsInstantly = true;
						return;
					}
				}
			}
			else
			{
				if (this.IsPhotoModeEnabled && this.Mission.CameraIsFirstPerson)
				{
					this.Mission.CameraIsFirstPerson = false;
				}
				this.CameraBearing += this._cameraBearingDelta;
				this.CameraElevation += this._cameraElevationDelta;
				this.CameraElevation = MBMath.ClampFloat(this.CameraElevation, -1.3659099f, 1.1219975f);
			}
		}
	}
}
GameKeyDefinition
*/