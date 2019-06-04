using System;
using System.Collections.Generic;
using TeslaExplorer.DataAccess;

namespace TeslaExplorer.Models
{
    public class ChargeViewModel
    {
        public string Id { get; set; }
        public int Percent { get; set; }
    }
}