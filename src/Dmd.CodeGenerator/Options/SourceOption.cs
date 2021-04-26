﻿using System.Collections.Generic;

namespace Dmd.CodeGenerator.Options
{
    public class SourceOption
    {
        public ICollection<string> Imports { get; set; }

        public string Namespace { get; set; }

        public string ProjectDirectory { get; set; }

        public string Directory { get; set; }

        public string Name { get; set; }

        public string BaseClass { get; set; }

        public ICollection<string> BaseInterfaces { get; set; }

        public SourceType CodeType { get; set; }

        public ICollection<PropertyOption> Properties { get; set; }

        protected SourceOption()
        {
            BaseInterfaces = new List<string>();
            Properties = new List<PropertyOption>();
            Imports = new List<string>();
        }

    }
}