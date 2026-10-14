var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i782 = root || request.c( 'UnityEngine.JointSpring' )
  var i783 = data
  i782.spring = i783[0]
  i782.damper = i783[1]
  i782.targetPosition = i783[2]
  return i782
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i784 = root || request.c( 'UnityEngine.JointMotor' )
  var i785 = data
  i784.m_TargetVelocity = i785[0]
  i784.m_Force = i785[1]
  i784.m_FreeSpin = i785[2]
  return i784
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i786 = root || request.c( 'UnityEngine.JointLimits' )
  var i787 = data
  i786.m_Min = i787[0]
  i786.m_Max = i787[1]
  i786.m_Bounciness = i787[2]
  i786.m_BounceMinVelocity = i787[3]
  i786.m_ContactDistance = i787[4]
  i786.minBounce = i787[5]
  i786.maxBounce = i787[6]
  return i786
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i788 = root || request.c( 'UnityEngine.JointDrive' )
  var i789 = data
  i788.m_PositionSpring = i789[0]
  i788.m_PositionDamper = i789[1]
  i788.m_MaximumForce = i789[2]
  i788.m_UseAcceleration = i789[3]
  return i788
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i790 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i791 = data
  i790.m_Spring = i791[0]
  i790.m_Damper = i791[1]
  return i790
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i792 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i793 = data
  i792.m_Limit = i793[0]
  i792.m_Bounciness = i793[1]
  i792.m_ContactDistance = i793[2]
  return i792
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i794 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i795 = data
  i794.m_ExtremumSlip = i795[0]
  i794.m_ExtremumValue = i795[1]
  i794.m_AsymptoteSlip = i795[2]
  i794.m_AsymptoteValue = i795[3]
  i794.m_Stiffness = i795[4]
  return i794
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i796 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i797 = data
  i796.m_LowerAngle = i797[0]
  i796.m_UpperAngle = i797[1]
  return i796
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i798 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i799 = data
  i798.m_MotorSpeed = i799[0]
  i798.m_MaximumMotorTorque = i799[1]
  return i798
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i800 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i801 = data
  i800.m_DampingRatio = i801[0]
  i800.m_Frequency = i801[1]
  i800.m_Angle = i801[2]
  return i800
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i802 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i803 = data
  i802.m_LowerTranslation = i803[0]
  i802.m_UpperTranslation = i803[1]
  return i802
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i804 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i805 = data
  i804.position = new pc.Vec3( i805[0], i805[1], i805[2] )
  i804.scale = new pc.Vec3( i805[3], i805[4], i805[5] )
  i804.rotation = new pc.Quat(i805[6], i805[7], i805[8], i805[9])
  return i804
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i806 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i807 = data
  i806.name = i807[0]
  i806.tagId = i807[1]
  i806.enabled = !!i807[2]
  i806.isStatic = !!i807[3]
  i806.layer = i807[4]
  return i806
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshFilter"] = function (request, data, root) {
  var i808 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshFilter' )
  var i809 = data
  request.r(i809[0], i809[1], 0, i808, 'sharedMesh')
  return i808
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshRenderer"] = function (request, data, root) {
  var i810 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshRenderer' )
  var i811 = data
  request.r(i811[0], i811[1], 0, i810, 'additionalVertexStreams')
  i810.enabled = !!i811[2]
  request.r(i811[3], i811[4], 0, i810, 'sharedMaterial')
  var i813 = i811[5]
  var i812 = []
  for(var i = 0; i < i813.length; i += 2) {
  request.r(i813[i + 0], i813[i + 1], 2, i812, '')
  }
  i810.sharedMaterials = i812
  i810.receiveShadows = !!i811[6]
  i810.shadowCastingMode = i811[7]
  i810.sortingLayerID = i811[8]
  i810.sortingOrder = i811[9]
  i810.lightmapIndex = i811[10]
  i810.lightmapSceneIndex = i811[11]
  i810.lightmapScaleOffset = new pc.Vec4( i811[12], i811[13], i811[14], i811[15] )
  i810.lightProbeUsage = i811[16]
  i810.reflectionProbeUsage = i811[17]
  return i810
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshCollider"] = function (request, data, root) {
  var i816 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshCollider' )
  var i817 = data
  request.r(i817[0], i817[1], 0, i816, 'sharedMesh')
  i816.convex = !!i817[2]
  i816.enabled = !!i817[3]
  i816.isTrigger = !!i817[4]
  request.r(i817[5], i817[6], 0, i816, 'material')
  return i816
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.BoxCollider"] = function (request, data, root) {
  var i818 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.BoxCollider' )
  var i819 = data
  i818.center = new pc.Vec3( i819[0], i819[1], i819[2] )
  i818.size = new pc.Vec3( i819[3], i819[4], i819[5] )
  i818.enabled = !!i819[6]
  i818.isTrigger = !!i819[7]
  request.r(i819[8], i819[9], 0, i818, 'material')
  return i818
}

Deserializers["Code.Gameplay.Coloring.ColorField"] = function (request, data, root) {
  var i820 = root || request.c( 'Code.Gameplay.Coloring.ColorField' )
  var i821 = data
  var i823 = i821[0]
  var i822 = []
  for(var i = 0; i < i823.length; i += 4) {
    i822.push( new pc.Color(i823[i + 0], i823[i + 1], i823[i + 2], i823[i + 3]) );
  }
  i820._botColors = i822
  request.r(i821[1], i821[2], 0, i820, '_paintMaterial')
  return i820
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh"] = function (request, data, root) {
  var i826 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh' )
  var i827 = data
  i826.name = i827[0]
  i826.halfPrecision = !!i827[1]
  i826.useSimplification = !!i827[2]
  i826.useUInt32IndexFormat = !!i827[3]
  i826.vertexCount = i827[4]
  i826.aabb = i827[5]
  var i829 = i827[6]
  var i828 = []
  for(var i = 0; i < i829.length; i += 1) {
    i828.push( !!i829[i + 0] );
  }
  i826.streams = i828
  i826.vertices = i827[7]
  var i831 = i827[8]
  var i830 = []
  for(var i = 0; i < i831.length; i += 1) {
    i830.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh', i831[i + 0]) );
  }
  i826.subMeshes = i830
  var i833 = i827[9]
  var i832 = []
  for(var i = 0; i < i833.length; i += 16) {
    i832.push( new pc.Mat4().setData(i833[i + 0], i833[i + 1], i833[i + 2], i833[i + 3],  i833[i + 4], i833[i + 5], i833[i + 6], i833[i + 7],  i833[i + 8], i833[i + 9], i833[i + 10], i833[i + 11],  i833[i + 12], i833[i + 13], i833[i + 14], i833[i + 15]) );
  }
  i826.bindposes = i832
  var i835 = i827[10]
  var i834 = []
  for(var i = 0; i < i835.length; i += 1) {
    i834.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape', i835[i + 0]) );
  }
  i826.blendShapes = i834
  return i826
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh"] = function (request, data, root) {
  var i840 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh' )
  var i841 = data
  i840.triangles = i841[0]
  return i840
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape"] = function (request, data, root) {
  var i846 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape' )
  var i847 = data
  i846.name = i847[0]
  var i849 = i847[1]
  var i848 = []
  for(var i = 0; i < i849.length; i += 1) {
    i848.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame', i849[i + 0]) );
  }
  i846.frames = i848
  return i846
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i850 = root || new pc.UnityMaterial()
  var i851 = data
  i850.name = i851[0]
  request.r(i851[1], i851[2], 0, i850, 'shader')
  i850.renderQueue = i851[3]
  i850.enableInstancing = !!i851[4]
  var i853 = i851[5]
  var i852 = []
  for(var i = 0; i < i853.length; i += 1) {
    i852.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i853[i + 0]) );
  }
  i850.floatParameters = i852
  var i855 = i851[6]
  var i854 = []
  for(var i = 0; i < i855.length; i += 1) {
    i854.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i855[i + 0]) );
  }
  i850.colorParameters = i854
  var i857 = i851[7]
  var i856 = []
  for(var i = 0; i < i857.length; i += 1) {
    i856.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i857[i + 0]) );
  }
  i850.vectorParameters = i856
  var i859 = i851[8]
  var i858 = []
  for(var i = 0; i < i859.length; i += 1) {
    i858.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i859[i + 0]) );
  }
  i850.textureParameters = i858
  var i861 = i851[9]
  var i860 = []
  for(var i = 0; i < i861.length; i += 1) {
    i860.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i861[i + 0]) );
  }
  i850.materialFlags = i860
  return i850
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i864 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i865 = data
  i864.name = i865[0]
  i864.value = i865[1]
  return i864
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i868 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i869 = data
  i868.name = i869[0]
  i868.value = new pc.Color(i869[1], i869[2], i869[3], i869[4])
  return i868
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i872 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i873 = data
  i872.name = i873[0]
  i872.value = new pc.Vec4( i873[1], i873[2], i873[3], i873[4] )
  return i872
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i876 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i877 = data
  i876.name = i877[0]
  request.r(i877[1], i877[2], 0, i876, 'value')
  return i876
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i880 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i881 = data
  i880.name = i881[0]
  i880.enabled = !!i881[1]
  return i880
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i882 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i883 = data
  i882.name = i883[0]
  i882.width = i883[1]
  i882.height = i883[2]
  i882.mipmapCount = i883[3]
  i882.anisoLevel = i883[4]
  i882.filterMode = i883[5]
  i882.hdr = !!i883[6]
  i882.format = i883[7]
  i882.wrapMode = i883[8]
  i882.alphaIsTransparency = !!i883[9]
  i882.alphaSource = i883[10]
  i882.graphicsFormat = i883[11]
  i882.sRGBTexture = !!i883[12]
  i882.desiredColorSpace = i883[13]
  i882.wrapU = i883[14]
  i882.wrapV = i883[15]
  return i882
}

Deserializers["Code.Gameplay.AI.GameAIContext"] = function (request, data, root) {
  var i884 = root || request.c( 'Code.Gameplay.AI.GameAIContext' )
  var i885 = data
  return i884
}

Deserializers["Code.Gameplay.AI.Services.BotFieldMap"] = function (request, data, root) {
  var i886 = root || request.c( 'Code.Gameplay.AI.Services.BotFieldMap' )
  var i887 = data
  request.r(i887[0], i887[1], 0, i886, '_fieldRoot')
  var i889 = i887[2]
  var i888 = []
  for(var i = 0; i < i889.length; i += 1) {
    i888.push( i889[i + 0] );
  }
  i886._obstaclePrefixes = i888
  i886._cellSize = i887[3]
  i886._agentRadius = i887[4]
  return i886
}

Deserializers["Code.Gameplay.AI.Waypoints.FieldPatrolWaypoint"] = function (request, data, root) {
  var i892 = root || request.c( 'Code.Gameplay.AI.Waypoints.FieldPatrolWaypoint' )
  var i893 = data
  request.r(i893[0], i893[1], 0, i892, '_fieldMap')
  return i892
}

Deserializers["Code.Gameplay.AI.Services.AISpawnController"] = function (request, data, root) {
  var i894 = root || request.c( 'Code.Gameplay.AI.Services.AISpawnController' )
  var i895 = data
  var i897 = i895[0]
  var i896 = new (System.Collections.Generic.List$1(Bridge.ns('Code.Gameplay.AI.Services.AISpawnPoint')))
  for(var i = 0; i < i897.length; i += 2) {
  request.r(i897[i + 0], i897[i + 1], 1, i896, '')
  }
  i894._spawnPoints = i896
  i894._spawnDelay = i895[1]
  i894._spawnOnStart = !!i895[2]
  return i894
}

Deserializers["Code.Gameplay.AI.Services.AISpawnPoint"] = function (request, data, root) {
  var i900 = root || request.c( 'Code.Gameplay.AI.Services.AISpawnPoint' )
  var i901 = data
  request.r(i901[0], i901[1], 0, i900, '_spawnPoint')
  request.r(i901[2], i901[3], 0, i900, '_prefab')
  i900._shouldReplaceAIBrain = !!i901[4]
  request.r(i901[5], i901[6], 0, i900, '_aiBrainProvider')
  return i900
}

Deserializers["Code.Gameplay.AI.Brains.Commands.CommandAIBrainProvider"] = function (request, data, root) {
  var i902 = root || request.c( 'Code.Gameplay.AI.Brains.Commands.CommandAIBrainProvider' )
  var i903 = data
  request.r(i903[0], i903[1], 0, i902, '_waypoint')
  return i902
}

Deserializers["Code.Gameplay.Common.Behaviours.TransformPoint"] = function (request, data, root) {
  var i904 = root || request.c( 'Code.Gameplay.Common.Behaviours.TransformPoint' )
  var i905 = data
  return i904
}

Deserializers["Code.Gameplay.AI.Waypoints.WaitWaypoint"] = function (request, data, root) {
  var i906 = root || request.c( 'Code.Gameplay.AI.Waypoints.WaitWaypoint' )
  var i907 = data
  i906._minTime = i907[0]
  i906._maxTime = i907[1]
  return i906
}

Deserializers["Code.Gameplay.AI.Waypoints.RouteWaypoint"] = function (request, data, root) {
  var i908 = root || request.c( 'Code.Gameplay.AI.Waypoints.RouteWaypoint' )
  var i909 = data
  var i911 = i909[0]
  var i910 = new (System.Collections.Generic.List$1(Bridge.ns('Code.Gameplay.AI.Waypoints.RouteCommandPoint')))
  for(var i = 0; i < i911.length; i += 1) {
    i910.add(request.d('Code.Gameplay.AI.Waypoints.RouteCommandPoint', i911[i + 0]));
  }
  i908._points = i910
  i908._maxDrawDistance = i909[1]
  return i908
}

Deserializers["Code.Gameplay.AI.Waypoints.RouteCommandPoint"] = function (request, data, root) {
  var i914 = root || request.c( 'Code.Gameplay.AI.Waypoints.RouteCommandPoint' )
  var i915 = data
  i914._localPosition = new pc.Vec3( i915[0], i915[1], i915[2] )
  var i917 = i915[3]
  var i916 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Vector3')))
  for(var i = 0; i < i917.length; i += 3) {
    i916.add(new pc.Vec3( i917[i + 0], i917[i + 1], i917[i + 2] ));
  }
  i914._variantLocalPositions = i916
  i914._allowNoise = !!i915[4]
  return i914
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Animator"] = function (request, data, root) {
  var i920 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Animator' )
  var i921 = data
  request.r(i921[0], i921[1], 0, i920, 'animatorController')
  request.r(i921[2], i921[3], 0, i920, 'avatar')
  i920.updateMode = i921[4]
  i920.hasTransformHierarchy = !!i921[5]
  i920.applyRootMotion = !!i921[6]
  var i923 = i921[7]
  var i922 = []
  for(var i = 0; i < i923.length; i += 2) {
  request.r(i923[i + 0], i923[i + 1], 2, i922, '')
  }
  i920.humanBones = i922
  i920.enabled = !!i921[8]
  return i920
}

Deserializers["Code.Gameplay.AI.Behaviours.AICharacter"] = function (request, data, root) {
  var i926 = root || request.c( 'Code.Gameplay.AI.Behaviours.AICharacter' )
  var i927 = data
  request.r(i927[0], i927[1], 0, i926, '_animator')
  i926._moveSpeed = i927[2]
  i926._rotationSpeed = i927[3]
  i926._jumpArcHeight = i927[4]
  i926._noiseAmplitude = i927[5]
  i926._noiseFrequency = i927[6]
  i926._maxLaneOffset = i927[7]
  return i926
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CapsuleCollider"] = function (request, data, root) {
  var i928 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CapsuleCollider' )
  var i929 = data
  i928.center = new pc.Vec3( i929[0], i929[1], i929[2] )
  i928.radius = i929[3]
  i928.height = i929[4]
  i928.direction = i929[5]
  i928.enabled = !!i929[6]
  i928.isTrigger = !!i929[7]
  request.r(i929[8], i929[9], 0, i928, 'material')
  return i928
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Rigidbody"] = function (request, data, root) {
  var i930 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Rigidbody' )
  var i931 = data
  i930.mass = i931[0]
  i930.drag = i931[1]
  i930.angularDrag = i931[2]
  i930.useGravity = !!i931[3]
  i930.isKinematic = !!i931[4]
  i930.constraints = i931[5]
  i930.maxAngularVelocity = i931[6]
  i930.collisionDetectionMode = i931[7]
  i930.interpolation = i931[8]
  return i930
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer"] = function (request, data, root) {
  var i932 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer' )
  var i933 = data
  request.r(i933[0], i933[1], 0, i932, 'sharedMesh')
  var i935 = i933[2]
  var i934 = []
  for(var i = 0; i < i935.length; i += 2) {
  request.r(i935[i + 0], i935[i + 1], 2, i934, '')
  }
  i932.bones = i934
  i932.updateWhenOffscreen = !!i933[3]
  i932.localBounds = i933[4]
  request.r(i933[5], i933[6], 0, i932, 'rootBone')
  var i937 = i933[7]
  var i936 = []
  for(var i = 0; i < i937.length; i += 1) {
    i936.push( request.d('Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight', i937[i + 0]) );
  }
  i932.blendShapesWeights = i936
  i932.enabled = !!i933[8]
  request.r(i933[9], i933[10], 0, i932, 'sharedMaterial')
  var i939 = i933[11]
  var i938 = []
  for(var i = 0; i < i939.length; i += 2) {
  request.r(i939[i + 0], i939[i + 1], 2, i938, '')
  }
  i932.sharedMaterials = i938
  i932.receiveShadows = !!i933[12]
  i932.shadowCastingMode = i933[13]
  i932.sortingLayerID = i933[14]
  i932.sortingOrder = i933[15]
  i932.lightmapIndex = i933[16]
  i932.lightmapSceneIndex = i933[17]
  i932.lightmapScaleOffset = new pc.Vec4( i933[18], i933[19], i933[20], i933[21] )
  i932.lightProbeUsage = i933[22]
  i932.reflectionProbeUsage = i933[23]
  return i932
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight"] = function (request, data, root) {
  var i942 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight' )
  var i943 = data
  i942.weight = i943[0]
  return i942
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.RectTransform"] = function (request, data, root) {
  var i944 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.RectTransform' )
  var i945 = data
  i944.pivot = new pc.Vec2( i945[0], i945[1] )
  i944.anchorMin = new pc.Vec2( i945[2], i945[3] )
  i944.anchorMax = new pc.Vec2( i945[4], i945[5] )
  i944.sizeDelta = new pc.Vec2( i945[6], i945[7] )
  i944.anchoredPosition3D = new pc.Vec3( i945[8], i945[9], i945[10] )
  i944.rotation = new pc.Quat(i945[11], i945[12], i945[13], i945[14])
  i944.scale = new pc.Vec3( i945[15], i945[16], i945[17] )
  return i944
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Canvas"] = function (request, data, root) {
  var i946 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Canvas' )
  var i947 = data
  i946.planeDistance = i947[0]
  i946.referencePixelsPerUnit = i947[1]
  i946.isFallbackOverlay = !!i947[2]
  i946.renderMode = i947[3]
  i946.renderOrder = i947[4]
  i946.sortingLayerName = i947[5]
  i946.sortingOrder = i947[6]
  i946.scaleFactor = i947[7]
  request.r(i947[8], i947[9], 0, i946, 'worldCamera')
  i946.overrideSorting = !!i947[10]
  i946.pixelPerfect = !!i947[11]
  i946.targetDisplay = i947[12]
  i946.overridePixelPerfect = !!i947[13]
  i946.enabled = !!i947[14]
  return i946
}

Deserializers["UnityEngine.UI.CanvasScaler"] = function (request, data, root) {
  var i948 = root || request.c( 'UnityEngine.UI.CanvasScaler' )
  var i949 = data
  i948.m_UiScaleMode = i949[0]
  i948.m_ReferencePixelsPerUnit = i949[1]
  i948.m_ScaleFactor = i949[2]
  i948.m_ReferenceResolution = new pc.Vec2( i949[3], i949[4] )
  i948.m_ScreenMatchMode = i949[5]
  i948.m_MatchWidthOrHeight = i949[6]
  i948.m_PhysicalUnit = i949[7]
  i948.m_FallbackScreenDPI = i949[8]
  i948.m_DefaultSpriteDPI = i949[9]
  i948.m_DynamicPixelsPerUnit = i949[10]
  i948.m_PresetInfoIsWorld = !!i949[11]
  return i948
}

Deserializers["UnityEngine.UI.GraphicRaycaster"] = function (request, data, root) {
  var i950 = root || request.c( 'UnityEngine.UI.GraphicRaycaster' )
  var i951 = data
  i950.m_IgnoreReversedGraphics = !!i951[0]
  i950.m_BlockingObjects = i951[1]
  i950.m_BlockingMask = UnityEngine.LayerMask.FromIntegerValue( i951[2] )
  return i950
}

Deserializers["PackshotView"] = function (request, data, root) {
  var i952 = root || request.c( 'PackshotView' )
  var i953 = data
  request.r(i953[0], i953[1], 0, i952, '_downloadButton')
  return i952
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer"] = function (request, data, root) {
  var i954 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer' )
  var i955 = data
  i954.cullTransparentMesh = !!i955[0]
  return i954
}

Deserializers["UnityEngine.UI.Image"] = function (request, data, root) {
  var i956 = root || request.c( 'UnityEngine.UI.Image' )
  var i957 = data
  request.r(i957[0], i957[1], 0, i956, 'm_Sprite')
  i956.m_Type = i957[2]
  i956.m_PreserveAspect = !!i957[3]
  i956.m_FillCenter = !!i957[4]
  i956.m_FillMethod = i957[5]
  i956.m_FillAmount = i957[6]
  i956.m_FillClockwise = !!i957[7]
  i956.m_FillOrigin = i957[8]
  i956.m_UseSpriteMesh = !!i957[9]
  i956.m_PixelsPerUnitMultiplier = i957[10]
  request.r(i957[11], i957[12], 0, i956, 'm_Material')
  i956.m_Maskable = !!i957[13]
  i956.m_Color = new pc.Color(i957[14], i957[15], i957[16], i957[17])
  i956.m_RaycastTarget = !!i957[18]
  i956.m_RaycastPadding = new pc.Vec4( i957[19], i957[20], i957[21], i957[22] )
  return i956
}

Deserializers["UnityEngine.UI.AspectRatioFitter"] = function (request, data, root) {
  var i958 = root || request.c( 'UnityEngine.UI.AspectRatioFitter' )
  var i959 = data
  i958.m_AspectMode = i959[0]
  i958.m_AspectRatio = i959[1]
  return i958
}

Deserializers["ButtonPulseDotween"] = function (request, data, root) {
  var i960 = root || request.c( 'ButtonPulseDotween' )
  var i961 = data
  i960._scaleCoefficient = i961[0]
  i960._duration = i961[1]
  i960._pauseBefore = i961[2]
  i960._pauseAfter = i961[3]
  i960._fadeDuration = i961[4]
  request.r(i961[5], i961[6], 0, i960, '_targetObject')
  i960._fromStart = !!i961[7]
  i960._isLoop = !!i961[8]
  i960._isNeedDeactivate = !!i961[9]
  return i960
}

Deserializers["UnityEngine.UI.Button"] = function (request, data, root) {
  var i962 = root || request.c( 'UnityEngine.UI.Button' )
  var i963 = data
  i962.m_OnClick = request.d('UnityEngine.UI.Button+ButtonClickedEvent', i963[0], i962.m_OnClick)
  i962.m_Navigation = request.d('UnityEngine.UI.Navigation', i963[1], i962.m_Navigation)
  i962.m_Transition = i963[2]
  i962.m_Colors = request.d('UnityEngine.UI.ColorBlock', i963[3], i962.m_Colors)
  i962.m_SpriteState = request.d('UnityEngine.UI.SpriteState', i963[4], i962.m_SpriteState)
  i962.m_AnimationTriggers = request.d('UnityEngine.UI.AnimationTriggers', i963[5], i962.m_AnimationTriggers)
  i962.m_Interactable = !!i963[6]
  request.r(i963[7], i963[8], 0, i962, 'm_TargetGraphic')
  return i962
}

Deserializers["UnityEngine.UI.Button+ButtonClickedEvent"] = function (request, data, root) {
  var i964 = root || request.c( 'UnityEngine.UI.Button+ButtonClickedEvent' )
  var i965 = data
  i964.m_PersistentCalls = request.d('UnityEngine.Events.PersistentCallGroup', i965[0], i964.m_PersistentCalls)
  return i964
}

Deserializers["UnityEngine.Events.PersistentCallGroup"] = function (request, data, root) {
  var i966 = root || request.c( 'UnityEngine.Events.PersistentCallGroup' )
  var i967 = data
  var i969 = i967[0]
  var i968 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Events.PersistentCall')))
  for(var i = 0; i < i969.length; i += 1) {
    i968.add(request.d('UnityEngine.Events.PersistentCall', i969[i + 0]));
  }
  i966.m_Calls = i968
  return i966
}

Deserializers["UnityEngine.Events.PersistentCall"] = function (request, data, root) {
  var i972 = root || request.c( 'UnityEngine.Events.PersistentCall' )
  var i973 = data
  request.r(i973[0], i973[1], 0, i972, 'm_Target')
  i972.m_TargetAssemblyTypeName = i973[2]
  i972.m_MethodName = i973[3]
  i972.m_Mode = i973[4]
  i972.m_Arguments = request.d('UnityEngine.Events.ArgumentCache', i973[5], i972.m_Arguments)
  i972.m_CallState = i973[6]
  return i972
}

Deserializers["UnityEngine.Events.ArgumentCache"] = function (request, data, root) {
  var i974 = root || request.c( 'UnityEngine.Events.ArgumentCache' )
  var i975 = data
  request.r(i975[0], i975[1], 0, i974, 'm_ObjectArgument')
  i974.m_ObjectArgumentAssemblyTypeName = i975[2]
  i974.m_IntArgument = i975[3]
  i974.m_FloatArgument = i975[4]
  i974.m_StringArgument = i975[5]
  i974.m_BoolArgument = !!i975[6]
  return i974
}

Deserializers["UnityEngine.UI.Navigation"] = function (request, data, root) {
  var i976 = root || request.c( 'UnityEngine.UI.Navigation' )
  var i977 = data
  i976.m_Mode = i977[0]
  i976.m_WrapAround = !!i977[1]
  request.r(i977[2], i977[3], 0, i976, 'm_SelectOnUp')
  request.r(i977[4], i977[5], 0, i976, 'm_SelectOnDown')
  request.r(i977[6], i977[7], 0, i976, 'm_SelectOnLeft')
  request.r(i977[8], i977[9], 0, i976, 'm_SelectOnRight')
  return i976
}

Deserializers["UnityEngine.UI.ColorBlock"] = function (request, data, root) {
  var i978 = root || request.c( 'UnityEngine.UI.ColorBlock' )
  var i979 = data
  i978.m_NormalColor = new pc.Color(i979[0], i979[1], i979[2], i979[3])
  i978.m_HighlightedColor = new pc.Color(i979[4], i979[5], i979[6], i979[7])
  i978.m_PressedColor = new pc.Color(i979[8], i979[9], i979[10], i979[11])
  i978.m_SelectedColor = new pc.Color(i979[12], i979[13], i979[14], i979[15])
  i978.m_DisabledColor = new pc.Color(i979[16], i979[17], i979[18], i979[19])
  i978.m_ColorMultiplier = i979[20]
  i978.m_FadeDuration = i979[21]
  return i978
}

Deserializers["UnityEngine.UI.SpriteState"] = function (request, data, root) {
  var i980 = root || request.c( 'UnityEngine.UI.SpriteState' )
  var i981 = data
  request.r(i981[0], i981[1], 0, i980, 'm_HighlightedSprite')
  request.r(i981[2], i981[3], 0, i980, 'm_PressedSprite')
  request.r(i981[4], i981[5], 0, i980, 'm_SelectedSprite')
  request.r(i981[6], i981[7], 0, i980, 'm_DisabledSprite')
  return i980
}

Deserializers["UnityEngine.UI.AnimationTriggers"] = function (request, data, root) {
  var i982 = root || request.c( 'UnityEngine.UI.AnimationTriggers' )
  var i983 = data
  i982.m_NormalTrigger = i983[0]
  i982.m_HighlightedTrigger = i983[1]
  i982.m_PressedTrigger = i983[2]
  i982.m_SelectedTrigger = i983[3]
  i982.m_DisabledTrigger = i983[4]
  return i982
}

Deserializers["DownloadManager"] = function (request, data, root) {
  var i984 = root || request.c( 'DownloadManager' )
  var i985 = data
  request.r(i985[0], i985[1], 0, i984, '_canvasSelector')
  return i984
}

Deserializers["CanvasSelector"] = function (request, data, root) {
  var i986 = root || request.c( 'CanvasSelector' )
  var i987 = data
  request.r(i987[0], i987[1], 0, i986, 'canvasLandscapeWide')
  request.r(i987[2], i987[3], 0, i986, 'canvas16x9')
  request.r(i987[4], i987[5], 0, i986, 'canvasPortraitWide')
  request.r(i987[6], i987[7], 0, i986, 'canvas9x16')
  i986.landscapeRange = i987[8]
  i986.portraitRange = i987[9]
  i986.autoUpdate = !!i987[10]
  request.r(i987[11], i987[12], 0, i986, '_activeCanvas')
  return i986
}

Deserializers["Code.Gameplay.PlayerLogic.Movement.GamePlayerMovement"] = function (request, data, root) {
  var i988 = root || request.c( 'Code.Gameplay.PlayerLogic.Movement.GamePlayerMovement' )
  var i989 = data
  i988._moveSpeed = i989[0]
  i988._rotationSpeed = i989[1]
  i988._jumpForce = i989[2]
  i988._jumpCooldown = i989[3]
  i988._groundCheckDistance = i989[4]
  i988._groundMask = UnityEngine.LayerMask.FromIntegerValue( i989[5] )
  i988._climbMask = UnityEngine.LayerMask.FromIntegerValue( i989[6] )
  i988._wallCheckDistance = i989[7]
  i988._jumpBlockTime = i989[8]
  i988._climbSpeed = i989[9]
  i988._climbHeadHeight = i989[10]
  request.r(i989[11], i989[12], 0, i988, '_jumpSound')
  return i988
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.AudioSource"] = function (request, data, root) {
  var i990 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.AudioSource' )
  var i991 = data
  request.r(i991[0], i991[1], 0, i990, 'clip')
  request.r(i991[2], i991[3], 0, i990, 'outputAudioMixerGroup')
  i990.playOnAwake = !!i991[4]
  i990.loop = !!i991[5]
  i990.time = i991[6]
  i990.volume = i991[7]
  i990.pitch = i991[8]
  i990.enabled = !!i991[9]
  return i990
}

Deserializers["Code.Gameplay.PlayerLogic.Behaviours.GamePlayerCharacter"] = function (request, data, root) {
  var i992 = root || request.c( 'Code.Gameplay.PlayerLogic.Behaviours.GamePlayerCharacter' )
  var i993 = data
  request.r(i993[0], i993[1], 0, i992, '_movement')
  return i992
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Cubemap"] = function (request, data, root) {
  var i994 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Cubemap' )
  var i995 = data
  i994.name = i995[0]
  i994.atlasId = i995[1]
  i994.mipmapCount = i995[2]
  i994.hdr = !!i995[3]
  i994.size = i995[4]
  i994.anisoLevel = i995[5]
  i994.filterMode = i995[6]
  var i997 = i995[7]
  var i996 = []
  for(var i = 0; i < i997.length; i += 4) {
    i996.push( UnityEngine.Rect.MinMaxRect(i997[i + 0], i997[i + 1], i997[i + 2], i997[i + 3]) );
  }
  i994.rects = i996
  i994.wrapU = i995[8]
  i994.wrapV = i995[9]
  return i994
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i1000 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i1001 = data
  i1000.name = i1001[0]
  i1000.index = i1001[1]
  i1000.startup = !!i1001[2]
  return i1000
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i1002 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i1003 = data
  i1002.aspect = i1003[0]
  i1002.orthographic = !!i1003[1]
  i1002.orthographicSize = i1003[2]
  i1002.backgroundColor = new pc.Color(i1003[3], i1003[4], i1003[5], i1003[6])
  i1002.nearClipPlane = i1003[7]
  i1002.farClipPlane = i1003[8]
  i1002.fieldOfView = i1003[9]
  i1002.depth = i1003[10]
  i1002.clearFlags = i1003[11]
  i1002.cullingMask = i1003[12]
  i1002.rect = i1003[13]
  request.r(i1003[14], i1003[15], 0, i1002, 'targetTexture')
  i1002.usePhysicalProperties = !!i1003[16]
  i1002.focalLength = i1003[17]
  i1002.sensorSize = new pc.Vec2( i1003[18], i1003[19] )
  i1002.lensShift = new pc.Vec2( i1003[20], i1003[21] )
  i1002.gateFit = i1003[22]
  i1002.commandBufferCount = i1003[23]
  i1002.cameraType = i1003[24]
  i1002.enabled = !!i1003[25]
  return i1002
}

Deserializers["Code.Gameplay.PlayerLogic.CameraControl.GamePlayerCamera"] = function (request, data, root) {
  var i1004 = root || request.c( 'Code.Gameplay.PlayerLogic.CameraControl.GamePlayerCamera' )
  var i1005 = data
  i1004._offset = new pc.Vec3( i1005[0], i1005[1], i1005[2] )
  i1004._lookAtOffset = new pc.Vec3( i1005[3], i1005[4], i1005[5] )
  i1004._angles = new pc.Vec3( i1005[6], i1005[7], i1005[8] )
  i1004._yawSensitivity = i1005[9]
  i1004._pitchSensitivity = i1005[10]
  i1004._minPitch = i1005[11]
  i1004._maxPitch = i1005[12]
  i1004._invertY = !!i1005[13]
  i1004._collisionRadius = i1005[14]
  i1004._returnSmoothTime = i1005[15]
  i1004._occludedFieldOfViewBoost = i1005[16]
  i1004._occludedNearClip = i1005[17]
  i1004._collisionMask = UnityEngine.LayerMask.FromIntegerValue( i1005[18] )
  return i1004
}

Deserializers["UnityEngine.Rendering.Universal.UniversalAdditionalCameraData"] = function (request, data, root) {
  var i1006 = root || request.c( 'UnityEngine.Rendering.Universal.UniversalAdditionalCameraData' )
  var i1007 = data
  i1006.m_RenderShadows = !!i1007[0]
  i1006.m_RequiresDepthTextureOption = i1007[1]
  i1006.m_RequiresOpaqueTextureOption = i1007[2]
  i1006.m_CameraType = i1007[3]
  var i1009 = i1007[4]
  var i1008 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Camera')))
  for(var i = 0; i < i1009.length; i += 2) {
  request.r(i1009[i + 0], i1009[i + 1], 1, i1008, '')
  }
  i1006.m_Cameras = i1008
  i1006.m_RendererIndex = i1007[5]
  i1006.m_VolumeLayerMask = UnityEngine.LayerMask.FromIntegerValue( i1007[6] )
  request.r(i1007[7], i1007[8], 0, i1006, 'm_VolumeTrigger')
  i1006.m_VolumeFrameworkUpdateModeOption = i1007[9]
  i1006.m_RenderPostProcessing = !!i1007[10]
  i1006.m_Antialiasing = i1007[11]
  i1006.m_AntialiasingQuality = i1007[12]
  i1006.m_StopNaN = !!i1007[13]
  i1006.m_Dithering = !!i1007[14]
  i1006.m_ClearDepth = !!i1007[15]
  i1006.m_AllowXRRendering = !!i1007[16]
  i1006.m_AllowHDROutput = !!i1007[17]
  i1006.m_UseScreenCoordOverride = !!i1007[18]
  i1006.m_ScreenSizeOverride = new pc.Vec4( i1007[19], i1007[20], i1007[21], i1007[22] )
  i1006.m_ScreenCoordScaleBias = new pc.Vec4( i1007[23], i1007[24], i1007[25], i1007[26] )
  i1006.m_RequiresDepthTexture = !!i1007[27]
  i1006.m_RequiresColorTexture = !!i1007[28]
  i1006.m_Version = i1007[29]
  i1006.m_TaaSettings = request.d('UnityEngine.Rendering.Universal.TemporalAA+Settings', i1007[30], i1006.m_TaaSettings)
  return i1006
}

Deserializers["UnityEngine.Rendering.Universal.TemporalAA+Settings"] = function (request, data, root) {
  var i1012 = root || request.c( 'UnityEngine.Rendering.Universal.TemporalAA+Settings' )
  var i1013 = data
  i1012.m_Quality = i1013[0]
  i1012.m_FrameInfluence = i1013[1]
  i1012.m_JitterScale = i1013[2]
  i1012.m_MipBias = i1013[3]
  i1012.m_VarianceClampScale = i1013[4]
  i1012.m_ContrastAdaptiveSharpening = i1013[5]
  return i1012
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Light"] = function (request, data, root) {
  var i1014 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Light' )
  var i1015 = data
  i1014.type = i1015[0]
  i1014.color = new pc.Color(i1015[1], i1015[2], i1015[3], i1015[4])
  i1014.cullingMask = i1015[5]
  i1014.intensity = i1015[6]
  i1014.range = i1015[7]
  i1014.spotAngle = i1015[8]
  i1014.shadows = i1015[9]
  i1014.shadowNormalBias = i1015[10]
  i1014.shadowBias = i1015[11]
  i1014.shadowStrength = i1015[12]
  i1014.shadowResolution = i1015[13]
  i1014.lightmapBakeType = i1015[14]
  i1014.renderMode = i1015[15]
  request.r(i1015[16], i1015[17], 0, i1014, 'cookie')
  i1014.cookieSize = i1015[18]
  i1014.shadowNearPlane = i1015[19]
  i1014.occlusionMaskChannel = i1015[20]
  i1014.isBaked = !!i1015[21]
  i1014.mixedLightingMode = i1015[22]
  i1014.enabled = !!i1015[23]
  return i1014
}

Deserializers["UnityEngine.Rendering.Universal.UniversalAdditionalLightData"] = function (request, data, root) {
  var i1016 = root || request.c( 'UnityEngine.Rendering.Universal.UniversalAdditionalLightData' )
  var i1017 = data
  i1016.m_Version = i1017[0]
  i1016.m_UsePipelineSettings = !!i1017[1]
  i1016.m_AdditionalLightsShadowResolutionTier = i1017[2]
  i1016.m_LightLayerMask = i1017[3]
  i1016.m_RenderingLayers = i1017[4]
  i1016.m_CustomShadowLayers = !!i1017[5]
  i1016.m_ShadowLayerMask = i1017[6]
  i1016.m_ShadowRenderingLayers = i1017[7]
  i1016.m_LightCookieSize = new pc.Vec2( i1017[8], i1017[9] )
  i1016.m_LightCookieOffset = new pc.Vec2( i1017[10], i1017[11] )
  i1016.m_SoftShadowQuality = i1017[12]
  return i1016
}

Deserializers["UnityEngine.EventSystems.EventSystem"] = function (request, data, root) {
  var i1018 = root || request.c( 'UnityEngine.EventSystems.EventSystem' )
  var i1019 = data
  request.r(i1019[0], i1019[1], 0, i1018, 'm_FirstSelected')
  i1018.m_sendNavigationEvents = !!i1019[2]
  i1018.m_DragThreshold = i1019[3]
  return i1018
}

Deserializers["UnityEngine.EventSystems.StandaloneInputModule"] = function (request, data, root) {
  var i1020 = root || request.c( 'UnityEngine.EventSystems.StandaloneInputModule' )
  var i1021 = data
  i1020.m_HorizontalAxis = i1021[0]
  i1020.m_VerticalAxis = i1021[1]
  i1020.m_SubmitButton = i1021[2]
  i1020.m_CancelButton = i1021[3]
  i1020.m_InputActionsPerSecond = i1021[4]
  i1020.m_RepeatDelay = i1021[5]
  i1020.m_ForceModuleActive = !!i1021[6]
  i1020.m_SendPointerHoverToParent = !!i1021[7]
  return i1020
}

Deserializers["Code.Gameplay.PlayerLogic.UI.GamePlayerHUD"] = function (request, data, root) {
  var i1022 = root || request.c( 'Code.Gameplay.PlayerLogic.UI.GamePlayerHUD' )
  var i1023 = data
  request.r(i1023[0], i1023[1], 0, i1022, '_joystick')
  request.r(i1023[2], i1023[3], 0, i1022, '_jumpButton')
  i1022._showControlsOnDesktop = !!i1023[4]
  return i1022
}

Deserializers["PlayerHUDView"] = function (request, data, root) {
  var i1024 = root || request.c( 'PlayerHUDView' )
  var i1025 = data
  request.r(i1025[0], i1025[1], 0, i1024, '_background')
  request.r(i1025[2], i1025[3], 0, i1024, '_joystickHandle')
  request.r(i1025[4], i1025[5], 0, i1024, '_finger')
  request.r(i1025[6], i1025[7], 0, i1024, '_joystick')
  request.r(i1025[8], i1025[9], 0, i1024, '_soundButton')
  request.r(i1025[10], i1025[11], 0, i1024, '_soundButtonImage')
  request.r(i1025[12], i1025[13], 0, i1024, '_topPanel')
  request.r(i1025[14], i1025[15], 0, i1024, '_timeText')
  request.r(i1025[16], i1025[17], 0, i1024, '_scoreText')
  request.r(i1025[18], i1025[19], 0, i1024, '_startGameTimeText')
  return i1024
}

Deserializers["UnityEngine.UI.Text"] = function (request, data, root) {
  var i1026 = root || request.c( 'UnityEngine.UI.Text' )
  var i1027 = data
  i1026.m_FontData = request.d('UnityEngine.UI.FontData', i1027[0], i1026.m_FontData)
  i1026.m_Text = i1027[1]
  request.r(i1027[2], i1027[3], 0, i1026, 'm_Material')
  i1026.m_Maskable = !!i1027[4]
  i1026.m_Color = new pc.Color(i1027[5], i1027[6], i1027[7], i1027[8])
  i1026.m_RaycastTarget = !!i1027[9]
  i1026.m_RaycastPadding = new pc.Vec4( i1027[10], i1027[11], i1027[12], i1027[13] )
  return i1026
}

Deserializers["UnityEngine.UI.FontData"] = function (request, data, root) {
  var i1028 = root || request.c( 'UnityEngine.UI.FontData' )
  var i1029 = data
  request.r(i1029[0], i1029[1], 0, i1028, 'm_Font')
  i1028.m_FontSize = i1029[2]
  i1028.m_FontStyle = i1029[3]
  i1028.m_BestFit = !!i1029[4]
  i1028.m_MinSize = i1029[5]
  i1028.m_MaxSize = i1029[6]
  i1028.m_Alignment = i1029[7]
  i1028.m_AlignByGeometry = !!i1029[8]
  i1028.m_RichText = !!i1029[9]
  i1028.m_HorizontalOverflow = i1029[10]
  i1028.m_VerticalOverflow = i1029[11]
  i1028.m_LineSpacing = i1029[12]
  return i1028
}

Deserializers["Code.Gameplay.Coloring.ColorScoreText"] = function (request, data, root) {
  var i1030 = root || request.c( 'Code.Gameplay.Coloring.ColorScoreText' )
  var i1031 = data
  request.r(i1031[0], i1031[1], 0, i1030, '_colorField')
  request.r(i1031[2], i1031[3], 0, i1030, '_scoreText')
  request.r(i1031[4], i1031[5], 0, i1030, '_scoreTextTmp')
  return i1030
}

Deserializers["Code.Gameplay.PlayerLogic.UI.GamePlayerFingerHint"] = function (request, data, root) {
  var i1032 = root || request.c( 'Code.Gameplay.PlayerLogic.UI.GamePlayerFingerHint' )
  var i1033 = data
  i1032._speed = i1033[0]
  i1032._width = i1033[1]
  i1032._height = i1033[2]
  i1032._offsetX = i1033[3]
  i1032._offsetY = i1033[4]
  return i1032
}

Deserializers["Code.Gameplay.PlayerLogic.UI.GamePlayerJumpButton"] = function (request, data, root) {
  var i1034 = root || request.c( 'Code.Gameplay.PlayerLogic.UI.GamePlayerJumpButton' )
  var i1035 = data
  return i1034
}

Deserializers["Code.Gameplay.PlayerLogic.GamePlayerContext"] = function (request, data, root) {
  var i1036 = root || request.c( 'Code.Gameplay.PlayerLogic.GamePlayerContext' )
  var i1037 = data
  return i1036
}

Deserializers["Code.Gameplay.PlayerLogic.Services.GamePlayerSpawnController"] = function (request, data, root) {
  var i1038 = root || request.c( 'Code.Gameplay.PlayerLogic.Services.GamePlayerSpawnController' )
  var i1039 = data
  request.r(i1039[0], i1039[1], 0, i1038, '_spawnPoint')
  i1038._spawnOnStart = !!i1039[2]
  return i1038
}

Deserializers["Code.Gameplay.PlayerLogic.Services.GamePlayerSpawnPoint"] = function (request, data, root) {
  var i1040 = root || request.c( 'Code.Gameplay.PlayerLogic.Services.GamePlayerSpawnPoint' )
  var i1041 = data
  request.r(i1041[0], i1041[1], 0, i1040, '_spawnPoint')
  request.r(i1041[2], i1041[3], 0, i1040, '_prefab')
  return i1040
}

Deserializers["PackshotController"] = function (request, data, root) {
  var i1042 = root || request.c( 'PackshotController' )
  var i1043 = data
  request.r(i1043[0], i1043[1], 0, i1042, '_tutorialManager')
  request.r(i1043[2], i1043[3], 0, i1042, '_packshot')
  i1042._activateTime = i1043[4]
  return i1042
}

Deserializers["StartController"] = function (request, data, root) {
  var i1044 = root || request.c( 'StartController' )
  var i1045 = data
  request.r(i1045[0], i1045[1], 0, i1044, '_canvasSelector')
  request.r(i1045[2], i1045[3], 0, i1044, '_gamePlayerJoystick')
  request.r(i1045[4], i1045[5], 0, i1044, '_playerHUDView')
  request.r(i1045[6], i1045[7], 0, i1044, '_timerText')
  var i1047 = i1045[8]
  var i1046 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Transform')))
  for(var i = 0; i < i1047.length; i += 2) {
  request.r(i1047[i + 0], i1047[i + 1], 1, i1046, '')
  }
  i1044._doors = i1046
  i1044._doorOpenSpeed = i1045[9]
  i1044._timeToStart = i1045[10]
  request.r(i1045[11], i1045[12], 0, i1044, '_packshot')
  request.r(i1045[13], i1045[14], 0, i1044, '_gameTimer')
  var i1049 = i1045[15]
  var i1048 = []
  for(var i = 0; i < i1049.length; i += 2) {
  request.r(i1049[i + 0], i1049[i + 1], 2, i1048, '')
  }
  i1044._bots = i1048
  return i1044
}

Deserializers["Code.Gameplay.PlayerLogic.UI.GamePlayerJoystick"] = function (request, data, root) {
  var i1054 = root || request.c( 'Code.Gameplay.PlayerLogic.UI.GamePlayerJoystick' )
  var i1055 = data
  request.r(i1055[0], i1055[1], 0, i1054, '_canvasSelector')
  request.r(i1055[2], i1055[3], 0, i1054, '_playerHUDView')
  request.r(i1055[4], i1055[5], 0, i1054, '_background')
  request.r(i1055[6], i1055[7], 0, i1054, '_handle')
  i1054._handleRange = i1055[8]
  request.r(i1055[9], i1055[10], 0, i1054, '_finger')
  i1054._timeActivate = i1055[11]
  return i1054
}

Deserializers["GameTimeController"] = function (request, data, root) {
  var i1056 = root || request.c( 'GameTimeController' )
  var i1057 = data
  request.r(i1057[0], i1057[1], 0, i1056, '_canvasSelector')
  request.r(i1057[2], i1057[3], 0, i1056, '_playerHUDView')
  i1056._timeToEnd = i1057[4]
  request.r(i1057[5], i1057[6], 0, i1056, '_packshot')
  return i1056
}

Deserializers["SoundUIButton"] = function (request, data, root) {
  var i1058 = root || request.c( 'SoundUIButton' )
  var i1059 = data
  request.r(i1059[0], i1059[1], 0, i1058, '_soundOnOffButton')
  var i1061 = i1059[2]
  var i1060 = []
  for(var i = 0; i < i1061.length; i += 2) {
  request.r(i1061[i + 0], i1061[i + 1], 2, i1060, '')
  }
  i1058._soundStateIcons = i1060
  request.r(i1059[3], i1059[4], 0, i1058, '_soundStateImage')
  request.r(i1059[5], i1059[6], 0, i1058, '_canvasSelector')
  return i1058
}

Deserializers["AudioController"] = function (request, data, root) {
  var i1064 = root || request.c( 'AudioController' )
  var i1065 = data
  request.r(i1065[0], i1065[1], 0, i1064, '_audioMixer')
  return i1064
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i1066 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i1067 = data
  i1066.ambientIntensity = i1067[0]
  i1066.reflectionIntensity = i1067[1]
  i1066.ambientMode = i1067[2]
  i1066.ambientLight = new pc.Color(i1067[3], i1067[4], i1067[5], i1067[6])
  i1066.ambientSkyColor = new pc.Color(i1067[7], i1067[8], i1067[9], i1067[10])
  i1066.ambientGroundColor = new pc.Color(i1067[11], i1067[12], i1067[13], i1067[14])
  i1066.ambientEquatorColor = new pc.Color(i1067[15], i1067[16], i1067[17], i1067[18])
  i1066.fogColor = new pc.Color(i1067[19], i1067[20], i1067[21], i1067[22])
  i1066.fogEndDistance = i1067[23]
  i1066.fogStartDistance = i1067[24]
  i1066.fogDensity = i1067[25]
  i1066.fog = !!i1067[26]
  request.r(i1067[27], i1067[28], 0, i1066, 'skybox')
  i1066.fogMode = i1067[29]
  var i1069 = i1067[30]
  var i1068 = []
  for(var i = 0; i < i1069.length; i += 1) {
    i1068.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i1069[i + 0]) );
  }
  i1066.lightmaps = i1068
  i1066.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i1067[31], i1066.lightProbes)
  i1066.lightmapsMode = i1067[32]
  i1066.mixedBakeMode = i1067[33]
  i1066.environmentLightingMode = i1067[34]
  i1066.ambientProbe = new pc.SphericalHarmonicsL2(i1067[35])
  request.r(i1067[36], i1067[37], 0, i1066, 'customReflection')
  request.r(i1067[38], i1067[39], 0, i1066, 'defaultReflection')
  i1066.defaultReflectionMode = i1067[40]
  i1066.defaultReflectionResolution = i1067[41]
  i1066.sunLightObjectId = i1067[42]
  i1066.pixelLightCount = i1067[43]
  i1066.defaultReflectionHDR = !!i1067[44]
  i1066.hasLightDataAsset = !!i1067[45]
  i1066.hasManualGenerate = !!i1067[46]
  return i1066
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i1072 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i1073 = data
  request.r(i1073[0], i1073[1], 0, i1072, 'lightmapColor')
  request.r(i1073[2], i1073[3], 0, i1072, 'lightmapDirection')
  request.r(i1073[4], i1073[5], 0, i1072, 'shadowMask')
  return i1072
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i1074 = root || new UnityEngine.LightProbes()
  var i1075 = data
  return i1074
}

Deserializers["CustomizationCanvasView"] = function (request, data, root) {
  var i1082 = root || request.c( 'CustomizationCanvasView' )
  var i1083 = data
  request.r(i1083[0], i1083[1], 0, i1082, '_soundButton')
  request.r(i1083[2], i1083[3], 0, i1082, '_soundImage')
  request.r(i1083[4], i1083[5], 0, i1082, '_characterSlot')
  i1082._isLandscape = !!i1083[6]
  var i1085 = i1083[7]
  var i1084 = new (System.Collections.Generic.List$1(Bridge.ns('ButtonPulseDotween')))
  for(var i = 0; i < i1085.length; i += 2) {
  request.r(i1085[i + 0], i1085[i + 1], 1, i1084, '')
  }
  i1082._tutorialStepsButtons = i1084
  request.r(i1083[8], i1083[9], 0, i1082, '_customizationScreenView')
  return i1082
}

Deserializers["UnityEngine.UI.LayoutElement"] = function (request, data, root) {
  var i1088 = root || request.c( 'UnityEngine.UI.LayoutElement' )
  var i1089 = data
  i1088.m_IgnoreLayout = !!i1089[0]
  i1088.m_MinWidth = i1089[1]
  i1088.m_MinHeight = i1089[2]
  i1088.m_PreferredWidth = i1089[3]
  i1088.m_PreferredHeight = i1089[4]
  i1088.m_FlexibleWidth = i1089[5]
  i1088.m_FlexibleHeight = i1089[6]
  i1088.m_LayoutPriority = i1089[7]
  return i1088
}

Deserializers["Source.Scripts.UI.Customization.Views.CustomizationScreenView"] = function (request, data, root) {
  var i1090 = root || request.c( 'Source.Scripts.UI.Customization.Views.CustomizationScreenView' )
  var i1091 = data
  request.r(i1091[0], i1091[1], 0, i1090, '_topPanel')
  request.r(i1091[2], i1091[3], 0, i1090, '_character')
  request.r(i1091[4], i1091[5], 0, i1090, '_activeTabSprite')
  request.r(i1091[6], i1091[7], 0, i1090, '_inactiveTabSprite')
  i1090._defaultCategory = i1091[8]
  request.r(i1091[9], i1091[10], 0, i1090, '_characterRoot')
  i1090._catalog = request.d('Source.Scripts.UI.Customization.Data.CustomizationItemsCatalog', i1091[11], i1090._catalog)
  return i1090
}

Deserializers["Source.Scripts.UI.Customization.Data.CustomizationItemsCatalog"] = function (request, data, root) {
  var i1092 = root || request.c( 'Source.Scripts.UI.Customization.Data.CustomizationItemsCatalog' )
  var i1093 = data
  var i1095 = i1093[0]
  var i1094 = []
  for(var i = 0; i < i1095.length; i += 2) {
  request.r(i1095[i + 0], i1095[i + 1], 2, i1094, '')
  }
  i1092._skinMeshes = i1094
  var i1097 = i1093[1]
  var i1096 = []
  for(var i = 0; i < i1097.length; i += 2) {
  request.r(i1097[i + 0], i1097[i + 1], 2, i1096, '')
  }
  i1092._backMeshes = i1096
  var i1099 = i1093[2]
  var i1098 = []
  for(var i = 0; i < i1099.length; i += 2) {
  request.r(i1099[i + 0], i1099[i + 1], 2, i1098, '')
  }
  i1092._shoeMeshes = i1098
  var i1101 = i1093[3]
  var i1100 = []
  for(var i = 0; i < i1101.length; i += 2) {
  request.r(i1101[i + 0], i1101[i + 1], 2, i1100, '')
  }
  i1092._capHairMeshes = i1100
  var i1103 = i1093[4]
  var i1102 = []
  for(var i = 0; i < i1103.length; i += 2) {
  request.r(i1103[i + 0], i1103[i + 1], 2, i1102, '')
  }
  i1092._capHatMeshes = i1102
  var i1105 = i1093[5]
  var i1104 = []
  for(var i = 0; i < i1105.length; i += 2) {
  request.r(i1105[i + 0], i1105[i + 1], 2, i1104, '')
  }
  i1092._glassesMeshes = i1104
  return i1092
}

Deserializers["UnityEngine.UI.HorizontalLayoutGroup"] = function (request, data, root) {
  var i1108 = root || request.c( 'UnityEngine.UI.HorizontalLayoutGroup' )
  var i1109 = data
  i1108.m_Spacing = i1109[0]
  i1108.m_ChildForceExpandWidth = !!i1109[1]
  i1108.m_ChildForceExpandHeight = !!i1109[2]
  i1108.m_ChildControlWidth = !!i1109[3]
  i1108.m_ChildControlHeight = !!i1109[4]
  i1108.m_ChildScaleWidth = !!i1109[5]
  i1108.m_ChildScaleHeight = !!i1109[6]
  i1108.m_ReverseArrangement = !!i1109[7]
  i1108.m_Padding = UnityEngine.RectOffset.FromPaddings(i1109[8], i1109[9], i1109[10], i1109[11])
  i1108.m_ChildAlignment = i1109[12]
  return i1108
}

Deserializers["TMPro.TextMeshProUGUI"] = function (request, data, root) {
  var i1110 = root || request.c( 'TMPro.TextMeshProUGUI' )
  var i1111 = data
  i1110.m_hasFontAssetChanged = !!i1111[0]
  request.r(i1111[1], i1111[2], 0, i1110, 'm_baseMaterial')
  i1110.m_maskOffset = new pc.Vec4( i1111[3], i1111[4], i1111[5], i1111[6] )
  i1110.m_text = i1111[7]
  i1110.m_isRightToLeft = !!i1111[8]
  request.r(i1111[9], i1111[10], 0, i1110, 'm_fontAsset')
  request.r(i1111[11], i1111[12], 0, i1110, 'm_sharedMaterial')
  var i1113 = i1111[13]
  var i1112 = []
  for(var i = 0; i < i1113.length; i += 2) {
  request.r(i1113[i + 0], i1113[i + 1], 2, i1112, '')
  }
  i1110.m_fontSharedMaterials = i1112
  request.r(i1111[14], i1111[15], 0, i1110, 'm_fontMaterial')
  var i1115 = i1111[16]
  var i1114 = []
  for(var i = 0; i < i1115.length; i += 2) {
  request.r(i1115[i + 0], i1115[i + 1], 2, i1114, '')
  }
  i1110.m_fontMaterials = i1114
  i1110.m_fontColor32 = UnityEngine.Color32.ConstructColor(i1111[17], i1111[18], i1111[19], i1111[20])
  i1110.m_fontColor = new pc.Color(i1111[21], i1111[22], i1111[23], i1111[24])
  i1110.m_enableVertexGradient = !!i1111[25]
  i1110.m_colorMode = i1111[26]
  i1110.m_fontColorGradient = request.d('TMPro.VertexGradient', i1111[27], i1110.m_fontColorGradient)
  request.r(i1111[28], i1111[29], 0, i1110, 'm_fontColorGradientPreset')
  request.r(i1111[30], i1111[31], 0, i1110, 'm_spriteAsset')
  i1110.m_tintAllSprites = !!i1111[32]
  request.r(i1111[33], i1111[34], 0, i1110, 'm_StyleSheet')
  i1110.m_TextStyleHashCode = i1111[35]
  i1110.m_overrideHtmlColors = !!i1111[36]
  i1110.m_faceColor = UnityEngine.Color32.ConstructColor(i1111[37], i1111[38], i1111[39], i1111[40])
  i1110.m_fontSize = i1111[41]
  i1110.m_fontSizeBase = i1111[42]
  i1110.m_fontWeight = i1111[43]
  i1110.m_enableAutoSizing = !!i1111[44]
  i1110.m_fontSizeMin = i1111[45]
  i1110.m_fontSizeMax = i1111[46]
  i1110.m_fontStyle = i1111[47]
  i1110.m_HorizontalAlignment = i1111[48]
  i1110.m_VerticalAlignment = i1111[49]
  i1110.m_textAlignment = i1111[50]
  i1110.m_characterSpacing = i1111[51]
  i1110.m_characterHorizontalScale = i1111[52]
  i1110.m_wordSpacing = i1111[53]
  i1110.m_lineSpacing = i1111[54]
  i1110.m_lineSpacingMax = i1111[55]
  i1110.m_paragraphSpacing = i1111[56]
  i1110.m_charWidthMaxAdj = i1111[57]
  i1110.m_TextWrappingMode = i1111[58]
  i1110.m_wordWrappingRatios = i1111[59]
  i1110.m_overflowMode = i1111[60]
  request.r(i1111[61], i1111[62], 0, i1110, 'm_linkedTextComponent')
  request.r(i1111[63], i1111[64], 0, i1110, 'parentLinkedComponent')
  i1110.m_enableKerning = !!i1111[65]
  var i1117 = i1111[66]
  var i1116 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.TextCore.OTL_FeatureTag')))
  for(var i = 0; i < i1117.length; i += 1) {
    i1116.add(i1117[i + 0]);
  }
  i1110.m_ActiveFontFeatures = i1116
  i1110.m_enableExtraPadding = !!i1111[67]
  i1110.checkPaddingRequired = !!i1111[68]
  i1110.m_isRichText = !!i1111[69]
  i1110.m_parseCtrlCharacters = !!i1111[70]
  i1110.m_isOrthographic = !!i1111[71]
  i1110.m_isCullingEnabled = !!i1111[72]
  i1110.m_horizontalMapping = i1111[73]
  i1110.m_verticalMapping = i1111[74]
  i1110.m_uvLineOffset = i1111[75]
  i1110.m_geometrySortingOrder = i1111[76]
  i1110.m_IsTextObjectScaleStatic = !!i1111[77]
  i1110.m_VertexBufferAutoSizeReduction = !!i1111[78]
  i1110.m_useMaxVisibleDescender = !!i1111[79]
  i1110.m_pageToDisplay = i1111[80]
  i1110.m_margin = new pc.Vec4( i1111[81], i1111[82], i1111[83], i1111[84] )
  i1110.m_isUsingLegacyAnimationComponent = !!i1111[85]
  i1110.m_isVolumetricText = !!i1111[86]
  request.r(i1111[87], i1111[88], 0, i1110, 'm_Material')
  i1110.m_EmojiFallbackSupport = !!i1111[89]
  i1110.m_Maskable = !!i1111[90]
  i1110.m_Color = new pc.Color(i1111[91], i1111[92], i1111[93], i1111[94])
  i1110.m_RaycastTarget = !!i1111[95]
  i1110.m_RaycastPadding = new pc.Vec4( i1111[96], i1111[97], i1111[98], i1111[99] )
  return i1110
}

Deserializers["TMPro.VertexGradient"] = function (request, data, root) {
  var i1118 = root || request.c( 'TMPro.VertexGradient' )
  var i1119 = data
  i1118.topLeft = new pc.Color(i1119[0], i1119[1], i1119[2], i1119[3])
  i1118.topRight = new pc.Color(i1119[4], i1119[5], i1119[6], i1119[7])
  i1118.bottomLeft = new pc.Color(i1119[8], i1119[9], i1119[10], i1119[11])
  i1118.bottomRight = new pc.Color(i1119[12], i1119[13], i1119[14], i1119[15])
  return i1118
}

Deserializers["UnityEngine.UI.VerticalLayoutGroup"] = function (request, data, root) {
  var i1122 = root || request.c( 'UnityEngine.UI.VerticalLayoutGroup' )
  var i1123 = data
  i1122.m_Spacing = i1123[0]
  i1122.m_ChildForceExpandWidth = !!i1123[1]
  i1122.m_ChildForceExpandHeight = !!i1123[2]
  i1122.m_ChildControlWidth = !!i1123[3]
  i1122.m_ChildControlHeight = !!i1123[4]
  i1122.m_ChildScaleWidth = !!i1123[5]
  i1122.m_ChildScaleHeight = !!i1123[6]
  i1122.m_ReverseArrangement = !!i1123[7]
  i1122.m_Padding = UnityEngine.RectOffset.FromPaddings(i1123[8], i1123[9], i1123[10], i1123[11])
  i1122.m_ChildAlignment = i1123[12]
  return i1122
}

Deserializers["Source.Scripts.UI.Customization.Views.LoadGameSceneButton"] = function (request, data, root) {
  var i1124 = root || request.c( 'Source.Scripts.UI.Customization.Views.LoadGameSceneButton' )
  var i1125 = data
  request.r(i1125[0], i1125[1], 0, i1124, '_button')
  i1124._sceneName = i1125[2]
  return i1124
}

Deserializers["Source.Scripts.Orientation.FitTransformToUiViewport"] = function (request, data, root) {
  var i1126 = root || request.c( 'Source.Scripts.Orientation.FitTransformToUiViewport' )
  var i1127 = data
  request.r(i1127[0], i1127[1], 0, i1126, '_target')
  request.r(i1127[2], i1127[3], 0, i1126, '_camera')
  request.r(i1127[4], i1127[5], 0, i1126, '_landscapeViewport')
  request.r(i1127[6], i1127[7], 0, i1126, '_portraitViewport')
  i1126._fill = i1127[8]
  i1126._worldZ = i1127[9]
  i1126._worldOffset = new pc.Vec3( i1127[10], i1127[11], i1127[12] )
  request.r(i1127[13], i1127[14], 0, i1126, '_canvasSelector')
  return i1126
}

Deserializers["Source.Scripts.Tutorial.TutorialManager"] = function (request, data, root) {
  var i1128 = root || request.c( 'Source.Scripts.Tutorial.TutorialManager' )
  var i1129 = data
  i1128.ButtonPressed = request.d('System.Action', i1129[0], i1128.ButtonPressed)
  request.r(i1129[1], i1129[2], 0, i1128, '_customizationScreen')
  var i1131 = i1129[3]
  var i1130 = new (System.Collections.Generic.List$1(Bridge.ns('ButtonPulseDotween')))
  for(var i = 0; i < i1131.length; i += 2) {
  request.r(i1131[i + 0], i1131[i + 1], 1, i1130, '')
  }
  i1128._tutorialStepsButtons = i1130
  request.r(i1129[4], i1129[5], 0, i1128, '_canvasSelector')
  return i1128
}

Deserializers["System.Action"] = function (request, data, root) {
  var i1132 = root || request.c( 'System.Action' )
  var i1133 = data
  return i1132
}

Deserializers["ButtonsAudioController"] = function (request, data, root) {
  var i1134 = root || request.c( 'ButtonsAudioController' )
  var i1135 = data
  request.r(i1135[0], i1135[1], 0, i1134, '_clickSound')
  request.r(i1135[2], i1135[3], 0, i1134, '_customizationScreen')
  request.r(i1135[4], i1135[5], 0, i1134, '_canvasSelector')
  return i1134
}

Deserializers["MusicManager"] = function (request, data, root) {
  var i1136 = root || request.c( 'MusicManager' )
  var i1137 = data
  return i1136
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.UniversalRenderPipelineAsset"] = function (request, data, root) {
  var i1138 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.UniversalRenderPipelineAsset' )
  var i1139 = data
  i1138.AdditionalLightsRenderingMode = i1139[0]
  i1138.LightRenderingMode = request.d('Luna.Unity.DTO.UnityEngine.Assets.LightRenderingMode', i1139[1], i1138.LightRenderingMode)
  i1138.MainLightRenderingModeValue = i1139[2]
  i1138.SupportsMainLightShadows = !!i1139[3]
  i1138.MixedLightingSupported = !!i1139[4]
  i1138.MainLightShadowmapResolutionValue = i1139[5]
  i1138.SupportsSoftShadows = !!i1139[6]
  i1138.SoftShadowQualityValue = i1139[7]
  i1138.ShadowDistance = i1139[8]
  i1138.ShadowCascadeCount = i1139[9]
  i1138.Cascade2Split = i1139[10]
  i1138.Cascade3Split = new pc.Vec2( i1139[11], i1139[12] )
  i1138.Cascade4Split = new pc.Vec3( i1139[13], i1139[14], i1139[15] )
  i1138.CascadeBorder = i1139[16]
  i1138.ShadowDepthBias = i1139[17]
  i1138.ShadowNormalBias = i1139[18]
  i1138.RequireDepthTexture = !!i1139[19]
  i1138.RequireOpaqueTexture = !!i1139[20]
  i1138.scriptableRendererData = request.d('Luna.Unity.DTO.UnityEngine.Assets.ScriptableRendererData', i1139[21], i1138.scriptableRendererData)
  return i1138
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.LightRenderingMode"] = function (request, data, root) {
  var i1140 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.LightRenderingMode' )
  var i1141 = data
  i1140.Disabled = i1141[0]
  i1140.PerVertex = i1141[1]
  i1140.PerPixel = i1141[2]
  return i1140
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ScriptableRendererData"] = function (request, data, root) {
  var i1142 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ScriptableRendererData' )
  var i1143 = data
  i1142.opaqueLayerMask = i1143[0]
  i1142.transparentLayerMask = i1143[1]
  var i1145 = i1143[2]
  var i1144 = []
  for(var i = 0; i < i1145.length; i += 1) {
    i1144.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderObjects', i1145[i + 0]) );
  }
  i1142.RenderObjectsFeatures = i1144
  i1142.name = i1143[3]
  return i1142
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderObjects"] = function (request, data, root) {
  var i1148 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderObjects' )
  var i1149 = data
  i1148.settings = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+RenderObjectsSettings', i1149[0], i1148.settings)
  i1148.name = i1149[1]
  i1148.typeName = i1149[2]
  return i1148
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i1150 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i1151 = data
  var i1153 = i1151[0]
  var i1152 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i1153.length; i += 1) {
    i1152.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i1153[i + 0]));
  }
  i1150.ShaderCompilationErrors = i1152
  i1150.name = i1151[1]
  i1150.guid = i1151[2]
  var i1155 = i1151[3]
  var i1154 = []
  for(var i = 0; i < i1155.length; i += 1) {
    i1154.push( i1155[i + 0] );
  }
  i1150.shaderDefinedKeywords = i1154
  var i1157 = i1151[4]
  var i1156 = []
  for(var i = 0; i < i1157.length; i += 1) {
    i1156.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i1157[i + 0]) );
  }
  i1150.passes = i1156
  var i1159 = i1151[5]
  var i1158 = []
  for(var i = 0; i < i1159.length; i += 1) {
    i1158.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i1159[i + 0]) );
  }
  i1150.usePasses = i1158
  var i1161 = i1151[6]
  var i1160 = []
  for(var i = 0; i < i1161.length; i += 1) {
    i1160.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i1161[i + 0]) );
  }
  i1150.defaultParameterValues = i1160
  request.r(i1151[7], i1151[8], 0, i1150, 'unityFallbackShader')
  i1150.readDepth = !!i1151[9]
  i1150.hasDepthOnlyPass = !!i1151[10]
  i1150.isCreatedByShaderGraph = !!i1151[11]
  i1150.disableBatching = !!i1151[12]
  i1150.compiled = !!i1151[13]
  return i1150
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i1164 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i1165 = data
  i1164.shaderName = i1165[0]
  i1164.errorMessage = i1165[1]
  return i1164
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i1168 = root || new pc.UnityShaderPass()
  var i1169 = data
  i1168.id = i1169[0]
  i1168.subShaderIndex = i1169[1]
  i1168.name = i1169[2]
  i1168.passType = i1169[3]
  i1168.grabPassTextureName = i1169[4]
  i1168.usePass = !!i1169[5]
  i1168.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[6], i1168.zTest)
  i1168.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[7], i1168.zWrite)
  i1168.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[8], i1168.culling)
  i1168.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i1169[9], i1168.blending)
  i1168.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i1169[10], i1168.alphaBlending)
  i1168.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[11], i1168.colorWriteMask)
  i1168.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[12], i1168.offsetUnits)
  i1168.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[13], i1168.offsetFactor)
  i1168.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[14], i1168.stencilRef)
  i1168.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[15], i1168.stencilReadMask)
  i1168.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1169[16], i1168.stencilWriteMask)
  i1168.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i1169[17], i1168.stencilOp)
  i1168.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i1169[18], i1168.stencilOpFront)
  i1168.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i1169[19], i1168.stencilOpBack)
  var i1171 = i1169[20]
  var i1170 = []
  for(var i = 0; i < i1171.length; i += 1) {
    i1170.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i1171[i + 0]) );
  }
  i1168.tags = i1170
  var i1173 = i1169[21]
  var i1172 = []
  for(var i = 0; i < i1173.length; i += 1) {
    i1172.push( i1173[i + 0] );
  }
  i1168.passDefinedKeywords = i1172
  var i1175 = i1169[22]
  var i1174 = []
  for(var i = 0; i < i1175.length; i += 1) {
    i1174.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i1175[i + 0]) );
  }
  i1168.passDefinedKeywordGroups = i1174
  var i1177 = i1169[23]
  var i1176 = []
  for(var i = 0; i < i1177.length; i += 1) {
    i1176.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i1177[i + 0]) );
  }
  i1168.variants = i1176
  var i1179 = i1169[24]
  var i1178 = []
  for(var i = 0; i < i1179.length; i += 1) {
    i1178.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i1179[i + 0]) );
  }
  i1168.excludedVariants = i1178
  i1168.hasDepthReader = !!i1169[25]
  return i1168
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i1180 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i1181 = data
  i1180.val = i1181[0]
  i1180.name = i1181[1]
  return i1180
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i1182 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i1183 = data
  i1182.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1183[0], i1182.src)
  i1182.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1183[1], i1182.dst)
  i1182.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1183[2], i1182.op)
  return i1182
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i1184 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i1185 = data
  i1184.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1185[0], i1184.pass)
  i1184.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1185[1], i1184.fail)
  i1184.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1185[2], i1184.zFail)
  i1184.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i1185[3], i1184.comp)
  return i1184
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i1188 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i1189 = data
  i1188.name = i1189[0]
  i1188.value = i1189[1]
  return i1188
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i1192 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i1193 = data
  var i1195 = i1193[0]
  var i1194 = []
  for(var i = 0; i < i1195.length; i += 1) {
    i1194.push( i1195[i + 0] );
  }
  i1192.keywords = i1194
  i1192.hasDiscard = !!i1193[1]
  return i1192
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i1198 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i1199 = data
  i1198.passId = i1199[0]
  i1198.subShaderIndex = i1199[1]
  var i1201 = i1199[2]
  var i1200 = []
  for(var i = 0; i < i1201.length; i += 1) {
    i1200.push( i1201[i + 0] );
  }
  i1198.keywords = i1200
  i1198.vertexProgram = i1199[3]
  i1198.fragmentProgram = i1199[4]
  i1198.exportedForWebGl2 = !!i1199[5]
  i1198.readDepth = !!i1199[6]
  return i1198
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i1204 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i1205 = data
  request.r(i1205[0], i1205[1], 0, i1204, 'shader')
  i1204.pass = i1205[2]
  return i1204
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i1208 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i1209 = data
  i1208.name = i1209[0]
  i1208.type = i1209[1]
  i1208.value = new pc.Vec4( i1209[2], i1209[3], i1209[4], i1209[5] )
  i1208.textureValue = i1209[6]
  i1208.shaderPropertyFlag = i1209[7]
  return i1208
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Sprite"] = function (request, data, root) {
  var i1210 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Sprite' )
  var i1211 = data
  i1210.name = i1211[0]
  request.r(i1211[1], i1211[2], 0, i1210, 'texture')
  i1210.aabb = i1211[3]
  i1210.vertices = i1211[4]
  i1210.triangles = i1211[5]
  i1210.textureRect = UnityEngine.Rect.MinMaxRect(i1211[6], i1211[7], i1211[8], i1211[9])
  i1210.packedRect = UnityEngine.Rect.MinMaxRect(i1211[10], i1211[11], i1211[12], i1211[13])
  i1210.border = new pc.Vec4( i1211[14], i1211[15], i1211[16], i1211[17] )
  i1210.transparency = i1211[18]
  i1210.bounds = i1211[19]
  i1210.pixelsPerUnit = i1211[20]
  i1210.textureWidth = i1211[21]
  i1210.textureHeight = i1211[22]
  i1210.nativeSize = new pc.Vec2( i1211[23], i1211[24] )
  i1210.pivot = new pc.Vec2( i1211[25], i1211[26] )
  i1210.textureRectOffset = new pc.Vec2( i1211[27], i1211[28] )
  return i1210
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.AudioClip"] = function (request, data, root) {
  var i1212 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.AudioClip' )
  var i1213 = data
  i1212.name = i1213[0]
  return i1212
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip"] = function (request, data, root) {
  var i1214 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip' )
  var i1215 = data
  i1214.name = i1215[0]
  i1214.wrapMode = i1215[1]
  i1214.isLooping = !!i1215[2]
  i1214.length = i1215[3]
  var i1217 = i1215[4]
  var i1216 = []
  for(var i = 0; i < i1217.length; i += 1) {
    i1216.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve', i1217[i + 0]) );
  }
  i1214.curves = i1216
  var i1219 = i1215[5]
  var i1218 = []
  for(var i = 0; i < i1219.length; i += 1) {
    i1218.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent', i1219[i + 0]) );
  }
  i1214.events = i1218
  i1214.halfPrecision = !!i1215[6]
  i1214._frameRate = i1215[7]
  i1214.localBounds = request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds', i1215[8], i1214.localBounds)
  i1214.hasMuscleCurves = !!i1215[9]
  var i1221 = i1215[10]
  var i1220 = []
  for(var i = 0; i < i1221.length; i += 1) {
    i1220.push( i1221[i + 0] );
  }
  i1214.clipMuscleConstant = i1220
  i1214.clipBindingConstant = request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant', i1215[11], i1214.clipBindingConstant)
  return i1214
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve"] = function (request, data, root) {
  var i1224 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve' )
  var i1225 = data
  i1224.path = i1225[0]
  i1224.hash = i1225[1]
  i1224.componentType = i1225[2]
  i1224.property = i1225[3]
  i1224.keys = i1225[4]
  var i1227 = i1225[5]
  var i1226 = []
  for(var i = 0; i < i1227.length; i += 1) {
    i1226.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey', i1227[i + 0]) );
  }
  i1224.objectReferenceKeys = i1226
  return i1224
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent"] = function (request, data, root) {
  var i1230 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent' )
  var i1231 = data
  i1230.functionName = i1231[0]
  i1230.floatParameter = i1231[1]
  i1230.intParameter = i1231[2]
  i1230.stringParameter = i1231[3]
  request.r(i1231[4], i1231[5], 0, i1230, 'objectReferenceParameter')
  i1230.time = i1231[6]
  return i1230
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds"] = function (request, data, root) {
  var i1232 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds' )
  var i1233 = data
  i1232.center = new pc.Vec3( i1233[0], i1233[1], i1233[2] )
  i1232.extends = new pc.Vec3( i1233[3], i1233[4], i1233[5] )
  return i1232
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant"] = function (request, data, root) {
  var i1236 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant' )
  var i1237 = data
  var i1239 = i1237[0]
  var i1238 = []
  for(var i = 0; i < i1239.length; i += 1) {
    i1238.push( i1239[i + 0] );
  }
  i1236.genericBindings = i1238
  var i1241 = i1237[1]
  var i1240 = []
  for(var i = 0; i < i1241.length; i += 1) {
    i1240.push( i1241[i + 0] );
  }
  i1236.pptrCurveMapping = i1240
  return i1236
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font"] = function (request, data, root) {
  var i1242 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font' )
  var i1243 = data
  i1242.name = i1243[0]
  i1242.ascent = i1243[1]
  i1242.originalLineHeight = i1243[2]
  i1242.fontSize = i1243[3]
  var i1245 = i1243[4]
  var i1244 = []
  for(var i = 0; i < i1245.length; i += 1) {
    i1244.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo', i1245[i + 0]) );
  }
  i1242.characterInfo = i1244
  request.r(i1243[5], i1243[6], 0, i1242, 'texture')
  i1242.originalFontSize = i1243[7]
  return i1242
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo"] = function (request, data, root) {
  var i1248 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo' )
  var i1249 = data
  i1248.index = i1249[0]
  i1248.advance = i1249[1]
  i1248.bearing = i1249[2]
  i1248.glyphWidth = i1249[3]
  i1248.glyphHeight = i1249[4]
  i1248.minX = i1249[5]
  i1248.maxX = i1249[6]
  i1248.minY = i1249[7]
  i1248.maxY = i1249[8]
  i1248.uvBottomLeftX = i1249[9]
  i1248.uvBottomLeftY = i1249[10]
  i1248.uvBottomRightX = i1249[11]
  i1248.uvBottomRightY = i1249[12]
  i1248.uvTopLeftX = i1249[13]
  i1248.uvTopLeftY = i1249[14]
  i1248.uvTopRightX = i1249[15]
  i1248.uvTopRightY = i1249[16]
  return i1248
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorController"] = function (request, data, root) {
  var i1250 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorController' )
  var i1251 = data
  i1250.name = i1251[0]
  var i1253 = i1251[1]
  var i1252 = []
  for(var i = 0; i < i1253.length; i += 1) {
    i1252.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer', i1253[i + 0]) );
  }
  i1250.layers = i1252
  var i1255 = i1251[2]
  var i1254 = []
  for(var i = 0; i < i1255.length; i += 1) {
    i1254.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter', i1255[i + 0]) );
  }
  i1250.parameters = i1254
  i1250.animationClips = i1251[3]
  i1250.avatarUnsupported = i1251[4]
  return i1250
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer"] = function (request, data, root) {
  var i1258 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer' )
  var i1259 = data
  i1258.name = i1259[0]
  i1258.defaultWeight = i1259[1]
  i1258.blendingMode = i1259[2]
  i1258.avatarMask = i1259[3]
  i1258.syncedLayerIndex = i1259[4]
  i1258.syncedLayerAffectsTiming = !!i1259[5]
  i1258.syncedLayers = i1259[6]
  i1258.stateMachine = request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine', i1259[7], i1258.stateMachine)
  return i1258
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine"] = function (request, data, root) {
  var i1260 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine' )
  var i1261 = data
  i1260.id = i1261[0]
  i1260.name = i1261[1]
  i1260.path = i1261[2]
  var i1263 = i1261[3]
  var i1262 = []
  for(var i = 0; i < i1263.length; i += 1) {
    i1262.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState', i1263[i + 0]) );
  }
  i1260.states = i1262
  var i1265 = i1261[4]
  var i1264 = []
  for(var i = 0; i < i1265.length; i += 1) {
    i1264.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine', i1265[i + 0]) );
  }
  i1260.machines = i1264
  var i1267 = i1261[5]
  var i1266 = []
  for(var i = 0; i < i1267.length; i += 1) {
    i1266.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition', i1267[i + 0]) );
  }
  i1260.entryStateTransitions = i1266
  var i1269 = i1261[6]
  var i1268 = []
  for(var i = 0; i < i1269.length; i += 1) {
    i1268.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition', i1269[i + 0]) );
  }
  i1260.exitStateTransitions = i1268
  var i1271 = i1261[7]
  var i1270 = []
  for(var i = 0; i < i1271.length; i += 1) {
    i1270.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition', i1271[i + 0]) );
  }
  i1260.anyStateTransitions = i1270
  i1260.defaultStateId = i1261[8]
  return i1260
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState"] = function (request, data, root) {
  var i1274 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState' )
  var i1275 = data
  i1274.id = i1275[0]
  i1274.name = i1275[1]
  i1274.cycleOffset = i1275[2]
  i1274.cycleOffsetParameter = i1275[3]
  i1274.cycleOffsetParameterActive = !!i1275[4]
  i1274.mirror = !!i1275[5]
  i1274.mirrorParameter = i1275[6]
  i1274.mirrorParameterActive = !!i1275[7]
  i1274.motionId = i1275[8]
  i1274.nameHash = i1275[9]
  i1274.fullPathHash = i1275[10]
  i1274.speed = i1275[11]
  i1274.speedParameter = i1275[12]
  i1274.speedParameterActive = !!i1275[13]
  i1274.tag = i1275[14]
  i1274.tagHash = i1275[15]
  i1274.writeDefaultValues = !!i1275[16]
  var i1277 = i1275[17]
  var i1276 = []
  for(var i = 0; i < i1277.length; i += 2) {
  request.r(i1277[i + 0], i1277[i + 1], 2, i1276, '')
  }
  i1274.behaviours = i1276
  var i1279 = i1275[18]
  var i1278 = []
  for(var i = 0; i < i1279.length; i += 1) {
    i1278.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition', i1279[i + 0]) );
  }
  i1274.transitions = i1278
  return i1274
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition"] = function (request, data, root) {
  var i1284 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition' )
  var i1285 = data
  i1284.fullPath = i1285[0]
  i1284.canTransitionToSelf = !!i1285[1]
  i1284.duration = i1285[2]
  i1284.exitTime = i1285[3]
  i1284.hasExitTime = !!i1285[4]
  i1284.hasFixedDuration = !!i1285[5]
  i1284.interruptionSource = i1285[6]
  i1284.offset = i1285[7]
  i1284.orderedInterruption = !!i1285[8]
  i1284.destinationStateId = i1285[9]
  i1284.isExit = !!i1285[10]
  i1284.mute = !!i1285[11]
  i1284.solo = !!i1285[12]
  var i1287 = i1285[13]
  var i1286 = []
  for(var i = 0; i < i1287.length; i += 1) {
    i1286.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition', i1287[i + 0]) );
  }
  i1284.conditions = i1286
  return i1284
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition"] = function (request, data, root) {
  var i1290 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition' )
  var i1291 = data
  i1290.mode = i1291[0]
  i1290.parameter = i1291[1]
  i1290.threshold = i1291[2]
  return i1290
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition"] = function (request, data, root) {
  var i1296 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition' )
  var i1297 = data
  i1296.destinationStateId = i1297[0]
  i1296.isExit = !!i1297[1]
  i1296.mute = !!i1297[2]
  i1296.solo = !!i1297[3]
  var i1299 = i1297[4]
  var i1298 = []
  for(var i = 0; i < i1299.length; i += 1) {
    i1298.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition', i1299[i + 0]) );
  }
  i1296.conditions = i1298
  return i1296
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter"] = function (request, data, root) {
  var i1302 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter' )
  var i1303 = data
  i1302.defaultBool = !!i1303[0]
  i1302.defaultFloat = i1303[1]
  i1302.defaultInt = i1303[2]
  i1302.name = i1303[3]
  i1302.nameHash = i1303[4]
  i1302.type = i1303[5]
  return i1302
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.TextAsset"] = function (request, data, root) {
  var i1304 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.TextAsset' )
  var i1305 = data
  i1304.name = i1305[0]
  i1304.bytes64 = i1305[1]
  i1304.data = i1305[2]
  return i1304
}

Deserializers["TMPro.TMP_FontAsset"] = function (request, data, root) {
  var i1306 = root || request.c( 'TMPro.TMP_FontAsset' )
  var i1307 = data
  i1306.normalStyle = i1307[0]
  i1306.normalSpacingOffset = i1307[1]
  i1306.boldStyle = i1307[2]
  i1306.boldSpacing = i1307[3]
  i1306.italicStyle = i1307[4]
  i1306.tabSize = i1307[5]
  request.r(i1307[6], i1307[7], 0, i1306, 'atlas')
  i1306.m_SourceFontFileGUID = i1307[8]
  i1306.m_CreationSettings = request.d('TMPro.FontAssetCreationSettings', i1307[9], i1306.m_CreationSettings)
  request.r(i1307[10], i1307[11], 0, i1306, 'm_SourceFontFile')
  i1306.m_SourceFontFilePath = i1307[12]
  i1306.m_AtlasPopulationMode = i1307[13]
  i1306.InternalDynamicOS = !!i1307[14]
  var i1309 = i1307[15]
  var i1308 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.TextCore.Glyph')))
  for(var i = 0; i < i1309.length; i += 1) {
    i1308.add(request.d('UnityEngine.TextCore.Glyph', i1309[i + 0]));
  }
  i1306.m_GlyphTable = i1308
  var i1311 = i1307[16]
  var i1310 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_Character')))
  for(var i = 0; i < i1311.length; i += 1) {
    i1310.add(request.d('TMPro.TMP_Character', i1311[i + 0]));
  }
  i1306.m_CharacterTable = i1310
  var i1313 = i1307[17]
  var i1312 = []
  for(var i = 0; i < i1313.length; i += 2) {
  request.r(i1313[i + 0], i1313[i + 1], 2, i1312, '')
  }
  i1306.m_AtlasTextures = i1312
  i1306.m_AtlasTextureIndex = i1307[18]
  i1306.m_IsMultiAtlasTexturesEnabled = !!i1307[19]
  i1306.m_GetFontFeatures = !!i1307[20]
  i1306.m_ClearDynamicDataOnBuild = !!i1307[21]
  i1306.m_AtlasWidth = i1307[22]
  i1306.m_AtlasHeight = i1307[23]
  i1306.m_AtlasPadding = i1307[24]
  i1306.m_AtlasRenderMode = i1307[25]
  var i1315 = i1307[26]
  var i1314 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.TextCore.GlyphRect')))
  for(var i = 0; i < i1315.length; i += 1) {
    i1314.add(request.d('UnityEngine.TextCore.GlyphRect', i1315[i + 0]));
  }
  i1306.m_UsedGlyphRects = i1314
  var i1317 = i1307[27]
  var i1316 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.TextCore.GlyphRect')))
  for(var i = 0; i < i1317.length; i += 1) {
    i1316.add(request.d('UnityEngine.TextCore.GlyphRect', i1317[i + 0]));
  }
  i1306.m_FreeGlyphRects = i1316
  i1306.m_FontFeatureTable = request.d('TMPro.TMP_FontFeatureTable', i1307[28], i1306.m_FontFeatureTable)
  i1306.m_ShouldReimportFontFeatures = !!i1307[29]
  var i1319 = i1307[30]
  var i1318 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_FontAsset')))
  for(var i = 0; i < i1319.length; i += 2) {
  request.r(i1319[i + 0], i1319[i + 1], 1, i1318, '')
  }
  i1306.m_FallbackFontAssetTable = i1318
  var i1321 = i1307[31]
  var i1320 = []
  for(var i = 0; i < i1321.length; i += 1) {
    i1320.push( request.d('TMPro.TMP_FontWeightPair', i1321[i + 0]) );
  }
  i1306.m_FontWeightTable = i1320
  var i1323 = i1307[32]
  var i1322 = []
  for(var i = 0; i < i1323.length; i += 1) {
    i1322.push( request.d('TMPro.TMP_FontWeightPair', i1323[i + 0]) );
  }
  i1306.fontWeights = i1322
  i1306.m_fontInfo = request.d('TMPro.FaceInfo_Legacy', i1307[33], i1306.m_fontInfo)
  var i1325 = i1307[34]
  var i1324 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_Glyph')))
  for(var i = 0; i < i1325.length; i += 1) {
    i1324.add(request.d('TMPro.TMP_Glyph', i1325[i + 0]));
  }
  i1306.m_glyphInfoList = i1324
  i1306.m_KerningTable = request.d('TMPro.KerningTable', i1307[35], i1306.m_KerningTable)
  var i1327 = i1307[36]
  var i1326 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_FontAsset')))
  for(var i = 0; i < i1327.length; i += 2) {
  request.r(i1327[i + 0], i1327[i + 1], 1, i1326, '')
  }
  i1306.fallbackFontAssets = i1326
  i1306.m_Version = i1307[37]
  i1306.m_FaceInfo = request.d('UnityEngine.TextCore.FaceInfo', i1307[38], i1306.m_FaceInfo)
  request.r(i1307[39], i1307[40], 0, i1306, 'm_Material')
  return i1306
}

Deserializers["TMPro.FontAssetCreationSettings"] = function (request, data, root) {
  var i1328 = root || request.c( 'TMPro.FontAssetCreationSettings' )
  var i1329 = data
  i1328.sourceFontFileName = i1329[0]
  i1328.sourceFontFileGUID = i1329[1]
  i1328.faceIndex = i1329[2]
  i1328.pointSizeSamplingMode = i1329[3]
  i1328.pointSize = i1329[4]
  i1328.padding = i1329[5]
  i1328.paddingMode = i1329[6]
  i1328.packingMode = i1329[7]
  i1328.atlasWidth = i1329[8]
  i1328.atlasHeight = i1329[9]
  i1328.characterSetSelectionMode = i1329[10]
  i1328.characterSequence = i1329[11]
  i1328.referencedFontAssetGUID = i1329[12]
  i1328.referencedTextAssetGUID = i1329[13]
  i1328.fontStyle = i1329[14]
  i1328.fontStyleModifier = i1329[15]
  i1328.renderMode = i1329[16]
  i1328.includeFontFeatures = !!i1329[17]
  return i1328
}

Deserializers["UnityEngine.TextCore.Glyph"] = function (request, data, root) {
  var i1332 = root || request.c( 'UnityEngine.TextCore.Glyph' )
  var i1333 = data
  i1332.m_Index = i1333[0]
  i1332.m_Metrics = request.d('UnityEngine.TextCore.GlyphMetrics', i1333[1], i1332.m_Metrics)
  i1332.m_GlyphRect = request.d('UnityEngine.TextCore.GlyphRect', i1333[2], i1332.m_GlyphRect)
  i1332.m_Scale = i1333[3]
  i1332.m_AtlasIndex = i1333[4]
  i1332.m_ClassDefinitionType = i1333[5]
  return i1332
}

Deserializers["UnityEngine.TextCore.GlyphMetrics"] = function (request, data, root) {
  var i1334 = root || request.c( 'UnityEngine.TextCore.GlyphMetrics' )
  var i1335 = data
  i1334.m_Width = i1335[0]
  i1334.m_Height = i1335[1]
  i1334.m_HorizontalBearingX = i1335[2]
  i1334.m_HorizontalBearingY = i1335[3]
  i1334.m_HorizontalAdvance = i1335[4]
  return i1334
}

Deserializers["UnityEngine.TextCore.GlyphRect"] = function (request, data, root) {
  var i1336 = root || request.c( 'UnityEngine.TextCore.GlyphRect' )
  var i1337 = data
  i1336.m_X = i1337[0]
  i1336.m_Y = i1337[1]
  i1336.m_Width = i1337[2]
  i1336.m_Height = i1337[3]
  return i1336
}

Deserializers["TMPro.TMP_Character"] = function (request, data, root) {
  var i1340 = root || request.c( 'TMPro.TMP_Character' )
  var i1341 = data
  i1340.m_ElementType = i1341[0]
  i1340.m_Unicode = i1341[1]
  i1340.m_GlyphIndex = i1341[2]
  i1340.m_Scale = i1341[3]
  return i1340
}

Deserializers["TMPro.TMP_FontFeatureTable"] = function (request, data, root) {
  var i1346 = root || request.c( 'TMPro.TMP_FontFeatureTable' )
  var i1347 = data
  var i1349 = i1347[0]
  var i1348 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.MultipleSubstitutionRecord')))
  for(var i = 0; i < i1349.length; i += 1) {
    i1348.add(request.d('TMPro.MultipleSubstitutionRecord', i1349[i + 0]));
  }
  i1346.m_MultipleSubstitutionRecords = i1348
  var i1351 = i1347[1]
  var i1350 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.LigatureSubstitutionRecord')))
  for(var i = 0; i < i1351.length; i += 1) {
    i1350.add(request.d('TMPro.LigatureSubstitutionRecord', i1351[i + 0]));
  }
  i1346.m_LigatureSubstitutionRecords = i1350
  var i1353 = i1347[2]
  var i1352 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.TextCore.LowLevel.GlyphPairAdjustmentRecord')))
  for(var i = 0; i < i1353.length; i += 1) {
    i1352.add(request.d('UnityEngine.TextCore.LowLevel.GlyphPairAdjustmentRecord', i1353[i + 0]));
  }
  i1346.m_GlyphPairAdjustmentRecords = i1352
  var i1355 = i1347[3]
  var i1354 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.MarkToBaseAdjustmentRecord')))
  for(var i = 0; i < i1355.length; i += 1) {
    i1354.add(request.d('TMPro.MarkToBaseAdjustmentRecord', i1355[i + 0]));
  }
  i1346.m_MarkToBaseAdjustmentRecords = i1354
  var i1357 = i1347[4]
  var i1356 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.MarkToMarkAdjustmentRecord')))
  for(var i = 0; i < i1357.length; i += 1) {
    i1356.add(request.d('TMPro.MarkToMarkAdjustmentRecord', i1357[i + 0]));
  }
  i1346.m_MarkToMarkAdjustmentRecords = i1356
  return i1346
}

Deserializers["TMPro.MultipleSubstitutionRecord"] = function (request, data, root) {
  var i1360 = root || request.c( 'TMPro.MultipleSubstitutionRecord' )
  var i1361 = data
  i1360.m_TargetGlyphID = i1361[0]
  i1360.m_SubstituteGlyphIDs = i1361[1]
  return i1360
}

Deserializers["TMPro.LigatureSubstitutionRecord"] = function (request, data, root) {
  var i1364 = root || request.c( 'TMPro.LigatureSubstitutionRecord' )
  var i1365 = data
  i1364.m_ComponentGlyphIDs = i1365[0]
  i1364.m_LigatureGlyphID = i1365[1]
  return i1364
}

Deserializers["UnityEngine.TextCore.LowLevel.GlyphPairAdjustmentRecord"] = function (request, data, root) {
  var i1368 = root || request.c( 'UnityEngine.TextCore.LowLevel.GlyphPairAdjustmentRecord' )
  var i1369 = data
  i1368.m_FirstAdjustmentRecord = request.d('UnityEngine.TextCore.LowLevel.GlyphAdjustmentRecord', i1369[0], i1368.m_FirstAdjustmentRecord)
  i1368.m_SecondAdjustmentRecord = request.d('UnityEngine.TextCore.LowLevel.GlyphAdjustmentRecord', i1369[1], i1368.m_SecondAdjustmentRecord)
  i1368.m_FeatureLookupFlags = i1369[2]
  return i1368
}

Deserializers["TMPro.MarkToBaseAdjustmentRecord"] = function (request, data, root) {
  var i1372 = root || request.c( 'TMPro.MarkToBaseAdjustmentRecord' )
  var i1373 = data
  i1372.m_BaseGlyphID = i1373[0]
  i1372.m_BaseGlyphAnchorPoint = request.d('TMPro.GlyphAnchorPoint', i1373[1], i1372.m_BaseGlyphAnchorPoint)
  i1372.m_MarkGlyphID = i1373[2]
  i1372.m_MarkPositionAdjustment = request.d('TMPro.MarkPositionAdjustment', i1373[3], i1372.m_MarkPositionAdjustment)
  return i1372
}

Deserializers["TMPro.MarkToMarkAdjustmentRecord"] = function (request, data, root) {
  var i1376 = root || request.c( 'TMPro.MarkToMarkAdjustmentRecord' )
  var i1377 = data
  i1376.m_BaseMarkGlyphID = i1377[0]
  i1376.m_BaseMarkGlyphAnchorPoint = request.d('TMPro.GlyphAnchorPoint', i1377[1], i1376.m_BaseMarkGlyphAnchorPoint)
  i1376.m_CombiningMarkGlyphID = i1377[2]
  i1376.m_CombiningMarkPositionAdjustment = request.d('TMPro.MarkPositionAdjustment', i1377[3], i1376.m_CombiningMarkPositionAdjustment)
  return i1376
}

Deserializers["TMPro.TMP_FontWeightPair"] = function (request, data, root) {
  var i1382 = root || request.c( 'TMPro.TMP_FontWeightPair' )
  var i1383 = data
  request.r(i1383[0], i1383[1], 0, i1382, 'regularTypeface')
  request.r(i1383[2], i1383[3], 0, i1382, 'italicTypeface')
  return i1382
}

Deserializers["TMPro.FaceInfo_Legacy"] = function (request, data, root) {
  var i1384 = root || request.c( 'TMPro.FaceInfo_Legacy' )
  var i1385 = data
  i1384.Name = i1385[0]
  i1384.PointSize = i1385[1]
  i1384.Scale = i1385[2]
  i1384.CharacterCount = i1385[3]
  i1384.LineHeight = i1385[4]
  i1384.Baseline = i1385[5]
  i1384.Ascender = i1385[6]
  i1384.CapHeight = i1385[7]
  i1384.Descender = i1385[8]
  i1384.CenterLine = i1385[9]
  i1384.SuperscriptOffset = i1385[10]
  i1384.SubscriptOffset = i1385[11]
  i1384.SubSize = i1385[12]
  i1384.Underline = i1385[13]
  i1384.UnderlineThickness = i1385[14]
  i1384.strikethrough = i1385[15]
  i1384.strikethroughThickness = i1385[16]
  i1384.TabWidth = i1385[17]
  i1384.Padding = i1385[18]
  i1384.AtlasWidth = i1385[19]
  i1384.AtlasHeight = i1385[20]
  return i1384
}

Deserializers["TMPro.TMP_Glyph"] = function (request, data, root) {
  var i1388 = root || request.c( 'TMPro.TMP_Glyph' )
  var i1389 = data
  i1388.id = i1389[0]
  i1388.x = i1389[1]
  i1388.y = i1389[2]
  i1388.width = i1389[3]
  i1388.height = i1389[4]
  i1388.xOffset = i1389[5]
  i1388.yOffset = i1389[6]
  i1388.xAdvance = i1389[7]
  i1388.scale = i1389[8]
  return i1388
}

Deserializers["TMPro.KerningTable"] = function (request, data, root) {
  var i1390 = root || request.c( 'TMPro.KerningTable' )
  var i1391 = data
  var i1393 = i1391[0]
  var i1392 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.KerningPair')))
  for(var i = 0; i < i1393.length; i += 1) {
    i1392.add(request.d('TMPro.KerningPair', i1393[i + 0]));
  }
  i1390.kerningPairs = i1392
  return i1390
}

Deserializers["TMPro.KerningPair"] = function (request, data, root) {
  var i1396 = root || request.c( 'TMPro.KerningPair' )
  var i1397 = data
  i1396.xOffset = i1397[0]
  i1396.m_FirstGlyph = i1397[1]
  i1396.m_FirstGlyphAdjustments = request.d('TMPro.GlyphValueRecord_Legacy', i1397[2], i1396.m_FirstGlyphAdjustments)
  i1396.m_SecondGlyph = i1397[3]
  i1396.m_SecondGlyphAdjustments = request.d('TMPro.GlyphValueRecord_Legacy', i1397[4], i1396.m_SecondGlyphAdjustments)
  i1396.m_IgnoreSpacingAdjustments = !!i1397[5]
  return i1396
}

Deserializers["UnityEngine.TextCore.FaceInfo"] = function (request, data, root) {
  var i1398 = root || request.c( 'UnityEngine.TextCore.FaceInfo' )
  var i1399 = data
  i1398.m_FaceIndex = i1399[0]
  i1398.m_FamilyName = i1399[1]
  i1398.m_StyleName = i1399[2]
  i1398.m_PointSize = i1399[3]
  i1398.m_Scale = i1399[4]
  i1398.m_UnitsPerEM = i1399[5]
  i1398.m_LineHeight = i1399[6]
  i1398.m_AscentLine = i1399[7]
  i1398.m_CapLine = i1399[8]
  i1398.m_MeanLine = i1399[9]
  i1398.m_Baseline = i1399[10]
  i1398.m_DescentLine = i1399[11]
  i1398.m_SuperscriptOffset = i1399[12]
  i1398.m_SuperscriptSize = i1399[13]
  i1398.m_SubscriptOffset = i1399[14]
  i1398.m_SubscriptSize = i1399[15]
  i1398.m_UnderlineOffset = i1399[16]
  i1398.m_UnderlineThickness = i1399[17]
  i1398.m_StrikethroughOffset = i1399[18]
  i1398.m_StrikethroughThickness = i1399[19]
  i1398.m_TabWidth = i1399[20]
  return i1398
}

Deserializers["DG.Tweening.Core.DOTweenSettings"] = function (request, data, root) {
  var i1400 = root || request.c( 'DG.Tweening.Core.DOTweenSettings' )
  var i1401 = data
  i1400.useSafeMode = !!i1401[0]
  i1400.safeModeOptions = request.d('DG.Tweening.Core.DOTweenSettings+SafeModeOptions', i1401[1], i1400.safeModeOptions)
  i1400.timeScale = i1401[2]
  i1400.unscaledTimeScale = i1401[3]
  i1400.useSmoothDeltaTime = !!i1401[4]
  i1400.maxSmoothUnscaledTime = i1401[5]
  i1400.rewindCallbackMode = i1401[6]
  i1400.showUnityEditorReport = !!i1401[7]
  i1400.logBehaviour = i1401[8]
  i1400.drawGizmos = !!i1401[9]
  i1400.defaultRecyclable = !!i1401[10]
  i1400.defaultAutoPlay = i1401[11]
  i1400.defaultUpdateType = i1401[12]
  i1400.defaultTimeScaleIndependent = !!i1401[13]
  i1400.defaultEaseType = i1401[14]
  i1400.defaultEaseOvershootOrAmplitude = i1401[15]
  i1400.defaultEasePeriod = i1401[16]
  i1400.defaultAutoKill = !!i1401[17]
  i1400.defaultLoopType = i1401[18]
  i1400.debugMode = !!i1401[19]
  i1400.debugStoreTargetId = !!i1401[20]
  i1400.showPreviewPanel = !!i1401[21]
  i1400.storeSettingsLocation = i1401[22]
  i1400.modules = request.d('DG.Tweening.Core.DOTweenSettings+ModulesSetup', i1401[23], i1400.modules)
  i1400.createASMDEF = !!i1401[24]
  i1400.showPlayingTweens = !!i1401[25]
  i1400.showPausedTweens = !!i1401[26]
  return i1400
}

Deserializers["DG.Tweening.Core.DOTweenSettings+SafeModeOptions"] = function (request, data, root) {
  var i1402 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+SafeModeOptions' )
  var i1403 = data
  i1402.logBehaviour = i1403[0]
  i1402.nestedTweenFailureBehaviour = i1403[1]
  return i1402
}

Deserializers["DG.Tweening.Core.DOTweenSettings+ModulesSetup"] = function (request, data, root) {
  var i1404 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+ModulesSetup' )
  var i1405 = data
  i1404.showPanel = !!i1405[0]
  i1404.audioEnabled = !!i1405[1]
  i1404.physicsEnabled = !!i1405[2]
  i1404.physics2DEnabled = !!i1405[3]
  i1404.spriteEnabled = !!i1405[4]
  i1404.uiEnabled = !!i1405[5]
  i1404.textMeshProEnabled = !!i1405[6]
  i1404.tk2DEnabled = !!i1405[7]
  i1404.deAudioEnabled = !!i1405[8]
  i1404.deUnityExtendedEnabled = !!i1405[9]
  i1404.epoOutlineEnabled = !!i1405[10]
  return i1404
}

Deserializers["TMPro.TMP_Settings"] = function (request, data, root) {
  var i1406 = root || request.c( 'TMPro.TMP_Settings' )
  var i1407 = data
  i1406.assetVersion = i1407[0]
  i1406.m_TextWrappingMode = i1407[1]
  i1406.m_enableKerning = !!i1407[2]
  var i1409 = i1407[3]
  var i1408 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.TextCore.OTL_FeatureTag')))
  for(var i = 0; i < i1409.length; i += 1) {
    i1408.add(i1409[i + 0]);
  }
  i1406.m_ActiveFontFeatures = i1408
  i1406.m_enableExtraPadding = !!i1407[4]
  i1406.m_enableTintAllSprites = !!i1407[5]
  i1406.m_enableParseEscapeCharacters = !!i1407[6]
  i1406.m_EnableRaycastTarget = !!i1407[7]
  i1406.m_GetFontFeaturesAtRuntime = !!i1407[8]
  i1406.m_missingGlyphCharacter = i1407[9]
  i1406.m_ClearDynamicDataOnBuild = !!i1407[10]
  i1406.m_warningsDisabled = !!i1407[11]
  request.r(i1407[12], i1407[13], 0, i1406, 'm_defaultFontAsset')
  i1406.m_defaultFontAssetPath = i1407[14]
  i1406.m_defaultFontSize = i1407[15]
  i1406.m_defaultAutoSizeMinRatio = i1407[16]
  i1406.m_defaultAutoSizeMaxRatio = i1407[17]
  i1406.m_defaultTextMeshProTextContainerSize = new pc.Vec2( i1407[18], i1407[19] )
  i1406.m_defaultTextMeshProUITextContainerSize = new pc.Vec2( i1407[20], i1407[21] )
  i1406.m_autoSizeTextContainer = !!i1407[22]
  i1406.m_IsTextObjectScaleStatic = !!i1407[23]
  var i1411 = i1407[24]
  var i1410 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_FontAsset')))
  for(var i = 0; i < i1411.length; i += 2) {
  request.r(i1411[i + 0], i1411[i + 1], 1, i1410, '')
  }
  i1406.m_fallbackFontAssets = i1410
  i1406.m_matchMaterialPreset = !!i1407[25]
  i1406.m_HideSubTextObjects = !!i1407[26]
  request.r(i1407[27], i1407[28], 0, i1406, 'm_defaultSpriteAsset')
  i1406.m_defaultSpriteAssetPath = i1407[29]
  i1406.m_enableEmojiSupport = !!i1407[30]
  i1406.m_MissingCharacterSpriteUnicode = i1407[31]
  var i1413 = i1407[32]
  var i1412 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_Asset')))
  for(var i = 0; i < i1413.length; i += 2) {
  request.r(i1413[i + 0], i1413[i + 1], 1, i1412, '')
  }
  i1406.m_EmojiFallbackTextAssets = i1412
  i1406.m_defaultColorGradientPresetsPath = i1407[33]
  request.r(i1407[34], i1407[35], 0, i1406, 'm_defaultStyleSheet')
  i1406.m_StyleSheetsResourcePath = i1407[36]
  request.r(i1407[37], i1407[38], 0, i1406, 'm_leadingCharacters')
  request.r(i1407[39], i1407[40], 0, i1406, 'm_followingCharacters')
  i1406.m_UseModernHangulLineBreakingRules = !!i1407[41]
  return i1406
}

Deserializers["UnityEngine.TextCore.LowLevel.GlyphAdjustmentRecord"] = function (request, data, root) {
  var i1416 = root || request.c( 'UnityEngine.TextCore.LowLevel.GlyphAdjustmentRecord' )
  var i1417 = data
  i1416.m_GlyphIndex = i1417[0]
  i1416.m_GlyphValueRecord = request.d('UnityEngine.TextCore.LowLevel.GlyphValueRecord', i1417[1], i1416.m_GlyphValueRecord)
  return i1416
}

Deserializers["UnityEngine.TextCore.LowLevel.GlyphValueRecord"] = function (request, data, root) {
  var i1418 = root || request.c( 'UnityEngine.TextCore.LowLevel.GlyphValueRecord' )
  var i1419 = data
  i1418.m_XPlacement = i1419[0]
  i1418.m_YPlacement = i1419[1]
  i1418.m_XAdvance = i1419[2]
  i1418.m_YAdvance = i1419[3]
  return i1418
}

Deserializers["TMPro.TMP_SpriteAsset"] = function (request, data, root) {
  var i1420 = root || request.c( 'TMPro.TMP_SpriteAsset' )
  var i1421 = data
  request.r(i1421[0], i1421[1], 0, i1420, 'spriteSheet')
  var i1423 = i1421[2]
  var i1422 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_Sprite')))
  for(var i = 0; i < i1423.length; i += 1) {
    i1422.add(request.d('TMPro.TMP_Sprite', i1423[i + 0]));
  }
  i1420.spriteInfoList = i1422
  var i1425 = i1421[3]
  var i1424 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_SpriteAsset')))
  for(var i = 0; i < i1425.length; i += 2) {
  request.r(i1425[i + 0], i1425[i + 1], 1, i1424, '')
  }
  i1420.fallbackSpriteAssets = i1424
  var i1427 = i1421[4]
  var i1426 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_SpriteCharacter')))
  for(var i = 0; i < i1427.length; i += 1) {
    i1426.add(request.d('TMPro.TMP_SpriteCharacter', i1427[i + 0]));
  }
  i1420.m_SpriteCharacterTable = i1426
  var i1429 = i1421[5]
  var i1428 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_SpriteGlyph')))
  for(var i = 0; i < i1429.length; i += 1) {
    i1428.add(request.d('TMPro.TMP_SpriteGlyph', i1429[i + 0]));
  }
  i1420.m_GlyphTable = i1428
  i1420.m_Version = i1421[6]
  i1420.m_FaceInfo = request.d('UnityEngine.TextCore.FaceInfo', i1421[7], i1420.m_FaceInfo)
  request.r(i1421[8], i1421[9], 0, i1420, 'm_Material')
  return i1420
}

Deserializers["TMPro.TMP_Sprite"] = function (request, data, root) {
  var i1432 = root || request.c( 'TMPro.TMP_Sprite' )
  var i1433 = data
  i1432.name = i1433[0]
  i1432.hashCode = i1433[1]
  i1432.unicode = i1433[2]
  i1432.pivot = new pc.Vec2( i1433[3], i1433[4] )
  request.r(i1433[5], i1433[6], 0, i1432, 'sprite')
  i1432.id = i1433[7]
  i1432.x = i1433[8]
  i1432.y = i1433[9]
  i1432.width = i1433[10]
  i1432.height = i1433[11]
  i1432.xOffset = i1433[12]
  i1432.yOffset = i1433[13]
  i1432.xAdvance = i1433[14]
  i1432.scale = i1433[15]
  return i1432
}

Deserializers["TMPro.TMP_SpriteCharacter"] = function (request, data, root) {
  var i1438 = root || request.c( 'TMPro.TMP_SpriteCharacter' )
  var i1439 = data
  i1438.m_Name = i1439[0]
  i1438.m_ElementType = i1439[1]
  i1438.m_Unicode = i1439[2]
  i1438.m_GlyphIndex = i1439[3]
  i1438.m_Scale = i1439[4]
  return i1438
}

Deserializers["TMPro.TMP_SpriteGlyph"] = function (request, data, root) {
  var i1442 = root || request.c( 'TMPro.TMP_SpriteGlyph' )
  var i1443 = data
  request.r(i1443[0], i1443[1], 0, i1442, 'sprite')
  i1442.m_Index = i1443[2]
  i1442.m_Metrics = request.d('UnityEngine.TextCore.GlyphMetrics', i1443[3], i1442.m_Metrics)
  i1442.m_GlyphRect = request.d('UnityEngine.TextCore.GlyphRect', i1443[4], i1442.m_GlyphRect)
  i1442.m_Scale = i1443[5]
  i1442.m_AtlasIndex = i1443[6]
  i1442.m_ClassDefinitionType = i1443[7]
  return i1442
}

Deserializers["TMPro.TMP_StyleSheet"] = function (request, data, root) {
  var i1444 = root || request.c( 'TMPro.TMP_StyleSheet' )
  var i1445 = data
  var i1447 = i1445[0]
  var i1446 = new (System.Collections.Generic.List$1(Bridge.ns('TMPro.TMP_Style')))
  for(var i = 0; i < i1447.length; i += 1) {
    i1446.add(request.d('TMPro.TMP_Style', i1447[i + 0]));
  }
  i1444.m_StyleList = i1446
  return i1444
}

Deserializers["TMPro.TMP_Style"] = function (request, data, root) {
  var i1450 = root || request.c( 'TMPro.TMP_Style' )
  var i1451 = data
  i1450.m_Name = i1451[0]
  i1450.m_HashCode = i1451[1]
  i1450.m_OpeningDefinition = i1451[2]
  i1450.m_ClosingDefinition = i1451[3]
  i1450.m_OpeningTagArray = i1451[4]
  i1450.m_ClosingTagArray = i1451[5]
  return i1450
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i1452 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i1453 = data
  var i1455 = i1453[0]
  var i1454 = []
  for(var i = 0; i < i1455.length; i += 1) {
    i1454.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i1455[i + 0]) );
  }
  i1452.files = i1454
  i1452.componentToPrefabIds = i1453[1]
  return i1452
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i1458 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i1459 = data
  i1458.path = i1459[0]
  request.r(i1459[1], i1459[2], 0, i1458, 'unityObject')
  return i1458
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i1460 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i1461 = data
  var i1463 = i1461[0]
  var i1462 = []
  for(var i = 0; i < i1463.length; i += 1) {
    i1462.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i1463[i + 0]) );
  }
  i1460.scriptsExecutionOrder = i1462
  var i1465 = i1461[1]
  var i1464 = []
  for(var i = 0; i < i1465.length; i += 1) {
    i1464.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i1465[i + 0]) );
  }
  i1460.sortingLayers = i1464
  var i1467 = i1461[2]
  var i1466 = []
  for(var i = 0; i < i1467.length; i += 1) {
    i1466.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i1467[i + 0]) );
  }
  i1460.cullingLayers = i1466
  i1460.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i1461[3], i1460.timeSettings)
  i1460.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i1461[4], i1460.physicsSettings)
  i1460.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i1461[5], i1460.physics2DSettings)
  i1460.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i1461[6], i1460.qualitySettings)
  i1460.enableRealtimeShadows = !!i1461[7]
  i1460.enableAutoInstancing = !!i1461[8]
  i1460.enableStaticBatching = !!i1461[9]
  i1460.enableDynamicBatching = !!i1461[10]
  i1460.usePreservativeDynamicBatching = !!i1461[11]
  i1460.lightmapEncodingQuality = i1461[12]
  i1460.desiredColorSpace = i1461[13]
  var i1469 = i1461[14]
  var i1468 = []
  for(var i = 0; i < i1469.length; i += 1) {
    i1468.push( i1469[i + 0] );
  }
  i1460.allTags = i1468
  return i1460
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i1472 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i1473 = data
  i1472.name = i1473[0]
  i1472.value = i1473[1]
  return i1472
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i1476 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i1477 = data
  i1476.id = i1477[0]
  i1476.name = i1477[1]
  i1476.value = i1477[2]
  return i1476
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i1480 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i1481 = data
  i1480.id = i1481[0]
  i1480.name = i1481[1]
  return i1480
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i1482 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i1483 = data
  i1482.fixedDeltaTime = i1483[0]
  i1482.maximumDeltaTime = i1483[1]
  i1482.timeScale = i1483[2]
  i1482.maximumParticleTimestep = i1483[3]
  return i1482
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i1484 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i1485 = data
  i1484.gravity = new pc.Vec3( i1485[0], i1485[1], i1485[2] )
  i1484.defaultSolverIterations = i1485[3]
  i1484.bounceThreshold = i1485[4]
  i1484.autoSyncTransforms = !!i1485[5]
  i1484.autoSimulation = !!i1485[6]
  var i1487 = i1485[7]
  var i1486 = []
  for(var i = 0; i < i1487.length; i += 1) {
    i1486.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i1487[i + 0]) );
  }
  i1484.collisionMatrix = i1486
  return i1484
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i1490 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i1491 = data
  i1490.enabled = !!i1491[0]
  i1490.layerId = i1491[1]
  i1490.otherLayerId = i1491[2]
  return i1490
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i1492 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i1493 = data
  request.r(i1493[0], i1493[1], 0, i1492, 'material')
  i1492.gravity = new pc.Vec2( i1493[2], i1493[3] )
  i1492.positionIterations = i1493[4]
  i1492.velocityIterations = i1493[5]
  i1492.velocityThreshold = i1493[6]
  i1492.maxLinearCorrection = i1493[7]
  i1492.maxAngularCorrection = i1493[8]
  i1492.maxTranslationSpeed = i1493[9]
  i1492.maxRotationSpeed = i1493[10]
  i1492.baumgarteScale = i1493[11]
  i1492.baumgarteTOIScale = i1493[12]
  i1492.timeToSleep = i1493[13]
  i1492.linearSleepTolerance = i1493[14]
  i1492.angularSleepTolerance = i1493[15]
  i1492.defaultContactOffset = i1493[16]
  i1492.autoSimulation = !!i1493[17]
  i1492.queriesHitTriggers = !!i1493[18]
  i1492.queriesStartInColliders = !!i1493[19]
  i1492.callbacksOnDisable = !!i1493[20]
  i1492.reuseCollisionCallbacks = !!i1493[21]
  i1492.autoSyncTransforms = !!i1493[22]
  var i1495 = i1493[23]
  var i1494 = []
  for(var i = 0; i < i1495.length; i += 1) {
    i1494.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i1495[i + 0]) );
  }
  i1492.collisionMatrix = i1494
  return i1492
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i1498 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i1499 = data
  i1498.enabled = !!i1499[0]
  i1498.layerId = i1499[1]
  i1498.otherLayerId = i1499[2]
  return i1498
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i1500 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i1501 = data
  var i1503 = i1501[0]
  var i1502 = []
  for(var i = 0; i < i1503.length; i += 1) {
    i1502.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i1503[i + 0]) );
  }
  i1500.qualityLevels = i1502
  var i1505 = i1501[1]
  var i1504 = []
  for(var i = 0; i < i1505.length; i += 1) {
    i1504.push( i1505[i + 0] );
  }
  i1500.names = i1504
  i1500.shadows = i1501[2]
  i1500.anisotropicFiltering = i1501[3]
  i1500.antiAliasing = i1501[4]
  i1500.lodBias = i1501[5]
  i1500.shadowCascades = i1501[6]
  i1500.shadowDistance = i1501[7]
  i1500.shadowmaskMode = i1501[8]
  i1500.shadowProjection = i1501[9]
  i1500.shadowResolution = i1501[10]
  i1500.softParticles = !!i1501[11]
  i1500.softVegetation = !!i1501[12]
  i1500.activeColorSpace = i1501[13]
  i1500.desiredColorSpace = i1501[14]
  i1500.masterTextureLimit = i1501[15]
  i1500.maxQueuedFrames = i1501[16]
  i1500.particleRaycastBudget = i1501[17]
  i1500.pixelLightCount = i1501[18]
  i1500.realtimeReflectionProbes = !!i1501[19]
  i1500.shadowCascade2Split = i1501[20]
  i1500.shadowCascade4Split = new pc.Vec3( i1501[21], i1501[22], i1501[23] )
  i1500.streamingMipmapsActive = !!i1501[24]
  i1500.vSyncCount = i1501[25]
  i1500.asyncUploadBufferSize = i1501[26]
  i1500.asyncUploadTimeSlice = i1501[27]
  i1500.billboardsFaceCameraPosition = !!i1501[28]
  i1500.shadowNearPlaneOffset = i1501[29]
  i1500.streamingMipmapsMemoryBudget = i1501[30]
  i1500.maximumLODLevel = i1501[31]
  i1500.streamingMipmapsAddAllCameras = !!i1501[32]
  i1500.streamingMipmapsMaxLevelReduction = i1501[33]
  i1500.streamingMipmapsRenderersPerFrame = i1501[34]
  i1500.resolutionScalingFixedDPIFactor = i1501[35]
  i1500.streamingMipmapsMaxFileIORequests = i1501[36]
  i1500.currentQualityLevel = i1501[37]
  return i1500
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixer"] = function (request, data, root) {
  var i1508 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixer' )
  var i1509 = data
  var i1511 = i1509[0]
  var i1510 = []
  for(var i = 0; i < i1511.length; i += 1) {
    i1510.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup', i1511[i + 0]) );
  }
  i1508.groups = i1510
  var i1513 = i1509[1]
  var i1512 = []
  for(var i = 0; i < i1513.length; i += 1) {
    i1512.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot', i1513[i + 0]) );
  }
  i1508.snapshots = i1512
  return i1508
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup"] = function (request, data, root) {
  var i1516 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup' )
  var i1517 = data
  i1516.id = i1517[0]
  i1516.childGroupIds = i1517[1]
  i1516.name = i1517[2]
  return i1516
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot"] = function (request, data, root) {
  var i1520 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot' )
  var i1521 = data
  i1520.id = i1521[0]
  var i1523 = i1521[1]
  var i1522 = []
  for(var i = 0; i < i1523.length; i += 1) {
    i1522.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter', i1523[i + 0]) );
  }
  i1520.parameters = i1522
  return i1520
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter"] = function (request, data, root) {
  var i1526 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter' )
  var i1527 = data
  i1526.name = i1527[0]
  i1526.value = i1527[1]
  return i1526
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar"] = function (request, data, root) {
  var i1528 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar' )
  var i1529 = data
  i1528.name = i1529[0]
  var i1531 = i1529[1]
  var i1530 = []
  for(var i = 0; i < i1531.length; i += 1) {
    i1530.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar+TOSPair', i1531[i + 0]) );
  }
  i1528.tos = i1530
  var i1533 = i1529[2]
  var i1532 = []
  for(var i = 0; i < i1533.length; i += 1) {
    i1532.push( i1533[i + 0] );
  }
  i1528.constant = i1532
  i1528.isValid = !!i1529[3]
  i1528.isHuman = !!i1529[4]
  i1528.hasRootMotion = !!i1529[5]
  return i1528
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar+TOSPair"] = function (request, data, root) {
  var i1536 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar+TOSPair' )
  var i1537 = data
  i1536.hash = i1537[0]
  i1536.path = i1537[1]
  return i1536
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame"] = function (request, data, root) {
  var i1540 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame' )
  var i1541 = data
  i1540.weight = i1541[0]
  i1540.vertices = i1541[1]
  i1540.normals = i1541[2]
  i1540.tangents = i1541[3]
  return i1540
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+RenderObjectsSettings"] = function (request, data, root) {
  var i1542 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+RenderObjectsSettings' )
  var i1543 = data
  i1542.Event = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1543[0], i1542.Event)
  i1542.filterSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+FilterSettings', i1543[1], i1542.filterSettings)
  i1542.overrideMaterialId = i1543[2]
  i1542.overrideMaterialPassIndex = i1543[3]
  i1542.overrideShaderId = i1543[4]
  i1542.overrideShaderPassIndex = i1543[5]
  i1542.overrideMode = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1543[6], i1542.overrideMode)
  i1542.overrideDepthState = !!i1543[7]
  i1542.depthCompareFunction = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1543[8], i1542.depthCompareFunction)
  i1542.enableWrite = !!i1543[9]
  i1542.stencilSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.StencilStateData', i1543[10], i1542.stencilSettings)
  i1542.cameraSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+CustomCameraSettings', i1543[11], i1542.cameraSettings)
  return i1542
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey"] = function (request, data, root) {
  var i1546 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey' )
  var i1547 = data
  i1546.time = i1547[0]
  request.r(i1547[1], i1547[2], 0, i1546, 'value')
  return i1546
}

Deserializers["TMPro.GlyphAnchorPoint"] = function (request, data, root) {
  var i1548 = root || request.c( 'TMPro.GlyphAnchorPoint' )
  var i1549 = data
  i1548.m_XCoordinate = i1549[0]
  i1548.m_YCoordinate = i1549[1]
  return i1548
}

Deserializers["TMPro.MarkPositionAdjustment"] = function (request, data, root) {
  var i1550 = root || request.c( 'TMPro.MarkPositionAdjustment' )
  var i1551 = data
  i1550.m_XPositionAdjustment = i1551[0]
  i1550.m_YPositionAdjustment = i1551[1]
  return i1550
}

Deserializers["TMPro.GlyphValueRecord_Legacy"] = function (request, data, root) {
  var i1552 = root || request.c( 'TMPro.GlyphValueRecord_Legacy' )
  var i1553 = data
  i1552.xPlacement = i1553[0]
  i1552.yPlacement = i1553[1]
  i1552.xAdvance = i1553[2]
  i1552.yAdvance = i1553[3]
  return i1552
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.EnumDescription"] = function (request, data, root) {
  var i1554 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.EnumDescription' )
  var i1555 = data
  i1554.Value = i1555[0]
  return i1554
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+FilterSettings"] = function (request, data, root) {
  var i1556 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+FilterSettings' )
  var i1557 = data
  i1556.RenderQueueType = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1557[0], i1556.RenderQueueType)
  i1556.LayerMask = i1557[1]
  var i1559 = i1557[2]
  var i1558 = []
  for(var i = 0; i < i1559.length; i += 1) {
    i1558.push( i1559[i + 0] );
  }
  i1556.PassNames = i1558
  return i1556
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.StencilStateData"] = function (request, data, root) {
  var i1560 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.StencilStateData' )
  var i1561 = data
  i1560.overrideStencilState = !!i1561[0]
  i1560.stencilReference = i1561[1]
  i1560.stencilCompareFunctionValue = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1561[2], i1560.stencilCompareFunctionValue)
  i1560.passOperationValue = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1561[3], i1560.passOperationValue)
  i1560.failOperationValue = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1561[4], i1560.failOperationValue)
  i1560.zFailOperationValue = request.d('Luna.Unity.DTO.UnityEngine.Assets.EnumDescription', i1561[5], i1560.zFailOperationValue)
  return i1560
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+CustomCameraSettings"] = function (request, data, root) {
  var i1562 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+CustomCameraSettings' )
  var i1563 = data
  i1562.overrideCamera = !!i1563[0]
  i1562.restoreCamera = !!i1563[1]
  i1562.offset = new pc.Vec4( i1563[2], i1563[3], i1563[4], i1563[5] )
  i1562.cameraFieldOfView = i1563[6]
  return i1562
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Components.Transform":{"position":0,"scale":3,"rotation":6},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Components.MeshFilter":{"sharedMesh":0},"Luna.Unity.DTO.UnityEngine.Components.MeshRenderer":{"additionalVertexStreams":0,"enabled":2,"sharedMaterial":3,"sharedMaterials":5,"receiveShadows":6,"shadowCastingMode":7,"sortingLayerID":8,"sortingOrder":9,"lightmapIndex":10,"lightmapSceneIndex":11,"lightmapScaleOffset":12,"lightProbeUsage":16,"reflectionProbeUsage":17},"Luna.Unity.DTO.UnityEngine.Components.MeshCollider":{"sharedMesh":0,"convex":2,"enabled":3,"isTrigger":4,"material":5},"Luna.Unity.DTO.UnityEngine.Components.BoxCollider":{"center":0,"size":3,"enabled":6,"isTrigger":7,"material":8},"Luna.Unity.DTO.UnityEngine.Assets.Mesh":{"name":0,"halfPrecision":1,"useSimplification":2,"useUInt32IndexFormat":3,"vertexCount":4,"aabb":5,"streams":6,"vertices":7,"subMeshes":8,"bindposes":9,"blendShapes":10},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh":{"triangles":0},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape":{"name":0,"frames":1},"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Textures.Texture2D":{"name":0,"width":1,"height":2,"mipmapCount":3,"anisoLevel":4,"filterMode":5,"hdr":6,"format":7,"wrapMode":8,"alphaIsTransparency":9,"alphaSource":10,"graphicsFormat":11,"sRGBTexture":12,"desiredColorSpace":13,"wrapU":14,"wrapV":15},"Luna.Unity.DTO.UnityEngine.Components.Animator":{"animatorController":0,"avatar":2,"updateMode":4,"hasTransformHierarchy":5,"applyRootMotion":6,"humanBones":7,"enabled":8},"Luna.Unity.DTO.UnityEngine.Components.CapsuleCollider":{"center":0,"radius":3,"height":4,"direction":5,"enabled":6,"isTrigger":7,"material":8},"Luna.Unity.DTO.UnityEngine.Components.Rigidbody":{"mass":0,"drag":1,"angularDrag":2,"useGravity":3,"isKinematic":4,"constraints":5,"maxAngularVelocity":6,"collisionDetectionMode":7,"interpolation":8},"Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer":{"sharedMesh":0,"bones":2,"updateWhenOffscreen":3,"localBounds":4,"rootBone":5,"blendShapesWeights":7,"enabled":8,"sharedMaterial":9,"sharedMaterials":11,"receiveShadows":12,"shadowCastingMode":13,"sortingLayerID":14,"sortingOrder":15,"lightmapIndex":16,"lightmapSceneIndex":17,"lightmapScaleOffset":18,"lightProbeUsage":22,"reflectionProbeUsage":23},"Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight":{"weight":0},"Luna.Unity.DTO.UnityEngine.Components.RectTransform":{"pivot":0,"anchorMin":2,"anchorMax":4,"sizeDelta":6,"anchoredPosition3D":8,"rotation":11,"scale":15},"Luna.Unity.DTO.UnityEngine.Components.Canvas":{"planeDistance":0,"referencePixelsPerUnit":1,"isFallbackOverlay":2,"renderMode":3,"renderOrder":4,"sortingLayerName":5,"sortingOrder":6,"scaleFactor":7,"worldCamera":8,"overrideSorting":10,"pixelPerfect":11,"targetDisplay":12,"overridePixelPerfect":13,"enabled":14},"Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer":{"cullTransparentMesh":0},"Luna.Unity.DTO.UnityEngine.Components.AudioSource":{"clip":0,"outputAudioMixerGroup":2,"playOnAwake":4,"loop":5,"time":6,"volume":7,"pitch":8,"enabled":9},"Luna.Unity.DTO.UnityEngine.Textures.Cubemap":{"name":0,"atlasId":1,"mipmapCount":2,"hdr":3,"size":4,"anisoLevel":5,"filterMode":6,"rects":7,"wrapU":8,"wrapV":9},"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"aspect":0,"orthographic":1,"orthographicSize":2,"backgroundColor":3,"nearClipPlane":7,"farClipPlane":8,"fieldOfView":9,"depth":10,"clearFlags":11,"cullingMask":12,"rect":13,"targetTexture":14,"usePhysicalProperties":16,"focalLength":17,"sensorSize":18,"lensShift":20,"gateFit":22,"commandBufferCount":23,"cameraType":24,"enabled":25},"Luna.Unity.DTO.UnityEngine.Components.Light":{"type":0,"color":1,"cullingMask":5,"intensity":6,"range":7,"spotAngle":8,"shadows":9,"shadowNormalBias":10,"shadowBias":11,"shadowStrength":12,"shadowResolution":13,"lightmapBakeType":14,"renderMode":15,"cookie":16,"cookieSize":18,"shadowNearPlane":19,"occlusionMaskChannel":20,"isBaked":21,"mixedLightingMode":22,"enabled":23},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"customReflection":36,"defaultReflection":38,"defaultReflectionMode":40,"defaultReflectionResolution":41,"sunLightObjectId":42,"pixelLightCount":43,"defaultReflectionHDR":44,"hasLightDataAsset":45,"hasManualGenerate":46},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2,"shadowMask":4},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.UniversalRenderPipelineAsset":{"AdditionalLightsRenderingMode":0,"LightRenderingMode":1,"MainLightRenderingModeValue":2,"SupportsMainLightShadows":3,"MixedLightingSupported":4,"MainLightShadowmapResolutionValue":5,"SupportsSoftShadows":6,"SoftShadowQualityValue":7,"ShadowDistance":8,"ShadowCascadeCount":9,"Cascade2Split":10,"Cascade3Split":11,"Cascade4Split":13,"CascadeBorder":16,"ShadowDepthBias":17,"ShadowNormalBias":18,"RequireDepthTexture":19,"RequireOpaqueTexture":20,"scriptableRendererData":21},"Luna.Unity.DTO.UnityEngine.Assets.LightRenderingMode":{"Disabled":0,"PerVertex":1,"PerPixel":2},"Luna.Unity.DTO.UnityEngine.Assets.ScriptableRendererData":{"opaqueLayerMask":0,"transparentLayerMask":1,"RenderObjectsFeatures":2,"name":3},"Luna.Unity.DTO.UnityEngine.Assets.RenderObjects":{"settings":0,"name":1,"typeName":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"hasDepthOnlyPass":10,"isCreatedByShaderGraph":11,"disableBatching":12,"compiled":13},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Textures.Sprite":{"name":0,"texture":1,"aabb":3,"vertices":4,"triangles":5,"textureRect":6,"packedRect":10,"border":14,"transparency":18,"bounds":19,"pixelsPerUnit":20,"textureWidth":21,"textureHeight":22,"nativeSize":23,"pivot":25,"textureRectOffset":27},"Luna.Unity.DTO.UnityEngine.Assets.AudioClip":{"name":0},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip":{"name":0,"wrapMode":1,"isLooping":2,"length":3,"curves":4,"events":5,"halfPrecision":6,"_frameRate":7,"localBounds":8,"hasMuscleCurves":9,"clipMuscleConstant":10,"clipBindingConstant":11},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve":{"path":0,"hash":1,"componentType":2,"property":3,"keys":4,"objectReferenceKeys":5},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent":{"functionName":0,"floatParameter":1,"intParameter":2,"stringParameter":3,"objectReferenceParameter":4,"time":6},"Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds":{"center":0,"extends":3},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant":{"genericBindings":0,"pptrCurveMapping":1},"Luna.Unity.DTO.UnityEngine.Assets.Font":{"name":0,"ascent":1,"originalLineHeight":2,"fontSize":3,"characterInfo":4,"texture":5,"originalFontSize":7},"Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo":{"index":0,"advance":1,"bearing":2,"glyphWidth":3,"glyphHeight":4,"minX":5,"maxX":6,"minY":7,"maxY":8,"uvBottomLeftX":9,"uvBottomLeftY":10,"uvBottomRightX":11,"uvBottomRightY":12,"uvTopLeftX":13,"uvTopLeftY":14,"uvTopRightX":15,"uvTopRightY":16},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorController":{"name":0,"layers":1,"parameters":2,"animationClips":3,"avatarUnsupported":4},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer":{"name":0,"defaultWeight":1,"blendingMode":2,"avatarMask":3,"syncedLayerIndex":4,"syncedLayerAffectsTiming":5,"syncedLayers":6,"stateMachine":7},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine":{"id":0,"name":1,"path":2,"states":3,"machines":4,"entryStateTransitions":5,"exitStateTransitions":6,"anyStateTransitions":7,"defaultStateId":8},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState":{"id":0,"name":1,"cycleOffset":2,"cycleOffsetParameter":3,"cycleOffsetParameterActive":4,"mirror":5,"mirrorParameter":6,"mirrorParameterActive":7,"motionId":8,"nameHash":9,"fullPathHash":10,"speed":11,"speedParameter":12,"speedParameterActive":13,"tag":14,"tagHash":15,"writeDefaultValues":16,"behaviours":17,"transitions":18},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition":{"fullPath":0,"canTransitionToSelf":1,"duration":2,"exitTime":3,"hasExitTime":4,"hasFixedDuration":5,"interruptionSource":6,"offset":7,"orderedInterruption":8,"destinationStateId":9,"isExit":10,"mute":11,"solo":12,"conditions":13},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition":{"mode":0,"parameter":1,"threshold":2},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition":{"destinationStateId":0,"isExit":1,"mute":2,"solo":3,"conditions":4},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter":{"defaultBool":0,"defaultFloat":1,"defaultInt":2,"name":3,"nameHash":4,"type":5},"Luna.Unity.DTO.UnityEngine.Assets.TextAsset":{"name":0,"bytes64":1,"data":2},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableStaticBatching":9,"enableDynamicBatching":10,"usePreservativeDynamicBatching":11,"lightmapEncodingQuality":12,"desiredColorSpace":13,"allTags":14},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixer":{"groups":0,"snapshots":1},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup":{"id":0,"childGroupIds":1,"name":2},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot":{"id":0,"parameters":1},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar":{"name":0,"tos":1,"constant":2,"isValid":3,"isHuman":4,"hasRootMotion":5},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.Avatar+TOSPair":{"hash":0,"path":1},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame":{"weight":0,"vertices":1,"normals":2,"tangents":3},"Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+RenderObjectsSettings":{"Event":0,"filterSettings":1,"overrideMaterialId":2,"overrideMaterialPassIndex":3,"overrideShaderId":4,"overrideShaderPassIndex":5,"overrideMode":6,"overrideDepthState":7,"depthCompareFunction":8,"enableWrite":9,"stencilSettings":10,"cameraSettings":11},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey":{"time":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.EnumDescription":{"Value":0},"Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+FilterSettings":{"RenderQueueType":0,"LayerMask":1,"PassNames":2},"Luna.Unity.DTO.UnityEngine.Assets.StencilStateData":{"overrideStencilState":0,"stencilReference":1,"stencilCompareFunctionValue":2,"passOperationValue":3,"failOperationValue":4,"zFailOperationValue":5},"Luna.Unity.DTO.UnityEngine.Assets.RenderObjects+CustomCameraSettings":{"overrideCamera":0,"restoreCamera":1,"offset":2,"cameraFieldOfView":6}}

Deserializers.requiredComponents = {"90":[91],"92":[91],"93":[91],"94":[91],"95":[91],"96":[91],"97":[26],"98":[47],"99":[25],"100":[25],"101":[25],"102":[25],"103":[25],"104":[25],"105":[106],"107":[106],"108":[106],"109":[106],"110":[106],"111":[106],"112":[106],"113":[106],"114":[106],"115":[106],"116":[106],"117":[106],"118":[106],"119":[47],"120":[3],"121":[122],"123":[122],"28":[27],"124":[27],"125":[24],"126":[25],"46":[42,25],"127":[53],"128":[28],"129":[27],"130":[47],"50":[47],"52":[51],"131":[27],"132":[34,27],"133":[3],"134":[34,27],"135":[27],"136":[27],"137":[3,27],"77":[27,34],"138":[139],"140":[139],"141":[139],"142":[27],"143":[27],"31":[28],"35":[34,27],"37":[27],"30":[28],"144":[27],"145":[27],"76":[27],"146":[27],"75":[27],"147":[27],"79":[27],"148":[27],"149":[27],"150":[34,27],"151":[27],"152":[27],"153":[27],"154":[27],"57":[34,27],"155":[27],"156":[53],"157":[53],"54":[53],"158":[53],"159":[47],"160":[47]}

Deserializers.types = ["UnityEngine.Transform","UnityEngine.MeshFilter","UnityEngine.Mesh","UnityEngine.MeshRenderer","UnityEngine.Material","UnityEngine.MeshCollider","UnityEngine.BoxCollider","UnityEngine.MonoBehaviour","Code.Gameplay.Coloring.ColorField","UnityEngine.Shader","UnityEngine.Texture2D","Code.Gameplay.AI.GameAIContext","Code.Gameplay.AI.Services.BotFieldMap","Code.Gameplay.AI.Waypoints.FieldPatrolWaypoint","Code.Gameplay.AI.Services.AISpawnController","Code.Gameplay.AI.Services.AISpawnPoint","Code.Gameplay.AI.Behaviours.AICharacter","Code.Gameplay.AI.Brains.Commands.CommandAIBrainProvider","Code.Gameplay.Common.Behaviours.TransformPoint","Code.Gameplay.AI.Waypoints.WaitWaypoint","Code.Gameplay.AI.Waypoints.RouteWaypoint","UnityEngine.Animator","UnityEditor.Animations.AnimatorController","UnityEngine.Avatar","UnityEngine.CapsuleCollider","UnityEngine.Rigidbody","UnityEngine.SkinnedMeshRenderer","UnityEngine.RectTransform","UnityEngine.Canvas","UnityEngine.EventSystems.UIBehaviour","UnityEngine.UI.CanvasScaler","UnityEngine.UI.GraphicRaycaster","PackshotView","UnityEngine.UI.Button","UnityEngine.CanvasRenderer","UnityEngine.UI.Image","UnityEngine.Sprite","UnityEngine.UI.AspectRatioFitter","ButtonPulseDotween","UnityEngine.GameObject","DownloadManager","CanvasSelector","Code.Gameplay.PlayerLogic.Movement.GamePlayerMovement","UnityEngine.AudioSource","UnityEngine.AudioClip","UnityEditor.Audio.AudioMixerGroupController","Code.Gameplay.PlayerLogic.Behaviours.GamePlayerCharacter","UnityEngine.Camera","UnityEngine.AudioListener","Code.Gameplay.PlayerLogic.CameraControl.GamePlayerCamera","UnityEngine.Rendering.Universal.UniversalAdditionalCameraData","UnityEngine.Light","UnityEngine.Rendering.Universal.UniversalAdditionalLightData","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.StandaloneInputModule","Code.Gameplay.PlayerLogic.UI.GamePlayerHUD","PlayerHUDView","UnityEngine.UI.Text","UnityEngine.Font","Code.Gameplay.Coloring.ColorScoreText","Code.Gameplay.PlayerLogic.UI.GamePlayerFingerHint","Code.Gameplay.PlayerLogic.UI.GamePlayerJumpButton","AudioController","Code.Gameplay.PlayerLogic.GamePlayerContext","Code.Gameplay.PlayerLogic.Services.GamePlayerSpawnController","Code.Gameplay.PlayerLogic.Services.GamePlayerSpawnPoint","PackshotController","StartController","Code.Gameplay.PlayerLogic.UI.GamePlayerJoystick","GameTimeController","SoundUIButton","UnityEditor.Audio.AudioMixerController","UnityEngine.Cubemap","CustomizationCanvasView","Source.Scripts.UI.Customization.Views.CustomizationScreenView","UnityEngine.UI.LayoutElement","UnityEngine.UI.HorizontalLayoutGroup","TMPro.TextMeshProUGUI","TMPro.TMP_FontAsset","UnityEngine.UI.VerticalLayoutGroup","Source.Scripts.UI.Customization.Views.LoadGameSceneButton","Source.Scripts.Orientation.FitTransformToUiViewport","Source.Scripts.Tutorial.TutorialManager","ButtonsAudioController","MusicManager","DG.Tweening.Core.DOTweenSettings","TMPro.TMP_Settings","TMPro.TMP_SpriteAsset","TMPro.TMP_StyleSheet","UnityEngine.TextAsset","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.FlareLayer","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.ConstantForce","UnityEngine.FixedJoint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.CompositeCollider2D","UnityEngine.Rigidbody2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","Source.Scripts.Orientation.FitAspectInParent","KinematicCharacterController.KinematicCharacterMotor","KinematicCharacterController.PhysicsMover","UnityEngine.InputSystem.UI.InputSystemUIInputModule","UnityEngine.InputSystem.UI.TrackedDeviceRaycaster","UnityEngine.Rendering.UI.UIFoldout","UnityEngine.Rendering.Universal.PixelPerfectCamera","TMPro.TMP_Dropdown","TMPro.TMP_SelectionCaret","TMPro.TMP_SubMesh","TMPro.TMP_SubMeshUI","TMPro.TMP_Text","TMPro.TextContainer","TMPro.TextMeshPro","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","Unity.VisualScripting.ScriptMachine","Unity.VisualScripting.StateMachine","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.ScrollRect","UnityEngine.UI.Scrollbar","UnityEngine.UI.Slider","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster"]

Deserializers.unityVersion = "6000.0.84f1";

Deserializers.productName = "PrisonEscapeObbyChallenge";

Deserializers.lunaInitializationTime = "10/08/2026 03:35:24";

Deserializers.lunaDaysRunning = "6.1";

Deserializers.lunaVersion = "7.2.0";

Deserializers.lunaSHA = "ea08d29afe2968efcb8d91d5624f033c6485cc68";

Deserializers.creativeName = "PrisonEscapeObbyChallengeAds";

Deserializers.lunaAppID = "43353";

Deserializers.projectId = "9f467428c690d994e8444328a29f4d79";

Deserializers.packagesInfo = "com.unity.inputsystem: 1.20.0\ncom.unity.render-pipelines.universal: 17.0.4\ncom.unity.ugui: 2.0.0";

Deserializers.externalJsLibraries = "";

Deserializers.androidLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.androidLink?window.$environment.packageConfig.androidLink:'Empty';

Deserializers.iosLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.iosLink?window.$environment.packageConfig.iosLink:'Empty';

Deserializers.base64Enabled = "False";

Deserializers.minifyEnabled = "True";

Deserializers.isForceUncompressed = "False";

Deserializers.isAntiAliasingEnabled = "False";

Deserializers.isRuntimeAnalysisEnabledForCode = "True";

Deserializers.runtimeAnalysisExcludedClassesCount = "1657";

Deserializers.runtimeAnalysisExcludedMethodsCount = "5166";

Deserializers.runtimeAnalysisExcludedModules = "physics2d, particle-system";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isLunaCompilerV2Used = "False";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "WebGL";

Deserializers.applicationIdentifier = "com.DefaultCompany.PrisonEscapeObbyChallenge";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 24;

Deserializers.linearColorSpace = true;

Deserializers.buildID = "59033f3d-b082-4511-b323-e8ebc598ee89";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Rendering","DebugUpdater","RuntimeInit"],["Unity","PerformanceTesting","PerformanceTest","ResetStaticsOnLoad"],["Unity","Burst","BurstCompiler","ResetStaticsOnLoad"],["Source","Scripts","UI","Customization","Data","CharacterAppearanceStore","HookSceneLoaded"],["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","GraphReference","ResetStaticsOnLoad"],["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"],["UnityEngine","InputSystem","PlayerInput","InitializeGlobalPlayerState"],["UnityEngine","InputSystem","InputSystem","RuntimeInitialize"],["UnityEngine","InputSystem","InputActionState","InitializeGlobalActionState"],["Unity","AI","Navigation","NavMeshModifierVolume","ClearNavMeshModifiers"],["Unity","AI","Navigation","NavMeshLink","ClearTrackedList"],["Unity","AI","Navigation","NavMeshSurface","ClearNavMeshSurfaces"],["Unity","AI","Navigation","NavMeshModifier","ClearNavMeshModifiers"],["Source","Scripts","Orientation","OrientationWatcher","Bootstrap"],["UnityEngine","AI","NavMesh","ClearPreUpdateListeners"]],[["Unity","VisualScripting","Dependencies","NCalc","Expression","ResetStaticsOnLoad"],["Unity","VisualScripting","Flow","ResetStaticsOnLoad"],["Unity","VisualScripting","GraphInstances","ResetStaticsOnLoad"],["Unity","VisualScripting","FullSerializer","fsGlobalConfig","ResetStaticsOnLoad"],["Unity","VisualScripting","RuntimeCodebase","ResetStaticsOnLoad"],["Unity","VisualScripting","FullSerializer","fsAotCompilationManager","ResetStaticsOnLoad"],["Unity","VisualScripting","FullSerializer","fsSerializer","ResetStaticsOnLoad"],["Unity","VisualScripting","EventBus","ResetStaticsOnLoad"],["Unity","VisualScripting","Ensure","ResetStaticsOnLoad"],["Unity","VisualScripting","UnityThread","ResetStaticsOnLoad"],["Unity","VisualScripting","Recursion","ResetStaticsOnLoad"],["Unity","VisualScripting","SavedVariables","ResetStaticsOnLoad"],["Unity","VisualScripting","FullSerializer","fsMetaType","ResetStaticsOnLoad"],["Unity","VisualScripting","FullSerializer","fsResult","ResetStaticsOnLoad"],["Unity","VisualScripting","ApplicationVariables","ResetStaticsOnLoad"],["Unity","VisualScripting","MessageListener","ResetStaticsOnLoad"],["Unity","VisualScripting","Serialization","ResetStaticsOnLoad"],["Unity","VisualScripting","ReferenceCollector","ResetStaticsOnLoad"],["Unity","VisualScripting","OptimizedReflection","ResetStaticsOnLoad"],["Unity","VisualScripting","EditorTimeBinding","ResetStaticsOnLoad"],["Unity","VisualScripting","ProfilingUtility","ResetStaticsOnLoad"],["Unity","VisualScripting","FullSerializer","Internal","fsPortableReflection","ResetStaticsOnLoad"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"]],[["UnityEngine","Experimental","Rendering","XRSystem","XRSystemInit"]],[["Unity","PerformanceTesting","Data","RunSettings","ResetStaticsOnLoad"],["Unity","PerformanceTesting","PlayerCallbacks","ResetStaticsOnLoad"],["UnityEngine","InputSystem","Plugins","InputForUI","InputSystemProvider","Bootstrap"],["UnityEngine","InputSystem","EnhancedTouch","Touch","InitializeGlobalTouchState"],["UnityEngine","InputSystem","Users","InputUser","InitializeGlobalUserState"],["UnityEngine","InputSystem","UI","InputSystemUIInputModule","ResetDefaultActions"],["Source","Scripts","Orientation","OrientationWatcher","ResetStatics"],["Source","Scripts","UI","Customization","Data","CharacterAppearanceStore","ResetStatics"]]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

