using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class Vec3
    {
        [JsonPropertyName("x")]
        public float X { get; set; }
        [JsonPropertyName("y")]
        public float Y { get; set; }
        [JsonPropertyName("z")]
        public float Z { get; set; }
    }
    public class PlayerMessage
    {
        /*
#### Spawn ids
Messages that describe an entity carry that entity's spawn id, so a consumer can
key on a stable identity instead of a display name. This matters because spawns
of the same type share an identical name (`an orc warrior`), and the gauge
stream describes a target or pet with only a name and an HP per-mille value.

- `raid` (type 5) and `group` (type 6) members each carry `spawn_id`
- the `player` message carries `spawn_id` for yourself, plus:
  - `target_id` - the spawn id of your current target
  - `pet_id` - the spawn id of your pet

`target_id` and `pet_id` are omitted entirely when there is no target or no pet.
Target descriptors are also omitted when there is no target. `target_loc` is
only included when the target is less than 250 units from the player.
         */
        public class PlayerData
        {
            [JsonPropertyName("zone")]
            public int ZoneId { get; set; }
            [JsonPropertyName("location")]
            public Vec3 Position { get; set; }
            [JsonPropertyName("heading")]
            public float heading { get; set; }
            [JsonPropertyName("autoattack")]
            public bool AutoAttack { get; set; }
            [JsonPropertyName("spawn_id")]
            public int SpawnId { get; set; }
            [JsonPropertyName("target_id")]
            public int? TargetId { get; set; }
            [JsonPropertyName("target_name")]
            public string TargetName { get; set; }
            [JsonPropertyName("target_type")]
            public int? TargetType { get; set; }
            [JsonPropertyName("target_level")]
            public int? TargetLevel { get; set; }
            [JsonPropertyName("target_class")]
            public int? TargetClass { get; set; }
            [JsonPropertyName("target_race")]
            public int? TargetRace { get; set; }
            [JsonPropertyName("target_loc")]
            public Vec3 TargetPosition { get; set; }
            [JsonPropertyName("pet_id")]
            public int? PetId { get; set; }

        }

        [JsonPropertyName("character")]
        public string Character { get; }
        public PlayerData Data { get; }
        public PipeMessageType Type { get; } = PipeMessageType.Player;
        public PlayerMessage(string character, string data)
        {
            Character = character;
            Data = JsonSerializer.Deserialize<PlayerData>(data);
        }

    }
}
