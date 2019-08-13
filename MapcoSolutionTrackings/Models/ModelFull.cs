using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapcoSolutionTrackings.Models
{
    public class ModelFull
    {
        public List<ModelResults> modelResult { get; set; }
        public List<ModeltotalesA> modelA { get; set; }
        public List<ModeltotalesB> modelB { get; set; }
        public List<ModeltotalesC> modelC { get; set; }
    }
}