﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using TREnvironmentEditor.Model;
using TREnvironmentEditor.Model.Types;

namespace TREnvironmentEditor.Parsing
{
    public class EMConverter : JsonConverter
    {
        private static readonly JsonSerializerSettings _resolver = new JsonSerializerSettings
        { 
            ContractResolver = new EMResolver() 
        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BaseEMFunction);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["EMType"] == null)
            {
                return null;
            }

            EMType type = (EMType)jo["EMType"].Value<int>();
            switch (type)
            {
                // Surface types
                case EMType.Ladder:
                    return JsonConvert.DeserializeObject<EMLadderFunction>(jo.ToString(), _resolver);
                case EMType.Floor:
                    return JsonConvert.DeserializeObject<EMFloorFunction>(jo.ToString(), _resolver);
                case EMType.Flood:
                    return JsonConvert.DeserializeObject<EMFloodFunction>(jo.ToString(), _resolver);
                case EMType.Drain:
                    return JsonConvert.DeserializeObject<EMDrainFunction>(jo.ToString(), _resolver);
                case EMType.Ceiling:
                    return JsonConvert.DeserializeObject<EMCeilingFunction>(jo.ToString(), _resolver);

                // Texture types
                case EMType.Reface:
                    return JsonConvert.DeserializeObject<EMRefaceFunction>(jo.ToString(), _resolver);
                case EMType.RemoveFace:
                    return JsonConvert.DeserializeObject<EMRemoveFaceFunction>(jo.ToString(), _resolver);
                case EMType.ModifyFace:
                    return JsonConvert.DeserializeObject<EMModifyFaceFunction>(jo.ToString(), _resolver);

                // Entity types
                case EMType.MoveSlot:
                    return JsonConvert.DeserializeObject<EMMoveSlotFunction>(jo.ToString(), _resolver);
                case EMType.MoveEnemy:
                    return JsonConvert.DeserializeObject<EMMoveEnemyFunction>(jo.ToString(), _resolver);
                case EMType.MovePickup:
                    return JsonConvert.DeserializeObject<EMMovePickupFunction>(jo.ToString(), _resolver);
                case EMType.MoveEntity:
                    return JsonConvert.DeserializeObject<EMMoveEntityFunction>(jo.ToString(), _resolver);

                // Trigger types
                case EMType.Trigger:
                    return JsonConvert.DeserializeObject<EMTriggerFunction>(jo.ToString(), _resolver);
                case EMType.RemoveTrigger:
                    return JsonConvert.DeserializeObject<EMRemoveTriggerFunction>(jo.ToString(), _resolver);
                case EMType.DuplicateTrigger:
                    return JsonConvert.DeserializeObject<EMDuplicateTriggerFunction>(jo.ToString(), _resolver);
                case EMType.DuplicateSwitchTrigger:
                    return JsonConvert.DeserializeObject<EMDuplicateSwitchTriggerFunction>(jo.ToString(), _resolver);
                case EMType.CameraTriggerFunction:
                    return JsonConvert.DeserializeObject<EMCameraTriggerFunction>(jo.ToString(), _resolver);

                // Portals
                case EMType.VisibilityPortal:
                    return JsonConvert.DeserializeObject<EMVisibilityPortalFunction>(jo.ToString(), _resolver);
                case EMType.CollisionalPortal:
                    return JsonConvert.DeserializeObject<EMCollisionalPortalFunction>(jo.ToString(), _resolver);

                // NOOP
                case EMType.NOOP:
                    return JsonConvert.DeserializeObject<EMPlaceholderFunction>(jo.ToString(), _resolver);

                default:
                    throw new InvalidOperationException();
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }
    }
}