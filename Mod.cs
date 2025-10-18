using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;

namespace Tosox.BlackExfilRevision
{
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader)]
    public class BlackExfilRevision(
        DatabaseServer databaseServer,
        ISptLogger<BlackExfilRevision> logger
    ) : IOnLoad
    {
        public Task OnLoad()
        {
            var items = databaseServer.GetTables().Templates.Items;

            if (items.TryGetValue(ItemTpl.HEADWEAR_TEAM_WENDY_EXFIL_BALLISTIC_HELMET_BLACK, out var blackExfil))
            {
                var slots = blackExfil.Properties!.Slots!.ToList();
                slots.Insert(2, new Slot
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
                blackExfil.Properties.Slots = slots;

                logger.Info("[BlackExfilRevision] Changes applied successfully!");
            }
            else
            {
                logger.Warning("[BlackExfilRevision] Couldn't find the Black Exfil in the database");
            }

            return Task.CompletedTask;
        }
    }
}
