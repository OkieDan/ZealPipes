using System.Text.Json;
using Xunit;
using ZealPipes.Common.Models;

namespace ZealPipes.Common.Tests
{
    public class MessageParsingTests
    {
        [Theory]
        [InlineData("\"9\"", "\"60\"")]
        [InlineData("9", "60")]
        public void RaidAcceptsQuotedAndNumericGroupAndLevel(string group, string level)
        {
            string payload = "[{\"name\":\"Tikzak\",\"group\":" + group + ",\"level\":" + level +
                ",\"class\":\"Shaman\",\"rank\":\"Group Leader\",\"spawn_id\":446," +
                "\"hp_current\":4800,\"hp_max\":5001,\"zone_id\":158}]";
            var raid = new RaidMessage("Stanza", payload);
            var member = Assert.Single(raid.Data);
            Assert.Equal(9, member.Group);
            Assert.Equal(60, member.Level);
            Assert.Equal(446, member.SpawnId);
            Assert.Equal(4800, member.HpCurrent);
            Assert.Equal(5001, member.HpMax);
            Assert.Equal(158, member.ZoneId);
            Assert.Equal("Shaman", member.Class);
            Assert.Equal("Group Leader", member.Rank);

            var character = new ZealCharacter("Stanza", 123);
            character.UpdateCharacterData(raid.Data);
            character.UpdateCharacterData(new PlayerMessage.PlayerData());
            var snapshot = Assert.Single(character.Detail.RaidData);
            Assert.NotSame(member, snapshot);
            Assert.Equal(9, snapshot.Group);
            Assert.Equal(60, snapshot.Level);
            Assert.Equal(446, snapshot.SpawnId);
            Assert.Equal(4800, snapshot.HpCurrent);
            Assert.Equal(5001, snapshot.HpMax);
            Assert.Equal(158, snapshot.ZoneId);
        }

        [Fact]
        public void RaidAllowsAbsentEntityAndVerboseFields()
        {
            var member = Assert.Single(new RaidMessage("Stanza",
                "[{\"name\":\"Remote\",\"group\":\"0\",\"level\":\"60\",\"class\":\"Bard\",\"rank\":\"\"}]").Data);
            Assert.Equal(0, member.Group);
            Assert.Equal(60, member.Level);
            Assert.Null(member.Position);
            Assert.Null(member.SpawnId);
            Assert.Null(member.HpCurrent);
            Assert.Null(member.HpMax);
            Assert.Null(member.ZoneId);
        }

        [Fact]
        public void RaidStillFiltersEmptyMemberSlots()
        {
            var member = Assert.Single(new RaidMessage("Stanza",
                "[{\"name\":\"\"},{\"name\":null},{\"name\":\"Tikzak\",\"group\":\"9\",\"level\":\"60\"}]").Data);
            Assert.Equal("Tikzak", member.Name);
        }

        [Theory]
        [InlineData("", null, null)]
        [InlineData(",\"target_id\":401,\"pet_id\":500", 401, 500)]
        [InlineData(",\"target_id\":0,\"pet_id\":0", 0, 0)]
        public void PlayerPreservesOptionalTargetAndPetIds(string fields, int? targetId, int? petId)
        {
            var player = new PlayerMessage("Stanza", "{\"spawn_id\":653,\"zone\":158,\"autoattack\":true" + fields + "}");
            Assert.Equal(653, player.Data.SpawnId);
            Assert.Equal(158, player.Data.ZoneId);
            Assert.True(player.Data.AutoAttack);
            Assert.Equal(targetId, player.Data.TargetId);
            Assert.Equal(petId, player.Data.PetId);
        }

        [Fact]
        public void PlayerAllowsAbsentTargetDescriptors()
        {
            var player = new PlayerMessage("Stanza", "{\"spawn_id\":653,\"zone\":158}");
            Assert.Null(player.Data.TargetName);
            Assert.Null(player.Data.TargetType);
            Assert.Null(player.Data.TargetLevel);
            Assert.Null(player.Data.TargetClass);
            Assert.Null(player.Data.TargetRace);
            Assert.Null(player.Data.TargetPosition);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PlayerTargetDescriptorsReachCharacterSnapshot(bool hasTargetPosition)
        {
            string position = hasTargetPosition ? ",\"target_loc\":{\"x\":-12.5,\"y\":24.25,\"z\":3.75}" : "";
            var player = new PlayerMessage("Stanza",
                "{\"target_id\":401,\"target_name\":\"an orc warrior\",\"target_type\":0," +
                "\"target_level\":60,\"target_class\":4,\"target_race\":2" + position + "}");
            var character = new ZealCharacter("Stanza", 123);
            character.UpdateCharacterData(player.Data);
            var data = character.Detail.PlayerData;

            Assert.Equal(401, data.TargetId);
            Assert.Equal("an orc warrior", data.TargetName);
            Assert.Equal(0, data.TargetType);
            Assert.Equal(60, data.TargetLevel);
            Assert.Equal(4, data.TargetClass);
            Assert.Equal(2, data.TargetRace);
            if (hasTargetPosition)
            {
                Assert.NotNull(data.TargetPosition);
                Assert.Equal(-12.5f, data.TargetPosition.X);
                Assert.Equal(24.25f, data.TargetPosition.Y);
                Assert.Equal(3.75f, data.TargetPosition.Z);
            }
            else
            {
                Assert.Null(data.TargetPosition);
            }
        }

        [Fact]
        public void PlayerSerializesCharacterUnderItsOwnName()
        {
            using var json = JsonDocument.Parse(JsonSerializer.Serialize(new PlayerMessage("Stanza", "{\"zone\":158}")));
            Assert.Equal("Stanza", json.RootElement.GetProperty("character").GetString());
            Assert.False(json.RootElement.TryGetProperty("zone", out _));
            Assert.Equal(158, json.RootElement.GetProperty("Data").GetProperty("zone").GetInt32());
        }

        [Fact]
        public void GroupPreservesVerboseFields()
        {
            var member = Assert.Single(new GroupMessage("Stanza",
                "[{\"name\":\"Mofen\",\"spawn_id\":519,\"hp_current\":4000,\"hp_max\":5000," +
                "\"class\":4,\"level\":60,\"zone_id\":158}]").Data);
            Assert.Equal(519, member.SpawnId);
            Assert.Equal(4000, member.HpCurrent);
            Assert.Equal(5000, member.HpMax);
            Assert.Equal(4, member.Class);
            Assert.Equal(60, member.Level);
            Assert.Equal(158, member.ZoneId);
        }

        [Fact]
        public void GroupAllowsAbsentVerboseFields()
        {
            var member = Assert.Single(new GroupMessage("Stanza", "[{\"name\":\"Mofen\"}]").Data);
            Assert.Null(member.SpawnId);
            Assert.Null(member.HpCurrent);
            Assert.Null(member.HpMax);
            Assert.Null(member.Class);
            Assert.Null(member.Level);
            Assert.Null(member.ZoneId);
        }

        [Fact]
        public void PipeCommandPreservesEscapedText()
        {
            const string text = "Timer \"CH\" }{ C:\\Logs\nStart";
            var message = new PipeCmdMessage("Stanza", JsonSerializer.Serialize(new { text }));
            Assert.Equal(PipeMessageType.PipeCmd, message.Type);
            Assert.Equal("Stanza", message.Character);
            Assert.Equal(text, message.Data.Text);
        }

        [Fact]
        public void LabelMetadataSurvivesCharacterSnapshot()
        {
            var label = Assert.Single(new LabelMessage("Stanza",
                "[{\"type\":45,\"value\":\"Spirit of Bih`Li\",\"meta\":{\"ticks\":224,\"extra\":{\"active\":true}}}]").Data);
            var character = new ZealCharacter("Stanza", 123);
            character.UpdateCharacterData(label);
            character.UpdateCharacterData(new PlayerMessage.PlayerData());
            var snapshot = Assert.Single(character.Detail.LabelData);
            Assert.Equal("Spirit of Bih`Li", snapshot.Value);
            Assert.Equal(224, snapshot.Meta["ticks"].GetInt32());
            Assert.True(snapshot.Meta["extra"].GetProperty("active").GetBoolean());
        }

        [Fact]
        public void UnknownSubtypeIdsAndSignedGaugeValuesArePreserved()
        {
            var log = new LogMessage("Stanza", "{\"type\":9999,\"text\":\"Future log\"}");
            var label = Assert.Single(new LabelMessage("Stanza", "[{\"type\":9999,\"value\":\"Future label\",\"meta\":{}}]").Data);
            var gauge = Assert.Single(new GaugeMessage("Stanza", "[{\"type\":9999,\"text\":\"Future gauge\",\"value\":-3.5}]").Data);
            Assert.Equal(9999, (int)log.Data.Type);
            Assert.Equal(9999, (int)label.Type);
            Assert.Equal(9999, (int)gauge.Type);
            Assert.Equal(-3.5m, gauge.Value);
        }
    }
}
