﻿using Dmd.SourceOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dmd.Designer.Models
{
    public class EntityModel : ClassOption
    {
        public new IList<PropertyModel> Properties { get; set; }

        public EntityModel()
        {
            Properties = new List<PropertyModel>();
        }

        public EntityModel(
            string directory,
            string projectDirectory,
            string nameSpace)
        {
            Directory = directory;
            ProjectDirectory = projectDirectory;
            Namespace = nameSpace;
        }
    }
}
