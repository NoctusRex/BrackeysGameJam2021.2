﻿using Bliss.Component.Sprites.Office;
using Bliss.Component.Sprites.Office.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bliss.Models
{
    public class PhoneCall
    {
        public bool IsImportant => NewValidators.Any();

        /// <summary>
        /// Key = Organizer Id
        /// </summary>
        public Dictionary<OrganizerIds, List<Rule>> NewValidators { get; set; } = new Dictionary<OrganizerIds, List<Rule>>();

        public List<VoiceLine> VoiceLines { get; set; }
    }
}
