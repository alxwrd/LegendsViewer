using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AssumeIdentity : WorldEvent
    {
        public HistoricalFigure Trickster { get; set; }
        public int IdentityId { get; set; }
        public Entity Target { get; set; }

        public AssumeIdentity(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "trickster_hfid": Trickster = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "identity_id": IdentityId = Convert.ToInt32(property.Value); break;
                    case "target_enid": Target = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "trickster": if (Trickster == null) { Trickster = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "target": if (Target == null) { Target = world.GetEntity(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }

            Trickster.AddEvent(this);
            Target.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Trickster?.ToLink(link, pov, this) ?? "an unknown creature";
            eventString += " fooled ";
            eventString += Target?.ToLink(link, pov, this) ?? "an unknown civilization";
            eventString += " into believing ";
            eventString += Trickster?.ToLink(link, pov, this) ?? "an unknown creature";
            eventString += " was ";
            Identity identity = Trickster?.Identities.FirstOrDefault(i => i.Id == IdentityId);
            if (identity != null)
            {
                eventString += identity.Print(link, pov, this);
            }
            else
            {
                eventString += "someone else";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}