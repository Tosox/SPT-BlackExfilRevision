using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace Tosox.BlackExfilRevision
{
    [Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
    public class BlackExfilRevision(
        ISptLogger<BlackExfilRevision> logger,
        TemplateTable templateTable
    ) : IOnLoad
    {
        // Insert face shield slot after NVGs like with Coyote Exfil
        internal const int InsertIndex = 2;

        public Task OnLoadAsync(CancellationToken cancellationToken)
        {
            if (!templateTable.Items.TryGetValue(ItemTpl.HEADWEAR_TEAM_WENDY_EXFIL_BALLISTIC_HELMET_BLACK, out var blackExfil))
            {
                logger.Warning($"[{ModMetadata.ModName}] Couldn't find the Black Exfil in the database");
                return Task.CompletedTask;
            }

            var props = blackExfil.Properties;
            if (props?.Slots == null || props.Slots.Count() < InsertIndex)
            {
                logger.Warning($"[{ModMetadata.ModName}] Black Exfil has invalid properties");
                return Task.CompletedTask;
            }

            var slots = props.Slots.ToList();
            slots.Insert(InsertIndex, new()
            {
                Id = "68f3c82a5b5826ab01018fba",
                MergeSlotWithChildren = false,
                Name = "mod_equipment_001",
                Parent = ItemTpl.HEADWEAR_TEAM_WENDY_EXFIL_BALLISTIC_HELMET_BLACK,
                Properties = new()
                {
                    Filters =
                    [
                        new()
                        {
                            Filter = [
                                ItemTpl.ARMOREDEQUIPMENT_TEAM_WENDY_EXFIL_BALLISTIC_FACE_SHIELD_BLACK,
                                ItemTpl.ARMOREDEQUIPMENT_TEAM_WENDY_EXFIL_BALLISTIC_FACE_SHIELD_COYOTE_BROWN
                            ],
                            Shift = 0,
                        },
                    ]
                },
                Prototype = "55d30c4c4bdc2db4468b457e",
                Required = false
            });
            props.Slots = slots;

            logger.Info($"[{ModMetadata.ModName}] Black Exfil can now attach face shields");
            return Task.CompletedTask;
        }
    }

    public record ModMetadata : IModMetadata
    {
        internal const string ModName = "Black Exfil Revision";
        internal const string ModVersion = "1.1.0";
        internal const string ModAuthor = "Tosox";
        internal const string ModSource = "https://github.com/Tosox/SPT-BlackExfilRevision";

        public string ModGuid { get; init; } = "de.tosox.blackexfilrevision";
        public string Name { get; init; } = ModName;
        public string Author { get; init; } = ModAuthor;
        public List<string>? Contributors { get; init; }
        public SemanticVersioning.Version Version { get; init; } = new(ModVersion);
        public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
        public bool HasPrepatcher { get; init; }
        public List<string>? Incompatibilities { get; init; }
        public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
        public string? Url { get; init; } = ModSource;
        public string License { get; init; } = "MIT";
    }
}
